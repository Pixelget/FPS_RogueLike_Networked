using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Weapon : MonoBehaviour {
	[Header ("Name and Projectile")]
    public string Name;

    public Transform shootFromObject;

    public AudioSource audioSource;
    public AudioClip shotSFX;
    public AudioClip reloadSFX;
    public AudioClip emptyClipSFX;

    public Sprite GunSprite;
    public SpriteRenderer gunRenderer;

    public GameObject MuzzleFlash;
    public GameObject WallHitFXPrefab;

    Animator animator;
	
	[Header ("Cooldowns")]
	public float totalCooldown;
    public float cooldown;
	public float fireRateTotal;
    public float fireRate;
	public int shotsPerBurst;
    public int burstShots;
	public int totalAmmo;
    public int ammo;
	public float reloadTimeTotal;
    public float reloadTime;

    [Header("Accuracy Variables")]
    public float range;
	public float shotSpread;
	public float accuracy;
	public float accuracyReductionTime;
    float accuracyReductionDelayTimer = 0f;
    public float accuracyReductionDelay;
    [Range (0f, 1f)]
	public float accuracyWeight;
    [Range(0f, 1f)]
    public float AccuracyReductionRate;
    float tempXSpread;
	float tempYSpread;

	[Header ("Projectile Stats")]
	public int damage;
	public float bulletHitDelay;
	
	List<GameObject> bulletSpawns;
	int spawnLocationCounter = 0;
	int spawnLocationTotal = 0;
	
	[Header ("Debug Fire Weapon")]
	public Vector2 direction;
	public bool isFiring;
    public bool reloading;
    public bool canShoot;
    public bool Fire;

    bool firstShotAccuracy = true;

	Vector2 AimDirection;
	Vector2 MoveDirection;

	Image ammoReloadScale;
	Text ammoRemainingText;
	float ammoScale = 1f;

    int currentSoundPriority = 10;
	
	void Start() {
		/* Static Starting Variables */
		isFiring = false;
		canShoot = false;
		Fire = false;

		cooldown = totalCooldown;
		fireRate = fireRateTotal;
		burstShots = shotsPerBurst;
		ammo = totalAmmo;

        gunRenderer.sprite = GunSprite;

        GetBulletSpawnLocations();

        //audioSource = transform.parent.GetComponent<AudioSource>();
        //audioSource.volume = GameManager.Instance.Volume_SoundEffects * GameManager.Instance.Volume_Master;
        audioSource.clip = shotSFX;

        animator = gunRenderer.gameObject.GetComponent<Animator>();
    }

	public void ResetWeapon() {
		cooldown = totalCooldown;
		fireRate = fireRateTotal;
		burstShots = shotsPerBurst;
		ammo = totalAmmo;
        isFiring = false;
	}

	void GetBulletSpawnLocations() {
		bulletSpawns = new List<GameObject>();
		foreach (Transform child in transform) {
			if (child.gameObject.tag == "bulletSpawnLocation") {
				bulletSpawns.Add(child.gameObject);
			}
		}
		spawnLocationTotal = bulletSpawns.Count;
	}
	
	void Update() {
		if (AimDirection.magnitude > 0.25f) {
			direction = AimDirection.normalized;
		} else if (MoveDirection.magnitude > 0.25f) {
			direction = MoveDirection.normalized;
		}

		cooldown += Time.deltaTime;
		if ((cooldown > totalCooldown) && (ammo > 0) && (!reloading)) {
			// Can shoot gun
			canShoot = true;
		} else if ((cooldown > totalCooldown) && (ammo <= 0) && (!reloading) && (isFiring)) {
            //Reload();
            PlayAudio(emptyClipSFX, 5);
            cooldown = 0f;
        }
		
		if (reloading) {
            if (isFiring) {
                // Play Empty Clip Sound
                //PlayAudio(emptyClipSFX, 5);
            }
			reloadTime += Time.deltaTime;
			if (reloadTime > reloadTimeTotal) {
				reloadTime = 0f;
				reloading = false;
				ammo = totalAmmo;
			}
			//ammoScale = reloadTime / reloadTimeTotal;
			//ammoReloadScale.rectTransform.localScale = new Vector3 (ammoScale, ammoReloadScale.rectTransform.localScale.y, ammoReloadScale.rectTransform.localScale.z);
			//ammoRemainingText.text = "none";
		} //else {
			//if ((ammoReloadScale != null) && (ammoRemainingText != null)) {
				//ammoScale = (float) ammo / totalAmmo;
				//ammoReloadScale.rectTransform.localScale = new Vector3 (ammoScale, ammoReloadScale.rectTransform.localScale.y, ammoReloadScale.rectTransform.localScale.z);
				//ammoRemainingText.text = ammo.ToString ();
			//}
		//}
		
		if (canShoot && isFiring) {
			// Can shoot
			Fire = true;
			cooldown = 0f;
        }
		
		if (Fire) {
			// Empty the clip
			fireRate += Time.deltaTime;
			if (fireRate > fireRateTotal) {
                audioSource.clip = shotSFX;
                if (fireRateTotal < 0.01f) {
                    if (burstShots == shotsPerBurst) {
                        // only play the audio once per round

                        animator.SetTrigger("Fire");
                    }
                } else {
                    PlayAudio(shotSFX, 5);
                }

                // Fire a shot
				Shoot();
				
				burstShots--;
				fireRate = 0f;
			}
			
			// if no ammo remains
			if (burstShots <= 0) {
				// Reset Weapon
				// Set the canShoot and Fire to false
				canShoot = false;
				Fire = false;
				// Set the ammo to totalAmmo
				burstShots = shotsPerBurst;
				ammo--;
			}
		}
		
		if (isFiring) {
			accuracy += Time.deltaTime;
			if (accuracy > accuracyReductionTime) {
				accuracy = accuracyReductionTime;
			}
		} else {
            accuracyReductionDelayTimer += Time.deltaTime;
            if (accuracyReductionDelayTimer > accuracyReductionDelay) {
                accuracy -= Time.deltaTime * AccuracyReductionRate;
            }
			if (accuracy <= 0f) {
                accuracy = 0f;
                firstShotAccuracy = true;
                accuracyReductionDelayTimer = 0f;
            }
		}

        animator.SetBool("Reloading", reloading);
    }
	
	public void Reload() {
        PlayAudio(reloadSFX, 2);
        reloading = true;
        reloadTime = 0f;
    }
	
	protected void Shoot() {
        // Raycast collision message sending
        // Do this in another script
        // Do mussle flash/sfx/random tracer rounds
        PlayAudio(shotSFX, 5);
        //Debug.DrawRay(transform.position + new Vector3(0f, 0.15f, 0f), -transform.up * distance, Color.red);
        MuzzleFlash.SetActive(true);

        if (burstShots == shotsPerBurst) {
            // only play the Animation once per round of shots
            animator.SetTrigger("Fire");
        }

        // Accuracy calculation
        tempXSpread = Random.Range(-shotSpread, shotSpread);
        tempYSpread = Random.Range(-shotSpread, shotSpread);
        float tempZSpread = Random.Range(-shotSpread, shotSpread);

        Vector3 dir;
        if (firstShotAccuracy) {
            dir = shootFromObject.forward;
            firstShotAccuracy = false;
        } else {
            dir = new Vector3(shootFromObject.forward.x + ((tempXSpread * (1f - accuracyWeight)) + (tempXSpread * (accuracy / accuracyReductionTime) * accuracyWeight)), shootFromObject.forward.y + ((tempYSpread * (1f - accuracyWeight)) + (tempYSpread * (accuracy / accuracyReductionTime) * accuracyWeight)), shootFromObject.forward.z + ((tempZSpread * (1f - accuracyWeight)) + (tempZSpread * (accuracy / accuracyReductionTime) * accuracyWeight))).normalized;
        }

        RaycastHit hit;
        Debug.DrawRay(shootFromObject.position, dir * range, Color.red, 25f);
        if (Physics.Raycast(shootFromObject.position, dir, out hit, range)) {
            hit.collider.gameObject.SendMessageUpwards("OnHit", new HitDetails(damage, hit.point, transform.root.gameObject), SendMessageOptions.DontRequireReceiver);
        }
        CancelInvoke("disableFlash");
        Invoke("disableFlash", 0.03f); // TODO Fix the issue with the timing of the disable

        // Could do a notice on shot to enemys in range to set an active target if they do not have one
    }

    void disableFlash() {
        MuzzleFlash.SetActive(false);
    }
	
    public void Firing(bool f) {
        isFiring = f;
    }
	
	void AimDirectionUpdate(Vector3 dir) {
		AimDirection = dir;
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