using UnityEngine;
using System.Collections;

public enum ItemType { AmmoPickup, HealthPickup, Weapon, WeaponMod, Currency }
[System.Serializable]
public class Item {
    public string Name = "Not Defined";
    public Sprite ItemArt;
    public ItemType Type = ItemType.AmmoPickup;
    public bool Equiped = false;
    public bool AutoPickup = false;
    public string PickupText = "";

    public WeaponData Weapon;
    //public WeaponMod WeaponMod;
    public int Amount = 0; // used for hp, ammo, and currency
    public AmmoType ammoType = AmmoType.PistolSMG;
}
