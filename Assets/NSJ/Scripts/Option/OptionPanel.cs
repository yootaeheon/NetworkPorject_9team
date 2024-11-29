using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionPanel : MonoBehaviour
{
    public static OptionPanel Instance;

    private void Awake()
    {
        InitSingleTon();
    }

    /// <summary>
    /// ΩÃ±€≈Ê º≥¡§
    /// </summary>
    private void InitSingleTon()
    {
        if(Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
