using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [SerializeField] private float m_MoveSpeed =5f;
    [SerializeField] private Transform m_MovePoint;
    

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
    }

    
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,m_MovePoint.position,m_MoveSpeed*Time.deltaTime);

        if(Vector3.Distance(transform.position,m_MovePoint.position)<=0.05f && moving==true)
        {
            moving = false;
            UIController.Instance.SetButtonsStatus(true);                       
        }
    }

    private void OnDestroy()
    {
        if(m_MovePoint!= null)
            Destroy(m_MovePoint.gameObject);
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
                    AudioManager.instance.PlaySound("Step");
                    UIController.Instance.SetButtonsStatus(false);                   
                    m_MovePoint.position += direction;
                }
                else
                {
                    AudioManager.instance.PlaySound("CantMove");
                }
            }
            else
            {
                AudioManager.instance.PlaySound("CantMove");
            }
        }
        else
        {
            moving = true;
            AudioManager.instance.PlaySound("Step");
            UIController.Instance.SetButtonsStatus(false);
            m_MovePoint.position += direction;
        }
    }

    public void ChangeSprite(Direction newDirection)
    {
        switch(newDirection)
        {
            case Direction.Up:
                sr.sprite = m_UpSprite;
                break;
            case Direction.Down:
                sr.sprite = m_DownSprite;
                break;
            case Direction.Left:
                sr.sprite = m_LeftSprite;
                break;
            case Direction.Right:
                sr.sprite = m_RightSprite;
                break;            
        }
    }
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}
