using NSJ_Test;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField] GameObject _miniMapCam;

    private void Start()
    {
        _miniMapCam.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ShowMiniMap();       
        }
    }

    /// <summary>
    /// ¹Ì´Ï¸Ê ¿­±â
    /// </summary>
    private void ShowMiniMap()
    {
        _miniMapCam.SetActive(!_miniMapCam.activeSelf);
    }
}