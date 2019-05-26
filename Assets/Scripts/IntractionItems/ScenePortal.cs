using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;

public class ScenePortal : MonoBehaviour {

    public PortalState PortalState = PortalState.closed;
	public string InputItemName;
	public string SceneToLoad;

    public bool TryChangeLocation(string itemName) {
		if (PortalState == PortalState.open) {
			SceneLoader.Instance.ChangeLocation(SceneToLoad);
			return true;
		}

		if (PortalState == PortalState.closed && !string.IsNullOrEmpty(itemName) && itemName == InputItemName)  {
            PortalState = PortalState.open;
			return true;
         }
        return false;
    }
}
