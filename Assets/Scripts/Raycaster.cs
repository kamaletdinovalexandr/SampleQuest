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
		public GameObject InteractionObject { get; private set; }

		private void Awake() {
            _pointerEventData = new PointerEventData(_EventSystem);
        }

        public void UpdateHit() {
			InteractionObject = null;
			TryGetGOInGameWorld();
			TryGetGOInUI();        
        }

        private void TryGetGOInUI() {
            _pointerEventData.position = Input.mousePosition;
            var results = new List<RaycastResult>();
            _raycaster.Raycast(_pointerEventData, results);
            if (results.Count == 0)
	            return;
            
			var hit = results.First();
			if (hit.gameObject != null) {
				InteractionObject = hit.gameObject;
			}
        }

        private void TryGetGOInGameWorld() {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (hit)
				InteractionObject = hit.transform.gameObject;
        }
    }
}
