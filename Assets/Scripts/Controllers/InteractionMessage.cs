using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionMessage : MBSingleton<InteractionMessage> {

	[SerializeField] private Text MessageBox;
	public void SetInteractionMessage(string text) {
		MessageBox.text = text;
	}
}
