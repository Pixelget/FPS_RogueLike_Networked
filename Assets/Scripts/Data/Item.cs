using UnityEngine;
using System.Collections;

public enum ItemType { Usable, Weapon, WeaponMod, Currency }
public class Item {
    public bool Equiped = false;
    public string Name = "Not Defined";
    public ItemType Type = ItemType.Usable;

    public WeaponData Weapon;
    public int Currency = 0;
    public int RecoverAmount = 0;
    public bool IsAmmo = false;
}
