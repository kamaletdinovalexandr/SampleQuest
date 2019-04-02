using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;

public class ScenePortal : BaseItem {

    public PortalState PortalState = PortalState.closed;
    public string SceneToLoad;

    public override Item Interact(Item item) {
         if (PortalState == PortalState.closed && item.Name == InputItemName) {
            PortalState = PortalState.open;
            GetComponent<SpriteRenderer>().sprite = CraftedItemIcon;
         }
              
        return null;
    }

    public void UsePortal() {
        SceneLoader.Instance.UnloadScene(SceneToLoad);
    }


}
