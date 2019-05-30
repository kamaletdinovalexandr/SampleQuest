using UnityEngine;

namespace Items {
	public class ScenePortal : ItemView {

		private PortalState PortalState = PortalState.closed;
		public string SceneToLoad;

		public bool isOpened { get { return PortalState == PortalState.open; }
		}

		public override bool Interact(Item item, out Item craftedItem) {
			craftedItem = new Item();
			if (isOpened) {
				UsePortal();
				return true;
			}

			if (TryOpenPortal(item.Name)) 
				return true;

			return false;
		}

		private bool TryOpenPortal(string itemName) {
			if (PortalState == PortalState.closed && !string.IsNullOrEmpty(itemName) && itemName == InputItemName)  {
				PortalState = PortalState.open;
				var sr = GetComponent<SpriteRenderer>();
				sr.sprite = CraftedItemIcon;
				return true;
			}

			return false;
		}

		private void UsePortal() {
			if (PortalState == PortalState.open) {
				SceneLoader.Instance.ChangeLocation(SceneToLoad);
			}
		}
	}
}
