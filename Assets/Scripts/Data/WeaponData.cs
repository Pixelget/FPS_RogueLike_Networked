using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum WeaponBase { Pistol, Revolver, SMG, PumpShotgun, CombatShotgun, MachineGun, BoltActionRifle, CombatRifle, Melee }
public enum WeaponManufacturers { Marauder, Modular, TypeZero, Variance, Fragcaster, Baseline, Legendary }
public enum WeaponQuality { Damaged, Worn, Normal, Modified, Heavy, Upgraded, Advanced, Legendary }

public enum FireType { Single, Burst, Auto }
public enum AmmoType { PistolSMG, Rifle, Shells, Heavy }

[System.Serializable]
public class WeaponData {
    [Header("Weapon Base")]
    public string Name = "";
    public WeaponBase Base;
    public WeaponManufacturers Manufacturer;
    public WeaponQuality Quality;

    public AmmoType AmmoType;
    public bool LaserBased = false; // changes shot visuals, increased damage and reduce accuracy?
    public FireType FireMode = FireType.Single;

    // Mod Slots - one for each mod type? Random chance to come with mods?
    //List<WeaponAttachment> WeaponAttachments = new List<WeaponAttachment>();
    //WeaponAttachment[] WeaponAttachments = new WeaponAttachment[3];

    public AudioClip shotSFX;
    public AudioClip reloadSFX;
    public AudioClip emptyClipSFX;

    public Sprite GunSprite;

    public RuntimeAnimatorController AnimatorController;

    // Weapon Settings
    private float _reloadTime;
    public float ReloadTime
    {
        get { return _reloadTime; }
    }
    private int _magazineSize;
    public int MagazineSize
    {
        get { return _magazineSize; }
    }

    //[Header ("Damage Settings")]
    private int _damage = 0;
    public int Damage {
        get { return _damage; }
    }

    // Shot Settings
    private int _bulletsPerShot; // standard 3 [range 3-5]
    public int BulletsPerShot {
        get { return _bulletsPerShot; }
    }

    // Fire Rate Settings
    private float _burstDelay; // Time between each bullet in a shot
    public float BurstDelay {
        get { return _burstDelay; }
    }
    private float _fireRate; // Min time between each player shot
    public float FireRate {
        get { return _fireRate; }
    }

    // Accuracy Settings
    private float _accuracy; // Range 0 - 1
    public float Accuracy {
        get { return _accuracy; }
    }

    // 100% - Accuracy = Spread ÷12
    private float _spread;
    public float Spread {
        get { return _spread; }
    }
    private float _recoil;
    public float Recoil {
        get { return _recoil; }
    }

    private float _accuracyReductionTime;
    public float AccuracyReductionTime {
        get { return _accuracyReductionTime; }
    }
    private float _accuracyReductionAmount;
    public float AccuracyReductionAmount {
        get { return _accuracyReductionAmount; }
    }
    private float _accuracyResetRate; // Range 0 - 1
    public float AccuracyResetRate {
        get { return _accuracyResetRate; }
    }

    // Visual and Sound FX
    public GameObject GunObject;
    public AudioClip ShotSound;

    #region ComputeVariables
    // Need to include the mods in these calculations as well

    public void ComputeWeapon(float damageVariance, float magazineVariance, float reloadVariance, float fireRateVariance, float accuracyVariance, float recoilVariance) {
        ComputeDamage(damageVariance);
        ComputeMagazineSize(magazineVariance);
        ComputeReloadTime(reloadVariance);
        ComputeBulletsPerShot();
        ComputeBurstDelay();
        ComputeFireRate(fireRateVariance);
        ComputeAccuracy(accuracyVariance);
        ComputeRecoil(recoilVariance);
    }

    protected void ComputeDamage(float variance) {
        float modifieddamage = GameWeaponData.GetBaseDamage(Base) * GameWeaponData.GetManufacturerDamageModifier(Manufacturer) * GameWeaponData.GetQualityDamageModifier(Quality) * variance;

        if (LaserBased)
            modifieddamage *= 1.15f;

        _damage = Mathf.RoundToInt(modifieddamage);
    }

    protected void ComputeMagazineSize(float variance) {
        float modifiedMagazine = ((float) GameWeaponData.GetBaseMagazineSize(Base)) * GameWeaponData.GetManufacturerMagazineModifier(Manufacturer) * GameWeaponData.GetQualityMagazineModifier(Quality) * variance;

        if (LaserBased)
            modifiedMagazine *= 0.9f;

        _magazineSize = Mathf.RoundToInt(modifiedMagazine);
    }

    protected void ComputeReloadTime(float variance) {
        float modifiedReloadTime = GameWeaponData.GetBaseReloadTime(Base) * GameWeaponData.GetManufacturerReloadModifier(Manufacturer) * GameWeaponData.GetQualityReloadModifier(Quality) * variance;

        if (LaserBased)
            modifiedReloadTime *= 0.95f;

        _reloadTime = modifiedReloadTime;
    }

    protected void ComputeBulletsPerShot() {
        _bulletsPerShot = Mathf.RoundToInt(GameWeaponData.GetBaseBulletsPerShot(Base));
    }

    protected void ComputeBurstDelay() {
        _burstDelay = GameWeaponData.GetBaseBurstDelay(Base);
    }

    protected void ComputeFireRate(float variance) {
        float modifiedFireRate = GameWeaponData.GetBaseFireRate(Base) * GameWeaponData.GetManufacturerFireRateModifier(Manufacturer) * GameWeaponData.GetQualityFireRateModifier(Quality) * variance;

        _fireRate = modifiedFireRate;
    }

    protected void ComputeAccuracy(float variance) {
        float modifiedAccuracy = GameWeaponData.GetBaseAccuracy(Base) * GameWeaponData.GetManufacturerAccuracyModifier(Manufacturer) * GameWeaponData.GetQualityAccuracyModifier(Quality) * variance;

        _accuracy = Mathf.Clamp(modifiedAccuracy, 0f, 1f);
        _spread = GameWeaponData.SpreadLevel * (1f - _accuracy);
    }

    protected void ComputeRecoil(float variance) {
        float modifiedRecoil = GameWeaponData.GetBaseRecoil(Base) * GameWeaponData.GetManufacturerRecoilModifier(Manufacturer) * GameWeaponData.GetQualityRecoilModifier(Quality) * variance;

        _recoil = modifiedRecoil;
    }

    protected void ComputeAccuracyResetRate() {
        float modifiedAccuracyResetRate = GameWeaponData.GetBaseAccuracyResetRate(Base) * GameWeaponData.GetManufacturerAccuracyResetRateModifier(Manufacturer);

        _accuracyResetRate = modifiedAccuracyResetRate;
    }

    protected void ComputeAccuracyReductionTime() {
        _accuracyReductionTime = GameWeaponData.GetBaseAccuracyReductionTime(Base);
    }

    protected void ComputeAccuracyReductionAmount() {
        _accuracyReductionAmount = GameWeaponData.GetBaseAccuracyReductionAmount(Base);
    }

    #endregion
}
