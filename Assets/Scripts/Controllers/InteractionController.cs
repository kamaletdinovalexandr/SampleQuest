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

        private BaseItem _currentBaseItem;

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
                _currentBaseItem = go.GetComponent<BaseItem>();
                if (_currentBaseItem != null)
                    ChangeCurrentAction(_currentBaseItem.GetItem().Type);
            }  else {
                _currentBaseItem = null;
                ItemAction.text = string.Empty;
            }

            if (Input.GetMouseButtonDown(0) && _currentBaseItem != null)
                Interact(_currentBaseItem.GetItem());
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
                    UseInteraction();
                    break;
                case ObjectType.takeble:
                    TakeInteraction(item);
                    break;
            }
        }

        private void LookInteraction(string message) {
            SetMessage(message);
        }

        private void UseInteraction() {

        }

        private void TakeInteraction(Item item) {
            if (InventoryManager.Instance.PutItem(item)) {
                _currentBaseItem.gameObject.SetActive(false);
                _currentBaseItem = null;
            }
        } 
    }
}
