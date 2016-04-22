using UnityEngine;
using System.Collections;

public class Interact_ItemDrop : Interactable {

    public Item item;
    GameObject player;

    void Start() {
        GetComponentInChildren<SpriteRenderer>().sprite = item.ItemArt;
        player = GameObject.FindGameObjectWithTag("Player");
        InteractText = item.PickupText;
    }
    
    public override void Interact() {
        // Pickup item
        if (Sender.GetComponent<PlayerManager>().Inventory.AddToPack(item)) {
            Debug.Log(Sender.name + " picked up " + item.Name);
            // Was added to pack
            Destroy(gameObject);
        } else {
            Debug.Log(Sender.name + " could not picked up " + item.Name);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (item.AutoPickup) {
            if (other.transform.root.GetComponent<PlayerManager>().Inventory.AddToPack(item)) {
                Debug.Log(other.name + " picked up " + item.Name + " with there butt");
                // Was added to pack
                Destroy(gameObject);
            } else {
                Debug.Log(other.name + " could not picked up " + item.Name + " with there butt");
            }
        }
    }
}
