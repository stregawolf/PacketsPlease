using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TheRuleTester))]
public class RuleTesterCustomEditor : Editor {

    TheRuleTester tester;

    public void OnEnable()
    {
        tester = (TheRuleTester)this.target;
    }

    public override void OnInspectorGUI()
    {
        if (tester.m_iterations > 0)
        {
            if(GUILayout.Button("Run Test"))
            {
                tester.RunTest();
            }
        }
        this.DrawDefaultInspector();
    }
}
