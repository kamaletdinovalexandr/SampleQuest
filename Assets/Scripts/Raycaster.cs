using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace InputModule {
    public class Raycaster : MBSingleton<Raycaster> {
        
        [SerializeField] private GraphicRaycaster _raycaster;
        [SerializeField] private EventSystem _EventSystem;
        
        public RaycastHit2D Hit { get; private set; }
        public RaycastResult HitUI { get; private set; }
        private PointerEventData _pointerEventData;

        private void Awake() {
            _pointerEventData = new PointerEventData(_EventSystem);
        }

        public void Update() {
            Hit = GetPhisicHit();
            HitUI = GetUIHit();
        }

        private RaycastResult GetUIHit() {
            _pointerEventData.position = Input.mousePosition;
            var results = new List<RaycastResult>();
            _raycaster.Raycast(_pointerEventData, results);
            return results.FirstOrDefault();
        }

        private static RaycastHit2D GetPhisicHit() {
            return Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        }
    }
}
