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

        [Space] [Header("Crafting area")]

        public string InputItemName;
        public string CraftedItemName;
        public Sprite CraftedItemIcon;
        public string CraftedItemDeskription;

        public Item Item;
    
        public Item CraftedItem {
            get { return Item.CraftedItem; }
        }

        public void Awake() {
            Init();
        }

        public virtual void Init() {
            var sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            	Icon = sr.sprite;
            
            Item = new Item(Name,
                Icon, 
                Description, 
                InputItemName, 
                CraftedItemName, 
                CraftedItemIcon, 
                CraftedItemDeskription );
        }

        public virtual bool Interact(Item item) {
            if (!Item.Interact(item))
                return false;

            return true;
        }
    }
}