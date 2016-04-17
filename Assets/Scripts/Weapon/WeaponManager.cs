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
    public GameObject PlayerCamera;

    // Weapon Firing Variables
    float cooldown;
    float firerate;
    int ammo;
    int bulletsPerShot;
    int burstFireShots = 3;
    int currentSoundPriority = 10;

    float reload;
    float accuracy = 0f;
    float accuracyOffset = 0f;
    float accuracyResetDelayTimer = 0f;

    Vector3 direction;

    bool Reloading = false;
    bool Firing = false; // The player is trying to shoot
    bool Shooting = false; // The weapon can shoot
    bool CurrentlyBurstFiring = false;

    void Start () {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = shotSFX;

        if (!PlayerCamera)
            PlayerCamera = Camera.main.gameObject;

        if (!GunAnimator) {
            Debug.LogError("GunAnimator has not been set.");
        }
    }
	
	void Update () {
        if (Weapon != null)
            WeaponUpdate();

        GunAnimator.SetBool("Reloading", Reloading);
    }

    void WeaponUpdate() {
        cooldown += Time.deltaTime;
        if ((cooldown > Weapon.FireRate) && (ammo > 0) && (!Reloading)) {
            Shooting = true;
        } else if ((cooldown > Weapon.FireRate) && (ammo <= 0) && (!Reloading) && (Firing)) {
            cooldown = 0f;
        }

        if (Reloading) {
            if (player.Inventory.Ammo[(int) Weapon.AmmoType] <= 0) {
                reload = 0f;
                Reloading = false;
                return;
            }

            reload += Time.deltaTime;
            if (reload > Weapon.ReloadTime) {
                // Completed the reload / Cooldown is over
                reload = 0f;
                Reloading = false;
                accuracy = 0f;
                accuracyOffset = 0f;
                GetWeaponAmmo();
                Reset();
            }

            Shooting = false;
            Firing = false;
        }

        if (Shooting && Firing) {
            // Can shoot
            Firing = true;
            cooldown = 0f;
        }

        if ((Firing || CurrentlyBurstFiring) && (ammo > 0)) {
            switch (Weapon.FireMode) {
                case FireType.Single:
                    SingleShotMode();
                    break;
                case FireType.Auto:
                    AutoShotMode();
                    break;
                case FireType.Burst:
                    BurstFireMode();
                    break;
                case FireType.Spray:
                    SprayShotMode();
                    break;
            }
        }

        if (Firing) {
            accuracyOffset += Time.deltaTime;
            if (accuracyOffset > Weapon.AccuracyReductionTime) {
                accuracy = Weapon.Accuracy - Weapon.AccuracyReductionAmount;
            } else {
                accuracy = Weapon.Accuracy - (Weapon.AccuracyReductionAmount * (accuracyOffset / Weapon.AccuracyReductionTime));
            }
        } else {
            accuracyOffset -= Time.deltaTime * Weapon.AccuracyResetRate;
            if (accuracyOffset < 0f) {
                accuracyOffset = 0f;
            }
            accuracy = Weapon.Accuracy - (Weapon.AccuracyReductionAmount * (accuracyOffset / Weapon.AccuracyReductionTime));
        }
    }

    void SingleShotMode() {
        // Fire a shot
        firerate += Time.deltaTime;
        if (firerate > Weapon.FireRate) {
            Shoot();

            Shooting = false;
            Firing = false;

            ammo--;
            firerate = 0f;
        }
    }

    void AutoShotMode() {
        // Fire a shot
        firerate += Time.deltaTime;
        if (firerate > Weapon.FireRate) {
            Shoot();

            ammo--;
            firerate = 0f;

            if (ammo <= 0) {
                Shooting = false;
            }
        }
    }

    void BurstFireMode() {
        // Empty the clip
        firerate += Time.deltaTime;
        if (firerate > Weapon.FireRate) {
            CurrentlyBurstFiring = true;
            // Fire a shot
            Shoot();

            burstFireShots--;
            ammo--;
            firerate = 0f;
            if (ammo <= 0) {
                burstFireShots = 0;
            }
        }

        // if no ammo remains
        if (burstFireShots <= 0) {
            // Reset Weapon
            // Set the canShoot and Fire to false
            Shooting = false;
            Firing = false;
            // Set the ammo to totalAmmo
            burstFireShots = 3;
            CurrentlyBurstFiring = false;
        }
    }

    void SprayShotMode() {
        firerate += Time.deltaTime;
        if (firerate > Weapon.FireRate) {
            // Fire a shot spray
            for (int i = 1; i <= Weapon.BulletsPerShot; i++) {
                Shoot();
            }

            Shooting = false;
            Firing = false;

            ammo--;
            firerate = 0f;
        }
    }

    protected void Shoot() {
        // Shot visual and audio effects
        ShotEffects();

        // Accuracy calculation
        float spread = GameWeaponData.SpreadLevel * (1f - accuracy); // in degrees aperature (degrees measured from left most side to right most)

        float spreadAngle = Random.Range(spread * -0.5f, spread * 0.5f);
        float rotateAngle = Random.Range(0f, 360f);

        Vector3 shotDirection = Quaternion.AngleAxis(spreadAngle, Vector3.up) // rotate to the left or right based on spread
            * PlayerCamera.transform.forward; // put it in local space

        shotDirection = Quaternion.AngleAxis(rotateAngle, PlayerCamera.transform.forward) * shotDirection; // rotate around the aim axis to create the accuracy cone

        // Code for collision and damage
        //Debug.Log("Shooting with a spread of " + spread + ", spread angle of " + spreadAngle + ", rotateAngle of " + rotateAngle + " in the " + shotDirection + " Direction");
        RaycastHit hit;
        Debug.DrawRay(PlayerCamera.transform.position, shotDirection * 500f, Color.red, 15f);
        var layerMask = ~(1 << LayerMask.NameToLayer("ItemPickup"));
        
        if (Physics.Raycast(PlayerCamera.transform.position, shotDirection, out hit, Weapon.Range, layerMask)) {
            hit.collider.gameObject.SendMessageUpwards("OnHit", new HitDetails(Weapon.Damage, hit.point, transform.root.gameObject), SendMessageOptions.DontRequireReceiver);
        }
        CancelInvoke("disableFlash");
        Invoke("disableFlash", 0.03f); // TODO Fix the issue with the timing of the disable

        // Recoil Here
    }

    public void Reset() {
        //GetWeaponAmmo();
        firerate = Weapon.FireRate;
        bulletsPerShot = Weapon.BulletsPerShot;
    }

    void GetWeaponAmmo() {
        int newamount = 0;
        int neededAmmo = Weapon.MagazineSize - ammo;
        if (neededAmmo > player.Inventory.Ammo[(int) Weapon.AmmoType]) {
            // less ammo then we need
            newamount = ammo + player.Inventory.Ammo[(int) Weapon.AmmoType];
            player.Inventory.Ammo[(int) Weapon.AmmoType] = 0;
        } else {
            newamount = ammo + neededAmmo;
            player.Inventory.Ammo[(int) Weapon.AmmoType] -= neededAmmo;
        }
        ammo = newamount;
    }
    
    public void Reload() {
        PlayAudio(reloadSFX, 2);
        Reloading = true;
        reload = 0f;
    }

    void ShotEffects() {
        // TODO Play Audio
        PlayAudio(shotSFX, 5);

        // Muzzle effects
        MuzzleEffects();
    }

    void MuzzleEffects() {
        // TODO MuzzleFlash
        MuzzleFlash.SetActive(true);

        GunAnimator.SetTrigger("Fire");

        CancelInvoke("disableFlash");
        Invoke("disableFlash", 0.03f);
    }

    void disableFlash() {
        MuzzleFlash.SetActive(false);
    }

    void AimDirectionUpdate(Vector3 direction) {
        this.direction = direction;
    }

    public void EquipWeapon(WeaponData weapon) {
        Weapon.Ammo = ammo;
        Weapon = weapon;
        ammo = Weapon.Ammo;

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
    
    public void IsFiring(bool f) {
        Firing = f;
    }

    public int GetAmmo() {
        return ammo;
    }
}
