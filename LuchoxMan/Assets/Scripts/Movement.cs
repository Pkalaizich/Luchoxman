using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [SerializeField] private float m_MoveSpeed =5f;
    [SerializeField] private Transform m_MovePoint;

    [SerializeField] private Button m_UpBtn;
    [SerializeField] private Button m_DownBtn;
    [SerializeField] private Button m_LeftBtn;
    [SerializeField] private Button m_RightBtn;

    [SerializeField] private LayerMask m_ObstacleLayers;

    [SerializeField] private Sprite m_UpSprite;
    [SerializeField] private Sprite m_DownSprite;
    [SerializeField] private Sprite m_LeftSprite;
    [SerializeField] private Sprite m_RightSprite;

    private SpriteRenderer sr;
    

    private bool moving =false;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        m_MovePoint.parent = null;

        m_UpBtn.onClick.AddListener(() => {
            MovePoint(new Vector3(0, 1, 0));
            sr.sprite = m_UpSprite;
        });
        m_DownBtn.onClick.AddListener(() => {
            MovePoint(new Vector3(0, -1, 0));
            sr.sprite = m_DownSprite;
        });
        m_LeftBtn.onClick.AddListener(() => {
            MovePoint(new Vector3(-1, 0, 0));
            sr.sprite = m_LeftSprite;
        });
        m_RightBtn.onClick.AddListener(() => {
            MovePoint(new Vector3(1, 0, 0));
            sr.sprite = m_RightSprite;
        });        
    }

    
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,m_MovePoint.position,m_MoveSpeed*Time.deltaTime);

        if(Vector3.Distance(transform.position,m_MovePoint.position)<=0.05f && moving==true)
        {
            moving = false;
            SetButtonsStatus(true);            
        }
    }

    public void SetButtonsStatus(bool status)
    {
        m_UpBtn.interactable = status;
        m_DownBtn.interactable = status;
        m_LeftBtn.interactable = status;
        m_RightBtn.interactable = status;
    }

    public void MovePoint(Vector3 direction)
    {
        GameObject other = Physics2D.OverlapCircle(m_MovePoint.position + direction, .2f, m_ObstacleLayers)? 
            Physics2D.OverlapCircle(m_MovePoint.position + direction, .2f, m_ObstacleLayers).gameObject:
            null;
        if (other !=null)
        {
            if(other.tag =="Box")
            {
                if(other.GetComponent<Box>().ChangePosition(direction))
                {
                    moving = true;
                    SetButtonsStatus(false);
                    m_MovePoint.position += direction;
                }
            }
        }
        else
        {
            moving = true;
            SetButtonsStatus(false);
            m_MovePoint.position += direction;
        }
    }

}
