using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerGUI : MonoBehaviour {

    public Text AmmoTrackerText;
    public Text HealthTrackerText;
    public Weapon WeaponTracker;
    public PlayerVitality HealthTracker;

	void Start () {
        AmmoTrackerText = GameObject.FindGameObjectWithTag("UI_AmmoTracker").GetComponent<Text>();
        HealthTrackerText = GameObject.FindGameObjectWithTag("UI_HealthTracker").GetComponent<Text>();
    }
	
	void Update () {
        if (AmmoTrackerText) {
            AmmoTrackerText.text = WeaponTracker.ammo.ToString();
        }
        if (HealthTrackerText) {
            HealthTrackerText.text = "+" + HealthTracker.CurrentHP.ToString();
        }
    }
}
