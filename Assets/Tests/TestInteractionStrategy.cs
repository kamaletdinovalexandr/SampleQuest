using Inventory;
using NUnit.Framework;
using UnityEngine;
using Items;

namespace Tests {
	public class TestInteractionStrategy {
        private InventoryManager _inventoryManager;
        private InteractionStrategy _interactionStrategy;


        [SetUp]
        public void Init() {
            _inventoryManager = new GameObject().AddComponent<InventoryManager>();
            var slot = new GameObject().AddComponent<Slot>();
            _inventoryManager.AddSlot(slot);
            _interactionStrategy = new InteractionStrategy(_inventoryManager);
        }

        [Test]
        public void TakeItemViewSuccess() {
			string targetItemName = "Test1";
			var go = new GameObject();
            var view = go.AddComponent<ItemView>();
            view.Name = targetItemName;
            view.itemType = ItemViewType.takable;
            view.Init();
            _interactionStrategy.Execute(go, true);
            var item = _inventoryManager.GetItem(view.Item);
			Assert.NotNull(item);
			var success = item.Name == targetItemName;
			Assert.True(success);
        }

        [Test]
        public void TakeItemViewFail() {
            var go = new GameObject();
            var view = go.AddComponent<ItemView>();
            view.Name = "Test1";
            view.Init();
            _interactionStrategy.Execute(go, true);
			var item = _inventoryManager.GetItem(view.Item);
			Assert.Null(item);
        }

        [Test]
        public void SlotToViewInteractionSuccess() {
            var inventoryItem = new Item {Name = "InventoryItem"};
            _inventoryManager.PutItem(inventoryItem);

            var go = new GameObject();
            var view = go.AddComponent<ItemView>();
            view.InputItemName = "InventoryItem";
            view.Init();
            _interactionStrategy.SlotItem = inventoryItem;
            var success = _interactionStrategy.InventoryInteract(go);
            Assert.True(success);
        }

        [Test]
        public void SlotToViewInteractionFail() {
            var inventoryItem = new Item {Name = "InventoryItem"};
            _inventoryManager.PutItem(inventoryItem);

            var go = new GameObject();
            var view = go.AddComponent<ItemView>();
            view.Init();
            _interactionStrategy.SlotItem = inventoryItem;
            var unSuccess = _interactionStrategy.InventoryInteract(go);
            Assert.False(unSuccess);
        }

        [Test]
        public void SlotToSlotInteractionSuccess() {
            var slot = new GameObject().AddComponent<Slot>();
            _inventoryManager.AddSlot(slot);

            var inventoryItem1 = new Item {Name = "InventoryItem1"};
            _inventoryManager.PutItem(inventoryItem1);
            var inventoryItem2 = new Item {Name = "InventoryItem2", InputItemName = "InventoryItem1"};
            _inventoryManager.PutItem(inventoryItem2);

            _interactionStrategy.SlotItem = inventoryItem1;
            var success = _interactionStrategy.InventoryInteract(slot.gameObject);
            Assert.True(success);
        }

        [Test]
        public void SlotToSlotInteractionFail() {
            var slot2 = new GameObject().AddComponent<Slot>();
            _inventoryManager.AddSlot(slot2);

            var inventoryItem1 = new Item {Name = "InventoryItem1"};
            _inventoryManager.PutItem(inventoryItem1);
            var inventoryItem2 = new Item {Name = "InventoryItem2"};
            _inventoryManager.PutItem(inventoryItem2);

            _interactionStrategy.SlotItem = inventoryItem1;
            var unSuccess = _interactionStrategy.InventoryInteract(slot2.gameObject);
            Assert.False(unSuccess);
        }
    }
}