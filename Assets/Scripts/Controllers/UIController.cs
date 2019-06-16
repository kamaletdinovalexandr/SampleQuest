using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers {
    public class UIController : MBSingleton<UIController> {
        [SerializeField] private Text Message;
        [SerializeField] private Text ItemAction;

        public string this[UITextType textType] {
            set {
                switch (textType) {
                    case UITextType.message:
                        Message.text = value;
                        break;
                    case UITextType.action:
                        ItemAction.text = value;
                        break;
                }
            }
        }

        private void Start() {
            InitText();
        }

        private void InitText() {
            Message.text = string.Empty;
            ItemAction.text = string.Empty;
        }
    }
}