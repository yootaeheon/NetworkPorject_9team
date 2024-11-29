using Firebase.Auth;
using Firebase.Extensions;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionDeleteBox : BaseUI
{
    enum Box { Delete, Confirm, Error, Success, Size}
    private GameObject[] _boxs = new GameObject[(int)Box.Size];

    private TMP_InputField _confirmEmailInput => GetUI<TMP_InputField>("ConfirmEmailInput");
    private TMP_InputField _confirmPasswordInput => GetUI<TMP_InputField>("ConfirmPasswordInput");
    private GameObject _confirmButton => GetUI("ConfirmButton");

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
        ChangeBox(Box.Delete);
    }

    /// <summary>
    /// 계정 삭제 전 계정 인증
    /// </summary>
    private void ConfirmEmail()
    {
        // 이메일 패스워드 캐싱
        string email = _confirmEmailInput.text;
        string password = _confirmPasswordInput.text;

        FirebaseUser user = BackendManager.Auth.CurrentUser;
        Credential credential = EmailAuthProvider.GetCredential(email, password);

        // 사용자 이메일 재 인증 시도
        user.ReauthenticateAsync(credential)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    ChangeBox(Box.Error);
                    return;
                }
                // 이메일 인증 성공 시 유저 삭제
                DeleteUser();
            });
    }

    /// <summary>
    /// 계정 삭제
    /// </summary>
    private void DeleteUser()
    {
        FirebaseUser user = BackendManager.Auth.CurrentUser;
        // 계정 삭제 시도
        user.DeleteAsync()
            .ContinueWithOnMainThread(task =>
            {
                if(task.IsCanceled || task.IsFaulted)
                {
                    ChangeBox(Box.Error); 
                    return;
                }

                ChangeBox(Box.Success);
            });
    }


    /// <summary>
    /// 서버 연결 해제
    /// </summary>
    private void DisconnectServer()
    {
        // 삭제 시 자동으로 서버 연결 해제
        LobbyScene.ActivateLoadingBox(true);
        PhotonNetwork.Disconnect();
    }

    /// <summary>
    /// 인증 버튼 활성화 체크
    /// </summary>
    /// <param name="text"></param>
    private void ActivateConfirmButton(string text)
    {
        _confirmButton.SetActive(false);
        // 이메일과 비밀번호 둘다 입력해야만 활성화
        if (_confirmEmailInput.text == string.Empty)
            return;
        if (_confirmPasswordInput.text == string.Empty)
            return;
        _confirmButton.SetActive(true);
    }
    /// <summary>
    /// UI 박스 변경
    /// </summary>
    /// <param name="box"></param>
    private void ChangeBox(Box box)
    {
        for (int i = 0; i < _boxs.Length; i++) 
        {
            if(i == (int)box)
            {
                _boxs[i].SetActive(true);
                ClearBox(box);
            }
            else
            {
                _boxs[i].SetActive(false);
            }
        }
    }
    /// <summary>
    /// 박스 클리어 선택
    /// </summary>
    /// <param name="box"></param>
    private void ClearBox(Box box)
    {
        switch(box)
        {
            case Box.Confirm:
                ClearConfirmBox();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 계정 인증 박스 클리어
    /// </summary>
    private void ClearConfirmBox()
    {
        _confirmEmailInput.text =string.Empty;
        _confirmPasswordInput.text = string.Empty;
        _confirmButton.SetActive(false);
    }

    private void Init()
    {
        _boxs[(int)Box.Delete] = GetUI("DeleteBox");
        _boxs[(int)Box.Confirm] = GetUI("ConfirmBox");
        _boxs[(int)Box.Error] = GetUI("ErrorBox");
        _boxs[(int)Box.Success] = GetUI("SuccessBox");
    }
    private void SubscribeEvent()
    {
        GetUI<Button>("DeleteButton").onClick.AddListener(()=> 
        {
            if (LobbyScene.Instance != null)
            {
                ChangeBox(Box.Confirm);
            }
        });
        _confirmEmailInput.onValueChanged.AddListener(ActivateConfirmButton);
        _confirmPasswordInput.onValueChanged.AddListener(ActivateConfirmButton);
        GetUI<Button>("ConfirmButton").onClick.AddListener(ConfirmEmail);
        GetUI<Button>("ErrorBackButton").onClick.AddListener(() => ChangeBox(Box.Delete));
        GetUI<Button>("SuccessButton").onClick.AddListener(DisconnectServer);
    }
}
