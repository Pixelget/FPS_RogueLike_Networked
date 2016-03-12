using UnityEngine;
using System.Collections;

public class AIChase : MonoBehaviour {

    public EnemyAIManager AIManager;
    GameObject target;
    MapGenerator map;
    Rigidbody rigid;

    void Start() {
        rigid = GetComponent<Rigidbody>();
        map = GameObject.FindGameObjectWithTag("MapObject").GetComponent<MapGenerator>();
        this.enabled = false;
    }

    void OnEnable() {
        target = AIManager.GetActiveTarget();
        StartCoroutine("UpdateTarget");
    }

    void OnDisable() {
        StopCoroutine("UpdateTarget");
    }

    void Update() {
        Chase();
    }

    void Chase() {
        if (target) {
            Vector3 direction = (target.transform.position - transform.position);
            direction.y = 0f;
            direction = direction.normalized;

            rigid.velocity = direction * AIManager.fleeSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, target.transform.position) < (AIManager.attackRange - (AIManager.attackRange * 0.2f))) {
                rigid.velocity = Vector3.zero;
            }
        }
    }

    IEnumerator UpdateTarget() {
        while (true) {
            target = AIManager.GetActiveTarget();
            yield return new WaitForSeconds(1f);
        }
    }
}
