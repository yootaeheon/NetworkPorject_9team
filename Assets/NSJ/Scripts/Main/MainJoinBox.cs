using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainJoinBox : BaseUI
{
    private TMP_InputField _joinRoomInput => GetUI<TMP_InputField>("JoinRoomInput");
    private TMP_InputField _joinNickNameInput => GetUI<TMP_InputField>("JoinNickNameInput");
    private GameObject _joinInvisibleOnImage => GetUI("JoinInvisibleOnImage");

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
        ClearJoinBox();
    }

    /// <summary>
    /// 로비 입장
    /// </summary>
    private void JoinLobby()
    {
        string nickName = _joinNickNameInput.text; // 닉네임 캐싱


        LoadingBox.StartLoading(); // 로딩창 활성화

        if (nickName != string.Empty) // 닉네임 변경점 있으면 닉네임 변경
        {
            nickName.ChangeNickName(); // 닉네임 변경(포톤네트워크 닉네임 변경, 데이터베이스 닉네임 변경      
        }

        LoadingBox.StartLoading();
        PhotonNetwork.JoinLobby(); // 로비 입장
    }
    /// <summary>
    /// 방 코드 자동 대문자 변환
    /// </summary>
    private void ChangeRoomCodeToUpper(string value)
    {
        _joinRoomInput.text = value.ToUpper();
    }

    /// <summary>
    /// 방코드 숨기기/보이기
    /// </summary>
    private void ChangeRoomCodeInvisible()
    {
        if (_joinRoomInput.contentType == TMP_InputField.ContentType.Password) //안보이는 상태
        {
            _joinRoomInput.contentType = TMP_InputField.ContentType.Standard;
            _joinInvisibleOnImage.SetActive(false);
        }
        else
        {
            _joinRoomInput.contentType = TMP_InputField.ContentType.Password;
            _joinInvisibleOnImage.SetActive(true);
        }
        StartCoroutine(ChangeRoomCodeInvisibleRoutine());
    }

    IEnumerator ChangeRoomCodeInvisibleRoutine()
    {
        string temp = _joinRoomInput.text;
        _joinRoomInput.text = string.Empty;
        yield return null;
        _joinRoomInput.text = temp;
    }


    /// <summary>
    /// 방 코드로 입장
    /// </summary>
    private void JoinRoomCode()
    {
        string roomCode = _joinRoomInput.text; //방 코드 캐싱
        if (roomCode == string.Empty)
            return;

        string nickName = _joinNickNameInput.text; // 닉네임 캐싱

        LoadingBox.StartLoading(); // 로딩창 활성화

        if (nickName != string.Empty) // 닉네임 변경점 있으면 닉네임 변경
        {
            nickName.ChangeNickName(); // 닉네임 변경(포톤네트워크 닉네임 변경, 데이터베이스 닉네임 변경      
        }

        PhotonNetwork.JoinRoom(roomCode); // 방 코드로 방 입장
    }


    /// <summary>
    /// 방 입장(게임 하기 버튼) 포기화
    /// </summary>
    private void ClearJoinBox()
    {
        _joinNickNameInput.text = string.Empty;
        _joinRoomInput.text = string.Empty;
        _joinInvisibleOnImage.SetActive(false);
    }
    private void Init()
    {

    }
    private void SubscribesEvent()
    {
        GetUI<Button>("JoinBackButton").onClick.AddListener(() => MainPanel.ChangeBox(MainPanel.Box.Main));
        GetUI<Button>("JoinBackButton").onClick.AddListener(() => SoundManager.SFXPlay(SoundManager.Data.ButtonOff));

        GetUI<Button>("JoinCreateRoomButton").onClick.AddListener(() => MainPanel.ChangeBox(MainPanel.Box.Create));
        GetUI<Button>("JoinCreateRoomButton").onClick.AddListener(() => SoundManager.SFXPlay(SoundManager.Data.ButtonClick));

        _joinRoomInput.onValueChanged.AddListener(ChangeRoomCodeToUpper);
        GetUI<Button>("JoinInvisibleButton").onClick.AddListener(ChangeRoomCodeInvisible);
        GetUI<Button>("JoinInvisibleButton").onClick.AddListener(() => SoundManager.SFXPlay(SoundManager.Data.ButtonClick));

        GetUI<Button>("JoinLobbyButton").onClick.AddListener(JoinLobby);
        GetUI<Button>("JoinLobbyButton").onClick.AddListener(() => SoundManager.SFXPlay(SoundManager.Data.ButtonClick));

        GetUI<Button>("JoinRoomButton").onClick.AddListener(JoinRoomCode);
        GetUI<Button>("JoinRoomButton").onClick.AddListener(() => SoundManager.SFXPlay(SoundManager.Data.ButtonClick));
    }
}
