using UnityEngine;

namespace Items {
	public class ScenePortal : MonoBehaviour {

		private PortalState PortalState = PortalState.closed;
		public string InputItemName;
		public string SceneToLoad;

		public bool isOpened { get { return PortalState == PortalState.open; }
		}
		public bool TryOpenPortal(string itemName) {
			if (PortalState == PortalState.closed && !string.IsNullOrEmpty(itemName) && itemName == InputItemName)  {
				PortalState = PortalState.open;
				return true;
			}
		
			return false;
		}

		public void UsePortal() {
			if (PortalState == PortalState.open) {
				SceneLoader.Instance.ChangeLocation(SceneToLoad);
			}
		}
	}
}
