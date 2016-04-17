using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour {
    // Ammo Types
    public int Currency = 0;
    public int[] Ammo;
    public List<Item> Backpack = new List<Item>();
    public int PackLimit = 10;
    public PlayerManager player;

    public List<WeaponData> EquipedWeapons;
    int currentlyEquiped = 0;
    // EquipedArmor

    void Start() {
        Ammo = new int[System.Enum.GetNames(typeof(AmmoType)).Length];

        Ammo[(int) AmmoType.PistolSMG] = Mathf.RoundToInt(GameWeaponData.MaxSMGPistolAmmo * 0.5f);
        Ammo[(int) AmmoType.Shells] = Mathf.RoundToInt(GameWeaponData.MaxShellAmmo * 0.5f);
        Ammo[(int) AmmoType.Rifle] = Mathf.RoundToInt(GameWeaponData.MaxRifleAmmo * 0.5f);
        Ammo[(int) AmmoType.Heavy] = Mathf.RoundToInt(GameWeaponData.MaxHeavyAmmo * 0.5f);
        
        Backpack.Clear();

        EquipedWeapons[0].ComputeWeapon(1f, 1f, 1f, 1f, 1f, 1f); // TODO Needs to be changed to on weapon creation
        EquipedWeapons[1].ComputeWeapon(1f, 1f, 1f, 1f, 1f, 1f);
        player.weaponManager.EquipWeapon(EquipedWeapons[currentlyEquiped]);
    }

    public bool AddToPack(Item item) {
        // Check item type and add it approapriatly
        switch (item.Type) {
            case ItemType.AmmoPickup:
                switch(item.ammoType) {
                    case AmmoType.PistolSMG:
                        if (Ammo[(int) AmmoType.PistolSMG] < GameWeaponData.MaxSMGPistolAmmo) {
                            int missingAmmo = GameWeaponData.MaxSMGPistolAmmo - Ammo[(int) AmmoType.PistolSMG];
                            if (missingAmmo < item.Amount) {
                                Ammo[(int) AmmoType.PistolSMG] += missingAmmo;
                            } else {
                                Ammo[(int) AmmoType.PistolSMG] += item.Amount;
                            }
                            return true;
                        }
                        return false;
                    case AmmoType.Rifle:
                        if (Ammo[(int) AmmoType.Rifle] < GameWeaponData.MaxRifleAmmo) {
                            int missingAmmo = GameWeaponData.MaxRifleAmmo - Ammo[(int) AmmoType.Rifle];
                            if (missingAmmo < item.Amount) {
                                Ammo[(int) AmmoType.Rifle] += missingAmmo;
                            } else {
                                Ammo[(int) AmmoType.Rifle] += item.Amount;
                            }
                            return true;
                        }
                        return false;
                    case AmmoType.Shells:
                        if (Ammo[(int) AmmoType.Shells] < GameWeaponData.MaxShellAmmo) {
                            int missingAmmo = GameWeaponData.MaxShellAmmo - Ammo[(int) AmmoType.Shells];
                            if (missingAmmo < item.Amount) {
                                Ammo[(int) AmmoType.Shells] += missingAmmo;
                            } else {
                                Ammo[(int) AmmoType.Shells] += item.Amount;
                            }
                            return true;
                        }
                        return false;
                    case AmmoType.Heavy:
                        if (Ammo[(int) AmmoType.Heavy] < GameWeaponData.MaxHeavyAmmo) {
                            int missingAmmo = GameWeaponData.MaxHeavyAmmo - Ammo[(int) AmmoType.Heavy];
                            if (missingAmmo < item.Amount) {
                                Ammo[(int) AmmoType.Heavy] += missingAmmo;
                            } else {
                                Ammo[(int) AmmoType.Heavy] += item.Amount;
                            }
                            return true;
                        }
                        return false;
                }
                break;
            case ItemType.HealthPickup:
                if (player.vitality.CurrentHP < player.vitality.TotalHP) { // TODO replace with player max
                    int missingHP = player.vitality.TotalHP - player.vitality.CurrentHP;
                    if (missingHP < item.Amount) {
                        player.vitality.CurrentHP += missingHP;
                    } else {
                        player.vitality.CurrentHP += item.Amount;
                    }
                    return true;
                }
                return false;
            case ItemType.Currency:
                Currency += item.Amount;
                break;
            case ItemType.Weapon:
            case ItemType.WeaponMod:
                if (Backpack.Count < PackLimit) {
                    Backpack.Add(item);
                    return true;
                }
                return false; // there was no space
        }
        return false;

    }

    public void SwapWeapon() {
        currentlyEquiped++;

        if (currentlyEquiped >= EquipedWeapons.Count)
            currentlyEquiped = 0;

        player.weaponManager.EquipWeapon(EquipedWeapons[currentlyEquiped]);
    }
}
