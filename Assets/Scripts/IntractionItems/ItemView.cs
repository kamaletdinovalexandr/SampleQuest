using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;

namespace Items {
    public class ItemView : MonoBehaviour {

        [Header("Item properties")]
        public string Name;
        public string Description;
        private Sprite Icon;
        public bool IsTakable;
        [Space]
        [Header("Crafting area")]
        public string InputItemName;
        public string CraftedItemName;
        public Sprite CraftedItemIcon;

        public void Awake() {
            Icon = GetComponent<SpriteRenderer>().sprite;
        }

        public Item GetItem() { 
            return new Item(Name, Icon, Description, InputItemName, CraftedItemName, CraftedItemIcon);
        }

		public virtual bool Interact(Item item, out Item craftedItem) {
			if (!GetItem().Interact(item, out craftedItem))
				return false;

			if (IsTakable) {
				gameObject.SetActive(false);
			}

			return true;
        }  
    }
}
