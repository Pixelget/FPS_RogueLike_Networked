using UnityEngine;
using System.Collections;

public class RotateTowardsCamera : MonoBehaviour {
    public GameObject player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update () {
        if (player) {
            Vector3 temp = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.LookAt(Camera.main.transform);
            transform.rotation = Quaternion.Euler(new Vector3(temp.x, player.transform.rotation.eulerAngles.y, temp.z));
        } else {
            player = GameObject.FindGameObjectWithTag("Player");
        }
	}
}
