using UnityEngine;
using System.Collections;

public class InteractionManager : MonoBehaviour {

    public Interactable interactableObject;

    void OnInteract(GameObject sender) {
        interactableObject.Sender = sender;
        interactableObject.enabled = true;
    }
}
