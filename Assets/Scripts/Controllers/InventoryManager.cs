using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Controllers;
using Items;

namespace Inventory {
    public class InventoryManager : MonoBehaviour {

        [SerializeField] private List<Slot> Slots;

        public void Init(List<Slot> slots) {
            Slots = slots;
        }

        public bool PutItem(Item item) {
            var slot = Slots.First(s => s.IsEmpty);
            if (slot != null) {
                slot.AddItem(item);
                Debug.Log("Item " + item.Name + " added");
                return true;
            }
            return false;
        }

        public void RemoveItem(Item item) {
            Slots.FindAll(s => s.Item == item).ForEach(s => s.ClearItem());
            Debug.Log("Item " + item.Name + " removed");
        }

        public bool IsInventoryContains(Item item) {
            return Slots.First(s => s.Item == item);
        }
    }
}
