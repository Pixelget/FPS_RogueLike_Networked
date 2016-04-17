using UnityEngine;
using System.Collections;

public abstract class Interactable : MonoBehaviour {
    public GameObject Sender = null;

    public abstract void Interact();
}
