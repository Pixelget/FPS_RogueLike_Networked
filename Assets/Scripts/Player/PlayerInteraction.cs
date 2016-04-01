using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInteraction : MonoBehaviour {

    public float InteractionRange = 2f;
    GameObject textObj;
    Text text;

    void Start() {
        textObj = DebugText.GetNextObj();
        if (textObj) {
            textObj.SetActive(true);
            text = textObj.GetComponent<Text>();
        }
    }

    void Update () {
        if (textObj == null) {
            textObj = DebugText.GetNextObj();
            textObj.SetActive(true);
            text = textObj.GetComponent<Text>();
        }
        
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, InteractionRange)) {
            if (hit.collider.tag == "Interactable") {
                textObj.SetActive(true);
                text.text = "USE";
                textObj.transform.position = hit.point + ((hit.point - Camera.main.transform.position).normalized * -0.5f);
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
