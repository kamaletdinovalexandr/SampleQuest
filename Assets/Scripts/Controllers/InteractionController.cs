using System.Collections;
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

        PointerEventData _PointerEventData;

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
                if (SlotItem == null) {
                    if (TryUsePortal(hit))
                        return;

                    var itemView = hit.collider.GetComponent<ItemView>();
                    if (itemView == null)
                        return;

                    SetMouseItemAction(itemView);
                    TryTakeOrLook(itemView);
                }
                else if (Input.GetMouseButtonUp(0) && TryItemViewInteract(hit)) {
                        Debug.Log("Inventory item interacted with view");
                        return;    
                }
            }
            else if (Input.GetMouseButtonUp(0) && TrySlotInteract()) {
                Debug.Log("Inventory item interacted with slot");
                return;
            }
            else                                    
                ItemAction.text = string.Empty;
        }

        private bool TryUsePortal(RaycastHit2D hit) {
            if (!Input.GetMouseButtonDown(0))
                return false;

            var portal = hit.collider.GetComponent<ScenePortal>();
            if (portal == null)
                return false;

            return portal.TryUsePortal();
        }

        private void SetMouseItemAction(ItemView item) {
            if (item.IsTakable) {
                ItemAction.text = "Take the " + item.name;
            } 
            else {
                ItemAction.text = "Look at the " + item.name;
            }
        }

        private void SetInventryItemAction(RaycastHit2D hit) {
            var itemView = hit.collider.GetComponent<ItemView>();
            if (itemView != null) {

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

        private bool TrySlotInteract() {
            var pointerEventData = new PointerEventData(_EventSystem);
            pointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            _raycaster.Raycast(pointerEventData, results);

            foreach (RaycastResult result in results) {                
                var otherSlot = result.gameObject.GetComponent<Slot>();
                if (otherSlot != null && otherSlot.Item != null && otherSlot.Item != SlotItem) {
                    var combinedItem = otherSlot.Item.Interact(SlotItem);
                    if (combinedItem != null && InventoryManager.Instance.PutItem(combinedItem)) {
                        InventoryManager.Instance.RemoveItem(otherSlot.Item);
                        InventoryManager.Instance.RemoveItem(SlotItem);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool TryItemViewInteract(RaycastHit2D hit) {
            var itemView = hit.collider.GetComponent<ItemView>();
            if (itemView != null) {
				var craftItem = itemView.Interact(SlotItem);
				if (itemView.gameObject.GetComponent<ScenePortal>() == null)
					itemView.gameObject.SetActive(false);
				
				InventoryManager.Instance.RemoveItem(SlotItem);
                if (craftItem != null && InventoryManager.Instance.PutItem(craftItem)) {
                    return true;
                }
            }
            return false;
        }
    }
}
