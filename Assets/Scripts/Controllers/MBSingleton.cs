using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBSingleton<T> : MonoBehaviour where T : MonoBehaviour {

	private static T _instance;

	public static T Instance { 
		get {
			if (_instance == null) {
				_instance = (T)FindObjectOfType(typeof(T));
				if (_instance == null) {
					Debug.LogError(typeof(T) + "is null");
					Application.Quit();
				}
			}
			return _instance;
		}
	}
}