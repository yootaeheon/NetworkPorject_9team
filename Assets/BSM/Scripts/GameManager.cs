using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //테스트용 코드

    public static GameManager Instance { get; private set; }
    [field:SerializeField] public bool MissionDelay { get; set; }
 
    [SerializeField] public Slider _sli;

    int total = 30;
    public int score = 0;

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TEST()
    { 
        score++;
        _sli.value = (float)(score / total);  
    }

}
