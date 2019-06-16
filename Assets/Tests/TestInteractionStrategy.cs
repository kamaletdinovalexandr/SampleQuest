using System.Collections;
using System.Collections.Generic;
using Inventory;
using NUnit.Framework;
using UnityEngine;
using Items;

namespace Tests {
    
    public class TestInteractionStrategy {

        [Test]
        public void TakeItemView() {
            var inventoryManager = new GameObject().AddComponent<InventoryManager>();
            var slot = new GameObject().AddComponent<Slot>();
            inventoryManager.Init(new List<Slot> { slot } );
            var interactionStrategy = new InteractionStrategy(inventoryManager);
            
            var go = new GameObject();
            var view = go.AddComponent<ItemView>();
            view.Name = "Test1";
            view.itemType = ItemViewType.takable;
            view.Init();
            interactionStrategy.Execute(go, true);
            var success = inventoryManager.IsInventoryContains(view.Item);
            Assert.True(success);
        }
        
        [Test]
        public void SlotToViewInteraction() {
            var inventoryManager = new GameObject().AddComponent<InventoryManager>();
            var slot = new GameObject().AddComponent<Slot>();
            inventoryManager.Init(new List<Slot> { slot } );
            var interactionStrategy = new InteractionStrategy(inventoryManager);
            
            var inventoryItem = new Item { Name = "InventoryItem" };
            inventoryManager.PutItem(inventoryItem);
            
            var go = new GameObject();
            var view = go.AddComponent<ItemView>();
            view.InputItemName = "InventoryItem";
            view.Init();
            interactionStrategy.SlotItem = inventoryItem;
            var success = interactionStrategy.InventoryInteract(go);
            Assert.True(success);
        }

        [Test]
        public void SlotToSlotInteraction() {
            var inventoryManager = new GameObject().AddComponent<InventoryManager>();
            var slot = new GameObject().AddComponent<Slot>();
            var slot2 = new GameObject().AddComponent<Slot>();
            inventoryManager.Init(new List<Slot> { slot, slot2 } );
            var interactionStrategy = new InteractionStrategy(inventoryManager);
            
            var inventoryItem1 = new Item { Name = "InventoryItem1" };
            inventoryManager.PutItem(inventoryItem1);
            
            var inventoryItem2 = new Item { Name = "InventoryItem2", InputItemName = "InventoryItem1"};
            inventoryManager.PutItem(inventoryItem2);

            interactionStrategy.SlotItem = inventoryItem1;
            var success = interactionStrategy.InventoryInteract(slot2.gameObject);
            Assert.True(success);
        }
    }
}