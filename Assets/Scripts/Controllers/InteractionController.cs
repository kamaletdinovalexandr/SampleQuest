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

        private void Awake() {
            _uiController = FindObjectOfType<UIController>();
            _inventoryManager = FindObjectOfType<InventoryManager>();
        }

        private void FixedUpdate() {
            if (IsInventoryInteraction()) {
                return;
            }

            ClearItemAction();
            var go = Raycaster.Instance.GetRaycastHit();
            if (go == null) {
                UpdateUI();
                return;
            }

            TrySetSlotActionMessage(go);
            TrySetItemViewActionMessage(go);

            if (!Input.GetMouseButtonUp(0)) {
                UpdateUI();
                return;
            }

            if (TryGetSlotItemDescription(go)) {
                UpdateUI();
                return;
            }
                
            TakeItemView(go);
            UpdateUI();
        }

        public void InventoryInteract() {
            var go = Raycaster.Instance.GetRaycastHit();
            if (go == null) {
                return;
            }

            var success = TrySlotToViewInteract(go);
            if (!success)
            	TrySlotToSlotInteract(go);
            
            UpdateUI();
        }

        private bool TryGetSlotItemDescription(GameObject go) {
            var slot = go.GetComponent<Slot>();
            if (slot != null && slot.Item != null) {
                SetInteractionStatus(slot.Item.Description);
                return true;
            }
            return false;
        }
        
        private bool IsInventoryInteraction() {
            return SlotItem != null;
        }
        
        private void TrySetSlotActionMessage(GameObject go) {
            var slot = go.GetComponent<Slot>();
            if (slot != null && !slot.IsEmpty) {
                SetItemAction("Look at the " + slot.Item.Name);
            }
        }

        private void TrySetItemViewActionMessage(GameObject go) {
            var itemView = go.GetComponent<ItemView>();
            if (itemView == null) {
                return;
            }

            string actionMessage = string.Empty;
            switch(itemView.itemType) {
                case ItemViewType.lookable:
                    actionMessage = "Look at the " + itemView.name;
                    break;
                case ItemViewType.takable:
                    actionMessage = "Take the " + itemView.name;
                    break;
                case ItemViewType.usable:
                    actionMessage = "Use the " + itemView.name;
                    break;
            }
            SetItemAction(actionMessage);
        }

        public void SetInventoryActionMessage() {
            SetItemAction("Use " + SlotItem.Name + " with ");

            var go = Raycaster.Instance.GetRaycastHit();
            if (go == null) {
                UpdateUI();
                return;
            }

            var itemView = go.GetComponent<ItemView>();
            if (itemView != null) {
                SetItemAction("Use " + SlotItem.Name + " with " + itemView.Name);
                UpdateUI();
                return;
            }
            
            var slot = go.GetComponent<Slot>();
            if (slot != null && !slot.IsEmpty && slot.Item.Name != SlotItem.Name) {
                SetItemAction("Use " + SlotItem.Name + " with " + slot.Item.Name);
            }
            
            UpdateUI();
        }
        
        private void TakeItemView(GameObject go) {
            var itemView = go.GetComponent<ItemView>();
            if (itemView == null)
                return;

            switch(itemView.itemType) {
                case ItemViewType.takable:
                    if (TryPutToInventory(itemView.Item)) {
                        itemView.gameObject.SetActive(false);
                        return;
                    }
                    break;
                case ItemViewType.usable:
                case ItemViewType.lookable:
                    itemView.Interact(new Item("", null, ""));
                    break;
            }

            SetInteractionStatus(itemView.Description);
        }

        private bool TryPutToInventory(Item item) {
            return _inventoryManager != null && _inventoryManager.PutItem(item);
        }

        private void RemoveFromInventory(Item item) {
            if (_inventoryManager != null) 
                _inventoryManager.RemoveItem(item);
        }

        private void TrySlotToSlotInteract(GameObject go) {
            if (go == null)
                return;

            var otherSlot = go.gameObject.GetComponent<Slot>();
            if (otherSlot != null && otherSlot.Item != null && otherSlot.Item != SlotItem) {
                SetItemAction("Use with " + otherSlot.Item.Name);

                if (otherSlot.Item.Interact(SlotItem)) {
                    var combinedItem = otherSlot.Item.CraftedItem;
                    RemoveFromInventory(otherSlot.Item);
                    RemoveFromInventory(SlotItem);
                    if (combinedItem != null && TryPutToInventory(combinedItem)) {
                        SetInteractionStatus("You picked a " + combinedItem.Name);
                    }
                }
            }
        }

        private bool TrySlotToViewInteract(GameObject go) {
            var item = go.GetComponent<ItemView>();
            if (item == null) {
                return false;
            }

            Debug.Log("SlotToView interaction");
            var success = item.Interact(SlotItem);

            if (!success) {
                SetInteractionStatus("Nothing happened");
                return false;
            }

            RemoveFromInventory(SlotItem);
            var craftedItem = item.CraftedItem;
            if (!IsItemEmpty(craftedItem)) {
                SetInteractionStatus("You picked a " + craftedItem.Name);
                TryPutToInventory(craftedItem);
            }
            else {
                SetInteractionStatus("It's worked!!!");
            }

            return true;
        }

        private bool IsItemEmpty(Item item) {
            return item == null || string.IsNullOrEmpty(item.Name);
        }

        private void ClearItemAction() {
            _itemAction = string.Empty;
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
