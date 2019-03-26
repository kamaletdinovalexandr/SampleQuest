using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MBSingleton<InteractionController> {
    [SerializeField] Camera _camera;
   private void Update() {
       if (Input.GetMouseButtonDown(0)) {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                                             Vector2.zero);
            if (hit != null) {
                var go = hit.collider.gameObject;
                var interacttiveObject = go.GetComponent<InteractiveObject>();
                interacttiveObject.Interact();
            }
        }
    }
}
