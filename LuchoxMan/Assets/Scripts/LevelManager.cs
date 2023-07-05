using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelManager : MonoBehaviour
{
    #region Singleton
    private static LevelManager _instance;
    public static LevelManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<LevelManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
    #endregion

    [System.Serializable]
    public class Data
    {
        public int maxLevelAchieved = 0;
    }

    private Data gameData;


    [SerializeField] private List<GameObject> m_AllLevels= new List<GameObject>();
    public List<GameObject> AllLevels => m_AllLevels;
    private int currentLevel = 0;
    public int CURRENTLEVEL => currentLevel;
    public int maxLevelUnlocked = 0;
    public int MAXLEVEL => maxLevelUnlocked;

    private LevelController lvlController;
    

    private void Awake()
    {
        GameplayEvents.OnLevelCompleted.AddListener(LevelWasCompleted);
    }

    private void Start()
    {
        LoadData();
        
    }

    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
    }

    public void SetMaxLevel(int level)
    {
        maxLevelUnlocked = level;
    }

    public void LoadLevelByIndex(int index)
    {
        DestroyLevel();
        lvlController = Instantiate(m_AllLevels[index]).GetComponent<LevelController>();
        SetCurrentLevel(index);
        UIController.Instance.SetMenuPanelVisibility(false);
        GameplayEvents.OnLevelLoaded.Invoke();

        FindObjectOfType<Camera>().orthographicSize = (lvlController.levelHeight*1f)/2f;
    }
    

    public void ResetLevel()
    {
        LoadLevelByIndex(currentLevel);
    }

    public void DestroyLevel()
    {
        if (lvlController != null)
        {
            Destroy(lvlController.gameObject);
            lvlController = null;
        }
    }

    public void LevelWasCompleted()
    {
        currentLevel++;        
        if (currentLevel > maxLevelUnlocked)
        {
            maxLevelUnlocked = currentLevel;
        }
        gameData.maxLevelAchieved= maxLevelUnlocked;
        SaveData();
    }


    public void LoadData()
    {

        if (File.Exists(GetCurrentPath() + "/game_saved.json"))
        {
#if UNITY_EDITOR_WIN
            string json = File.ReadAllText(Application.dataPath + "/game_saved.json");
#else
            string json = File.ReadAllText(Application.persistentDataPath + "/game_saved.json");
#endif
            gameData = JsonUtility.FromJson<Data>(json);
            maxLevelUnlocked = gameData.maxLevelAchieved;

            Debug.Log("SAVED DATA LOADED");
        }
        else
        {
            //SI CARGA POR PRIMERA VEZ SE ASEGURA DE VER SI EL SEGUI PARTICIPANDO ES PREMIO VALIDO O NO
            gameData = new Data();
            gameData.maxLevelAchieved = maxLevelUnlocked;
            SaveData();
            
            Debug.Log("NOT SAVED DATA EXIST");
        }
        GameplayEvents.OnDataLoaded.Invoke();
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(gameData);
#if UNITY_EDITOR_WIN
        File.WriteAllText(Application.dataPath + "/game_saved.json", json);
#else 
        File.WriteAllText(Application.persistentDataPath + "/game_saved.json", json);
#endif
        Debug.Log("DATA SAVED");
    }

    private string GetCurrentPath()
    {
#if UNITY_EDITOR_WIN
        string toReturn = Application.dataPath;
#else
        string toReturn = Application.persistentDataPath;
#endif
        return toReturn;
    }

}
