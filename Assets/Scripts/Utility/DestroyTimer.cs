using UnityEngine;
using System.Collections;

public class DestroyTimer : MonoBehaviour {

    public float duration = 1f;
    float timer = 0f;
    
	void Update () {
        timer += Time.deltaTime;

        if (timer > duration) {
            Destroy(gameObject);
        }
	}
}
