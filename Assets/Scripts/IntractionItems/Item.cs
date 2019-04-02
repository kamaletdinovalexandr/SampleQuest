using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {
    public class Item {

        public string Name;
        public string Description;
        public Sprite Icon;
        public bool IsTakable;

        public Item(string name, Sprite icon, string descriprion, bool isTakable) {
            Name = name;
            Description = descriprion;
            Icon = icon;
            IsTakable = isTakable;
        }
    }
}
