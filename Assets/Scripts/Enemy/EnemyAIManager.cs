using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum EnemyAIState { Wander, Chase, Attack, Flee }
public class EnemyAIManager : MonoBehaviour {
    // TODO Break this stuff out into a data layer for enemy data and just load-in the data
    [Header ("AI Values")]
    public float TimeBetweenChoices = 0.5f;
    public bool canFlee = false;
    [Range(0f, 1f)]
    public float fleeUnderHPPercent = 0.3f;
    public float aggroRange = 10f;

    public float wanderSpeed = 0.75f;
    public float fleeSpeed = 2f;
    public float chaseSpeed = 1.5f;
    public float attackCooldown = 2.5f;
    public float attackRange = 1f;

    float attackTimer = 2.5f;
    
    [Header ("Reference Scripts")]
    public Vitality vitality;
    public AIAttack aiAttack;
    public AIChase aiChase;
    public AIFlee aiFlee;
    public AIWander aiWander;
    
    EnemyAIState AIState = EnemyAIState.Wander;

    List<GameObject> playersInRange = new List<GameObject>();
    GameObject ActiveTarget = null;

    void Start () {
        Invoke("StartCR", Random.Range(0f, 1f));
	}

    void StartCR() {
        StartCoroutine("Evaluate");
    }

    void Update() {
        attackTimer += Time.deltaTime;

        // Should attack be part of chase?
        // when in range to attack and CD is good, do it?
        if (AIState == EnemyAIState.Chase) {
            // AND we can attack (atk is off cd and we are in range) then attack
            if (ActiveTarget) {
                if ((attackTimer >= attackCooldown) && (Vector3.Distance(transform.position, ActiveTarget.transform.position) <= attackRange)) {
                    aiAttack.enabled = true;
                    attackTimer = 0f;
                }
            }
        }
    }

    IEnumerator Evaluate() {
        while (true) {
            if (AIState != EnemyAIState.Flee) {
                ResetEvaluateVars();

                // Check health for danger levels
                if ((canFlee) && (vitality.CurrentHP / vitality.TotalHP < fleeUnderHPPercent)) {
                    // Less than required % hp and can flee
                    AIState = EnemyAIState.Flee;
                } else {
                    // Check if player in range
                    GetActionablePlayers();

                    if (playersInRange.Count > 0) {
                        // There are targets, find the closest and chase/attack them
                        // Determine the Active Target

                        if (ActiveTarget) {
                            // There is currently an active target
                            if (!playersInRange.Contains(ActiveTarget)) {
                                // switch active target
                                ActiveTarget = playersInRange[0]; // TODO Fix this
                            }
                            // else use the current one
                        } else {
                            // Set active target
                            ActiveTarget = playersInRange[0];// TODO Fix this
                        }

                        // is in range to attack and can attack then attack
                        // else chase
                        AIState = EnemyAIState.Chase;
                    } else {
                        // Wander
                        AIState = EnemyAIState.Wander;
                        ActiveTarget = null;
                    }
                }

                ExecuteDecision();
            }

            yield return new WaitForSeconds(TimeBetweenChoices);
        }
    }

    void ExecuteDecision() {
        // disable all and enable the correct one
        switch(AIState) {
        case EnemyAIState.Chase:
            aiFlee.enabled = false;
            aiChase.enabled = true;
            aiWander.enabled = false;
            break;
        case EnemyAIState.Flee:
            aiFlee.enabled = true;
            aiChase.enabled = false;
            aiWander.enabled = false;
            break;
        case EnemyAIState.Wander:
            aiFlee.enabled = false;
            aiChase.enabled = false;
            aiWander.enabled = true;
            break;
        }
    }

    public void NotifyOfAttack(GameObject playerThatAttacked) {
        // Could build in that weapons have a threat level and that threat level determines the chance of enemy target switching
        // Do some aggro math here for now its a random chance it will change targets to the attacker
        if (ActiveTarget) {
            // 20% chance to change the target when getting attacked or 100% chance to run away from the last source of damage
            if ((Random.Range(0f, 1f) < 0.2f) || (AIState == EnemyAIState.Flee)) {
                ActiveTarget = playerThatAttacked;
            }
        } else {
            ActiveTarget = playerThatAttacked;

            if (AIState == EnemyAIState.Wander) {
                AIState = EnemyAIState.Chase;
            }

            ExecuteDecision();
        }
    }

    void GetActionablePlayers() {
        List<Collider> hitColliders = Physics.OverlapSphere(transform.position, aggroRange).ToList<Collider>();
        for (int i = 0; i < hitColliders.Count; i++) {
            // If this player distance is within range keep it
            if (hitColliders[i].transform.root.tag == "Player") {
                playersInRange.Add(hitColliders[i].transform.root.gameObject);
            }
        }

        if (ActiveTarget) {
            playersInRange.Add(ActiveTarget);
        }
    }

    public GameObject GetActiveTarget() {
        return ActiveTarget;
    }

    void ResetEvaluateVars() {
        playersInRange.Clear();
    }

    public string GetState() {
        return AIState.ToString();
    }
}
