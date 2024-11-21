using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class VotePanel : MonoBehaviour
{
    [SerializeField] VoteData _voteData;

    [SerializeField] Image _characterImage; // 투표창에서 각 플레이어 캐릭터 이미지

    [SerializeField] Image _voteSignImage; // 투표한 플레이어 표시 이미지

    [SerializeField] Image _deadSignImage; // 죽은 상태 표시 이미지

    [SerializeField] TMP_Text _nickNameText; // 각 플레이어 닉네임

    [SerializeField] GameObject _votePanel; // 투표창 전체 패널

    [SerializeField] GameObject _playerPanel; // 각 플레이어 패널

    [SerializeField] Button _voteButton; // 각 플레이어 패널을 누를 시 투표 되는 버튼

    [SerializeField] Button _skipButton;

    [SerializeField] Slider _reportTimeCountSlider; // 신고자만 말할 수 있는 시간 카운트

    [SerializeField] Slider _voteTimeCountSlider; // 투표 가능 시간 카운트

    [SerializeField] Player _targetPlayer; // 선택한 플레이어 


    public void SetPlayerPanel(Player target)     // 각 플레이어 패널을 초기화하는 함수
    {
        _targetPlayer = target;

        _nickNameText.text = target.NickName;


       // 임포스터 캐릭터는 그 로컬플레이어에게만 표시
       // if (localPlayer.IsImposter == true)
       // {
       //     _nickNameText.color = Color.red ;
       // }
       //

       //TODO : _characterImage = ""; // 캐릭터 이미지 불러오기

       // 플레이어가 죽은 상태라면 그 플레이어 패널을 가리는 회색 패널 on

    }
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _voteData.ReportTimeCount = _reportTimeCountSlider.value;
        _voteData.VoteTimeCount = _voteTimeCountSlider.value;
    }

    private void Update()
    {
        _voteData.ReportTimeCount -= (float)PhotonNetwork.Time;

        if (_voteData.ReportTimeCount == 0)
        {
            _voteData.VoteTimeCount -= (float)PhotonNetwork.Time;
        }
    }

    public void Vote(Player targetPlayer) // 플레이어 패널을 눌러 투표
    {
        //TODO: 누른 플레이어 득표수 ++;
        _voteData.DidVote = true;
    }

    public void OnClickSkip() 
    {
        _voteData.SkipCount++;
        _voteData.DidVote = true;
    }


}
