using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;

public class ScenePortal : ItemView {

    public PortalState PortalState = PortalState.closed;
    public string SceneToLoad;

    public override bool Interact(Item item, out Item craftedItem) {
		if (PortalState == PortalState.open) {
			SceneLoader.Instance.ChangeLocation(SceneToLoad);
			craftedItem = null;
			return true;
		}

		if (PortalState == PortalState.closed && item != null && item.Name == InputItemName)  {
            PortalState = PortalState.open;
            GetComponent<SpriteRenderer>().sprite = CraftedItemIcon;
			craftedItem = null;
			return true;
         }

		craftedItem = null;
        return false;
    }
}
