using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;

namespace Items {
    public class BaseItem : MonoBehaviour {

        [Header("Item properties")]
        public string Name;
        public string Description;
        public Sprite Icon;
        public bool isTakable;
        [Space]
        [Header("Crafting area")]
        public string InputItemName;
        public string CraftedItemName;
        public Sprite CraftedItemIcon;

        public void Awake() {
            Icon = GetComponent<SpriteRenderer>().sprite;
        }

        public Item GetItem() { 
            return new Item(Name, Icon, Description, isTakable);
        }

        public Item Interact(Item item) {
            if (item.Name == InputItemName) {
                return new Item(CraftedItemName, CraftedItemIcon, "", true);
            }
            return null;
        }  
    }
}
