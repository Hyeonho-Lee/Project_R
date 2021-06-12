using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private float horizontal;
    private float vertical;

    public float mass;
    public float jump_speed;
    public float walk_speed;
    public float dash_speed;
    public float move_speed;
    public float velocity;
    public float roll_wait;
    public float roll_time;
    public float roll_speed;

    public bool is_ground;
    public bool is_roll;
    public bool is_timer;
    public bool is_dash;
    public bool is_ctrl;
    public float ground_height;
    public float ground_range;

    public LayerMask ground_hit;

    public Vector3 movement;
    public Vector3 player_dir;
    public Vector3 player_forward;
    public Vector3 impact;
    private Vector3 dir_forward, dir_f_right, dir_right, dir_b_right, dir_back, dir_b_left, dir_left, dir_f_left;

    CharacterController cc;
    TPSCamera tpscamera;

    void Awake() {
        cc = GetComponent<CharacterController>();
        tpscamera = GameObject.Find("TPS_Camera").GetComponent<TPSCamera>();
    }

    void Start() {
        mass = 1.0f;
        jump_speed = 1.2f;
        walk_speed = 5.0f;
        dash_speed = 5.0f;
        velocity = 0.0f;
        ground_height = -0.08f;
        ground_range = 0.28f;
        roll_wait = 0.0f;
        roll_time = 0.3f;
        roll_speed = 30.0f;
        impact = Vector3.zero;
    }

    void Update() {
        Check_Input();
        Check_Ground();
        Check_Value();
    }

    void FixedUpdate() {
        Move();
    }

    void OnDrawGizmos() {
        Color green = new Color(0.0f, 1.0f, 0.0f, 0.5f);
        Color red = new Color(1.0f, 0.0f, 0.0f, 0.5f);

        if (is_ground)
            Gizmos.color = green;
        else
            Gizmos.color = red;

        Gizmos.DrawSphere(transform.position - new Vector3(0, ground_height, 0), ground_range);
    }

    void Check_Input() {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        player_dir = tpscamera.player_dir.normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            is_dash = true;
            if (!is_timer) {
                roll_wait = Time.time;
                is_timer = true;
            } else if (is_timer && ((Time.time - roll_wait) < roll_time)) {
                StartCoroutine(Roll());
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
            is_dash = false;

        if (Input.GetKeyDown(KeyCode.Space) && is_ground)
            StartCoroutine(Jump());

        if (Input.GetKeyDown(KeyCode.LeftControl))
            is_ctrl = true;

        if (Input.GetKeyUp(KeyCode.LeftControl))
            is_ctrl = false;
    }

    void Check_Value() {
        if (is_ground) {
            if (velocity < 0.0f) {
                velocity = -2.0f;
            }
        } else {
            if (velocity < 53.0f) {
                velocity += -15.0f * Time.deltaTime;
            }
        }

        if (is_dash)
            move_speed = walk_speed + dash_speed;
        else
            move_speed = walk_speed;

        if (is_ctrl)
            move_speed = move_speed / 2;

        if (is_timer && ((Time.time - roll_wait) > roll_time)) {
            is_timer = false;
        }
    }

    void Move() {
        movement.Set(horizontal, 0, vertical);
        movement = movement.normalized;

        dir_forward = Quaternion.Euler(0f, 0f, 0f) * player_dir;
        dir_f_right = Quaternion.Euler(0f, 45f, 0f) * player_dir;
        dir_right = Quaternion.Euler(0f, 90f, 0f) * player_dir;
        dir_b_right = Quaternion.Euler(0f, 135f, 0f) * player_dir;
        dir_back = Quaternion.Euler(0f, 180f, 0f) * player_dir;
        dir_b_left = Quaternion.Euler(0f, 225f, 0f) * player_dir;
        dir_left = Quaternion.Euler(0f, 270f, 0f) * player_dir;
        dir_f_left = Quaternion.Euler(0f, 315f, 0f) * player_dir;

        if (movement.z == 0 && movement.x == 0) {
            if (is_roll) {
                StartCoroutine(Add_Impact(transform.forward, roll_speed));
                is_roll = false;
            }else {
                cc.Move(movement * (walk_speed * Time.deltaTime) + new Vector3(0.0f, velocity, 0.0f) * Time.deltaTime);
            }
        }

        // 앞오
        if ((movement.z > 0 && movement.z <= 1) && (movement.x > 0 && movement.x <= 1)) {
            if (is_roll) {
                StartCoroutine(Add_Impact(dir_f_right, roll_speed));
                is_roll = false;
            }else {
                Move_Vector(dir_f_right, dir_f_right);
            }
        }

        // 앞왼
        if ((movement.z > 0 && movement.z <= 1) && (movement.x < 0 && movement.x >= -1)) {
            if (is_roll) {
                StartCoroutine(Add_Impact(dir_f_left, roll_speed));
                is_roll = false;
            }else {
                Move_Vector(dir_f_left, dir_f_left);
            }
        }

        // 뒤왼
        if ((movement.z < 0 && movement.z >= -1) && (movement.x < 0 && movement.x >= -1)) {
            if (is_roll) {
                StartCoroutine(Add_Impact(dir_b_left, roll_speed));
                is_roll = false;
            }else {
                Move_Vector(dir_b_left, dir_b_left);
            }
        }

        // 뒤오
        if ((movement.z < 0 && movement.z >= -1) && (movement.x > 0 && movement.x <= 1)) {
            if (is_roll) {
                StartCoroutine(Add_Impact(dir_b_right, roll_speed));
                is_roll = false;
            }else {
                Move_Vector(dir_b_right, dir_b_right);
            }
        }

        // 앞
        if ((movement.z > 0 && movement.z <= 1) && (movement.x == 0)) {
            if (is_roll) {
                StartCoroutine(Add_Impact(dir_forward, roll_speed));
                is_roll = false;
            }else {
                Move_Vector(dir_forward, dir_forward);
            }
        }

        // 뒤
        if ((movement.z < 0 && movement.z >= -1) && (movement.x == 0)) {
            if (is_roll) {
                StartCoroutine(Add_Impact(dir_back, roll_speed));
                is_roll = false;
            }else {
                Move_Vector(dir_back, dir_back);
            }
        }

        // 오
        if ((movement.x > 0 && movement.x <= 1) && (movement.z == 0)) {
            if (is_roll) {
                StartCoroutine(Add_Impact(dir_right, roll_speed));
                is_roll = false;
            }else {
                Move_Vector(dir_right, dir_right);
            }
        }

        // 왼
        if ((movement.x < 0 && movement.x >= -1) && (movement.z == 0)) {
            if (is_roll) {
                StartCoroutine(Add_Impact(dir_left, roll_speed));
                is_roll = false;
            }else {
                Move_Vector(dir_left, dir_left);
            }
        }

        if (impact.magnitude > 0.2)
            cc.Move(impact * Time.deltaTime);
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
    }

    void Move_Vector(Vector3 input_vector, Vector3 rotate_vector) {
        Quaternion newRotation = Quaternion.LookRotation(rotate_vector);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, newRotation, 10.0f * Time.deltaTime);
        cc.Move(input_vector * (move_speed * Time.deltaTime) + new Vector3(0.0f, velocity, 0.0f) * Time.deltaTime);
    }

    IEnumerator Jump() {
        velocity = Mathf.Sqrt(jump_speed * -2f * -15.0f);
        yield return 0;
    }

    IEnumerator Roll() {
        is_roll = true;
        yield return new WaitForSeconds(0.5f);
        is_roll = false;
    }

    void Check_Ground() {
        Vector3 ground_pos = transform.position - new Vector3(0, ground_height, 0);
        is_ground = Physics.CheckSphere(ground_pos, ground_range, ground_hit, QueryTriggerInteraction.Ignore);
    }

    IEnumerator Add_Impact(Vector3 dir, float force) {
        dir.Normalize();
        if (dir.y < 0)
            dir.y = -dir.y;
        impact += dir.normalized * force / mass;
        yield return 0;
    }
}
