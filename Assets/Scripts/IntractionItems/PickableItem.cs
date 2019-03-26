using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PickableItem", menuName = "items/PickableItem")]
public class PickableItem : Item {

    public override void Interact() {
        InventoryManager.Instance.PutItem(this);
    }
}
