using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    [SerializeField] private string StartSceneName;

	void Start () {
        SceneManager.LoadScene(StartSceneName, LoadSceneMode.Additive);
    }
}
