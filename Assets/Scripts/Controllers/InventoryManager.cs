using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;
using Items;

namespace Inventory {
    public class InventoryManager : MBSingleton<InventoryManager> {

        [SerializeField] private List<Slot> Slots;

        public bool PutItem(Item item) {
            foreach (var slot in Slots) {
                if (slot.IsEmpty) {
                    slot.AddItem(item);
                    Debug.Log("Item " + item.Name + "added");
                    return true;
                }
            }
            //InteractionController.Instance.SetMessage("Inventory is full");
            return false;
        }

        public void RemoveItem(Item item) {
            foreach (var slot in Slots) {
                if (slot.Item == item) {
                    Debug.Log("Item " + item.Name + " removed");
                    slot.ClearItem();
                }
            }
        }

        public bool IsInventoryContains(Item item) {
            foreach (var slot in Slots) {
                if (slot.Item == item) {
                    return true;
                }
            }
            return false;
        }
    }
}
