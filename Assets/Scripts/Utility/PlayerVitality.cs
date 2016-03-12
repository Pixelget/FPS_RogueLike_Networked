using UnityEngine;
using System.Collections;

public enum PlayerState { Alive, Down, Respawn, Die }
public class PlayerVitality : MonoBehaviour {
    public int CurrentHP;
    public int TotalHP;

    public GameObject DeathObject;
    public MapGenerator map;

    PlayerState state = PlayerState.Alive;

    float timer = 0f;
    float DownedTime = 3f;

    void Start() {
        CurrentHP = TotalHP;
        map = GameObject.FindGameObjectWithTag("MapObject").GetComponent<MapGenerator>();
    }

    public void TakeDamage(int damage) {
        CurrentHP -= damage;

        if (CurrentHP <= 0) {
            Die();
        }
    }

    void Update() {
        if (state == PlayerState.Down) {
            timer += Time.deltaTime;

            if (timer > DownedTime) {
                // after downed state expires die/respawn
                state = PlayerState.Respawn;
                timer = 0f;
            }
        }

        if (state == PlayerState.Respawn) {
            state = PlayerState.Alive;

            if (map) {
                transform.position = map.spawnLocation;
            }
            CurrentHP = TotalHP;
        }
    }

    void Die() {
        Debug.Log(gameObject.name + " Has Died! in player vitality.");
        // Play death animation
        // Spawn item from drop table
        if (DeathObject) {
            Instantiate(DeathObject, transform.position, transform.rotation);
        }

        // Go into a downed state
        state = PlayerState.Down;
    }
}
