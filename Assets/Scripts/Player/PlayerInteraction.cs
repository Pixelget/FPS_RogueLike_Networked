using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInteraction : MonoBehaviour {

    public float InteractionRange = 2f;
    public GameObject textObj;
    Text text;

    void Start() {
        if (textObj) {
            textObj.SetActive(true);
            text = textObj.GetComponent<Text>();
        }
    }

    void Update () {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, InteractionRange)) {
            if (hit.collider.tag == "Interactable") {
                textObj.SetActive(true);
                text.text = hit.collider.gameObject.GetComponent<Interactable>().InteractText;
            } else {
                textObj.SetActive(false);
            }
        } else {
            textObj.SetActive(false);
        }
    }

    public void Interact() {
        // ray cast directly forward and send an OnInteract message upwards on the object hit, do not require receiver
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, InteractionRange)) {
            if (hit.collider.tag == "Interactable") {
                hit.collider.gameObject.SendMessageUpwards("OnInteract", gameObject);
            }
        }
    }
}
