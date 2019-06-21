using InputModule;
using UnityEngine;
using Items;
using Inventory;

namespace Controllers {
	public class InteractionController : MBSingleton<InteractionController> {
        
        private string _itemAction;
        private string _interactionStatus;

        private UIController _uiController;
        private InventoryManager _inventoryManager;
        private InteractionStrategy _interactionStrategy;

        private void Awake() {
            _uiController = FindObjectOfType<UIController>();
            _inventoryManager = FindObjectOfType<InventoryManager>();
            if (_uiController == null || _inventoryManager == null) {
                Debug.LogWarning("UIController is null");
                return;
            }
            _interactionStrategy = new InteractionStrategy(_inventoryManager);
        }

        private void FixedUpdate() {
            var go = Raycaster.Instance.GetRaycastHit();
            _interactionStrategy.Execute(go, Input.GetMouseButtonUp(0));
            UpdateUI();
        }

        public void StartInventoryInteraction(Item slotItem) {
            _interactionStrategy.SlotItem = slotItem;
        }

        public void UpdateInventoryActionMessage() {
            var go = Raycaster.Instance.GetRaycastHit();
            _interactionStrategy.SetInventoryActionMessage(go);
            UpdateUI();
        }

        public void EndInventoryInteraction() {
            _interactionStrategy.SlotItem = null;
        }

        public void InventoryInteract() {
            var go = Raycaster.Instance.GetRaycastHit();
            _interactionStrategy.InventoryInteract(go);
        }

        private void UpdateUI() {
            _uiController[UITextType.message] = _interactionStrategy.InteractionStatus;
            _uiController[UITextType.action] = _interactionStrategy.ItemAction;
        }
    }
}
