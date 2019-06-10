using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Items;
using Inventory;

namespace InputModule {
    public class Raycaster : MBSingleton<Raycaster> {
        
        [SerializeField] private GraphicRaycaster _raycaster;
        [SerializeField] private EventSystem _EventSystem;
        
        private PointerEventData _pointerEventData;
		
		private void Awake() {
            _pointerEventData = new PointerEventData(_EventSystem);
        }

        public GameObject GetRaycastHit() {
			var go = TryGetGOInGameWorld();
			return go != null ? go : TryGetGOInUI();
        }

        private GameObject TryGetGOInUI() {
            _pointerEventData.position = Input.mousePosition;
            var results = new List<RaycastResult>();
            _raycaster.Raycast(_pointerEventData, results);
            if (results.Count == 0)
	            return null;
            
			return results.First().gameObject;
        }

        private GameObject TryGetGOInGameWorld() {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			return hit == true ? hit.transform.gameObject : null;
        }
    }
}
