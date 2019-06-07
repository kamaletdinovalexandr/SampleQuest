using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Items;

namespace Tests {
    public class TestItem {
        
        [Test]
        public void ItemInteractionSuccess() {
           var itemA = new Item("a", null, "itemA", "b", "", null, "");
           var itemB = new Item("b", null, "itemB", "", "", null, "");
           Assert.True(itemA.Interact(itemB));
        }
        
        public void ItemInteractionUnSuccess() {
            var itemA = new Item("a", null, "itemA", "b", "", null, "");
            var itemC = new Item("c", null, "itemC", "", "", null, "");
            Assert.True(itemA.Interact(itemC));
        }
    }
}