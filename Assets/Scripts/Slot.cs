using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Items;
using Controllers;

namespace Inventory {
    public class Slot : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {

        [SerializeField] private Image _icon;
        public Item Item { get; private set; }
        public bool IsEmpty = true;
        private Vector2 _iconPosition;

        private void Awake() {
            _icon = GetComponent<Image>();
            _icon.enabled = false;
        }

        public void AddItem(Item item) {
            Item = item;
            _icon.enabled = true;
            _icon.sprite = item.Icon;
            IsEmpty = false;
        }

        public void ClearItem() {
            Item = null;
            _icon.sprite = null;
            _icon.enabled = false;
            IsEmpty = true;
        }

        public void OnBeginDrag(PointerEventData eventData) {
            _iconPosition = transform.localPosition;
            InteractionController.Instance.InventryInteraction = true;
        }

        public void OnDrag(PointerEventData eventData) {
            _icon.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData) {
            InteractionController.Instance.InventoryInteract(Item);
            transform.localPosition = _iconPosition;
            InteractionController.Instance.InventryInteraction = false;
        }  
    }
}
