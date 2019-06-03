using System;
using InputModule;
using UnityEngine;
using Items;
using Inventory;

namespace Controllers {
    public class InteractionController : MBSingleton<InteractionController> {
        
        public Item SlotItem;

        private void FixedUpdate() {
            if (IsInventoryInteraction()) {
                return;
            }

            ClearItemAction();
            if (!HasRaycastItem()) {
                return;
            }

            TrySetSlotActionMessage();
            TrySetItemViewActionMessage();

            if (!Input.GetMouseButtonUp(0)) {
                return;
            }
            
            if (TryGetSlotItemDescription())
                return;
                
            TakeItemView();
        }

        public void InventoryInteract() {
            if (!HasRaycastItem())
                return;
            
            var success = TrySlotToViewInteract();
            if (!success)
            	TrySlotToSlotInteract();
        }
        
        private bool HasRaycastItem() {
            Raycaster.Instance.UpdateHit();
            return Raycaster.Instance.InteractionObject != null;
        }

        private bool TryGetSlotItemDescription() {
            var slot = Raycaster.Instance.InteractionObject.GetComponent<Slot>();
            if (slot != null && slot.Item != null) {
                SetInteractionStatus(slot.Item.Description);
                return true;
            }
            return false;
        }
        
        private bool IsInventoryInteraction() {
            return SlotItem != null;
        }
        
        private void TrySetSlotActionMessage() {
            var slot = Raycaster.Instance.InteractionObject.GetComponent<Slot>();
            if (slot != null && !slot.IsEmpty) {
                SetItemAction("Look at the " + slot.Item.Name);
            }
        }

        private void TrySetItemViewActionMessage() {
            var itemView = Raycaster.Instance.InteractionObject.GetComponent<ItemView>();
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
            
            if (!HasRaycastItem()) 
                return;

            var itemView = Raycaster.Instance.InteractionObject.GetComponent<ItemView>();
            if (itemView != null) {
                SetItemAction("Use " + SlotItem.Name + " with " + itemView.Name);
                return;
            }
            
            var slot = Raycaster.Instance.InteractionObject.GetComponent<Slot>();
            if (slot != null && !slot.IsEmpty && slot.Item.Name != SlotItem.Name) {
                SetItemAction("Use " + SlotItem.Name + " with " + slot.Item.Name);
            }
        }
        
        private void TakeItemView() {
            var itemView = Raycaster.Instance.InteractionObject.GetComponent<ItemView>();
            if (itemView == null)
                return;
            
            
            switch(itemView.itemType) {
                case ItemViewType.takable:
                    if (TakeInteraction(itemView.Item)) {
                        itemView.gameObject.SetActive(false);
                        ClearItemAction();
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
            return InventoryManager.Instance.PutItem(item);
        }

        private void TrySlotToSlotInteract() {
            var go = Raycaster.Instance.InteractionObject;
            if (go == null)
                return;

            var otherSlot = go.gameObject.GetComponent<Slot>();
            if (otherSlot != null && otherSlot.Item != null && otherSlot.Item != SlotItem) {
                SetItemAction("Use with " + otherSlot.Item.Name);

                if (otherSlot.Item.Interact(SlotItem)) {
                    var combinedItem = otherSlot.Item.CraftedItem;
                    InventoryManager.Instance.RemoveItem(otherSlot.Item);
                    InventoryManager.Instance.RemoveItem(SlotItem);
                    if (combinedItem != null && InventoryManager.Instance.PutItem(combinedItem)) {
                        SetInteractionStatus("You picked a " + combinedItem.Name);
                    }
                }
            }
        }

        private bool TrySlotToViewInteract() {
            var item = Raycaster.Instance.InteractionObject.GetComponent<ItemView>();
            if (item == null) {
                return false;
            }

            Debug.Log("SlotToView interaction");
            var success = item.Interact(SlotItem);

            if (!success) {
                SetInteractionStatus("Nothing happened");
                return false;
            }

            InventoryManager.Instance.RemoveItem(SlotItem);
            var craftedItem = item.CraftedItem;
            if (!IsItemEmpty(craftedItem)) {
                SetInteractionStatus("You picked a " + craftedItem.Name);
                InventoryManager.Instance.PutItem(craftedItem);
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
            UIController.Instance.SetItemAction(string.Empty);
        }

        private void ClearInteractionStatus() {
            UIController.Instance.SetMessage(string.Empty);
        }

        private void SetItemAction(string message) {
            UIController.Instance.SetItemAction(message);
        }
        
        private void SetInteractionStatus(string message) {
            UIController.Instance.SetMessage(message);
        }
        
        
    }
}
