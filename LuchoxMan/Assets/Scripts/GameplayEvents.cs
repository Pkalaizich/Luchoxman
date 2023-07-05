using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameplayEvents : MonoBehaviour
{
    private static GameplayEvents instance;

    private readonly UnityEvent levelCompleted = new UnityEvent();
    private readonly UnityEvent levelLoaded = new UnityEvent();
    private readonly UnityEvent dataLoaded= new UnityEvent();

    public static UnityEvent OnLevelCompleted => instance.levelCompleted;
    public static UnityEvent OnLevelLoaded => instance.levelLoaded;

    public static UnityEvent OnDataLoaded => instance.dataLoaded;


    private void Awake()
    {
        instance = this;
    }
}
