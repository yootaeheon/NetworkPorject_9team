using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionPanel : MonoBehaviour
{
    public static OptionPanel Instance;

    [SerializeField] private GameObject _optionUI;
    public static GameObject OptionUI { get { return Instance._optionUI; } }

    private void Awake()
    {
        InitSingleTon();
    }

    public static void SetActiveOption(bool active)
    {
        Instance._optionUI.SetActive(active);
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
