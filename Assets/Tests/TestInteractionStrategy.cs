using System.Collections;
using System.Collections.Generic;
using Inventory;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
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
            
            interactionStrategy.Execute(go, true);
            var sucess = inventoryManager.IsInventoryContains(view.Item);
            Assert.True(sucess);
        }
        
        [Test]
        public void SlotToViewInteraction() {
            var inventoryManager = new GameObject().AddComponent<InventoryManager>();
            var slot = new GameObject().AddComponent<Slot>();
            inventoryManager.Init(new List<Slot> { slot } );
            var interactionStrategy = new InteractionStrategy(inventoryManager);
            
            var go = new GameObject();
            var view = go.AddComponent<ItemView>();
            view.Name = "Test1";
            view.InputItemName = "Test2";
            inventoryManager.PutItem(view.Item);
            var go2 = new GameObject();
            var view2 = go2.AddComponent<ItemView>();
            view2.Name = "Test2";
            
            var sucess = interactionStrategy.InventoryInteract(go2);
            Assert.True(sucess);
        }
    }
}