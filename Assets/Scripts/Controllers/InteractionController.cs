using System;
using InputModule;
using UnityEngine;
using Items;
using Inventory;

namespace Controllers {
    public class InteractionController : MBSingleton<InteractionController> {
        
        public Item SlotItem;
        
        private void FixedUpdate() {
            if (Raycaster.Instance.HitObject == null) {
                ClearItemAction();
                return;
            }
            
            SetMouseOverAction();

            if (!Input.GetMouseButtonUp(0))
                return;

            if (TryUsePortal()) {
                ClearInteractionStatus();
                return;
            } 
            
            if (IsInventoryInteraction()) {
                TryOpenPortal();
                TrySlotToViewInteract();
                TrySlotToSlotInteract();
            } else {
                TryGetSlotItemDescription(); 
                TryTakeOrLook();
            }
        }

        private void TryOpenPortal() {
            var portal = Raycaster.Instance.HitObject.GetComponent<ScenePortal>();
            if (portal == null || portal.isOpened) 
                return;
            
            portal.TryOpenPortal(SlotItem.Name);
        }

        private bool TryUsePortal() {
            var portal = Raycaster.Instance.HitObject.GetComponent<ScenePortal>();
            if (portal != null && portal.isOpened) {
                portal.UsePortal();
                return true;
                
            }
            return false;
        }

        private bool TryGetSlotItemDescription() {
            var slot = Raycaster.Instance.HitObject.GetComponent<Slot>();
            if (slot != null && slot.Item != null) {
                SetItemAction("Look at " + slot.Item.Name);
                SetInteractionStatus(slot.Item.Description);
                ClearItemAction();
                
                return true;
            }
            return false;
        }
        
        private bool IsInventoryInteraction() {
            return SlotItem != null;
        }
        
        private void SetMouseOverAction() {
            var portal = Raycaster.Instance.HitObject.GetComponent<ScenePortal>();
            if (portal != null) {
                SetItemAction("Use door");
                return;
            }
            
            var slot = Raycaster.Instance.HitObject.GetComponent<Slot>();
            if (slot != null && !slot.IsEmpty) {
                SetItemAction("Look at the " + slot.Item.Name);
                return;
            }

            var item = Raycaster.Instance.HitObject.GetComponent<ItemView>();
            if (item == null) {
                return;
            }
            
            if (item.IsTakable) {
                SetItemAction("Take the " + item.name);
            } 
            else {
                SetItemAction("Look at the " + item.name);
            }
        }

        private bool TryTakeOrLook() {
            var item = Raycaster.Instance.HitObject.GetComponent<ItemView>();
            if (item == null)
                return false;
            
            if (item.IsTakable) {
                if (TakeInteraction(item.GetItem())) {
                    item.gameObject.SetActive(false);
                    ClearItemAction();
                    return true;
                }

                return false;
            }
            SetInteractionStatus(item.Description);
            return true;
        }

        private bool TakeInteraction(Item item) {
            if (InventoryManager.Instance.PutItem(item)) {
                SetInteractionStatus("You took the " + item.Name);
                return true;
            }
            return false;
        }

        private bool TrySlotToSlotInteract() {
            var go = Raycaster.Instance.HitObject;
            if (go == null)
                return false; 
            
            var otherSlot = go.gameObject.GetComponent<Slot>();
            if (otherSlot != null && otherSlot.Item != null && otherSlot.Item != SlotItem) {
                SetItemAction("Use with " + otherSlot.Item.Name);
                
				var combinedItem = new Item();
				if (Input.GetMouseButtonUp(0) && otherSlot.Item.Interact(SlotItem, out combinedItem) && InventoryManager.Instance.PutItem(combinedItem)) {
                    InventoryManager.Instance.RemoveItem(otherSlot.Item);
                    InventoryManager.Instance.RemoveItem(SlotItem);
                    if (combinedItem != null) {
                        SetInteractionStatus("You picked a " + combinedItem.Name);
                    }

                    ClearItemAction();
                    return true;
                }
            }
            
            return false;
        }

		private bool TrySlotToViewInteract() {
            var item = Raycaster.Instance.HitObject.GetComponent<ItemView>();
            if (item == null)
                return false;
            
            SetItemAction("Use " + SlotItem.Name + " with " + item.Name);

            var craftedItem = new Item();
            if (item.Interact(SlotItem, out craftedItem)) {
				InventoryManager.Instance.RemoveItem(SlotItem);
			}

            if (!IsItemEmpty(craftedItem)) {
                SetInteractionStatus("You picked a " + craftedItem.Name);
                InventoryManager.Instance.PutItem(craftedItem);
            }
            else {
                ClearInteractionStatus();
            }
            ClearItemAction();
            return true;
        }

        private bool IsItemEmpty(Item item) {
            return item == null ||  item.Name == String.Empty;
        }

        private void ClearItemAction() {
            UIController.Instance.SetItemAction(String.Empty);
        }

        private void ClearInteractionStatus() {
            UIController.Instance.SetMessage(String.Empty);
        }

        private void SetItemAction(string message) {
            UIController.Instance.SetItemAction(message);
        }
        
        private void SetInteractionStatus(string message) {
            UIController.Instance.SetMessage(message);
        }
        
        
    }
}
