using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    [Header("PlayerMovement Status")]
    public float change_speed = 10f;
    private float move_speed;
    public float dash_speed = 20f;
    public float jump_speed = 10f;
    public float rolling_speed = 1f;
    public float rot_speed = 1f;
    public float gravity = 50f;
    private float ground_value = 1f;
    private float ground_value_bu;

    private float horizontal_move;
    private float vertical_move;
    private Vector3 jump_movement;
    private Vector3 movement;
    private Vector3 look_center;
    private Vector3 camera_center;

    [Header("PlayerMovement Check")]
    public bool is_ground;
    public bool any_keydown;
    private bool is_ctrl;
    public bool is_dash;
    private bool is_timer = false;
    public bool is_rolling = false;
    public bool is_rolling_lock = false;

    private bool jp_roll_tf = false;
    private float jp_roll = 0.25f;
    private float jp_cool;

    private float mass = 3.0f;
    private Vector3 impact = Vector3.zero;

    [Header("PlayerMovement Transform")]
    public Transform main_camera;
    private Transform main_character;

    private Animator this_am;
    private Rigidbody this_rb;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        this_am = GetComponent<Animator>();
        this_rb = GetComponent<Rigidbody>();
        main_character = this.transform;
        move_speed = change_speed;
        jp_cool = jp_roll;
        ground_value_bu = ground_value;
    }

    void Update()
    {
        //enter_key();
        move();
        //check_value();
        //check_ground();
    }

    void FixedUpdate()
    {
        //jump();
    }

    void jump()
    {
        if (jp_roll_tf == true)
        {
            //this_am.SetBool("input_jump", true);
            jp_roll -= Time.deltaTime;
            AddImpact(Vector3.up, jump_speed);
            if (jp_roll <= 0)
            {
                jp_roll_tf = false;
                jp_roll = jp_cool;
                //this_am.SetBool("input_jump", false);
            }
        }
    }

    void check_value()
    {

        if (jp_roll_tf == false)
        {
            movement.y += Physics.gravity.y * gravity * Time.deltaTime;
        }

        if (is_ctrl == true)
        {
            move_speed = change_speed / 2;
        }
        else
        {
            move_speed = change_speed;
        }

        if (is_dash == true)
        {
            move_speed = change_speed + dash_speed;
        }

        if (is_dash == true)
        {
            move_speed = (change_speed + dash_speed) / 2;
        }

        if (is_dash == true && is_timer == true)
        {
            is_timer = false;
            if (is_ground == true && is_rolling_lock == false)
            {
                StartCoroutine(rolling());
            }
        }
    }

    IEnumerator rolling()
    {
        is_rolling = true;
        //this_am.SetBool("rolling", true);
        //GameObject.Find("Player").GetComponent<PlayerStatus>().stamina -= 20f;
        yield return new WaitForSeconds(0.5f);
        is_rolling = false;
        //this_am.SetBool("rolling", false);
    }

    void enter_key()
    {
        if (is_ground == true && Input.GetKeyDown(KeyCode.Space))
        {
            jp_roll_tf = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            is_dash = true;
            is_timer = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            is_dash = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            is_ctrl = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            is_dash = false;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            is_ctrl = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("mouse_l");
            AddImpact(look_center, rolling_speed * 10f);
        }
    }

    void move()
    {
        /*horizontal_move = Input.GetAxis("Horizontal");
        vertical_move = Input.GetAxis("Vertical");

        if (horizontal_move == 0 && vertical_move == 0)
        {
            any_keydown = false;
        }
        else
        {
            any_keydown = true;
        }

        if (impact.magnitude > 0.2f)
        {
            controller.Move(impact * Time.deltaTime);
        }
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);

        Vector2 mouse_movement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camera_angle = main_camera.rotation.eulerAngles;

        if (is_dash == true)
        {
            //this_am.SetFloat("movement_x", horizontal_move * 2);
            //this_am.SetFloat("movement_z", vertical_move * 2);
        }
        else
        {
            //this_am.SetFloat("movement_x", horizontal_move);
            //this_am.SetFloat("movement_z", vertical_move);
        }

        movement = new Vector3(horizontal_move, movement.y, vertical_move).normalized;
        look_center = new Vector3(main_camera.forward.x, 0, main_camera.forward.z).normalized;
        camera_center = new Vector3(main_camera.forward.x, main_camera.forward.y, main_camera.forward.z);

        Vector3 v_u_right = Quaternion.Euler(0f, 45f, 0f) * look_center;
        Vector3 v_right = Quaternion.Euler(0f, 90f, 0f) * look_center;
        Vector3 v_d_right = Quaternion.Euler(0f, 135f, 0f) * look_center;
        Vector3 v_down = Quaternion.Euler(0f, 180f, 0f) * look_center;
        Vector3 v_d_left = Quaternion.Euler(0f, 225f, 0f) * look_center;
        Vector3 v_left = Quaternion.Euler(0f, 270f, 0f) * look_center;
        Vector3 v_u_left = Quaternion.Euler(0f, 315f, 0f) * look_center;
        Debug.DrawRay(main_character.position, look_center * 3f, Color.red);
        Debug.DrawRay(main_character.position, v_u_right * 2f, Color.blue);
        Debug.DrawRay(main_character.position, v_d_right * 2f, Color.blue);
        Debug.DrawRay(main_character.position, v_d_left * 2f, Color.blue);
        Debug.DrawRay(main_character.position, v_u_left * 2f, Color.blue);
        Debug.DrawRay(main_character.position, v_right * 2f, Color.blue);
        Debug.DrawRay(main_character.position, v_left * 2f, Color.blue);
        Debug.DrawRay(main_character.position, v_down * 2f, Color.blue);
        Debug.DrawRay(main_character.position, camera_center * 5f, Color.red);

        //정지
        if ((movement.z == 0) && (movement.x == 0))
        {
            controller.Move(movement * move_speed * Time.deltaTime);
        }

        //앞오
        if ((movement.z > 0 && movement.z <= 1) && (movement.x > 0 && movement.x <= 1))
        {
            Quaternion newRotation = Quaternion.LookRotation(v_u_right);
            main_character.rotation = Quaternion.Slerp(main_character.rotation, newRotation, rot_speed * Time.deltaTime);
            controller.Move(new Vector3(v_u_right.x, movement.y, v_u_right.z) * move_speed * Time.deltaTime);
            if (is_rolling == true)
            {
                AddImpact(v_u_right, rolling_speed);
            }
        }
        //앞왼
        if ((movement.z > 0 && movement.z <= 1) && (movement.x < 0 && movement.x >= -1))
        {
            Quaternion newRotation = Quaternion.LookRotation(v_u_left);
            main_character.rotation = Quaternion.Slerp(main_character.rotation, newRotation, rot_speed * Time.deltaTime);
            controller.Move(new Vector3(v_u_left.x, movement.y, v_u_left.z) * move_speed * Time.deltaTime);
            if (is_rolling == true)
            {
                AddImpact(v_u_left, rolling_speed);
            }
        }
        //뒤오
        if ((movement.z < 0 && movement.z >= -1) && (movement.x > 0 && movement.x <= 1))
        {
            Quaternion newRotation = Quaternion.LookRotation(v_d_right);
            main_character.rotation = Quaternion.Slerp(main_character.rotation, newRotation, rot_speed * Time.deltaTime);
            controller.Move(new Vector3(v_d_right.x, movement.y, v_d_right.z) * move_speed * Time.deltaTime);
            if (is_rolling == true)
            {
                AddImpact(v_d_right, rolling_speed);
            }
        }
        //뒤왼
        if ((movement.z < 0 && movement.z >= -1) && (movement.x < 0 && movement.x >= -1))
        {
            Quaternion newRotation = Quaternion.LookRotation(v_d_left);
            main_character.rotation = Quaternion.Slerp(main_character.rotation, newRotation, rot_speed * Time.deltaTime);
            controller.Move(new Vector3(v_d_left.x, movement.y, v_d_left.z) * move_speed * Time.deltaTime);
            if (is_rolling == true)
            {
                AddImpact(v_d_left, rolling_speed);
            }
        }
        //앞
        if ((movement.z > 0 && movement.z <= 1) && (movement.x == 0))
        {
            Quaternion newRotation = Quaternion.LookRotation(look_center);
            main_character.rotation = Quaternion.Slerp(main_character.rotation, newRotation, rot_speed * Time.deltaTime);
            controller.Move(new Vector3(look_center.x, movement.y, look_center.z) * move_speed * Time.deltaTime);
            if (is_rolling == true)
            {
                AddImpact(look_center, rolling_speed);
            }
        }
        //뒤
        if ((movement.z < 0 && movement.z >= -1) && (movement.x == 0))
        {
            Quaternion newRotation = Quaternion.LookRotation(look_center);
            main_character.rotation = Quaternion.Slerp(main_character.rotation, newRotation, rot_speed * Time.deltaTime);
            controller.Move(new Vector3(v_down.x, movement.y, v_down.z) * move_speed * Time.deltaTime);
            if (is_rolling == true)
            {
                AddImpact(v_down, rolling_speed);
            }
        }
        //오
        if ((movement.x > 0 && movement.x <= 1) && (movement.z == 0))
        {
            Quaternion newRotation = Quaternion.LookRotation(look_center);
            main_character.rotation = Quaternion.Slerp(main_character.rotation, newRotation, rot_speed * Time.deltaTime);
            controller.Move(new Vector3(v_right.x, movement.y, v_right.z) * move_speed * Time.deltaTime);
            if (is_rolling == true)
            {
                AddImpact(v_right, rolling_speed);
            }
        }
        //왼
        if ((movement.x < 0 && movement.x >= -1) && (movement.z == 0))
        {
            Quaternion newRotation = Quaternion.LookRotation(look_center);
            main_character.rotation = Quaternion.Slerp(main_character.rotation, newRotation, rot_speed * Time.deltaTime);
            controller.Move(new Vector3(v_left.x, movement.y, v_left.z) * move_speed * Time.deltaTime);
            if (is_rolling == true)
            {
                AddImpact(v_left, rolling_speed);
            }
        }*/
    }

    void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        if (dir.y < 0)
        {
            dir.y = -dir.y;
        }
        impact += dir.normalized * force / mass;
    }

    void check_ground()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, ground_value) ||
            Physics.Raycast(this.transform.position + (Vector3.forward * 0.5f), Vector3.down, out hit, ground_value) ||
            Physics.Raycast(this.transform.position + (Vector3.back * 0.5f), Vector3.down, out hit, ground_value) ||
            Physics.Raycast(this.transform.position + (Vector3.left * 0.5f), Vector3.down, out hit, ground_value) ||
            Physics.Raycast(this.transform.position + (Vector3.right * 0.5f), Vector3.down, out hit, ground_value) ||
            Physics.Raycast(this.transform.position + (new Vector3(1, 0, 1) * 0.5f), Vector3.down, out hit, ground_value) ||
            Physics.Raycast(this.transform.position + (new Vector3(1, 0, -1) * 0.5f), Vector3.down, out hit, ground_value) ||
            Physics.Raycast(this.transform.position + (new Vector3(-1, 0, 1) * 0.5f), Vector3.down, out hit, ground_value) ||
            Physics.Raycast(this.transform.position + (new Vector3(-1, 0, -1) * 0.5f), Vector3.down, out hit, ground_value))
        {
            if (hit.transform.tag == "Ground")
            {
                is_ground = true;
                //this_am.SetBool("is_ground", true);
                ground_value = ground_value_bu;
                return;
            }
        }
        is_ground = false;
        //this_am.SetBool("is_ground", false);
        ground_value = 0.56f;
    }
}
