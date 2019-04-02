using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Items;
using Inventory;

namespace Controllers {
    public class InteractionController : MBSingleton<InteractionController> {

        [SerializeField] Camera _camera;
        [SerializeField] private Text Message;
        [SerializeField] private Text ItemAction;
        public bool InventryInteraction;

        private BaseItem _currentHoveredItem;

        private void Start() {
            InitText();
        }

        private void InitText() {
            Message.text = string.Empty;
            ItemAction.text = string.Empty;
        }

        private void FixedUpdate() {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit) {
                var go = hit.collider.gameObject;
                _currentHoveredItem = go.GetComponent<BaseItem>();
                if (_currentHoveredItem == null) {
                    ItemAction.text = string.Empty;
                    return;
                }

                SetItemActionText();

                if (Input.GetMouseButtonDown(0) && !InventryInteraction) {
                    Message.text = string.Empty;
                    Interact(_currentHoveredItem.GetItem());
                }
            }
            else {
                _currentHoveredItem = null;
                ItemAction.text = string.Empty;
            }
        }

        private void SetItemActionText() {
            if (InventryInteraction) {
                ItemAction.text = "Use with " + _currentHoveredItem.name;
            }
            else if (_currentHoveredItem.isTakable)
                ItemAction.text = "Take the " + _currentHoveredItem.name;
            else
                ItemAction.text = "Look at " + _currentHoveredItem.name;
        }

        public void SetMessage(string text) {
            Message.text = text;
        }

        public void InventoryInteract(Item item) {
            if (_currentHoveredItem == null)
                return;

            var craftItem = _currentHoveredItem.Interact(item);
            if (craftItem != null) {
                InventoryManager.Instance.RemoveItem(item);
                InventoryManager.Instance.PutItem(craftItem);
                Message.text = "You took the " + craftItem.Name;
            }
        }

        private void Interact(Item item) {
            if (item.IsTakable) {
                TakeInteraction(item);
            }
            else
                Message.text = item.Description;
        }

        private void TakeInteraction(Item item) {
            if (InventoryManager.Instance.PutItem(item)) {
                _currentHoveredItem.gameObject.SetActive(false);
                _currentHoveredItem = null;
                Message.text = "You took the " + item.Name;
            }
        }

        public void SetSelectedItem(Item item) {
           
        }
    }
}
