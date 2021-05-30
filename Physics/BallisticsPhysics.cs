using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallisticsPhysics : MonoBehaviour {
    Rigidbody rigidbody;
    public float speed = 10f;

    void Start() {
        rigidbody = GetComponent<Rigidbody>();
    }

    
    void FixedUpdate()
    {
    }
}
