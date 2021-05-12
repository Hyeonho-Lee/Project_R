using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    [Serializable]
    public struct Part_Position
    {
        public Transform head_position;
    }
    [Serializable]
    public struct Part_Object
    {
        public GameObject head;
        public GameObject hair;
        public GameObject head_accessories;
    }

    public Part_Position part_position;
    public Part_Object part_object;

    void Start()
    {
        //playeritem_change();
    }
    void Update()
    {

    }

    public void playeritem_change()
    {
        GameObject[] head_object = { part_object.head, part_object.hair, part_object.head_accessories };
        for (int i = 0; i < part_position.head_position.childCount; i++)
        {
            //Debug.Log(part_position.head_position.GetChild(i));
            // 아이템이 이미 있을시 삭제
            if (part_position.head_position.GetChild(i).childCount != 0)
            {
                GameObject child_delete = part_position.head_position.GetChild(i).GetChild(0).gameObject;
                if (Application.isEditor)
                    DestroyImmediate(child_delete);
            }

            Transform child_position = part_position.head_position.GetChild(i).transform;
            GameObject child_create = Instantiate(head_object[i], child_position.position, child_position.rotation) as GameObject;
            child_create.transform.parent = child_position.transform;
            child_create.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
