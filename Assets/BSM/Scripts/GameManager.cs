using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
        Debug.Log((score / 30f) * 100f);
        _sli.value = (score / 30f); 
        
    }

}
