using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [Header("PlayerMovement Status")]
    public float player_speed;
    public float player_dash_speed;
    public float player_jump;
    public float rotation_speed;
    public float roll_speed;

    private float move_speed;
    private float horizontal;
    private float vertical;
    private float roll_wait;
    public float roll_time;
    public float ground_height;

    [Header("PlayerMovement Check")]
    public bool is_jump;
    public bool is_dash;
    public bool is_roll;
    public bool is_ctrl;
    public bool is_ground;

    private bool is_timer;

    public Vector3 movement;
    public Vector3 player_dir;
    private Vector3 dir_forward, dir_f_right, dir_right, dir_b_right, dir_back, dir_b_left, dir_left, dir_f_left;
    public RaycastHit ground_hit;

    new Rigidbody rigidbody;
    TPSCamera tpscamera;

    void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        tpscamera = GameObject.Find("TPS_Camera").GetComponent<TPSCamera>();
    }

    void Start() {
        player_speed = 10.0f;
        player_dash_speed = 10.0f;
        player_jump = 7.0f;
        roll_speed = 0.5f;
        ground_height = 1.75f;
        rotation_speed = 10.0f;
        roll_wait = 0.0f;
        roll_time = 0.3f;

        move_speed = player_speed;
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
        if (Input.GetKeyDown(KeyCode.Space))
            is_jump = true;

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            is_dash = true;
            if (!is_timer) {
                roll_wait = Time.time;
                is_timer = true;
            }else if (is_timer && ((Time.time - roll_wait) < roll_time)) {
                StartCoroutine(Roll());
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
            is_dash = false;

        if (Input.GetKeyDown(KeyCode.LeftControl))
            is_ctrl = true;

        if (Input.GetKeyUp(KeyCode.LeftControl))
            is_ctrl = false;
    }

    void Check_Value() {
        if (is_jump && is_ground)
            Jump(player_jump);

        if (is_dash)
            move_speed = player_speed + player_dash_speed;
        else
            move_speed = player_speed;

        if (is_ctrl)
            move_speed = player_speed / 2;

        if (is_timer && ((Time.time - roll_wait) > roll_time)) {
            is_timer = false;
        }
    }

    void Move(float h, float v) {
        movement.Set(h, 0, v);
        movement = movement.normalized;

        dir_forward = Quaternion.Euler(0f, 0f, 0f) * player_dir;
        dir_f_right = Quaternion.Euler(0f, 45f, 0f) * player_dir;
        dir_right = Quaternion.Euler(0f, 90f, 0f) * player_dir;
        dir_b_right = Quaternion.Euler(0f, 135f, 0f) * player_dir;
        dir_back = Quaternion.Euler(0f, 180f, 0f) * player_dir;
        dir_b_left = Quaternion.Euler(0f, 225f, 0f) * player_dir;
        dir_left = Quaternion.Euler(0f, 270f, 0f) * player_dir;
        dir_f_left = Quaternion.Euler(0f, 315f, 0f) * player_dir;

        // 앞오
        if ((movement.z > 0 && movement.z <= 1) && (movement.x > 0 && movement.x <= 1)) {
            Move_Vector(dir_f_right);
            if (is_roll)
                AddImpact(dir_f_right, roll_speed);
        }

        // 앞왼
        if ((movement.z > 0 && movement.z <= 1) && (movement.x < 0 && movement.x >= -1)) {
            Move_Vector(dir_f_left);
            if (is_roll)
                AddImpact(dir_f_left, roll_speed);
        }

        // 뒤왼
        if ((movement.z < 0 && movement.z >= -1) && (movement.x < 0 && movement.x >= -1)) {
            Move_Vector(dir_b_left);
            if (is_roll)
                AddImpact(dir_b_left, roll_speed);
        }

        // 뒤오
        if ((movement.z < 0 && movement.z >= -1) && (movement.x > 0 && movement.x <= 1)) {
            Move_Vector(dir_b_right);
            if (is_roll)
                AddImpact(dir_b_right, roll_speed);
        }

        // 앞
        if ((movement.z > 0 && movement.z <= 1) && (movement.x == 0)) {
            Move_Vector(dir_forward);
            if (is_roll)
                AddImpact(dir_forward, roll_speed);
        }

        // 뒤
        if ((movement.z < 0 && movement.z >= -1) && (movement.x == 0)) {
            Move_Vector(dir_back);
            if (is_roll)
                AddImpact(dir_back, roll_speed);
        }

        // 오
        if ((movement.x > 0 && movement.x <= 1) && (movement.z == 0)) {
            Move_Vector(dir_right);
            if (is_roll)
                AddImpact(dir_right, roll_speed);
        }

        // 왼
        if ((movement.x < 0 && movement.x >= -1) && (movement.z == 0)) {
            Move_Vector(dir_left);
            if (is_roll)
                AddImpact(dir_left, roll_speed);
        }
    }

    void Move_Vector(Vector3 input_vector) {
        Quaternion newRotation = Quaternion.LookRotation(input_vector);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, newRotation, rotation_speed * Time.deltaTime);
        rigidbody.MovePosition(this.transform.position + input_vector * move_speed * Time.deltaTime);
    }

    void Jump(float speed) {
        rigidbody.AddForce(Vector3.up * speed, ForceMode.Impulse);
        is_jump = false;
    }

    void AddImpact(Vector3 dir, float force) {
        //rigidbody.AddForce(dir * force, ForceMode.Impulse);
        this.transform.position += dir * force;
    }

    IEnumerator Roll() {
        is_roll = true;
        //this_am.SetBool("rolling", true);
        //GameObject.Find("Player").GetComponent<PlayerStatus>().stamina -= 20f;
        yield return new WaitForSeconds(0.5f);
        is_roll = false;
        //this_am.SetBool("rolling", false);
    }

    void Check_Ground(float height) {
        Debug.DrawRay(this.transform.position, Vector3.down * height, Color.red);

        if (Physics.Raycast(transform.position, Vector3.down, out ground_hit, height)) {
            if(ground_hit.transform.tag == "Ground") {
                is_ground = true;
                return;
            }
        }else {
            is_ground = false;
        }
    }
}
