using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArtRenderer : MonoBehaviour {

    public List<SpriteRenderer> spriteList;

    Vector3 camLocation;

    float timer = 0f;
    float TickRate = 0.33f;

	void FixedUpdate () {
        timer += Time.fixedDeltaTime;

        if (timer > TickRate) {
            camLocation = Camera.main.transform.position;
            SpriteRenderer[] array = new SpriteRenderer[spriteList.Count];

            float distance = Mathf.Infinity;

            for (int i = 0;i < spriteList.Count;i++) {
                distance = Vector3.Distance(spriteList[i].gameObject.transform.position, camLocation);
                array[i] = spriteList[i];
                for (int x = i + 1;x < spriteList.Count;x++) {
                    if (distance > Vector3.Distance(spriteList[x].gameObject.transform.position, camLocation)) {
                        distance = Vector3.Distance(spriteList[x].gameObject.transform.position, camLocation);
                        array[i] = spriteList[x];
                    }
                }
            }

            for (int i = 0;i < array.Length - 1;i++) {
                array[i].sortingOrder = i;
            }
        }
	}
}
