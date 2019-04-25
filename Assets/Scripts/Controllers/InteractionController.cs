﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Items;
using Inventory;

namespace Controllers {
    public class InteractionController : MBSingleton<InteractionController> {

        [SerializeField] private Camera _camera;
        [SerializeField] private GraphicRaycaster _raycaster;
        [SerializeField] private EventSystem _EventSystem;
        [SerializeField] private Text Message;
        [SerializeField] private Text ItemAction;
        public Item SlotItem;

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
				if (TrySlotToViewInteract(hit)) {
					return;
				}

				SetMouseOverMessage(hit);
				return;
			}

			if (TrySlotToSlotInteract()) {
				return;
			}

			//ItemAction.text = string.Empty;
        }

		private void SetMouseOverMessage(RaycastHit2D hit) {
			var itemView = hit.collider.GetComponent<ItemView>();
			if (itemView == null)
				return;

			SetMouseItemAction(itemView);
			TryTakeOrLook(itemView);
		}

        private void SetMouseItemAction(ItemView item) {
            if (item.IsTakable) {
                ItemAction.text = "Take the " + item.name;
            } 
            else {
                ItemAction.text = "Look at the " + item.name;
            }
        }

        private bool TryTakeOrLook(ItemView item) {
            if (item.IsTakable) {
                if (Input.GetMouseButtonDown(0) && TakeInteraction(item.GetItem())) {
                    item.gameObject.SetActive(false);
                    return true;
                }                  
            }
            else {
                if (Input.GetMouseButtonDown(0)) {
                    Message.text = item.Description;
                    return true;
                }
            }   
            return false; 
        }

        private bool TakeInteraction(Item item) {
            if (InventoryManager.Instance.PutItem(item)) {
                Message.text = "You took the " + item.Name;
                return true;
            }
            return false;
        }

        private bool TrySlotToSlotInteract() {
			var pointerEventData = new PointerEventData(_EventSystem);
            pointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            _raycaster.Raycast(pointerEventData, results);

            foreach (RaycastResult result in results) {                
                var otherSlot = result.gameObject.GetComponent<Slot>();
                if (otherSlot != null && otherSlot.Item != null && otherSlot.Item != SlotItem) {
                    ItemAction.text = "Use with " + otherSlot.Item.Name;
                    
					var combinedItem = new Item();
					if (Input.GetMouseButtonUp(0) && otherSlot.Item.Interact(SlotItem, out combinedItem) && InventoryManager.Instance.PutItem(combinedItem)) {
                        InventoryManager.Instance.RemoveItem(otherSlot.Item);
                        InventoryManager.Instance.RemoveItem(SlotItem);
                        if (combinedItem != null) {
                            Message.text = "You picked a " + combinedItem.Name;
                        }

                        ItemAction.text = "";
                        return true;
                    }
                }
            }
            return false;
        }

		private bool TrySlotToViewInteract(RaycastHit2D hit) {
			if (!Input.GetMouseButtonUp(0))
				return false;

			var craftedItem = new Item();
            var itemView = hit.collider.GetComponent<ItemView>();

			if (itemView == null)
				return false;

            if (itemView.Interact(SlotItem, out craftedItem)) {
				InventoryManager.Instance.RemoveItem(SlotItem);
			}

            if (!IsItemEmpty(craftedItem)) {
                InventoryManager.Instance.PutItem(craftedItem);
            }
			
			return true;
        }

        private bool IsItemEmpty(Item item) {
            return item == null ||  item.Name == "";
        }
    }
}
