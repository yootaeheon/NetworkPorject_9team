using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RoomPanel : BaseUI
{
    [SerializeField] GameObject _loadingBox;

    #region private 필드
    private const string SHOWTEXT = "보이기";
    private const string HIDETEXT = "숨기기";
    enum Box { Room, Size }
    private GameObject[] _boxs = new GameObject[(int)Box.Size];

    private TMP_InputField _roomCodeText => GetUI<TMP_InputField>("RoomCodeText");
    private TMP_Text _roomTitleText => GetUI<TMP_Text>("RoomTitleText");
    private TMP_Text _roomCodeActiveText => GetUI<TMP_Text>("RoomCodeActiveText");
    private GameObject _roomStartButton => GetUI("RoomStartButton");
    private TMP_Text _roomPlayerCountText => GetUI<TMP_Text>("RoomPlayerCountText");
    #endregion

    private void Awake()
    {
        Bind();
        Init();
    }

    private void Start()
    {
        SubscribesEvent();
    }

    private void OnEnable()
    {
        ChangeBox(Box.Room);
    }

    /// <summary>
    /// 플레이어 변화에 따른 룸 업데이트
    /// </summary>
    private void UpdateChangeRoom()
    {
        UpdatePlayerCount();
    }
    /// <summary>
    /// 플레이어 카운트 업데이트
    /// </summary>
    private void UpdatePlayerCount()
    {
        _roomPlayerCountText.SetText($"{PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}");
    }

    /// <summary>
    /// 방코드 숨기기/ 보이기
    /// </summary>
    private void ToggleActiveRoomCode()
    {
        if (_roomCodeText.contentType == TMP_InputField.ContentType.Password)
        {
            // 보이기
            _roomCodeText.contentType = TMP_InputField.ContentType.Standard;
            _roomCodeActiveText.SetText(HIDETEXT);
        }
        else
        {
            // 숨기기
            _roomCodeText.contentType = TMP_InputField.ContentType.Password;
            _roomCodeActiveText.SetText(SHOWTEXT);
        }
        _roomCodeText.Select();
        EventSystem.current.SetSelectedGameObject(null);
    }

    /// <summary>
    /// 방 떠나기
    /// </summary>
    private void LeftRoom()
    {
        ActivateLoadingBox(true);
        PhotonNetwork.LeaveRoom();
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
            case Box.Room:
                ClearRoomBox(); 
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 룸 초기화
    /// </summary>
    private void ClearRoomBox()
    {
        _roomTitleText.SetText($"{PhotonNetwork.LocalPlayer.NickName}의 방".GetText());

        _roomCodeText.text = $"{PhotonNetwork.CurrentRoom.Name}";
        _roomCodeText.contentType = TMP_InputField.ContentType.Standard;
        _roomCodeActiveText.text = HIDETEXT;

        _roomPlayerCountText.SetText($"{PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}".GetText());
        _roomStartButton.SetActive(false);
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

    // 초기 설정
    private void Init()
    {
        _boxs[(int)Box.Room] = GetUI("RoomBox");
    }

    // 이벤트 구독
    private void SubscribesEvent()
    {
        PlayerNumbering.OnPlayerNumberingChanged += UpdateChangeRoom;

        GetUI<Button>("RoomLeftButton").onClick.AddListener(LeftRoom);
        GetUI<Button>("RoomCodeActiveButton").onClick.AddListener(ToggleActiveRoomCode);
    }

}
