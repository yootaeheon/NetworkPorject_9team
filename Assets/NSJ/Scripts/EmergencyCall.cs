using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EmergencyCall : BaseUI
{
    private EmergencyCallButton _button => GetUI<EmergencyCallButton>("Button");
    private GameObject _buttonPush => GetUI("ButtonPush");
    private Animator _animator;

    int _closePopUpHash = Animator.StringToHash("ClosePopup");

    private void Awake()
    {
        Bind();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SubscribeEvents();
    }

    /// <summary>
    /// 버튼 클릭시 누른 이미지 나오기
    /// </summary>
    private void ClickDownButton()
    {
        _buttonPush.SetActive(true);
    } 

    /// <summary>
    /// 버튼 클릭 해제 시 누른 이미지 비표시 및 긴급회의
    /// </summary>
    private void ClickUpButton()
    {
        _buttonPush.SetActive(false);
        // 마우스 포인트가 버튼 위에 있을 시에 긴급소집
        if (_button.OnButton)
        {
            // TODO : 긴급회의
        }
    }

    [PunRPC]
    private void RPCEmergencyCall()
    {
        StartCoroutine(EmergencyCallRoutine());
    }

    IEnumerator EmergencyCallRoutine()
    {
       // GameUI.ShowEmergency();
        yield return GameUI.Emergency.Duration.GetDelay();
        if (PhotonNetwork.IsMasterClient == true)
        {
            SceneChanger.LoadScene("VoteScene", LoadSceneMode.Additive);
        }
    }


    private void Close()
    {
        StartCoroutine(CloseRoutine());    
    }

    IEnumerator CloseRoutine()
    {
       _animator.Play(_closePopUpHash);
        yield return 0.2f.GetDelay();
        gameObject.SetActive(false);
    }

    private void SubscribeEvents()
    {
        GetUI<Button>("CloseButton").onClick.AddListener(Close);
        _button.OnClickDown += ClickDownButton;
        _button.OnClickUp += ClickUpButton;
    }
}
