using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject {

	public string Id;

	public Sprite Icon;

	public virtual void Interact() {}
}
