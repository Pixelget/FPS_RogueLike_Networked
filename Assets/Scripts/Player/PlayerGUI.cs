using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerGUI : MonoBehaviour {

    public Text AmmoTrackerText;
    public Text HealthTrackerText;
    public WeaponManager WeaponTracker;
    public PlayerVitality HealthTracker;
    public PlayerManager player;

	void Start () {
        AmmoTrackerText = GameObject.FindGameObjectWithTag("UI_AmmoTracker").GetComponent<Text>();
        HealthTrackerText = GameObject.FindGameObjectWithTag("UI_HealthTracker").GetComponent<Text>();
    }
	
	void Update () {
        if (AmmoTrackerText) {
            AmmoTrackerText.text = WeaponTracker.GetAmmo().ToString() + " / " + player.Inventory.Ammo[(int) WeaponTracker.Weapon.AmmoType];
        }
        if (HealthTrackerText) {
            HealthTrackerText.text = "+" + HealthTracker.CurrentHP.ToString();
        }
    }
}
