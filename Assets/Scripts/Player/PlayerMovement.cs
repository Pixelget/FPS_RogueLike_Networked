using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {
    [Header("Movement Variables")]
    public float speed = 70f;
    public float sprintModifier = 1.5f;
    public float drag = 0.9f;

    float tempSpeed = 0f;

    Vector3 MoveDirection = Vector2.zero;
    Vector3 FacingDirection = Vector2.zero;

    Vector3 Velocity = Vector3.zero;
    public Vector3 Gravity = new Vector3(0f, -19.81f, 0f);
    Rigidbody rigid;

    public float jumpCooldown = 0f;
    float jumpTime = 0.5f;
    float jumpTimer = 0f;
    public Vector3 jumpVector = new Vector3(0f, 10f, 0f);

    public bool Sprinting = false;

    public float footstepTimer = 0f;
    public float footstepDelay = 1f;

    public AudioSource source;
    public AudioClip step;

    PlayerAnimationManager playerAnimations;

    void Start() {
        rigid = GetComponent<Rigidbody>();
        playerAnimations = GetComponent<PlayerAnimationManager>();
    }

    void Update() {
        if (jumpCooldown <= 0f) {
            jumpCooldown = 0f;
        } else {
            jumpCooldown -= Time.deltaTime;
            jumpTimer += Time.deltaTime;

            if (jumpTimer <= jumpTime) {
                Velocity += jumpVector * (1f - (jumpTimer/jumpTime)) * Time.deltaTime;
            }
        }
       
        if (!OnSolid()) {
            Velocity += Gravity * Time.deltaTime;
        }

        if (Sprinting) {
            if (Velocity.magnitude <= 0.1f) {
                Sprinting = false;
            }
            tempSpeed = speed * sprintModifier;
        } else {
            tempSpeed = speed;
        }
        
        Velocity += ((transform.forward * MoveDirection.x) + (transform.right * MoveDirection.z)) * tempSpeed * Time.deltaTime;
        rigid.velocity = Velocity;
        
        if (Velocity.magnitude > 0.1f) {
            if (Sprinting) {
                footstepTimer += Time.deltaTime * 1.5f;
            } else {
                footstepTimer += Time.deltaTime;
            }
        }
        if (footstepTimer > footstepDelay) {
            // Play step sound
            source.clip = step;
            source.Play();
            footstepTimer = 0f;
        }

        Velocity *= drag;


        //playerAnimations.GunAnimator.SetBool("Sprinting", Sprinting);
    }

    public void Move(Vector2 direction, Quaternion facing) {
        // Rotate for forward direction?
        MoveDirection = new Vector3(direction.x, 0f, direction.y);
        transform.localRotation = facing;// new Quaternion(0f, facing.y, 0f, facing.w);
    }

    public void Jump() {
        if (OnSolid() && (jumpCooldown <= 0f)) {
            Debug.Log("Jumping");
            jumpCooldown = 0.8f;
            jumpTimer = 0f;
        }
    }

    public void Sprint(bool b) {
        // TODO Set the animation state here

        if (Velocity.magnitude > 0.1f) {
            Sprinting = b;
            playerAnimations.GunAnimator.SetBool("Sprinting", b);
        }
    }

    bool OnSolid() {
        float distance = 0.3f;
        //Debug.DrawRay(transform.position + new Vector3(0f, 0.15f, 0f), -transform.up * distance, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0f, 0.15f, 0f), -transform.up, out hit, distance)) {
            //Debug.Log("Hit: " + hit.collider.name + ", " + hit.collider.tag);
            return true;
        }

        return false;
    }
}
