using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items {
    public class Item {

        public string Name { get; private set; }
        public string Description { get; private set; }
        public Sprite Icon { get; private set; }
        public ObjectType Type { get; private set; }

        public Item(string name, string descriprion, Sprite icon, ObjectType type) {
            Name = name;
            Description = descriprion;
            Icon = icon;
            Type = type;
        }
    }
}
