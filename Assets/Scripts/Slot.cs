using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Items;
using Controllers;

namespace Inventory {
    public class Slot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {

        [SerializeField]
        private Image _icon;
        public Item Item;
        public bool IsEmpty { get { return Item == null; } }
        private Vector2 _iconPosition;

        private void Awake() {
            _icon = GetComponent<Image>();
            _icon.enabled = false;
        }

        public void AddItem(Item item) {
            Item = item;
            if (_icon == null) {
                Debug.LogWarning("Slot: Icon is null");
                return;
            }

            _icon.enabled = true;
            _icon.sprite = item.Icon;
        }

        public void ClearItem() {
            Item = null;
            if (_icon == null) {
                Debug.LogWarning("Slot: Icon is null");
                return;
            }
            
            _icon.sprite = null;
            _icon.enabled = false;
        }

        public void OnBeginDrag(PointerEventData eventData) {
            _icon.raycastTarget = false;
            _iconPosition = transform.localPosition;
            InteractionController.Instance.StartInventoryInteraction(Item);
        }

        public void OnDrag(PointerEventData eventData) {
            _icon.transform.position = Input.mousePosition;
            InteractionController.Instance.UpdateInventoryActionMessage();
        }

        public void OnEndDrag(PointerEventData eventData) {
            InteractionController.Instance.InventoryInteract();
            InteractionController.Instance.EndInventoryInteraction();
            _icon.raycastTarget = true;
            transform.localPosition = _iconPosition;
        }
    }
}
