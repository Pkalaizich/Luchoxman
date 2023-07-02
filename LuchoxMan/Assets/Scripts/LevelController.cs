using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private int m_Goals;
    public int GOALS => m_Goals;
    
    public int completedGoals =0;

    private void Start()
    {
        completedGoals = 0;
    }

    public void ChangeCompletedGoals(int i)
    {
        completedGoals += i;
        if(completedGoals >= m_Goals) 
        {
            Debug.Log("Ganaste");
        }
    }


    public void AddAmountOfGoals()
    {
        m_Goals++;
    }

}
