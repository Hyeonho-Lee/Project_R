using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {
    private Transform Target;
    private Vector3 offset;

    void Start() {
        Target = GameObject.Find("Player").transform;
        offset = this.transform.position - Target.position;
    }

    void FixedUpdate() {
        Vector3 targetPos = Target.position + offset;
        transform.position = targetPos;
    }
}
