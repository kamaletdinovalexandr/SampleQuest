using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {

    public class Item {

        public string Name;
        public string Description;
        public Sprite Icon;
        public string InputItemName;
        public string CraftedItemName;
        public Sprite CraftedItemIcon;

        public Item(string name, Sprite icon, string descriprion, string inputItem, string craftedItemName, Sprite craftedIcon) {
            Name = name;
            Description = descriprion;
            Icon = icon;
            InputItemName = inputItem;
            CraftedItemName = craftedItemName;
            CraftedItemIcon = craftedIcon;
        }

        public Item Interact(Item item) {
            return item != null && item.Name == InputItemName ? new Item(CraftedItemName, CraftedItemIcon, "", "", "", null) : null;
        }
    }
}
