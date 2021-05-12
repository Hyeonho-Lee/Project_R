using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSCamera : MonoBehaviour {
    [Header("Camera Status")]
    public Transform Player_Transform;
    public Transform Camera_Arm;

    private float mouse_horizontal;
    private float mouse_vertical;

    public Vector3 camera_dir;
    public Vector3 player_dir;
    public Vector3 camera_angle;

    void Update() {
        Look_Movement();
    }

    void Look_Movement() {
        mouse_horizontal = Input.GetAxis("Mouse X");
        mouse_vertical = Input.GetAxis("Mouse Y");

        camera_angle = Camera_Arm.rotation.eulerAngles;
        float x = camera_angle.x - mouse_vertical;

        if (x < 180f)
            x = Mathf.Clamp(x, -0.5f, 50f);
        else
            x = Mathf.Clamp(x, 335f, 361f);

        Camera_Arm.rotation = Quaternion.Euler(x, camera_angle.y + mouse_horizontal, camera_angle.z);

        camera_dir = Player_Transform.transform.position - this.transform.position;
        player_dir.Set(camera_dir.x, 0, camera_dir.z);
        Debug.DrawRay(Player_Transform.transform.position, player_dir * 0.3f, Color.red);
    }
}
