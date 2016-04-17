using UnityEngine;
using System.Collections;

public class Interact_ExitLevelObject : Interactable {
    public string nextLevel = ""; // TODO change this dynamically on generation
    float timer = 0f;
    public float TimeToAdvance = 5f;
    bool leave = false;

    public override void Interact() {
        Debug.Log(Sender.name + " interacted with " + this.name);
        leave = true;
    }

    void Update() {
        if (leave) {
            timer += Time.deltaTime;
            if (timer > TimeToAdvance) {
                UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevel);
            }
        }
    }
}
