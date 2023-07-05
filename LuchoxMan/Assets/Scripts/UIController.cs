using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIController : MonoBehaviour
{
    #region Singleton
    private static UIController _instance;
    public static UIController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<UIController>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
    #endregion
        

    [Header("Gameplay Buttons")]
    [SerializeField] private Button m_UpBtn;
    [SerializeField] private Button m_DownBtn;
    [SerializeField] private Button m_LeftBtn;
    [SerializeField] private Button m_RightBtn;
    [SerializeField] private Button m_ResetBtn;
    [SerializeField] private Button m_VolumeBtn;
    [SerializeField] private Sprite m_SoundOnSprite;
    [SerializeField] private Sprite m_SoundOffSprite;

    [SerializeField] private Button m_NextLvlBtn;
    [SerializeField] private Button m_BackToMenu;
    [SerializeField] private Button m_LevelSelectButton;

    [SerializeField] private TextMeshProUGUI m_CurrentLevelText;

    [Header("Menu Buttons")]
    [SerializeField] private GameObject m_LevelButtonsGrid;
    [SerializeField] private GameObject m_LevelButtonPrefab;
    [SerializeField] private Sprite m_AvailableLvlSprite;
    [SerializeField] private Sprite m_BlockedLvlSprite;
    private List<GameObject> lvlButtons = new List<GameObject>();

    [Header("Panels")]
    [SerializeField] private GameObject m_MenuPanel;
    [SerializeField] private GameObject levelCompletedPanel;


    
    private void Awake()
    {        
        GameplayEvents.OnLevelCompleted.AddListener(ShowLevelCompletedPanel);
        GameplayEvents.OnDataLoaded.AddListener(CreateLvlButtons);
        GameplayEvents.OnLevelLoaded.AddListener(AddPlayerMoevementToButtons);
        GameplayEvents.OnLevelLoaded.AddListener(UpdateCurrentLevelText);
    }

    private void Start()
    {
        m_VolumeBtn.onClick.AddListener(() =>
        {
            AudioManager.instance.SetVolume();

        });
        m_BackToMenu.onClick.AddListener(()=> {
            BackToMenu();
            AudioManager.instance.PlaySound("Button");
            Vibrator.Vibrate(50);
        });
        m_NextLvlBtn.onClick.AddListener(() => {
            LevelManager.Instance.LoadLevelByIndex(LevelManager.Instance.CURRENTLEVEL);
            SetLevelCompletedPanelVisibility(false);
            AudioManager.instance.PlaySound("Button");
            Vibrator.Vibrate(50);
        });
        m_ResetBtn.onClick.AddListener(() => {
            AudioManager.instance.PlaySound("Button");
            LevelManager.Instance.ResetLevel();
            AudioManager.instance.PlaySound("Queixo1");
            Vibrator.Vibrate(50);
        });
        m_LevelSelectButton.onClick.AddListener(() => {
            AudioManager.instance.PlaySound("Button");
            BackToMenu();
            Vibrator.Vibrate(50);
        });
    }

    public void SetVolumeIcon(bool volumeOn)
    {
        if(volumeOn)
        {
            m_VolumeBtn.image.sprite = m_SoundOnSprite;
        }
        else
        {
            m_VolumeBtn.image.sprite = m_SoundOffSprite;
        }        
    }

    public void ShowLevelCompletedPanel()
    {
        StartCoroutine(delayedAction());
        AudioManager.instance.PlaySound("IenChe1");
    }

    public void SetMenuPanelVisibility(bool visible)
    {
        m_MenuPanel.SetActive(visible);
        if(visible)
        {
            RefreshButtonStatus();
        }
    }

    public void SetLevelCompletedPanelVisibility(bool visible)
    {
        if(LevelManager.Instance.CURRENTLEVEL >= LevelManager.Instance.AllLevels.Count)
        {
            m_NextLvlBtn.interactable = false;
        }
        else
        {
            m_NextLvlBtn.interactable= true;
        }
        levelCompletedPanel.SetActive(visible);
    }

    private IEnumerator delayedAction()
    {
        yield return new WaitForSeconds(0.5f);
        SetLevelCompletedPanelVisibility(true);
    }

    public void RemoveButtonsListeners()
    {
        m_UpBtn.onClick.RemoveAllListeners();
        m_DownBtn.onClick.RemoveAllListeners();
        m_LeftBtn.onClick.RemoveAllListeners();
        m_RightBtn.onClick.RemoveAllListeners();
    }

    public void AddPlayerMoevementToButtons()
    {
        RemoveButtonsListeners();
        Movement player = FindObjectOfType<Movement>();
        m_UpBtn.onClick.AddListener(() => {
            player.MovePoint(new Vector3(0,1,0));
            player.ChangeSprite(Direction.Up);
            //Handheld.Vibrate();
            Vibrator.Vibrate(50);
        });
        m_DownBtn.onClick.AddListener(() => {
            player.MovePoint(new Vector3(0, -1, 0));
            player.ChangeSprite(Direction.Down);
            //Handheld.Vibrate();
            Vibrator.Vibrate(50);
        });
        m_LeftBtn.onClick.AddListener(() => {
            player.MovePoint(new Vector3(-1, 0, 0));
            player.ChangeSprite(Direction.Left);
            //Handheld.Vibrate();
            Vibrator.Vibrate(50);
        });
        m_RightBtn.onClick.AddListener(() => {
            player.MovePoint(new Vector3(1, 0, 0));
            player.ChangeSprite(Direction.Right);
            //Handheld.Vibrate();
            Vibrator.Vibrate(50);
        });
    }

    public void SetButtonsStatus(bool status)
    {
        m_UpBtn.interactable = status;
        m_DownBtn.interactable = status;
        m_LeftBtn.interactable = status;
        m_RightBtn.interactable = status;
    }

    public void CreateLvlButtons()
    {
        for (int i =0; i< LevelManager.Instance.AllLevels.Count; i++)
        {
            var capturedIndex = i;
            lvlButtons.Add(Instantiate(m_LevelButtonPrefab, m_LevelButtonsGrid.transform));
            lvlButtons[capturedIndex].GetComponent<Button>().onClick.AddListener(() => { 
                LevelManager.Instance.LoadLevelByIndex(capturedIndex);
                Vibrator.Vibrate(50);
                AudioManager.instance.PlaySound("Button");
            });
        }
        RefreshButtonStatus();
    }

    public void RefreshButtonStatus()
    {
        for(int i =0; i< lvlButtons.Count; i++) 
        {
            var capturedIndex = i;
            Button btn = lvlButtons[capturedIndex].GetComponent<Button>();
            if (i<=LevelManager.Instance.MAXLEVEL)
            {
                btn.image.sprite = m_AvailableLvlSprite;
                btn.interactable = true;
                btn.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = (capturedIndex+1).ToString("00");
                btn.gameObject.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            }
            else
            {
                btn.interactable=false;
                btn.image.sprite = m_BlockedLvlSprite;
                btn.GetComponentInChildren<TextMeshProUGUI>().enabled= false;
            }
        }        
    }

    public void BackToMenu()
    {
        SetLevelCompletedPanelVisibility(false);
        SetMenuPanelVisibility(true);
        LevelManager.Instance.DestroyLevel();
    }

    public void UpdateCurrentLevelText()
    {
        m_CurrentLevelText.text = "Nivel " + (LevelManager.Instance.CURRENTLEVEL+1).ToString("00");
    }    

}
