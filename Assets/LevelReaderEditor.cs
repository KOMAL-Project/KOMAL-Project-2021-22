using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(LevelReader))]
public class LevelReaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelReader mapGen = (LevelReader)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Generate"))
        {
            mapGen.GenerateLevel();
        }
        if (GUILayout.Button("Reset"))
        {
            mapGen.DestroyGamePrefabs();
        }
    }
}
