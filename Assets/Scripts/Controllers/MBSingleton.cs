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
					var go = new GameObject();
					_instance = go.AddComponent<T>();
				}
			}
			return _instance;
		}
	}
}