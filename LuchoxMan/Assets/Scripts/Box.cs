using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private LayerMask m_ObstacleLayers;
    [SerializeField] private float m_MoveSpeed = 5f;
    [SerializeField] private Transform m_MovePoint;

    [SerializeField] private LayerMask m_GoalLayers;

    [SerializeField] private Color m_NormalColor;
    [SerializeField] private Color m_OnGoalColor;

    private bool onGoal = false;

    private LevelController lvlController;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        m_MovePoint.parent = null;
        onGoal = false;
        lvlController = FindObjectOfType<LevelController>();
    }

    void Update()
    {        
        transform.position = Vector3.MoveTowards(transform.position, m_MovePoint.position, m_MoveSpeed * Time.deltaTime);
    }

    public bool ChangePosition(Vector3 direction)
    {
        if (!Physics2D.OverlapCircle(m_MovePoint.position + direction, .05f, m_ObstacleLayers))
        {                      
            m_MovePoint.position += direction;
            if (Physics2D.OverlapCircle(m_MovePoint.position, .05f, m_GoalLayers))
            {
                if(!onGoal)
                    lvlController.ChangeCompletedGoals(1);
                onGoal = true;
                sr.color = m_OnGoalColor;
            }
            else
            {
                if(onGoal)
                {
                    lvlController.ChangeCompletedGoals(-1);
                    onGoal = false;
                    sr.color = m_NormalColor;
                }
            }
            return true;
        }
        return false;
    }
}
