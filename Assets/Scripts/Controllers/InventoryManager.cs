using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Controllers;
using Items;

namespace Inventory {
    public class InventoryManager : MonoBehaviour, IInventoryManager {
        [SerializeField] private List<Slot> Slots;

        public bool PutItem(Item item) {
            var emptySlot = Slots.FirstOrDefault(s => s.IsEmpty);
            
            if (emptySlot == null) {
                return false;
            }

            emptySlot.AddItem(item);
            Debug.Log("Item " + item.Name + " added");
            return true;
        }

        public void RemoveItem(Item item) {
            Slots.Where(s => s.Item == item).ToList().ForEach(s => {
                s.ClearItem();
                Debug.Log("Item " + item.Name + " removed");
            });
        }

        public bool IsInventoryContains(Item item) {
            return Slots.Any(s => s.Item == item);
        }

        public void AddSlot(Slot newSlot) {
            if (Slots == null)
                Slots = new List<Slot>();

            Slots.Add(newSlot);
        }
    }
}