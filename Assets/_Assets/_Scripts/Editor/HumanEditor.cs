using DNA;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Human))]
public class HumanEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var t = (Human) target;
        if (GUILayout.Button("Add Human Parts"))
        {
            t.SetHumanParts();
        }
    }
}