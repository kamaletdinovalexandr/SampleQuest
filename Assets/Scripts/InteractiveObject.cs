using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour {

	[SerializeField] private Item _item;

	public void Interact() {
        _item.Interact();
	}
}
