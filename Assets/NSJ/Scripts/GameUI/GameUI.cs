using GameUIs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    public static ReportUI Report { get { return Instance._reportUI; } }
    public static EmergencyUI Emergency { get { return Instance._emergencyUI; } }  
    public static GameStartUI GameStart { get { return Instance._gameStartUI; } }
    public static PlayerUI Player { get { return Instance._playerUI; } }
    public static VoteResultUI VoteResult { get { return Instance._voteResultUI; } }


    [SerializeField] ReportUI _reportUI;
    [SerializeField] EmergencyUI _emergencyUI;
    [SerializeField] GameStartUI _gameStartUI;
    [SerializeField] PlayerUI _playerUI;
    [SerializeField] VoteResultUI _voteResultUI;

    private void Awake()
    {
        InitSingleTon();
    }

    /// <summary>
    /// 직업선정 UI 세팅하면서 활성화
    /// </summary>
    public static void ShowGameStart(PlayerType type,Color color)
    {
        GameStart.SetActive(true);
        GameStart.SetUI(type, color);
    }

    /// <summary>
    /// 플레이어 UI 활성화
    /// </summary>
    public static void ShowPlayer(PlayerType type)
    {
        Player.SetActive(true);
        Player.SetUI(type);
    }

    /// <summary>
    /// 리폿 활성화
    /// </summary>
    public static void ShowReport(Color reporterColor, Color corpseColor)
    {
        Report.SetColor(reporterColor, corpseColor);
        Report.SetActive(true);
    }

    /// <summary>
    /// 긴급 소집 활성화
    /// </summary>
    /// <param name="playerColor"></param>
    public static void ShowEmergency(Color playerColor)
    {
        Emergency.SetColor(playerColor);
        Emergency.SetActive(true);
    }

    /// <summary>
    /// 추방 컷씬 활성화
    /// </summary>
    public static void ShowVoteKick(Color playerColor, string name, PlayerType type)
    {
        VoteResult.SetUI(playerColor, name, type);
        VoteResult.SetActiveKick(true);
    }
    /// <summary>
    /// 스킵 컷씬 활성화
    /// </summary>
    /// <param name="result"></param>
    public static void ShowVoteSkip()
    {
        VoteResult.SetActiveSkip(true);
    }

    private void InitSingleTon()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
