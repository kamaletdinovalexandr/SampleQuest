using UnityEngine;

namespace Items {
	public class ScenePortal : ItemView {
		[Header("State Visual Links")]
		[SerializeField] private GameObject PortalClosed;
		[SerializeField] private GameObject PortalOpened;

		public string SceneToLoad;

		public bool IsOpened { get { return PortalOpened.activeInHierarchy; } }

		public override bool Interact(Item item) {
			if (IsOpened) {
				UsePortal();
				return true;
			}

			if (TryChangeState(item.Name)) {
				return true;
			}
			return false;
		}

		private bool TryChangeState(string itemName) {
			if (!string.IsNullOrEmpty(itemName) && itemName == InputItemName)  {
				OpenState();
				return true;
			}

			return false;
		}

		private void UsePortal() {
			SceneLoader.Instance.ChangeLocation(SceneToLoad);
		}

		private void OpenState() {
			PortalClosed.SetActive(false);
			PortalOpened.SetActive(true);
		}
	}
}
