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
        
        private void Update() {
			if (Raycaster.Instance.HitObject == null) {
				UIController.Instance.SetItemAction(string.Empty);
				return;
			}

			SetMouseItemAction(Raycaster.Instance.HitObject);

			if (Input.GetMouseButtonUp(0)) {

				if (TryUsePortal(Raycaster.Instance.HitObject))
					return;

				if (IsInventoryInteraction())
					TrySlotToSlotInteract(Raycaster.Instance.HitObject);
				else 
					TrySlotToViewInteract(Raycaster.Instance.HitObject);

				return;
			}
		}

        private bool TryUsePortal(GameObject go) {
			var portal = go.GetComponent<ScenePortal>();
			if (portal == null)
				return false;
                if (portal.TryChangeLocation(string.Empty)) {
                    UIController.Instance.ClearAction();
                    UIController.Instance.ClearMessage();
                    return true;
                }
            
            return false;
        }

        private bool TryGetSlotItemDescription(GameObject go) {
        	var slot = go.GetComponent<Slot>();
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
        
        private void SetMouseItemAction(GameObject go) {
			var portal = go.GetComponent<ScenePortal>();
			if (portal != null) {
				UIController.Instance.SetItemAction("Use door");
				return;
			}

			var item = go.GetComponent<ItemView>();
			if (item != null) {
				var message = item.IsTakable ? "Take the " : "Look at the ";
				UIController.Instance.SetItemAction(message + item.name);
            } 
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

        private bool TrySlotToSlotInteract(GameObject go) {
			var otherSlot = go.gameObject.GetComponent<Slot>();
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

		private bool TrySlotToViewInteract(GameObject go) {
			var item = go.GetComponent<ItemView>();
            UIController.Instance.SetItemAction("Use " + SlotItem.Name + " with " + item.Name);

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
