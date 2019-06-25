using UnityEngine;

namespace Items {
	public class ScenePortal : ItemView {
		[Header("State Visual Links")]
		[SerializeField] private GameObject PortalClosed;
		[SerializeField] private GameObject PortalOpened;
		[Header("Start State")]
		[SerializeField] private PortalState PortalState = PortalState.closed;

		public string SceneToLoad;

		public bool isOpened { get { return PortalOpened.gameObject.activeInHierarchy; }
		}

		public override bool Interact(Item item) {
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
				PortalClosed.SetActive(false);
				PortalOpened.SetActive(true);
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
