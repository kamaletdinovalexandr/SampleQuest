using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;

namespace Items {
    public class ItemView : MonoBehaviour {

        [Header("Item properties")]
        public string Name;
        public string Description;
		 public ItemViewType itemType;
		 private Sprite Icon;

        [Space]
        [Header("Crafting area")]
        public string InputItemName;
        public string CraftedItemName;
        public Sprite CraftedItemIcon;
        public string CraftedItemDeskription;

		 public Item Item { get; private set; } 

        public void Awake() {
            Icon = GetComponent<SpriteRenderer>().sprite;
			  Item = new Item(Name, Icon, Description, InputItemName, CraftedItemName, CraftedItemIcon, CraftedItemDeskription);
		}

		public virtual bool Interact(Item item, out Item craftedItem) {
			if (!Item.Interact(item, out craftedItem))
				return false;

			return true;
        }  
    }
}
