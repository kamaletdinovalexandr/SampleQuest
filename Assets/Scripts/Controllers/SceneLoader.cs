using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using CameraExtention;

public class SceneLoader : MBSingleton<SceneLoader> {
    [SerializeField] private string StartSceneName;
	public const string OBJECTS_CONTAINER = "ObjectsContainer";
	private const string SEPARATOR = "-";
	private Dictionary<string, bool> _sceneObjects = new Dictionary<string, bool>();
	private Scene _currentScene;

    void Start () {
		StartCoroutine(LoadScene(StartSceneName));
	}

    public void ChangeLocation(string location) {
		UnloadScene();
		StartCoroutine(LoadScene(location));
    }

	private void UnloadScene() {
		var currentScene = SceneManager.GetActiveScene().name;
		var objectsContainer = GameObject.FindGameObjectWithTag(OBJECTS_CONTAINER);
		var sceneObjects = objectsContainer.GetComponentsInChildren<Transform>(true);

		foreach (var sceneObject in sceneObjects) {
			var objectState = sceneObject.gameObject.activeInHierarchy;
			var objectSaveName = currentScene + SEPARATOR + sceneObject.gameObject.name;

			if (!_sceneObjects.ContainsKey(objectSaveName)) {
				_sceneObjects.Add(objectSaveName, objectState);
				Debug.Log("Added: " + objectSaveName + " State: " + objectState);
			}
			else {
				_sceneObjects[objectSaveName] = objectState;
			}
		}
		SceneManager.UnloadSceneAsync(_currentScene);
	}

	private IEnumerator LoadScene(string scene) {
		var loading = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        while (!loading.isDone) {
			yield return null;
        }
		_currentScene = SceneManager.GetSceneByName(scene);
		SceneManager.SetActiveScene(_currentScene);
		LoadSceneState(scene);
	}

    private void LoadSceneState(string scene) {
		var objectsContainer = GameObject.FindGameObjectWithTag(OBJECTS_CONTAINER);
		var sceneTransforms = objectsContainer.GetComponentsInChildren<Transform>(true);
		foreach (var sceneObject in sceneTransforms) {
			var objectSaveName = scene + SEPARATOR + sceneObject.gameObject.name;
            if (_sceneObjects.ContainsKey(objectSaveName)) {
				Debug.Log("Found" + objectSaveName);
                var objectState = _sceneObjects[objectSaveName];
				sceneObject.gameObject.SetActive(objectState);
            }
        }
    }

    private void OnEnable() {
	    SceneManager.sceneLoaded += CameraSizeAdjuster.ResizeCamera;
    }

    private void OnDisable() {
	    SceneManager.sceneLoaded -= CameraSizeAdjuster.ResizeCamera;
    }
}
