using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DescripriveItem", menuName = "items/DescriptiveItem")]
public class DescriptiveItem : Item {

    public string Descriprion;

    public override void Interact() {
        InteractionMessage.Instance.SetInteractionMessage(Descriprion);
    }
}
