using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;

namespace Items {
    public class BaseItem : MonoBehaviour {

        [SerializeField] private string Name;
        [SerializeField] private string Description;
        [SerializeField] private ObjectType Type;
        private Sprite _icon;

        private void Awake() {
            _icon = GetComponent<SpriteRenderer>().sprite;
        }

        public Item GetItem() {
            return new Item(Name, Description, _icon, Type);
        }
    }
}
