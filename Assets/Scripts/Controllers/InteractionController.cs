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

        private BaseItem _currentHoveredItem;
        private Item _currentSelectedItem;

        private void Start() {
            InitText();
        }

        private void InitText() {
            Message.text = string.Empty;
            ItemAction.text = string.Empty;
        }

        private void Update() {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit) {
                var go = hit.collider.gameObject;
                _currentHoveredItem = go.GetComponent<BaseItem>();
                var item = _currentHoveredItem.GetItem();
                if (_currentSelectedItem != null)
                    ItemAction.text = "Interact " + _currentSelectedItem.Name + "with " + item.Name;
                else if (_currentHoveredItem != null)
                        ChangeCurrentAction(item.Type);
            }  else {
                _currentHoveredItem = null;
                return;
            }

            if (Input.GetMouseButtonDown(0)) {
                Message.text = string.Empty;
                ItemAction.text = string.Empty;
                Interact(_currentHoveredItem.GetItem());
            }
        }

        public void SetMessage(string text) {
            Message.text = text;
        }

        private void ChangeCurrentAction(ObjectType type) {
            string objectActionText = string.Empty;
            switch (type) {
                case ObjectType.descriptable:
                    objectActionText = "Look At";
                    break;
                case ObjectType.interactable:
                    objectActionText = "Use";
                    break;
                case ObjectType.takeble:
                    objectActionText = "Take";
                    break;
            }

            ItemAction.text = objectActionText;
        }

        private void Interact(Item item) {
            switch (item.Type) {
                case ObjectType.descriptable:
                    LookInteraction(item.Description);
                    break;
                case ObjectType.interactable:
                    UseInteraction(item);
                    break;
                case ObjectType.takeble:
                    TakeInteraction(item);
                    break;
            }
        }

        private void LookInteraction(string message) {
            SetMessage(message);
        }

        private void UseInteraction(Item item) {
            if (_currentSelectedItem == null) {
                Message.text = "Nothing to use with";
            }
               


        }

        private void TakeInteraction(Item item) {
            if (InventoryManager.Instance.PutItem(item)) {
                _currentHoveredItem.gameObject.SetActive(false);
                _currentHoveredItem = null;
            }
        }

        public void SetSelectedItem(Item item) {
            _currentSelectedItem = item;
            ItemAction.text = item.Name + " was selected";
        }
    }
}
