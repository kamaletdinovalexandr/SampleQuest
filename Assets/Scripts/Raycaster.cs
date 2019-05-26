﻿using System;
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
		public GameObject HitObject { get; private set; }

		private void Awake() {
            _pointerEventData = new PointerEventData(_EventSystem);
        }

        public void Update() {
			HitObject = null;
			TryGetItemInGameWorld();
			TryGetItemInUI();        
        }

        private void TryGetItemInUI() {
            _pointerEventData.position = Input.mousePosition;
            var results = new List<RaycastResult>();
            _raycaster.Raycast(_pointerEventData, results);
			HitObject = results.FirstOrDefault().gameObject;
        }

        private void TryGetItemInGameWorld() {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (!hit)
				return;

			HitObject = hit.transform.gameObject;
        }
    }
}
