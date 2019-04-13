using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items;

public class ItemViewTrigger : ItemView {

	[SerializeField] private List<TriggerTarget> Targets;
	[SerializeField] private TriggerAction SelfAction;
    
	public override Item Interact(Item item) {
		if (item.Name == InputItemName) {
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
			if (SelfAction == TriggerAction.disable)
				gameObject.SetActive(false);
		}

		return null;
	}
}
