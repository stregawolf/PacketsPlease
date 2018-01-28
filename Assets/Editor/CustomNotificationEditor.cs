
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(NotificationData))]
public class CustomNotificationEditor : Editor {

    public override void OnInspectorGUI()
    {
        ((NotificationData)target).m_message = GUILayout.TextArea(((NotificationData)target).m_message);
        this.DrawDefaultInspector();
    }
}
