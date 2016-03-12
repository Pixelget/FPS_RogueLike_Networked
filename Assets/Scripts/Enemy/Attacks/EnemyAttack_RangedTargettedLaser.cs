using UnityEngine;
using System.Collections;

[RequireComponent (typeof(LineRenderer))]
public class EnemyAttack_RangedTargettedLaser : EnemyAttack {
    
    public EnemyAIManager AIManager;
    public GameObject laserFromObject;
    public int Damage = 1;
    public float TickRate = 0.133f;
    public int Ticks = 15;

    float timer = 0f;
    int tickCount = 0;

    LineRenderer line;

    void Awake() {
        line = GetComponent<LineRenderer>();
        enabled = false;
    }

	void OnEnable() {
        // Reset the variables
        tickCount = 0;
        timer = TickRate;

        // Activate the LineRenderer
        line.SetVertexCount(2);
    }

    void Update() {
        if (AIManager.GetActiveTarget()) {
            line.SetPosition(0, laserFromObject.transform.position);
            line.SetPosition(1, AIManager.GetActiveTarget().transform.position);

            if (tickCount < Ticks) {
                timer += Time.deltaTime;

                if (timer >= TickRate) {
                    Ray ray = new Ray(laserFromObject.transform.position, (AIManager.GetActiveTarget().transform.position - laserFromObject.transform.position).normalized);
                    RaycastHit hit;
                    bool hitplayer = false;

                    if (Physics.Raycast(ray, out hit, AIManager.attackRange)) {
                        if (hit.collider.tag == "Player") {
                            hit.collider.SendMessageUpwards("OnHit", new HitDetails(Damage, hit.point, gameObject));
                            tickCount++;
                            hitplayer = true;
                        }
                    }

                    if (!hitplayer) {
                        enabled = false;
                    }

                    timer = 0f;
                }
            } else {
                enabled = false;
            }
        }
    }

    void OnDisable() {
        line.SetVertexCount(0);
    }
}
