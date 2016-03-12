using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EnemyAttack_MeleeSwipe : EnemyAttack {

    public EnemyAIManager AIManager;

    public int damage = 10;

    void OnEnable() {
        List<Collider> hitColliders = Physics.OverlapSphere(transform.position, AIManager.attackRange).ToList<Collider>();
        HitDetails hit = new HitDetails(damage, transform.position, gameObject);

        for (int i = 0; i < hitColliders.Count; i++) {
            // If this player distance is within range keep it
            if (hitColliders[i].transform.root.tag == "Player") {
                hitColliders[i].SendMessageUpwards("OnHit", hit);
            }
        }

        enabled = false;
    }
}
