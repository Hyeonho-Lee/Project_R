using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour {
    [Header("PlayerInventory")]
    public float test;
    void Start() {
        
    }

    void Update() {
        Input_Key();
    }

    void Input_Key() {
        if (Input.GetKeyDown(KeyCode.I))
            Debug.Log("인벤토리 오픈");
    }
}
