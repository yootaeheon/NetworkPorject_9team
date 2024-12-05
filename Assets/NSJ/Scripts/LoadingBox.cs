using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBox : BaseUI
{
    public static LoadingBox Instance;

    private GameObject _loadingUI => GetUI("LoadingUI");
    public static GameObject LoadingUI { get { return Instance._loadingUI; } }

    private void Awake()
    {
        InitSingleTon();
        Bind();
        Init();
    }
    private void Start()
    {
        SubscribesEvents();
    }

    /// <summary>
    /// ·Îµù ½ÃÀÛ
    /// </summary>
    public static void StartLoading()
    {
        Instance._loadingUI.SetActive(true);
    }

    /// <summary>
    /// ·Îµù ½ºÅé
    /// </summary>
    public static void StopLoading()
    {

        Instance._loadingUI.SetActive(false);
    }

    private void ClickStopButton()
    {
        if (LobbyScene.Instance != null)
        {
            LobbyScene.SetIsLoadingCancel();
        }
    }

    private void Init()
    {

    }
    private void SubscribesEvents()
    {
        GetUI<Button>("LoadingCancelButton").onClick.AddListener(StopLoading);      
        GetUI<Button>("LoadingCancelButton").onClick.AddListener(ClickStopButton);
        GetUI<Button>("LoadingCancelButton").onClick.AddListener(() => SoundManager.SFXPlay(SoundManager.Data.ButtonClick));
    }

    /// <summary>
    /// ½Ì±ÛÅæ ¼³Á¤
    /// </summary>
    private void InitSingleTon()
    {
        if (Instance == null)
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
