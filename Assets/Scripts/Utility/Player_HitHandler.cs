using UnityEngine;
using System.Collections;

public class Player_HitHandler : MonoBehaviour {
    public GameObject HitVFX;
    public GameObject HitDecal;
    public AudioClip HitSFX;
    public PlayerVitality vitality;
    public EnemyAIManager enemyAIManager;

    public void OnHit(HitDetails hit) {
        //Debug.Log(gameObject.name + " has been hit for " + hit.Damage + ".");
        if (HitVFX != null) {
            // Spawn a hit effect at the hit point
            // ex. blood, sparks etc.
            Instantiate(HitVFX, hit.HitPosition, transform.rotation);
        }

        if (HitDecal != null) {
            // Spawn a hit decal at the hit location
            // ex. bullet hit, wood chip
            Instantiate(HitDecal, hit.HitPosition, transform.rotation);
        }

        if (HitSFX != null) {
            // Play hit sound for this enemy
            // ex. wood sound for boxes, grunt for enemies, etc.
        }

        if (vitality != null) {
            // There is an attached vitality component so apply damage
            vitality.TakeDamage(hit.Damage);
        }

        if (enemyAIManager != null) {
            enemyAIManager.NotifyOfAttack(hit.Attacker);
        }

        // Trigger hit animation / WhiteFlash
    }
}
