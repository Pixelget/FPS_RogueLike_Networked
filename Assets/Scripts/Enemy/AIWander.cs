using UnityEngine;
using System.Collections;

public class AIWander : MonoBehaviour {

    public EnemyAIManager AIManager;

    Vector3 targetLocation;
    MapGenerator map;
    Rigidbody rigid;
    float timer = 0f;

    void Start() {
        rigid = GetComponent<Rigidbody>();
        map = GameObject.FindGameObjectWithTag("MapObject").GetComponent<MapGenerator>();
        this.enabled = false;
    }

    void OnEnable() {
        targetLocation = transform.position + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
        timer = 0f;
        // TODO Verify the position
    }

    void Update () {
        Move();
	}

    void Move() {
        timer += Time.deltaTime;

        if (Vector3.Distance(transform.position, targetLocation) < 0.1f || timer > 1f) {
            timer = 0f;
            targetLocation = transform.position + new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));
        }

        Vector3 direction = (targetLocation - transform.position);
        direction.y = 0f;
        direction = direction.normalized;

        rigid.velocity = direction * AIManager.wanderSpeed * Time.deltaTime;
    }
}
