using System;
using System.Collections;
using System.Collections.Generic;
using InputModule;
using UnityEngine;

using Items;
using Inventory;

namespace Controllers {
    public class InteractionController : MBSingleton<InteractionController> {
        
        public Item SlotItem;
        
        private void FixedUpdate() {
            ItemView itemView = null;
            var hit = Raycaster.Instance.Hit;
            if (hit) {
                itemView = hit.collider.GetComponent<ItemView>();
            }
            
            if (itemView != null && TryUsePortal(itemView))
                return;

            if (IsInventoryInteraction()) {
                if (itemView != null && TrySlotToViewInteract(itemView)) {
                    return;
                }

                if (TrySlotToSlotInteract()) {
                    return;
                }
            }
            
            if (!IsInventoryInteraction()) {
                if (itemView != null) {
                    SetMouseItemAction(itemView);
                    TryTakeOrLook(itemView);
                    return;
                }

                if (TryGetSlotItemDescription()) {
                    return;
                }

                if (itemView == null)
                    UIController.Instance.ClearAction();
            }
        }

        private bool TryUsePortal(ItemView item) {
            Item craftedItem;
            if (Input.GetMouseButtonDown(0) && item is ScenePortal) {
                if (item.Interact(new Item(), out craftedItem)) {
                    UIController.Instance.ClearAction();
                    UIController.Instance.ClearMessage();
                    return true;
                }
            }
            return false;
        }

        private bool TryGetSlotItemDescription() {
            var hitUI = Raycaster.Instance.HitUI;
            if (hitUI.gameObject == null)
                return false;
            
            var slot = hitUI.gameObject.GetComponent<Slot>();
            if (slot != null && slot.Item != null) {
                UIController.Instance.SetItemAction("Look at " + slot.Item.Name);

                if (Input.GetMouseButtonDown(0)) {
                    UIController.Instance.SetMessage(slot.Item.Description);
                    UIController.Instance.ClearAction();
                }
                return true;
            }
            return false;
        }
        
        private bool IsInventoryInteraction() {
            return SlotItem != null;
        }
        
        private void SetMouseItemAction(ItemView item) {
            string message;
            if (item.IsTakable) {
                message = "Take the " + item.name;
            } 
            else {
                if (item is ScenePortal) 
                    message = "Use door";
                else 
                    message = "Look at the " + item.name;
            }
            UIController.Instance.SetItemAction(message);
        }

        private bool TryTakeOrLook(ItemView item) {
            if (!Input.GetMouseButtonDown(0))
                return false;
            
            if (item.IsTakable) {
                if (TakeInteraction(item.GetItem())) {
                    item.gameObject.SetActive(false);
                    UIController.Instance.ClearAction();
                    return true;
                }

                return false;
            }
            UIController.Instance.SetMessage(item.Description);
            return true;
        }

        private bool TakeInteraction(Item item) {
            if (InventoryManager.Instance.PutItem(item)) {
                UIController.Instance.SetMessage("You took the " + item.Name);
                return true;
            }
            return false;
        }

        private bool TrySlotToSlotInteract() {
            var hitUI = Raycaster.Instance.HitUI;
            if (hitUI.gameObject == null)
                return false; 
            
            var otherSlot = hitUI.gameObject.GetComponent<Slot>();
            if (otherSlot != null && otherSlot.Item != null && otherSlot.Item != SlotItem) {
                UIController.Instance.SetItemAction("Use with " + otherSlot.Item.Name);
                
				var combinedItem = new Item();
				if (Input.GetMouseButtonUp(0) && otherSlot.Item.Interact(SlotItem, out combinedItem) && InventoryManager.Instance.PutItem(combinedItem)) {
                    InventoryManager.Instance.RemoveItem(otherSlot.Item);
                    InventoryManager.Instance.RemoveItem(SlotItem);
                    if (combinedItem != null) {
                        UIController.Instance.SetMessage("You picked a " + combinedItem.Name);
                    }

                    UIController.Instance.ClearAction();
                    return true;
                }
            }
            
            return false;
        }

		private bool TrySlotToViewInteract(ItemView item) {
            UIController.Instance.SetItemAction("Use " + SlotItem.Name + " with " + item.Name);
            
			if (!Input.GetMouseButtonUp(0))
				return false;

			var craftedItem = new Item();
            if (item.Interact(SlotItem, out craftedItem)) {
				InventoryManager.Instance.RemoveItem(SlotItem);
			}

            if (!IsItemEmpty(craftedItem)) {
                UIController.Instance.SetMessage("You picked a " + craftedItem.Name);
                InventoryManager.Instance.PutItem(craftedItem);
            }
            else {
                UIController.Instance.ClearMessage();
            }
            UIController.Instance.ClearAction();
            return true;
        }

        private bool IsItemEmpty(Item item) {
            return item == null ||  item.Name == String.Empty;
        }
    }
}
