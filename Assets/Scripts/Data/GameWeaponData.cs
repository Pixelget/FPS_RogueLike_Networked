public static class GameWeaponData {
    #region BaseData
    public static float SpreadLevel = 12f;
    public static float WeaponAccuracyResetDelay = 0.75f;
    public static int StartingAmmo = 30;
    #endregion

    #region DamageData
    public static float GetBaseDamage(WeaponBase _base) {
        switch(_base) {
            case WeaponBase.Pistol:
                return 10f;
            case WeaponBase.Revolver:
                return 35f;
            case WeaponBase.SMG:
                return 10f;
            case WeaponBase.PumpShotgun:
                return 5.5f;
            case WeaponBase.CombatShotgun:
                return 7f;
            case WeaponBase.MachineGun:
                return 15f;
            case WeaponBase.BoltActionRifle:
                return 30f;
            case WeaponBase.CombatRifle:
                return 15f;
            case WeaponBase.Melee:
                return 50f;
            default:
                return 10f;
        }
    }

    public static float GetManufacturerDamageModifier(WeaponManufacturers _manufacturer) {
        switch(_manufacturer) {
            case WeaponManufacturers.Marauder:
            case WeaponManufacturers.Variance:
            case WeaponManufacturers.Baseline:
                return 1.15f;
            case WeaponManufacturers.Modular:
                return 0.85f;
            case WeaponManufacturers.TypeZero:
            case WeaponManufacturers.Legendary:
                return 1.25f;
            case WeaponManufacturers.Fragcaster:
            default:
                return 1f;
        }
    }

    public static float GetQualityDamageModifier(WeaponQuality _quality) {
        switch(_quality) {
            case WeaponQuality.Damaged:
                return 0.5f;
            case WeaponQuality.Worn:
                return 0.8f;
            case WeaponQuality.Modified:
                return 1.1f;
            case WeaponQuality.Heavy:
                return 1.15f;
            case WeaponQuality.Upgraded:
                return 1.25f;
            case WeaponQuality.Advanced:
                return 1.5f;
            case WeaponQuality.Legendary:
                return 2.25f;
            default:
                return 1f;
        }
    }
    #endregion

    #region MagazineData
    public static int GetBaseMagazineSize(WeaponBase _base) {
        switch (_base) {
            case WeaponBase.Pistol:
                return 9;
            case WeaponBase.Revolver:
                return 6;
            case WeaponBase.SMG:
                return 30;
            case WeaponBase.PumpShotgun:
                return 2;
            case WeaponBase.CombatShotgun:
                return 7;
            case WeaponBase.MachineGun:
                return 50;
            case WeaponBase.BoltActionRifle:
                return 8;
            case WeaponBase.CombatRifle:
                return 30;
            default:
                return 9;
        }
    }

    public static float GetManufacturerMagazineModifier(WeaponManufacturers _manufacturer) {
        switch (_manufacturer) {
            case WeaponManufacturers.Variance:
            case WeaponManufacturers.Fragcaster:
                return 0.85f;
            case WeaponManufacturers.Baseline:
            case WeaponManufacturers.Modular:
            case WeaponManufacturers.Legendary:
                return 1.15f;
            case WeaponManufacturers.Marauder:
            case WeaponManufacturers.TypeZero:
            default:
                return 1f;
        }
    }

    public static float GetQualityMagazineModifier(WeaponQuality _quality) {
        switch (_quality) {
            case WeaponQuality.Damaged:
                return 0.5f;
            case WeaponQuality.Worn:
                return 0.8f;
            case WeaponQuality.Modified:
                return 1.1f;
            case WeaponQuality.Heavy:
                return 1.1f;
            case WeaponQuality.Upgraded:
                return 1.15f;
            case WeaponQuality.Advanced:
                return 1.3f;
            case WeaponQuality.Legendary:
                return 1.7f;
            default:
                return 1f;
        }
    }
    #endregion

    #region ReloadTimeData
    public static float GetBaseReloadTime(WeaponBase _base) {
        switch (_base) {
            case WeaponBase.Pistol:
                return 1f;
            case WeaponBase.Revolver:
                return 1.55f;
            case WeaponBase.SMG:
                return 1.45f;
            case WeaponBase.PumpShotgun:
                return 1.7f;
            case WeaponBase.CombatShotgun:
                return 2.0f;
            case WeaponBase.MachineGun:
                return 4f;
            case WeaponBase.BoltActionRifle:
                return 1.3f;
            case WeaponBase.CombatRifle:
                return 1.85f;
            case WeaponBase.Melee:
                return 0f;
            default:
                return 1f;
        }
    }

    public static float GetManufacturerReloadModifier(WeaponManufacturers _manufacturer) {
        switch (_manufacturer) {
            case WeaponManufacturers.Modular:
            case WeaponManufacturers.Variance:
            case WeaponManufacturers.Baseline:
                return 0.9f;
            case WeaponManufacturers.Legendary:
                return 0.85f;
            case WeaponManufacturers.TypeZero:
            case WeaponManufacturers.Fragcaster:
                return 1.1f;
            case WeaponManufacturers.Marauder:
            default:
                return 1f;
        }
    }

    public static float GetQualityReloadModifier(WeaponQuality _quality) {
        switch (_quality) {
            case WeaponQuality.Damaged:
                return 1.25f;
            case WeaponQuality.Worn:
                return 1.1f;
            case WeaponQuality.Modified:
            case WeaponQuality.Heavy:
            case WeaponQuality.Upgraded:
                return 0.95f;
            case WeaponQuality.Advanced:
                return 0.9f;
            case WeaponQuality.Legendary:
                return 0.85f;
            default:
                return 1f;
        }
    }
    #endregion

    #region BulletsPerShotData
    public static float GetBaseBulletsPerShot(WeaponBase _base) {
        switch (_base) {
            case WeaponBase.Pistol:
            case WeaponBase.Revolver:
            case WeaponBase.MachineGun:
            case WeaponBase.BoltActionRifle:
            case WeaponBase.CombatRifle:
            case WeaponBase.Melee:
            case WeaponBase.SMG:
                return 1f;
            case WeaponBase.PumpShotgun:
                return 7f;
            case WeaponBase.CombatShotgun:
                return 9f;
            default:
                return 1f;
        }
    }
    #endregion

    #region BurstDelayData
    public static float GetBaseBurstDelay(WeaponBase _base) {
        switch (_base) {
            case WeaponBase.Pistol:
            case WeaponBase.Revolver:
            case WeaponBase.MachineGun:
            case WeaponBase.BoltActionRifle:
            case WeaponBase.CombatRifle:
            case WeaponBase.Melee:
            case WeaponBase.SMG:
                return 0.15f;
            case WeaponBase.PumpShotgun:
            case WeaponBase.CombatShotgun:
                return 0f;
            default:
                return 0.1f;
        }
    }
    #endregion

    #region FireDelayData
    public static float GetBaseFireRate(WeaponBase _base) {
        switch (_base) { // 1 Second / Number of rounds per second
            case WeaponBase.Pistol:
                return 1f / 3f;
            case WeaponBase.Revolver:
                return 1f / 0.8f;
            case WeaponBase.SMG:
                return 1f / 6f;
            case WeaponBase.MachineGun:
                return 1f / 4f;
            case WeaponBase.PumpShotgun:
                return 1f / 0.75f;
            case WeaponBase.CombatShotgun:
                return 1.0f;
            case WeaponBase.BoltActionRifle:
                return 1.0f;
            case WeaponBase.CombatRifle:
                return 1f / 5f;
            case WeaponBase.Melee:
                return 1.0f;
            default:
                return 1.0f;
        }
    }

    public static float GetManufacturerFireRateModifier(WeaponManufacturers _manufacturer) {
        switch (_manufacturer) {
            case WeaponManufacturers.Marauder:
            case WeaponManufacturers.Variance:
            case WeaponManufacturers.Baseline:
                return 1.15f;
            case WeaponManufacturers.Modular:
                return 0.85f;
            case WeaponManufacturers.TypeZero:
            case WeaponManufacturers.Legendary:
                return 1.25f;
            case WeaponManufacturers.Fragcaster:
            default:
                return 1f;
        }
    }

    public static float GetQualityFireRateModifier(WeaponQuality _quality) {
        switch (_quality) {
            case WeaponQuality.Damaged:
                return 1.5f;
            case WeaponQuality.Worn:
                return 1.15f;
            case WeaponQuality.Heavy:
                return 0.95f;
            case WeaponQuality.Legendary:
                return 0.75f;
            case WeaponQuality.Modified:
            case WeaponQuality.Upgraded:
            case WeaponQuality.Advanced:
            default:
                return 1f;
        }
    }
    #endregion

    #region AccuracyData
    public static float GetBaseAccuracy(WeaponBase _base) {
        switch (_base) { // 1 Second / Number of rounds per second
            case WeaponBase.Pistol:
                return 0.95f;
            case WeaponBase.Revolver:
                return 0.9f;
            case WeaponBase.SMG:
                return 0.8f;
            case WeaponBase.MachineGun:
                return 0.65f;
            case WeaponBase.PumpShotgun:
                return 0.45f;
            case WeaponBase.CombatShotgun:
                return 0.45f;
            case WeaponBase.BoltActionRifle:
                return 0.95f;
            case WeaponBase.CombatRifle:
                return 0.9f;
            case WeaponBase.Melee:
                return 1.0f;
            default:
                return 1.0f;
        }
    }

    public static float GetManufacturerAccuracyModifier(WeaponManufacturers _manufacturer) {
        switch (_manufacturer) {
            case WeaponManufacturers.Marauder:
            case WeaponManufacturers.Modular:
            case WeaponManufacturers.Variance:
            case WeaponManufacturers.Fragcaster:
                return 0.85f;
            case WeaponManufacturers.Baseline:
            case WeaponManufacturers.Legendary:
                return 1.15f;
            case WeaponManufacturers.TypeZero:
            default:
                return 1f;
        }
    }

    public static float GetQualityAccuracyModifier(WeaponQuality _quality) {
        switch (_quality) {
            case WeaponQuality.Damaged:
                return 0.5f;
            case WeaponQuality.Worn:
                return 0.8f;
            case WeaponQuality.Modified:
                return 1.05f;
            case WeaponQuality.Upgraded:
                return 1.15f;
            case WeaponQuality.Legendary:
                return 1.25f;
            case WeaponQuality.Heavy:
            case WeaponQuality.Advanced:
            default:
                return 1f;
        }
    }
    #endregion

    #region RecoilData
    public static float GetBaseRecoil(WeaponBase _base) {
        switch (_base) { // 1 Second / Number of rounds per second
            case WeaponBase.Pistol:
                return 1.0f;
            case WeaponBase.Revolver:
                return 2.5f;
            case WeaponBase.SMG:
                return 0.75f;
            case WeaponBase.MachineGun:
                return 1.65f;
            case WeaponBase.PumpShotgun:
                return 2.75f;
            case WeaponBase.CombatShotgun:
                return 2.0f;
            case WeaponBase.BoltActionRifle:
                return 1.75f;
            case WeaponBase.CombatRifle:
                return 0.9f;
            case WeaponBase.Melee:
                return 0f;
            default:
                return 1.0f;
        }
    }

    public static float GetManufacturerRecoilModifier(WeaponManufacturers _manufacturer) {
        switch (_manufacturer) {
            case WeaponManufacturers.Marauder:
            case WeaponManufacturers.Modular:
                return 1.2f;
            case WeaponManufacturers.Variance:
            case WeaponManufacturers.Fragcaster:
                return 1.1f;
            case WeaponManufacturers.Legendary:
                return 0.85f;
            case WeaponManufacturers.TypeZero:
                return 0.95f;
            case WeaponManufacturers.Baseline:
            default:
                return 1f;
        }
    }

    public static float GetQualityRecoilModifier(WeaponQuality _quality) {
        switch (_quality) {
            case WeaponQuality.Damaged:
                return 1.5f;
            case WeaponQuality.Worn:
                return 1.2f;
            case WeaponQuality.Modified:
                return 0.95f;
            case WeaponQuality.Upgraded:
                return 0.85f;
            case WeaponQuality.Legendary:
                return 0.75f;
            case WeaponQuality.Heavy:
            case WeaponQuality.Advanced:
            default:
                return 1f;
        }
    }
    #endregion

    #region AccuracyResetRateData
    public static float GetBaseAccuracyResetRate(WeaponBase _base) {
        switch (_base) { // 1 Second / Number of rounds per second
            case WeaponBase.Pistol:
                return 0.75f;
            case WeaponBase.Revolver:
                return 0.5f;
            case WeaponBase.SMG:
                return 0.6f;
            case WeaponBase.MachineGun:
                return 0.25f;
            case WeaponBase.PumpShotgun:
                return 0.5f;
            case WeaponBase.CombatShotgun:
                return 0.55f;
            case WeaponBase.BoltActionRifle:
                return 0.9f;
            case WeaponBase.CombatRifle:
                return 0.75f;
            case WeaponBase.Melee:
            default:
                return 1.0f;
        }
    }

    public static float GetManufacturerAccuracyResetRateModifier(WeaponManufacturers _manufacturer) {
        switch (_manufacturer) {
            case WeaponManufacturers.Marauder:
                return 1.2f;
            case WeaponManufacturers.Modular:
            case WeaponManufacturers.Fragcaster:
                return 1.1f;
            case WeaponManufacturers.Variance:
            case WeaponManufacturers.TypeZero:
                return 0.95f;
            case WeaponManufacturers.Legendary:
                return 0.85f;
            case WeaponManufacturers.Baseline:
            default:
                return 1f;
        }
    }
    #endregion

    #region AccuracyReductionTimeData
    public static float GetBaseAccuracyReductionTime(WeaponBase _base) {
        switch (_base) { // 1 Second / Number of rounds per second
            case WeaponBase.Pistol:
                return 1.5f;
            case WeaponBase.Revolver:
                return 2.0f;
            case WeaponBase.SMG:
                return 2.0f;
            case WeaponBase.MachineGun:
                return 3.0f;
            case WeaponBase.PumpShotgun:
                return 1.2f;
            case WeaponBase.CombatShotgun:
                return 1.0f;
            case WeaponBase.BoltActionRifle:
                return 1.75f;
            case WeaponBase.CombatRifle:
                return 1.75f;
            case WeaponBase.Melee:
            default:
                return 1.0f;
        }
    }
    #endregion

    #region AccuracyReductionAmountData
    public static float GetBaseAccuracyReductionAmount(WeaponBase _base) {
        switch (_base) { // 1 Second / Number of rounds per second
            case WeaponBase.Pistol:
                return 0.25f;
            case WeaponBase.Revolver:
                return 0.4f;
            case WeaponBase.SMG:
                return 0.3f;
            case WeaponBase.MachineGun:
                return 0.5f;
            case WeaponBase.PumpShotgun:
                return 0.7f;
            case WeaponBase.CombatShotgun:
                return 0.65f;
            case WeaponBase.BoltActionRifle:
                return 0.15f;
            case WeaponBase.CombatRifle:
                return 0.25f;
            case WeaponBase.Melee:
            default:
                return 0.2f;
        }
    }
    #endregion
}
