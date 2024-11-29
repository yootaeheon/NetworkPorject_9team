using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionQuitBox : BaseUI
{
    private TMP_Text _quitText => GetUI<TMP_Text>("QuitText");
    enum ButtonType { Quit , MainMenu, Size}
    private GameObject _gameQuitButton => GetUI("GameQuitButton");
    private GameObject _mainMenuButton => GetUI("MainMenuButton");
    private GameObject[] _buttons = new GameObject[(int)ButtonType.Size];
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
        ClearQuitBox();
    }

    /// <summary>
    /// 패널에 따른 UI 변화
    /// </summary>
    private void ClearQuitBox()
    {
      
        // 게임중일때(로비씬이 아닌경우
        if(LobbyScene.Instance == null )
        {
            ChangeButton(ButtonType.MainMenu);
            _quitText.SetText("게임에서 나갑니까?".GetText());
        }
               // 방에 있을때는 메인 메뉴 버튼이 나오도록
        else if (PhotonNetwork.InRoom)
        {
            ChangeButton(ButtonType.MainMenu);
            _quitText.SetText("돌아가시겠습니까?".GetText());
        }
        else
        {
            ChangeButton(ButtonType.Quit);
            _quitText.SetText("가시는 건가요?".GetText());
        }
    }

    /// <summary>
    /// 버튼 변경
    /// </summary>
    /// <param name="button"></param>
    private void ChangeButton(ButtonType button)
    {
        //인자값 button 에 해당하는 버튼 빼고 모두 false
        for (int i = 0; i < _buttons.Length; i++) 
        {
            if(i == (int)button) 
            {
                _buttons[i].SetActive(true);
            }
            else
            {
                _buttons[i].SetActive(false);
            }
        }
    }
    /// <summary>
    ///  게임 종료
    /// </summary>
   private void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();        
#endif
    }

    private void ClickMainMenu()
    {
        if (LobbyScene.Instance == null)
        {
            LeaveGame();
        }
        else
        {
            ChangeMainmenu();
        }
    }

    /// <summary>
    /// 메인 메뉴 이동
    /// 방떠나기
    /// </summary>
    private void ChangeMainmenu()
    {
        // 옵션 끄기
        LobbyScene.ActivateOptionBox(false);

        // 로딩화면
        LobbyScene.ActivateLoadingBox(true);
        // 방떠나기
        PhotonNetwork.LeaveRoom();     
    }

    private void LeaveGame()
    {
        // 방떠나기
        PhotonNetwork.LeaveRoom();
        SceneChanger.LoadLevel(0);
    }

    private void Init()
    {
        _buttons[(int)ButtonType.Quit] = _gameQuitButton;
        _buttons[(int)ButtonType.MainMenu] = _mainMenuButton;       
    }
    private void SubscribeEvent()
    {
        GetUI<Button>("GameQuitButton").onClick.AddListener(GameQuit);
        GetUI<Button>("MainMenuButton").onClick.AddListener(ClickMainMenu);
    }
}
