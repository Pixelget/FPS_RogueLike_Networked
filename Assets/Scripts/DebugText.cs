using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class DebugText {
    static List<GameObject> list = new List<GameObject>();
    
    public static void init() {
        list = GameObject.FindGameObjectsWithTag("debug_text").ToList<GameObject>();

        for (int i = 0; i < list.Count; i++) {
            list[i].SetActive(false);
        }
    }

    public static GameObject GetNextObj() {
        for (int i = 0; i < list.Count; i++) {
            if (!list[i].activeSelf) {
                return list[i];
            }
        }

        return null;
    }
}
