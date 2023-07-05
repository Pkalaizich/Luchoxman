using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        GameplayEvents.OnDataLoaded.Invoke();
        
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
    }
}
