using System;
using InputModule;
using UnityEngine;
using Items;
using Inventory;

namespace Controllers {
    public class InteractionController : MBSingleton<InteractionController> {
        
        public Item SlotItem;

        private string _itemAction;
        private string _interactionStatus;

        private UIController _uiController;
        private InventoryManager _inventoryManager;
        private InteractionStrategy _interactionStrategy;

        private void Awake() {
            _uiController = FindObjectOfType<UIController>();
            _inventoryManager = FindObjectOfType<InventoryManager>();
            _interactionStrategy = new InteractionStrategy();
        }

        private void FixedUpdate() {
            if (IsInventoryInteraction()) {
                return;
            }
            
            var go = Raycaster.Instance.GetRaycastHit();
            _interactionStrategy.Execute(go, _inventoryManager);
            
        }

        private bool IsInventoryInteraction() {
            return SlotItem != null;
        }

        private void RemoveFromInventory(Item item) {
            if (_inventoryManager != null) 
                _inventoryManager.RemoveItem(item);
        }

        private bool IsItemEmpty(Item item) {
            return item == null || string.IsNullOrEmpty(item.Name);
        }
        
        private void ClearInteractionStatus() {
            _interactionStatus = string.Empty;
        }

        private void SetItemAction(string message) {
            _itemAction = message;
        }
        
        private void SetInteractionStatus(string message) {
            _interactionStatus = message;
        }

        private void UpdateUI() {
            if (_uiController == null) {
                Debug.LogWarning("UIController is null");
                return;
            }

            _uiController.SetMessage(_interactionStatus);
            _uiController.SetItemAction(_itemAction);
        }
    }
}
