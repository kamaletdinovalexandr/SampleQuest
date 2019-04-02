using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MBSingleton<SceneLoader> {
    [SerializeField] private string StartSceneName;

	void Start () {
        ChangeLocation(StartSceneName);
    }

    public void ChangeLocation(string location) {
        SceneManager.LoadScene(location, LoadSceneMode.Additive);
    }

    public void UnloadScene(string location) {
        SceneManager.UnloadScene(location);
    }
}
