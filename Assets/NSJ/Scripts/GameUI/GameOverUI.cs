using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : BaseUI 
{
    private GameObject _gooseWinUI => GetUI("GooseWinGround");
    private GameObject _duckWinUI => GetUI("DuckWinGround");

    private void Awake()
    {
        Bind();
    }
    private void Start()
    {
        SubscribesEvent();

        SetActive(false);
        // 마스터 클라이언트만 백 버튼 이용 가능        
    }

    /// <summary>
    /// UI 끄기(false 전용)
    /// </summary>
    public void SetActive(bool active)
    {
        GetUI("GooseWinUI").SetActive(active);
    }

    /// <summary>
    /// UI 나타나기, 오리 승리, 거위 승리 지정가능
    /// </summary>
    /// <param name="active"></param>
    /// <param name="type"></param>
    public void SetActive(bool active,PlayerType type)
    {
        GetUI("GooseWinUI").SetActive(active);

        GetUI("BackButton").SetActive(PhotonNetwork.IsMasterClient == true);
        _gooseWinUI.SetActive(type == PlayerType.Goose);
        _duckWinUI.SetActive(type == PlayerType.Duck);
    }

    private void SubscribesEvent()
    {
        GetUI<Button>("BackButton").onClick.AddListener(() => { GameLoadingScene.BackLobby(); });
    }
}
