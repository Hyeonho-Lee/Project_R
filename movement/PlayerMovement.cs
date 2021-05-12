using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [Header("PlayerMovement Status")]
    public float player_speed;
    public float player_jump;
    public float rotation_speed;

    private float horizontal;
    private float vertical;
    public float ground_height;

    [Header("PlayerMovement Check")]
    public bool is_jump;
    public bool is_ground;

    public Vector3 movement;
    public Vector3 player_dir;

    Rigidbody rigidbody;
    TPSCamera tpscamera;

    void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        tpscamera = GameObject.Find("TPS_Camera").GetComponent<TPSCamera>();
    }

    void Start() {
        player_speed = 10.0f;
        player_jump = 7.0f;
        ground_height = 1.75f;
        rotation_speed = 10.0f;
    }

    void Update() {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        player_dir = tpscamera.player_dir.normalized;

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
        movement = movement.normalized;

        Vector3 dir_forward = Quaternion.Euler(0f, 0f, 0f) * player_dir;
        Vector3 dir_f_right = Quaternion.Euler(0f, 45f, 0f) * player_dir;
        Vector3 dir_right = Quaternion.Euler(0f, 90f, 0f) * player_dir;
        Vector3 dir_b_right = Quaternion.Euler(0f, 135f, 0f) * player_dir;
        Vector3 dir_back = Quaternion.Euler(0f, 180f, 0f) * player_dir;
        Vector3 dir_b_left = Quaternion.Euler(0f, 225f, 0f) * player_dir;
        Vector3 dir_left = Quaternion.Euler(0f, 270f, 0f) * player_dir;
        Vector3 dir_f_left = Quaternion.Euler(0f, 315f, 0f) * player_dir;

        // 앞오
        if ((movement.z > 0 && movement.z <= 1) && (movement.x > 0 && movement.x <= 1))
            Move_Vector(dir_f_right);

        // 앞왼
        if ((movement.z > 0 && movement.z <= 1) && (movement.x < 0 && movement.x >= -1))
            Move_Vector(dir_f_left);

        // 뒤왼
        if ((movement.z < 0 && movement.z >= -1) && (movement.x < 0 && movement.x >= -1))
            Move_Vector(dir_b_left);

        // 뒤오
        if ((movement.z < 0 && movement.z >= -1) && (movement.x > 0 && movement.x <= 1))
            Move_Vector(dir_b_right);

        // 앞
        if ((movement.z > 0 && movement.z <= 1) && (movement.x == 0))
            Move_Vector(dir_forward);

        // 뒤
        if ((movement.z < 0 && movement.z >= -1) && (movement.x == 0))
            Move_Vector(dir_back);

        // 오
        if ((movement.x > 0 && movement.x <= 1) && (movement.z == 0))
            Move_Vector(dir_right);

        // 왼
        if ((movement.x < 0 && movement.x >= -1) && (movement.z == 0))
            Move_Vector(dir_left);
    }

    void Move_Vector(Vector3 input_vector) {
        Quaternion newRotation = Quaternion.LookRotation(input_vector);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, newRotation, rotation_speed * Time.deltaTime);
        rigidbody.MovePosition(this.transform.position + input_vector * player_speed * Time.deltaTime);
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
