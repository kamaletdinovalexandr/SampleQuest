using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour {

    private static InteractionController _instance;

    public static InteractionController Instance { get { return _instance; } }

    public List<GameObject> InteractionPairs; 

    private void Awake() {
        _instance = this;
    }


}
