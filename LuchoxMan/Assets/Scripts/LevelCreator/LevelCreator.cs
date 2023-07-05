#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class LevelCreator : MonoBehaviour
{    
    [SerializeField] private LevelController levelController;
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private GameObject goalPrefab;
    [SerializeField] private GameObject obstaclePrefab;

    [SerializeField] private TextAsset levelCSV;

    public void LoadLevelFromCSV(string directory)
    {
        //GameObject go = Instantiate(levelObject);
        int rowCount=0;
        int columnCount =0;

        string[] lines = File.ReadAllLines(directory);
        rowCount = lines.Length;

        // Parse the first row to get the number of columns
        if (rowCount > 0)
        {
            string[] firstRow = lines[0].Split(';');
            columnCount = firstRow.Length;
        }

        string currentData = File.ReadAllText(directory);
        string[] data = currentData.Split(new string[] { ",", ";", "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        
        
        

        for (int i = 0; i < rowCount; i++)
        {
            for (int j =0; j< columnCount;j++)
            {
                float xPos = (columnCount *-1f) / 2 + 0.5f + j;
                float yPos = (rowCount * 1f) / 2 - 0.5f - i;

                if (data[j+columnCount*i] == "X")
                {
                    UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/SimpleObjects/Obstacle.prefab", typeof(GameObject));
                    if (prefab != null)
                    {
                        GameObject go = PrefabUtility.InstantiatePrefab(prefab, this.transform) as GameObject;
                        go.transform.position = new Vector3(xPos, yPos, 0);
                    }                    
                }
                else if(data[j + columnCount * i] == "F")
                {
                    UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/SimpleObjects/FloorTile.prefab", typeof(GameObject));
                    if (prefab != null)
                    {
                        GameObject go = PrefabUtility.InstantiatePrefab(prefab, this.transform) as GameObject;
                        go.transform.position = new Vector3(xPos, yPos, 0);
                    }
                }
                else if(data[j + columnCount * i] == "G")
                {
                    UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/SimpleObjects/FloorTile.prefab", typeof(GameObject));
                    if (prefab != null)
                    {
                        GameObject go = PrefabUtility.InstantiatePrefab(prefab, this.transform) as GameObject;
                        go.transform.position = new Vector3(xPos, yPos, 0);
                    }
                    UnityEngine.Object goalPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/SimpleObjects/Goal.prefab", typeof(GameObject));
                    if (goalPrefab != null)
                    {
                        GameObject go = PrefabUtility.InstantiatePrefab(goalPrefab, this.transform) as GameObject;
                        go.transform.position = new Vector3(xPos, yPos, 0);
                    }                    
                    levelController.AddAmountOfGoals();
                }
                else if(data[j + columnCount * i] == "B")
                {
                    UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/SimpleObjects/FloorTile.prefab", typeof(GameObject));
                    if (prefab != null)
                    {
                        GameObject go = PrefabUtility.InstantiatePrefab(prefab, this.transform) as GameObject;
                        go.transform.position = new Vector3(xPos, yPos, 0);
                    }
                    UnityEngine.Object boxPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/SimpleObjects/Box.prefab", typeof(GameObject));
                    if (boxPrefab != null)
                    {
                        GameObject gotwo = PrefabUtility.InstantiatePrefab(boxPrefab, this.transform) as GameObject;
                        gotwo.transform.position = new Vector3(xPos, yPos, 0);
                    }                    
                }
                else if (data[j + columnCount * i] == "BG")
                {
                    UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/SimpleObjects/FloorTile.prefab", typeof(GameObject));
                    if (prefab != null)
                    {
                        GameObject go = PrefabUtility.InstantiatePrefab(prefab, this.transform) as GameObject;
                        go.transform.position = new Vector3(xPos, yPos, 0);
                    }
                    UnityEngine.Object boxPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/SimpleObjects/Box.prefab", typeof(GameObject));
                    if (boxPrefab != null)
                    {
                        GameObject gotwo = PrefabUtility.InstantiatePrefab(boxPrefab, this.transform) as GameObject;
                        gotwo.transform.position = new Vector3(xPos, yPos, 0);
                    }
                    UnityEngine.Object goalPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/SimpleObjects/Goal.prefab", typeof(GameObject));
                    if (goalPrefab != null)
                    {
                        GameObject go = PrefabUtility.InstantiatePrefab(goalPrefab, this.transform) as GameObject;
                        go.transform.position = new Vector3(xPos, yPos, 0);
                    }
                    levelController.AddAmountOfGoals();
                }
                else if(data[j + columnCount * i] == "P")
                {
                    UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/SimpleObjects/FloorTile.prefab", typeof(GameObject));
                    if (prefab != null)
                    {
                        GameObject go = PrefabUtility.InstantiatePrefab(prefab, this.transform) as GameObject;
                        go.transform.position = new Vector3(xPos, yPos, 0);
                    }
                    UnityEngine.Object playerPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/SimpleObjects/Player.prefab", typeof(GameObject));
                    if (playerPrefab != null)
                    {
                        GameObject gotwo = PrefabUtility.InstantiatePrefab(playerPrefab, this.transform) as GameObject;
                        gotwo.transform.position = new Vector3(xPos, yPos, 0);
                    }
                }
            }
        }
    }
}
#endif
