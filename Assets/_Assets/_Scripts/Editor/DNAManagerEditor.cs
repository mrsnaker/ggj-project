﻿using DNA;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DNAManager))]
public class DNAManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var t = (DNAManager) target;
        if (GUILayout.Button("Rebuild DNA"))
        {
            t.RebuildDNA();
        }
    }
}
