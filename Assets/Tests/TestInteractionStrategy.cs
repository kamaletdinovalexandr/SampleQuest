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
        public void ViewInteractionSuccess() {
            var inventoryManager = new GameObject().AddComponent<InventoryManager>();
            var slot = new GameObject().AddComponent<Slot>();
            inventoryManager.Init(new List<Slot> { slot } );
            var interactionStrategy = new InteractionStrategy(inventoryManager);
            var go = new GameObject();
            var view = go.AddComponent<ItemView>();
            view.name = "Test1";
            view.itemType = ItemViewType.takable;
            interactionStrategy.Execute(go, true);
            var sucess = inventoryManager.IsInventoryContains(view.Item);
            Assert.True(sucess);
        }
    }
}