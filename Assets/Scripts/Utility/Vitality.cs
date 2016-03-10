using UnityEngine;
using System.Collections;

public class Vitality : MonoBehaviour {
    public int CurrentHP;
    public int TotalHP;

    public GameObject DeathObject;

    void Start() {
        CurrentHP = TotalHP;
    }

    public void TakeDamage(int damage) {
        CurrentHP -= damage;

        if (CurrentHP <= 0) {
            Die();
        }
    }

    void Die() {
        Debug.Log(gameObject.name + " Has Died!");
        // Play death animation
        // Spawn item from drop table
        if (DeathObject) {
            Instantiate(DeathObject, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }
}
