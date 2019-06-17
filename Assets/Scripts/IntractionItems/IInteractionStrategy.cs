using UnityEngine;

namespace Items {
    
    public interface IInteractionStrategy {
        
        Item SlotItem { set; }
        
        string ItemAction { get; }
        string InteractionStatus { get; }
        void Execute(GameObject go, bool endInteraction);
        bool InventoryInteract(GameObject go);
        void SetInventoryActionMessage(GameObject go);
    }
}