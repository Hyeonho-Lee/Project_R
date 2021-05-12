using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform camera_pos;
    private Transform main_character;
    public float camera_speed = 7f;

    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        main_character = this.transform;
        offset = camera_pos.transform.position - main_character.position;
    }

    // Update is called once per frame
    void Update()
    {
        look_movement();
    }

    public void look_movement()
    {
        Vector2 mouse_movement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camera_angle = camera_pos.rotation.eulerAngles;
        float x = camera_angle.x - mouse_movement.y;

        if (x < 180f)
        {
            x = Mathf.Clamp(x, -0.5f, 50f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        Vector3 find_target = main_character.position + offset;
        camera_pos.transform.position = Vector3.Lerp(camera_pos.transform.position, find_target, camera_speed);

        camera_pos.rotation = Quaternion.Euler(x, camera_angle.y + mouse_movement.x, camera_angle.z);
        //Debug.DrawRay(main_character.position, new Vector3(camera_pos.forward.x, 0, camera_pos.forward.z).normalized, Color.red);
    }
}
