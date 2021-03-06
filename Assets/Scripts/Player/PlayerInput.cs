﻿using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
    public PlayerManager player;
    public PlayerMovement movement;
    public GameObject CameraMount;
    MouseLook mouseLook;
    Camera playerCamera;

    public WeaponManager weapon;
    public float fireSprintDelay = 0f;

    public PlayerInteraction interaction;

	void Start () {
        mouseLook = GetComponent<MouseLook>();
        playerCamera = GetComponentInChildren<Camera>();
        mouseLook.Init(CameraMount);
	}
	
	void Update () {
        if (fireSprintDelay > 0f) {
            fireSprintDelay -= Time.deltaTime;
        }

        // Movement
        Vector2 MovementDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) {
            MovementDirection.x += 1f;
        }
        if (Input.GetKey(KeyCode.S)) {
            MovementDirection.x -= 1f;
        }
        if (Input.GetKey(KeyCode.A)) {
            MovementDirection.y -= 1f;
        }
        if (Input.GetKey(KeyCode.D)) {
            MovementDirection.y += 1f;
        }

        movement.Move(MovementDirection.normalized, CameraMount.transform.localRotation);
        mouseLook.LookRotation();
        
        // Jump Action
        if (Input.GetKeyDown(KeyCode.Space)) {
            // Jump if on the ground
            movement.Jump();
        }

        // Fire Action
        if (Input.GetMouseButtonDown(0) && CanFire()) {
            weapon.IsFiring(true);
        }
        if (Input.GetMouseButtonUp(0)) {
            weapon.IsFiring(false);
        }

        // Reload Action
        if (Input.GetKeyDown(KeyCode.R)) {
            // Reload
            Debug.Log("Reloading");
            weapon.Reload();
        }

        // Sprint Action
        if (Input.GetKey(KeyCode.LeftShift)) {
            // Sprint On
            movement.Sprint(true);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            // Sprint Off
            movement.Sprint(false);

            // Delay on fire from coming out of sprint
            fireSprintDelay = 0.5f;
        }

        // Use/Talk Action
        if (Input.GetKeyDown(KeyCode.E)) {
            // Use
            //Debug.Log("Using/Talking/Interacting");
            interaction.Interact();
        }

        // Switch Weapons
        if (Input.GetKeyDown(KeyCode.Q)) {
            player.Inventory.SwapWeapon();
        }

        // Debug break of the scene
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Debug.Break();
        }
    }

    bool CanFire() {
        if (fireSprintDelay > 0f) {
            return false;
        }
        if (movement.Sprinting) {
            return false;
        }

        return true;
    }
}
