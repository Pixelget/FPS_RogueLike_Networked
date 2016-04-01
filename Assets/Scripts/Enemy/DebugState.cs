using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DebugState : MonoBehaviour {

    public EnemyAIManager AIManager;
    GameObject textObj;
    Text text;

	void Start () {
        textObj = DebugText.GetNextObj();
        if (textObj) {
            textObj.SetActive(true);
            text = textObj.GetComponent<Text>();
        }
    }
	
	void Update () {
        if (textObj == null) {
            textObj = DebugText.GetNextObj();
            textObj.SetActive(true);
            text = textObj.GetComponent<Text>();
        }

        text.text = AIManager.GetState();
        textObj.transform.position = transform.position + new Vector3(0f, 1.1f, 0f);
    }

    public void Dieing() {
        textObj.SetActive(false);
    }
}
