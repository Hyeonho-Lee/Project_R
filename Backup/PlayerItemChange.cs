using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerItem))]
public class PlayerItemChange : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlayerItem change = (PlayerItem)target;
        if (GUILayout.Button("PlayerItem_Change"))
        {
            change.playeritem_change();
        }
    }
}
