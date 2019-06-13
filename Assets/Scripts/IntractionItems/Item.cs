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
        public string CraftedItemDescription;

        public Item() { }

        public Item CraftedItem { get; private set; }

        public Item(string name, 
					Sprite icon, 
					string descriprion, 
					string inputItem = "", 
					string craftedItemName = "", 
					Sprite craftedIcon = null, 
					string craftedItemDescription = "") {
            Name = name;
            Description = descriprion;
            Icon = icon;
            InputItemName = inputItem;
            CraftedItemName = craftedItemName;
            CraftedItemIcon = craftedIcon;
            CraftedItemDescription = craftedItemDescription;
		}

		public bool Interact(Item item) {
			if (item == null || item.Name != InputItemName) {
				return false;
			}

			CraftedItem = new Item(CraftedItemName, CraftedItemIcon, CraftedItemDescription);
			return true;
		}
    }
}
