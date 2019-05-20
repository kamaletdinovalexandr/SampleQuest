using UnityEngine;
using UnityEngine.SceneManagement;
using CameraExtention;

public class SceneLoader : MBSingleton<SceneLoader> {
    [SerializeField] private string StartSceneName;
	private string _currentScene;

	void Start () {
		SceneManager.LoadScene(StartSceneName, LoadSceneMode.Additive);
		_currentScene = StartSceneName;
    }

    public void ChangeLocation(string location) {
		SceneManager.UnloadSceneAsync(_currentScene);
		SceneManager.LoadScene(location, LoadSceneMode.Additive);
		_currentScene = location;
    }

    private void OnEnable() {
	    SceneManager.sceneLoaded += CameraSizeAdjuster.ResizeCamera;
    }

    private void OnDisable() {
	    SceneManager.sceneLoaded -= CameraSizeAdjuster.ResizeCamera;
    }
}
