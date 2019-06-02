using System;
using InputModule;
using UnityEngine;
using Items;
using Inventory;

namespace Controllers {
    public class InteractionController : MBSingleton<InteractionController> {
        
        public Item SlotItem;

        private void FixedUpdate() {
            Raycaster.Instance.UpdateHit();
            
            if (IsInventoryInteraction()) {
                return;
            }

            if (!HasRaycastItem()) {
                ClearItemAction();
                return;
            }

            SetItemActionMessage();

            if (!Input.GetMouseButtonUp(0)) {
                return;
            }
            
            if (TryGetSlotItemDescription())
                return;
                
            TryTakeItemView();
        }

        public void InventoryInteract() {
            if (!HasRaycastItem())
                return;
            
            var success = TrySlotToViewInteract();
            if (!success)
            	TrySlotToSlotInteract();
        }
        
        private bool HasRaycastItem() {
            return Raycaster.Instance.InteractionObject != null;
        }

        private bool TryGetSlotItemDescription() {
            var slot = Raycaster.Instance.InteractionObject.GetComponent<Slot>();
            if (slot != null && slot.Item != null) {
                SetInteractionStatus(slot.Item.Description);
                ClearItemAction();
                
                return true;
            }
            return false;
        }
        
        private bool IsInventoryInteraction() {
            return SlotItem != null;
        }
        
        private void SetItemActionMessage() {
            var slot = Raycaster.Instance.InteractionObject.GetComponent<Slot>();
            if (slot != null && !slot.IsEmpty) {
                SetItemAction("Look at the " + slot.Item.Name);
                return;
            }

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
        
        private void TryTakeItemView() {
            var itemView = Raycaster.Instance.InteractionObject.GetComponent<ItemView>();
            if (itemView == null)
                return;
            
            if (itemView.itemType == ItemViewType.takable) {
                if (TakeInteraction(itemView.Item)) {
                    itemView.gameObject.SetActive(false);
                    ClearItemAction();
                    return;
                }
            }
            else {
                var craftedItem = new Item();
                itemView.Interact(new Item(), out craftedItem);
            }
            SetInteractionStatus(itemView.Description);
        }

        private bool TakeInteraction(Item item) {
            if (InventoryManager.Instance.PutItem(item)) {
                SetInteractionStatus("You took the " + item.Name);
                return true;
            }
            return false;
        }

        private void TrySlotToSlotInteract() {
            var go = Raycaster.Instance.InteractionObject;
            if (go == null)
                return; 
            
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
                }
            }
        }

		private bool TrySlotToViewInteract() {
           var item = Raycaster.Instance.InteractionObject.GetComponent<ItemView>();
           if (item == null) {
               ClearItemAction();
               return false;
            }

           Debug.Log("SlotToView interaction");
           var craftedItem = new Item();
		    var success = item.Interact(SlotItem, out craftedItem);

			if (!success) {
				SetInteractionStatus("Nothing happened");
				return false;
			}

			InventoryManager.Instance.RemoveItem(SlotItem);
			if (!IsItemEmpty(craftedItem)) {
              SetInteractionStatus("You picked a " + craftedItem.Name);
              InventoryManager.Instance.PutItem(craftedItem);
          } else {
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
