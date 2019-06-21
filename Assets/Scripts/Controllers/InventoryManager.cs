using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Items;

namespace Inventory {
	public class InventoryManager : MonoBehaviour, IInventory {
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

		public Item GetItem(Item item) {
			Item slotItem = null;
			var slot = Slots.FirstOrDefault(s => s.Item == item);
			if (slot != null) {
				slotItem = slot.Item;
				slot.ClearItem();
			}
			return slotItem;
		}

		public void AddSlot(Slot newSlot) {
			if (Slots == null)
				Slots = new List<Slot>();

			Slots.Add(newSlot);
		}
	}
}