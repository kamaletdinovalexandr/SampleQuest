using Items;

namespace Inventory {
	public interface IInventory {
		bool PutItem(Item item);
		Item GetItem(Item item);
	}
}