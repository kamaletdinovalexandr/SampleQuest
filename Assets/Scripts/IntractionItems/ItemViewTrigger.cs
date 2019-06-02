﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;

public class ItemViewTrigger : ItemView {

	[SerializeField] private List<TriggerTarget> Targets;
	[SerializeField] private TriggerAction SelfAction;
    
	public override bool Interact(Item item) {
        if (Item.Interact(item)) {
            SetAfterInteractionState();
            SetSelfState();

            return true;
		}

        return false;
	}

    private void SetAfterInteractionState() {
        foreach (var target in Targets) {
            switch (target.TriggerAction) {
                case TriggerAction.enable:
                    target.gameObject.SetActive(true);
                    break;
                case TriggerAction.disable:
                    target.gameObject.SetActive(false);
                    break;
            }
        }
    }

    private void SetSelfState() {
        if (SelfAction == TriggerAction.disable)
            gameObject.SetActive(false);
    }
}
