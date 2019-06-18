using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Controllers;
using Items;

namespace Inventory {
    public class InventoryManager : MonoBehaviour, IInventoryManager {
        [SerializeField] private List<Slot> Slots;

        public void Init(List<Slot> slots) {
            Slots = slots;
        }

        public bool PutItem(Item item) {
            var emptySlot = Slots.FirstOrDefault(s => s.IsEmpty);

            if (emptySlot != null) {
                emptySlot.AddItem(item);
                Debug.Log("Item " + item.Name + " added");
                return true;
            }

            return false;
        }

        public void RemoveItem(Item item) {
            Slots.FindAll(s => s.Item == item).ForEach(s => {
                s.ClearItem();
                Debug.Log("Item " + item.Name + " removed");
            });
        }

        public bool IsInventoryContains(Item item) {
            return Slots.Any(s => s.Item == item);
        }

        public void AddSlot(Slot newSlot) {
            Slots.Add(newSlot);
        }
    }
}