using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers {
    
    public class UIController :  MBSingleton<UIController> {
        [SerializeField] private Text Message;
        [SerializeField] private Text ItemAction;
        
        private void Start() {
            InitText();
        }
        private void InitText() {
            ClearAction();
            ClearMessage();
        }

        public void ClearAction() {
            ItemAction.text = string.Empty;
        }
        
        public void ClearMessage() {
            Message.text = string.Empty;
        }

        public void SetMessage(string message) {
            Message.text = message;
        }
        
        public void SetItemAction(string itemAction) {
            ItemAction.text = itemAction;
        }
    }
}

