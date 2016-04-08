using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class WeaponManager : MonoBehaviour {
    // Links
    public WeaponData Weapon;

    public PlayerManager player;

    public SpriteRenderer GunVisual;
    public Animator GunAnimator;

    public AudioClip EquipGunSFX;
    public AudioClip shotSFX;
    public AudioClip reloadSFX;
    public AudioClip emptyClipSFX;
    public AudioSource audioSource;

    public GameObject MuzzleFlash;

    // Weapon Firing Variables
    float cooldown;
    float firerate;
    int magazineSize;
    int bulletsPerShot;
    int currentSoundPriority = 10;

    float reload;
    float accuracy = 0f;
    float accuracyResetDelayTimer = 0f;

    Vector2 direction;

    bool Reloading = false;
    bool Firing = false;
    bool Shooting = false;
    bool CurrentlyBurstFiring = false;

    void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	
	void Update () {
	
	}
    
    public void Reset() {
        magazineSize = Weapon.MagazineSize;
        firerate = Weapon.FireRate;
        bulletsPerShot = Weapon.BulletsPerShot;
    }
    
    public void Reload() {
        PlayAudio(reloadSFX, 2);
        Reloading = true;
        reload = 0f;
    }

    void ShotEffects() {
        // TODO Play Audio

        // Muzzle effects
        MuzzleEffects();
    }

    void MuzzleEffects() {
        // TODO MuzzleFlash
        MuzzleFlash.SetActive(true);
        CancelInvoke("disableFlash");
        Invoke("disableFlash", 0.03f);
    }

    void disableFlash() {
        MuzzleFlash.SetActive(false);
    }

    void AimDirectionUpdate(Vector3 dir) {
        direction = dir;
    }

    public void EquipWeapon(WeaponData weapon) {
        Weapon = weapon;

        ChangeGunVisuals();
        Reset();
    }

    void ChangeGunVisuals() {
        // Play swap out animation and change art once down?

        // Change the gun sprite based on the weapondata
        if (Weapon.GunSprite)
            GunVisual.sprite = Weapon.GunSprite;

        // Switch the animations based on the weapondata
        if (Weapon.AnimatorController)
            GunAnimator.runtimeAnimatorController = Weapon.AnimatorController;

        // Play Switch Sound fx
        if (EquipGunSFX)
            PlayAudio(EquipGunSFX, 1);
    }

    void PlayAudio(AudioClip clip, int priority) {
        if (currentSoundPriority < priority) {
            currentSoundPriority = priority;
            audioSource.clip = clip;
            audioSource.Play();

            return;
        }
        audioSource.clip = clip;
        audioSource.Play();
    }
}
