using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlope : MonoBehaviour {
    [Header("PlayerSlope Status")]
    public float max_ground_angle;
    private float slope_weight;
    private float slope_height;
    private float slope_padding;

    private bool is_ground;
    private bool is_wall;

    public float ground_angle;
    private Vector3 forward;
    private Vector3 slope_transform;
    private RaycastHit ground_hit;
    private RaycastHit forward_hit;
    private RaycastHit transform_hit;

    PlayerMovement playermovement;

    void Awake() {
        playermovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    void Start() {
        slope_weight = 1f;
        slope_height = 0.9f;
        slope_padding = 0.05f;
        max_ground_angle = 120f;
        is_wall = false;
    }

    void FixedUpdate() {
        Check_Slope();
        Check_GroundAngle();
    }

    void Check_Slope() {
        forward = playermovement.player_forward;
        is_ground = playermovement.is_ground;
        ground_hit = playermovement.ground_hit;

        if(playermovement.movement.z == 0 && playermovement.movement.x == 0) {
            slope_weight = 0f;
        }else {
            slope_weight = 1f;
        }

        slope_transform = new Vector3(transform.position.x, transform.position.y - slope_height, transform.position.z);
        Debug.DrawLine(slope_transform, slope_transform + (forward * slope_weight), Color.blue);
        Debug.DrawLine(transform.position, transform.position + (forward * slope_weight), Color.blue);

        if (Physics.Raycast(transform.position, forward, out transform_hit, slope_weight)) {
            is_wall = true;
        }else {
            is_wall = false;
        }

        if (Physics.Raycast(slope_transform, forward, out forward_hit, slope_weight)) {
            if(Vector3.Distance(slope_transform, forward_hit.point) < slope_weight && is_wall == false) {
                transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * 3, 5 * Time.deltaTime);
            }
        }
    }

    void Check_GroundAngle() {
        if (ground_angle >= max_ground_angle)
            return;

        ground_angle = Vector3.Angle(transform.right, ground_hit.normal);
    }
}
