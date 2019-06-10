using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using Items;

public class InteractionStrategy {
    
    public string ItemAction { get; private set; }
    public string InteractionStatus { get; private set; }
    public void Execute(GameObject go, InventoryManager inventoryManager) {
        ClearItemAction();
        
        if (go == null) {
            return;
        }

        TrySetSlotActionMessage(go);
        TrySetItemViewActionMessage(go);

        if (!Input.GetMouseButtonUp(0)) {
            return;
        }

        if (TryGetSlotItemDescription(go)) {
            return;
        }
                
        TakeItemView(go, inventoryManager);
    }
    
    public void InventoryInteract(GameObject go) {
        if (go == null) {
            return;
        }

        var success = TrySlotToViewInteract(go);
        if (!success)
            TrySlotToSlotInteract(go);
    }
    
    private bool TryGetSlotItemDescription(GameObject go) {
        var slot = go.GetComponent<Slot>();
        if (slot != null && slot.Item != null) {
            SetInteractionStatus(slot.Item.Description);
            return true;
        }
        return false;
    }
    
    public void SetInventoryActionMessage() {
        SetItemAction("Use " + SlotItem.Name + " with ");

        var go = Raycaster.Instance.GetRaycastHit();
        if (go == null) {
            UpdateUI();
            return;
        }

        var itemView = go.GetComponent<ItemView>();
        if (itemView != null) {
            SetItemAction("Use " + SlotItem.Name + " with " + itemView.Name);
            UpdateUI();
            return;
        }
            
        var slot = go.GetComponent<Slot>();
        if (slot != null && !slot.IsEmpty && slot.Item.Name != SlotItem.Name) {
            SetItemAction("Use " + SlotItem.Name + " with " + slot.Item.Name);
        }
            
        UpdateUI();
    }
    private void TrySetSlotActionMessage(GameObject go) {
        var slot = go.GetComponent<Slot>();
        if (slot != null && !slot.IsEmpty) {
            SetItemAction("Look at the " + slot.Item.Name);
        }
    }
    
    private void TrySetItemViewActionMessage(GameObject go) {
        var itemView = go.GetComponent<ItemView>();
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
    
    private void TakeItemView(GameObject go, InventoryManager inventoryManager) {
        var itemView = go.GetComponent<ItemView>();
        if (itemView == null)
            return;

        switch(itemView.itemType) {
            case ItemViewType.takable:
                if (TryPutToInventory(inventoryManager, itemView.Item)) {
                    itemView.gameObject.SetActive(false);
                    return;
                }
                break;
            case ItemViewType.usable:
            case ItemViewType.lookable:
                itemView.Interact(new Item("", null, ""));
                break;
        }

        SetInteractionStatus(itemView.Description);
    }
    
    private void TrySlotToSlotInteract(GameObject go) {
        if (go == null)
            return;

        var otherSlot = go.gameObject.GetComponent<Slot>();
        if (otherSlot != null && otherSlot.Item != null && otherSlot.Item != SlotItem) {
            SetItemAction("Use with " + otherSlot.Item.Name);

            if (otherSlot.Item.Interact(SlotItem)) {
                var combinedItem = otherSlot.Item.CraftedItem;
                RemoveFromInventory(otherSlot.Item);
                RemoveFromInventory(SlotItem);
                if (combinedItem != null && TryPutToInventory(combinedItem)) {
                    SetInteractionStatus("You picked a " + combinedItem.Name);
                }
            }
        }
    }
    
    private bool TrySlotToViewInteract(GameObject go) {
        var item = go.GetComponent<ItemView>();
        if (item == null) {
            return false;
        }

        Debug.Log("SlotToView interaction");
        var success = item.Interact(SlotItem);

        if (!success) {
            SetInteractionStatus("Nothing happened");
            return false;
        }

        RemoveFromInventory(SlotItem);
        var craftedItem = item.CraftedItem;
        if (!IsItemEmpty(craftedItem)) {
            SetInteractionStatus("You picked a " + craftedItem.Name);
            TryPutToInventory(craftedItem);
        }
        else {
            SetInteractionStatus("It's worked!!!");
        }

        return true;
    }
    
    private bool TryPutToInventory(InventoryManager inventoryManager, Item item) {
        return inventoryManager != null && inventoryManager.PutItem(item);
    }
    
    private void ClearItemAction() {
        ItemAction = string.Empty;
    }
    
    private void ClearInteractionStatus() {
        InteractionStatus = string.Empty;
    }

    private void SetItemAction(string message) {
        ItemAction = message;
    }
        
    private void SetInteractionStatus(string message) {
        InteractionStatus = message;
    }
}
