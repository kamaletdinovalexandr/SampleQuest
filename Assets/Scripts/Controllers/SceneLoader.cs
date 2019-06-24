using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using CameraExtention;
using Items;

public class SceneLoader : MBSingleton<SceneLoader> {
    [SerializeField] private string StartSceneName;
	private string _currentScene;
    private Dictionary<string, bool> _sceneObjects = new Dictionary<string, bool>();
    private const string SEPARATOR = "-";

    void Start () {
		SceneManager.LoadScene(StartSceneName, LoadSceneMode.Additive);
		_currentScene = StartSceneName;
    }

    public void ChangeLocation(string location) {
        SaveSceneState();
        SceneManager.UnloadScene(_currentScene);
        StartCoroutine(LoadScene(location));
        _currentScene = location;
        LoadSceneState();
    }

    private IEnumerator LoadScene(string scene) {
        var loading = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        while (!loading.isDone) {
            yield return null;
        }
    }

    private void LoadSceneState() {
        var sceneObjects = FindObjectsOfType<ItemView>();
        foreach (var sceneObject in sceneObjects) {
            var objectSaveName = _currentScene + SEPARATOR + sceneObject.name;
            if (_sceneObjects.ContainsKey(objectSaveName)) {
                var objectState = _sceneObjects[objectSaveName];
                sceneObject.gameObject.SetActive(objectState);
            }
        }
    }

    private void SaveSceneState() {
        var sceneObjects = FindObjectsOfType<ItemView>();
        foreach(var sceneObject in sceneObjects) {
            var objectState = sceneObject.isActiveAndEnabled;
            var objectSaveName = _currentScene + SEPARATOR + sceneObject.name;
            if (!_sceneObjects.ContainsKey(objectSaveName)) {
                _sceneObjects.Add(objectSaveName, objectState);
            }
            else {
                _sceneObjects[objectSaveName] = objectState;
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
