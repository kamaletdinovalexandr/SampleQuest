using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	    SceneManager.sceneLoaded += CameraSizeController.ResizeCamera;
    }

    private void OnDisable() {
	    SceneManager.sceneLoaded -= CameraSizeController.ResizeCamera;
    }
}
