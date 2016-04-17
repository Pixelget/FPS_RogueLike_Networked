using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ItemDropManager))]
[RequireComponent(typeof(DebugState))]
public class Vitality : MonoBehaviour {
    public int CurrentHP;
    public int TotalHP;

    public GameObject DeathObject;

    bool diedAlready = false;

    void Start() {
        CurrentHP = TotalHP;
    }

    public void TakeDamage(int damage) {
        CurrentHP -= damage;

        if ((CurrentHP <= 0) && (!diedAlready)) {
            diedAlready = true;
            Die();
        }
    }

    void Die() {
        
        // Play death animation
        // Spawn item from drop table
        if (DeathObject) {
            Instantiate(DeathObject, transform.position, transform.rotation);
        }
        
        GetComponent<ItemDropManager>().DropItems();
        GetComponent<DebugState>().Dieing();

        Destroy(gameObject);
    }
}
