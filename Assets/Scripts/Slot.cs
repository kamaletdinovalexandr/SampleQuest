﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Items;
using Controllers;

namespace Inventory {
    public class Slot : MonoBehaviour {

        [SerializeField] private Image _icon;
        private Button _button;
        public Item Item { get; private set; }
        public bool IsEmpty = true;

        private void Awake() {
            _button = GetComponent<Button>();
            _icon = GetComponent<Image>();
            _button.onClick.AddListener(OnItemSelect);
        }

        public void AddItem(Item item) {
            Item = item;
            _icon.sprite = item.Icon;
            _button.interactable = true;
            IsEmpty = false;
        }

        public void ClearItem() {
            Item = null;
            _icon = null;
            _button.interactable = false;
            IsEmpty = true;
        }

        private void OnItemSelect() {
            InteractionController.Instance.SetSelectedItem(Item);
        }

        private void OnDestroy() {
            _button.onClick.RemoveAllListeners();
        }
    }
}
