using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelCreator))]
public class LevelCreatorEditor : Editor
{
    private SerializedProperty levelController;
    private SerializedProperty floorPrefab;
    private SerializedProperty playerPrefab;
    private SerializedProperty boxPrefab;
    private SerializedProperty goalPrefab;
    private SerializedProperty obstaclePrefab;

    private void OnEnable()
    {
        levelController = serializedObject.FindProperty("levelController");
        floorPrefab = serializedObject.FindProperty("floorPrefab");
        playerPrefab = serializedObject.FindProperty("playerPrefab");
        boxPrefab = serializedObject.FindProperty("boxPrefab");
        goalPrefab = serializedObject.FindProperty("goalPrefab");
        obstaclePrefab = serializedObject.FindProperty("obstaclePrefab");
    }

    public override void OnInspectorGUI()
    {
        LevelCreator creator = (LevelCreator)target;

        EditorGUILayout.PropertyField(levelController);
        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(floorPrefab);
        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(playerPrefab);
        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(boxPrefab);
        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(goalPrefab);
        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(obstaclePrefab);
        EditorGUILayout.Space(10);

        if (GUILayout.Button("CREATE LEVEL FROM CSV", GUILayout.Height(30)))
        {
            string[] filter = { "Text files", "csv" };
            string directory = EditorUtility.OpenFilePanelWithFilters("Select Directory", "Assets/", filter);
            if (!string.IsNullOrEmpty(directory))
            {
                creator.LoadLevelFromCSV(directory);
            }

            GUIUtility.ExitGUI();
        }
        serializedObject.ApplyModifiedProperties();
        Repaint();
    }
}
