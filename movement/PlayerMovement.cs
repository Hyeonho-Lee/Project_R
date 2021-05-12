using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [Header("PlayerMovement Status")]
    public float player_speed;
    public float player_jump;

    private float horizontal;
    private float vertical;
    public float ground_height;

    public bool is_jump;
    public bool is_ground;

    Vector3 movement;

    Rigidbody rigidbody;

    void Awake() {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start() {
        player_speed = 5f;
        player_jump = 10f;
        ground_height = 1.75f;
    }

    void Update() {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        Input_Key();
        Check_Value();
    }

    void FixedUpdate() {
        Move(horizontal, vertical);
        Check_Ground(ground_height);
    }

    void Input_Key() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            is_jump = true;
        }
    }

    void Check_Value() {
        if(is_jump == true && is_ground == true) {
            Jump(player_jump);
        }
    }

    void Move(float h, float v) {
        movement.Set(h, 0, v);
        movement = movement.normalized * player_speed * Time.deltaTime;

        rigidbody.MovePosition(this.transform.position + movement);
    }

    void Jump(float speed) {

        rigidbody.AddForce(Vector3.up * speed, ForceMode.Impulse);
        is_jump = false;
    }

    void Check_Ground(float height) {
        RaycastHit hit;
        Debug.DrawRay(this.transform.position, Vector3.down * height, Color.red);

        if (Physics.Raycast(transform.position, Vector3.down, out hit, height)) {
            if(hit.transform.tag == "Ground") {
                is_ground = true;
                return;
            }
        }else {
            is_ground = false;
        }
    }
}
