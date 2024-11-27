using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBox : BaseUI
{
    private void Awake()
    {
        Bind();
        Init();
    }
    private void Start()
    {
        SubscribesEvents();
    }

    /// <summary>
    /// 로딩 스톱
    /// </summary>
    private void Stop()
    {
        // TODO : 스톱기능을 어떻게 구현해야 하는가
        LobbyScene.SetIsLoadingCancel();
        gameObject.SetActive(false);
    }

    private void Init()
    {

    }
    private void SubscribesEvents()
    {
        GetUI<Button>("LoadingCancelButton").onClick.AddListener(Stop);
    }


}
