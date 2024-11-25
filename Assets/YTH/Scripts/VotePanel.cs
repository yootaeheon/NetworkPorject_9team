using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VotePanel : MonoBehaviourPunCallbacks
{
    [SerializeField] VoteSceneData _voteData;

    [SerializeField] VoteScenePlayerData _playerData;


    // [SerializeField] GameObject _characterImage; // 투표창에서 각 플레이어 캐릭터 이미지

    //  [SerializeField] Image _voteSignImage; // 투표한 플레이어 표시 이미지

    //  [SerializeField] Image _deadSignImage; // 죽은 상태 표시 이미지

    [SerializeField] GameObject _anonymPlayerImage; // 투표한 익명의 플레이어 이미지

    [SerializeField] TMP_Text _nickNameText; // 각 플레이어 닉네임

    [SerializeField] GameObject _votePanel; // 투표창 전체 패널

    [SerializeField] GameObject _playerPanel; // 각 플레이어 패널

    [SerializeField] Button _voteButton; // 각 플레이어 패널을 누를 시 투표 되는 버튼

    [SerializeField] Button _skipButton;

    [SerializeField] Slider _reportTimeCountSlider; // 신고자만 말할 수 있는 시간 카운트

    [SerializeField] Slider _voteTimeCountSlider; // 투표 가능 시간 카운트

    [SerializeField] Player _targetPlayer; // 선택한 플레이어 

    private PlayerType _playerType; // Duck이 마피아

    public void SetPlayerPanel(Player player)     // 각 플레이어 패널을 초기화하는 함수
    {
         _nickNameText.text = player.NickName;
        //TODO : _characterImage = ""; // 캐릭터 이미지 불러오기
        //TODO : 플레이어가 죽은 상태라면 그 플레이어 투표 버튼 비활성화
        _targetPlayer = player;
    }

    private void Awake()
    {
        Init();
        PhotonNetwork.PlayerList.InitCustomProperties();
    }

    private void Init()
    {
        _reportTimeCountSlider.value = _voteData.ReportTimeCount;
        _voteTimeCountSlider.value = _voteData.VoteTimeCount;
    }
   

    private void Update()
    {
        CountTime();
    }

    public void OnClickPlayerPanel() // 플레이어 패널을 눌러 투표
    {
        PhotonNetwork.LocalPlayer.VotePlayer();
        _playerData.DidVote = true;
    }

    
    public void OnClickSkip() // 스킵 버튼 누를 시
    {
        _voteData.SkipCount++;
        _playerData.DidVote = true;
       
    }

    public void CountTime()
    {
        _voteData.ReportTimeCount -= (float)Time.deltaTime;
        Debug.Log(_voteData.ReportTimeCount);

        if (_voteData.ReportTimeCount <= 0)
        {

            _reportTimeCountSlider.gameObject.SetActive(false);
            _voteData.VoteTimeCount -= (float)PhotonNetwork.Time;
            Debug.Log(_voteData.VoteTimeCount);
        }
    }

   
}
