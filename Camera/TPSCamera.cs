using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCamera : MonoBehaviour {
    [Header("Camera Status")]
    public Transform Player_Transform;
    public Transform Camera_Arm;
    public Transform Reset_Transform;
    public GameObject box;

    private float mouse_horizontal;
    private float mouse_vertical;
    public float whell_value;
    public float camera_distance;

    public Vector3 camera_dir;
    public Vector3 player_dir;
    public Vector3 camera_angle;
    public Vector3 dir_nor;

    private RaycastHit camera_hit;

    void Start() {
        whell_value = 1f;
    }

    void Update() {
        Look_Movement();
        Input_whell();
    }

    void FixedUpdate() {
        Raycast_Camera();
    }

    void Look_Movement() {
        mouse_horizontal = Input.GetAxis("Mouse X");
        mouse_vertical = Input.GetAxis("Mouse Y");

        camera_angle = Camera_Arm.rotation.eulerAngles;
        float x = camera_angle.x - mouse_vertical;

        Camera_Arm.rotation = Quaternion.Euler(x, camera_angle.y + mouse_horizontal, camera_angle.z);

        camera_dir = Player_Transform.transform.position - this.transform.position;
        dir_nor = camera_dir.normalized;
        player_dir.Set(camera_dir.x, 0, camera_dir.z);
        Debug.DrawRay(Player_Transform.transform.position, camera_dir * 0.3f, Color.red);
    }

    void Raycast_Camera() {
        Debug.DrawRay(this.transform.position, camera_dir, Color.blue);
        camera_distance = Vector3.Distance(Player_Transform.transform.position, this.transform.position);

        if (Physics.Linecast(Player_Transform.transform.position, this.transform.position, out camera_hit)) {
            camera_distance = Vector3.Distance(Player_Transform.transform.position, camera_hit.point);
            if (camera_hit.point != Vector3.zero && camera_distance >= 3.5f) {
                box.transform.position = camera_hit.point + (dir_nor * 2f);
            }
        }else {
            box.transform.position = Reset_Transform.transform.position + (dir_nor * whell_value);
        }
    }

    void Input_whell() {
        float whell = Input.GetAxisRaw("Mouse ScrollWheel");
        if (whell > 0) {
            if (whell_value < 9)
                whell_value = whell_value + 0.5f;
        } else if (whell < 0) {
            if (whell_value > 1)
                whell_value = whell_value - 0.5f;
        }
    }
}
