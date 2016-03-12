using UnityEngine;
using System.Collections;

public class PlayerInteraction : MonoBehaviour {

    public float InteractionRange = 2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void Interact() {
        // ray cast directly forward and send an OnInteract message upwards on the object hit, do not require receiver
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, InteractionRange)) {
            hit.collider.gameObject.SendMessageUpwards("OnInteract", gameObject);
        }
    }
}
