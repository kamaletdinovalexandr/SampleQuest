using Items;

namespace Inventory {
    public interface IInventoryManager {
        bool PutItem(Item item);
        void RemoveItem(Item item);
        bool IsInventoryContains(Item item);
    }
}