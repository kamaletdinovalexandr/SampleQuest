using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraSizeController : MonoBehaviour {

  private static float _resizeStep = 0.1f;

  private void Start() {
    ResizeCamera();
  }

  public static void ResizeCamera(Scene scene, LoadSceneMode mode) {
    ResizeCamera(); 
  }

  public static void ResizeCamera() {
    var items = FindObjectsOfType<ItemView>();
    if (items == null)
      return;

    foreach (var item in items) {
      while (!IsInCameraView(item.transform.position)) {
        Camera.main.orthographicSize += _resizeStep;
      }
    }
  }

  private static bool IsInCameraView(Vector3 position) {
    var cameraViewPosition = Camera.main.WorldToViewportPoint(position);
    return cameraViewPosition.x >= 0 
           && cameraViewPosition.x <= 1 
           && cameraViewPosition.y >= 0 
           && cameraViewPosition.y <= 1;
  }
}
