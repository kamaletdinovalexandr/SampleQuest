﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MBSingleton<InventoryManager> {

	[SerializeField] private List<Slot> Slots;

    public bool PutItem(Item item) {
        foreach (var slot in Slots) {
            if (slot.IsEmpty) {
                slot.AddItem(item);
                return true;
            }
        }
        InteractionMessage.Instance.SetInteractionMessage("Inventory is full");
        return false;
    }

    public void RemoveItem(Item item) {
        foreach (var slot in Slots) {
            if (slot.Item == item) {
                slot.ClearItem();
            }
        }
    }
}