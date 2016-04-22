using UnityEngine;
using System.Collections;

public abstract class Interactable : MonoBehaviour {
    public GameObject Sender = null;
    public string InteractText = "Use";

    public abstract void Interact();
}
