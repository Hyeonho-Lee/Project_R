using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlope : MonoBehaviour {
    [Header("PlayerSlope Status")]
    public float slope_weight;
    public float slope_height;
    public float slope_padding;
    public float max_ground_angle;

    public bool is_ground;

    public float ground_angle;
    private Vector3 forward;
    private Vector3 slope_transform;
    private RaycastHit ground_hit;
    private RaycastHit forward_hit;

    PlayerMovement playermovement;

    void Awake() {
        playermovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    void Start() {
        slope_weight = 5f;
        slope_height = 0.7f;
        slope_padding = 0.05f;
        max_ground_angle = 120f;
    }

    void Update() {
        //Check_Slope();
    }

    void FixedUpdate() {
        Check_Slope();
        Check_Forward();
        Check_GroundAngle();
    }

    void Check_Slope() {
        is_ground = playermovement.is_ground;
        ground_hit = playermovement.ground_hit;
        slope_transform = new Vector3(transform.position.x, transform.position.y - slope_height, transform.position.z);
        Debug.DrawLine(transform.position, transform.position + (forward * slope_weight), Color.blue);

        if(Physics.Raycast(slope_transform, forward, out forward_hit, slope_weight)) {
            if(Vector3.Distance(slope_transform, forward_hit.point) < 3) {
                Debug.Log(Vector3.Distance(slope_transform, forward_hit.point));
                //transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * 3, 5 * Time.deltaTime);
            }
        }
    }

    void Check_Forward() {
        if (!is_ground) {
            forward = transform.forward;
            return;
        }

        forward = Vector3.Cross(transform.right, ground_hit.normal);
    }

    void Check_GroundAngle() {
        if (ground_angle >= max_ground_angle)
            return;

        ground_angle = Vector3.Angle(transform.right, ground_hit.normal);
    }
}
