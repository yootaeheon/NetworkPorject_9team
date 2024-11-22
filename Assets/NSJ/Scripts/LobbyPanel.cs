using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanel : BaseUI
{
    [SerializeField] GameObject _loadingBox;

    #region private 필드
    enum Box { Lobby ,Size}
    private GameObject[] _boxs = new GameObject[(int)Box.Size];
    #endregion

    private void Awake()
    {
        Bind();
        Init();
        
    }

    private void Start()
    {
        SubscribeEvent();
    }

    private void OnEnable()
    {
        ChangeBox(Box.Lobby);
    }

    /// <summary>
    /// 로비 떠나기
    /// </summary>
    private void LeftLobby()
    {
        PhotonNetwork.LeaveLobby();
    }

    #region 패널 조작

    /// <summary>
    /// UI 박스 변경
    /// </summary>
    private void ChangeBox(Box box)
    {
        ActivateLoadingBox(false);

        for (int i = 0; i < _boxs.Length; i++)
        {
            if (_boxs[i] == null)
                return;

            if (i == (int)box) // 바꾸고자 하는 박스만 활성화
            {
                _boxs[i].SetActive(true);
                ClearBox(box); // 초기화 작업도 진행
            }
            else
            {
                _boxs[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// UI 박스 초기화 작업
    /// </summary>
    /// <param name="box"></param>
    private void ClearBox(Box box)
    {
        switch (box)
        {
            default:
                break;
        }
    }

    /// <summary>
    /// 로딩 화면 활성화 / 비활성화
    /// </summary>
    private void ActivateLoadingBox(bool isActive)
    {
        if (isActive)
        {
            _loadingBox.SetActive(true);
        }
        else
        {
            _loadingBox.SetActive(false);
        }
    }

    #endregion

    /// <summary>
    /// 초기 설정
    /// </summary>
    private void Init()
    {
        _boxs[(int)Box.Lobby] = GetUI("LobbyBox");
    }
    /// <summary>
    /// 이벤트 구독
    /// </summary>
    private void SubscribeEvent()
    {
        GetUI<Button>("LobbyBackButton").onClick.AddListener(LeftLobby);
    }
}
