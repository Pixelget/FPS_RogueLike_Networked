using UnityEngine;
using System.Collections;

public class AIAttack : MonoBehaviour {

    public EnemyAIManager AIManager;
    //public EnemyAttack attack;
    public EnemyAttack attackScript;

    void Start() {
        enabled = false;
    }

    void OnEnable() {
        // Play attack animation
        // Play SFX for attack
        // Attack (range shoots something, melee checks range)
        //Debug.Log("Enemy is Attacking!");
        if (attackScript) {
            attackScript.enabled = true;
        }
        
        enabled = false; // might need a coroutine or something to handle the attack stuff/timing of attack
    }
}
