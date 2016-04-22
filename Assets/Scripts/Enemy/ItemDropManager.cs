using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDropManager : MonoBehaviour {

    public List<Item> LootTable = new List<Item>();
    public GameObject ItemDropPrefab;

    public float dropChance = 0.5f;

    Vector3 offset;

    public void DropItems() {
        if (LootTable.Count > 0) {
            if (Random.Range(0f, 1f) < dropChance) {
                offset = new Vector3(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f));

                GameObject temp = (GameObject) Instantiate(ItemDropPrefab, transform.position + offset, Quaternion.identity);
                temp.GetComponent<Interact_ItemDrop>().item = LootTable[Random.Range(0, LootTable.Count)];
            }
        }
    }
}
