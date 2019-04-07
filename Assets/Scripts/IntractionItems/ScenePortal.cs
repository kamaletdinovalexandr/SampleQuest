using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;

public class ScenePortal : ItemView {

    public PortalState PortalState = PortalState.closed;
    public string SceneToLoad;

    public override Item Interact(Item item) {
         if (PortalState == PortalState.closed && item.Name == InputItemName) {
            PortalState = PortalState.open;
            GetComponent<SpriteRenderer>().sprite = CraftedItemIcon;
         }
              
        return null;
    }

    public bool TryUsePortal() {
        if (PortalState == PortalState.open) {
            SceneLoader.Instance.UnloadScene(SceneToLoad);
            return true;
        }
        
        return false;
    }


}
