using UnityEngine;
using System.Collections;

public class AIFlee : MonoBehaviour {

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
        Run();
    }

    void Run() {
        if (target) {
            Vector3 direction = (target.transform.position - transform.position);
            direction.y = 0f;
            direction = direction.normalized;

            rigid.velocity = -direction * AIManager.fleeSpeed * Time.deltaTime;
        }
    }

    IEnumerator UpdateTarget() {
        while(true) {
            target = AIManager.GetActiveTarget();
            yield return new WaitForSeconds(1f);
        }
    }
}
