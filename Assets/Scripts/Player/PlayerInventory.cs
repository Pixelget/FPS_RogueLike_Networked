using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour {
    // Ammo Types
    public int[] Ammo;
    public List<Item> Backpack = new List<Item>();
    public int PackLimit = 10;

    void Start() {
        Ammo = new int[System.Enum.GetNames(typeof(AmmoType)).Length];
        for (int i = 0; i < Ammo.Length; i++) {
            Ammo[i] = GameWeaponData.StartingAmmo;
        }

        Backpack.Clear();
    }

    public bool AddToPack(Item item) {
        if (Backpack.Count < PackLimit) {
            Backpack.Add(item);
            return true;
        }
        return false; // there was no space
    }
}
