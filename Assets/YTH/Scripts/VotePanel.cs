using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VotePanel : MonoBehaviourPunCallbacks
{
    public const string RoomName = "playerpaneltest";

    [SerializeField] VoteManager _voteManager;

    [SerializeField] VoteSceneData _voteData;

    [SerializeField] VoteScenePlayerData _playerData;

    [SerializeField] Player _targetPlayer; // 선택한 플레이어 

    [SerializeField] Player[] player;

    private PlayerType _playerType; // Duck이 마피아

    public GameObject[] panelList;

    #region UI Property
    [Header("UI")]
    // [SerializeField] GameObject _characterImage; // 투표창에서 각 플레이어 캐릭터 이미지

    //  [SerializeField] Image _voteSignImage; // 투표한 플레이어 표시 이미지

    //  [SerializeField] Image _deadSignImage; // 죽은 상태 표시 이미지

    [SerializeField] GameObject _anonymPlayerImage; // 투표한 익명의 플레이어 이미지

    [SerializeField] TMP_Text _nickNameText; // 각 플레이어 닉네임 텍스트

    [SerializeField] GameObject _votePanel; // 투표창 전체 패널

    [SerializeField] GameObject _playerPanel; // 각 플레이어 패널

    [SerializeField] Button _voteButton; // 각 플레이어 패널을 누를 시 투표 되는 버튼

    [SerializeField] Button _skipButton;

    [SerializeField] Slider _reportTimeCountSlider; // 신고자만 말할 수 있는 시간 카운트

    [SerializeField] Slider _voteTimeCountSlider; // 투표 가능 시간 카운트
    #endregion

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

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void Update()
    {
        CountTime();
       // _voteButton.interactable = _voteData.ReportTimeCount <= 0;  위치 수정할것
       // _skipButton.interactable = _voteData.ReportTimeCount <= 0;
       //
       // _voteButton.interactable = _voteData.VoteTimeCount <= 0;
       // _skipButton.interactable = _voteData.VoteTimeCount <= 0;
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 8;
        options.IsVisible = false;

        PhotonNetwork.JoinOrCreateRoom(RoomName, options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        SpawnPanelWithSetParent();
        SetPlayerPanel(PhotonNetwork.LocalPlayer); // 모든 플레이어를 업데이트하게 수정하기
        foreach (Player player in PhotonNetwork.PlayerList) // 수정 필요
        {
            GetComponent<VoteScenePlayerData>();
        }

       

       // _playerData.IsDead = false;
       // _playerData.IsReporter = false;
       // _playerData.DidVote = false;
    }

    // 각 플레이어 패널을 세팅하는 함수
    public void SetPlayerPanel(Player player)     
    {
        // _nickNameText.text = player.NickName;
        //TODO : _characterImage = ""; // 캐릭터 이미지 불러오기
        //TODO : 플레이어가 죽은 상태라면 그 플레이어 투표 버튼 비활성화
        _targetPlayer = player;
    }

    // 플레이어 패널 생성 함수
    public void SpawnPanelWithSetParent()
    {
        GameObject myPanel = PhotonNetwork.Instantiate("PlayerPanel", Vector2.zero, Quaternion.identity);
        //myPanel.transform.SetParent(_voteData.PlayerPanelParent, false);
        Debug.Log($"{PhotonNetwork.LocalPlayer} 생성 완료");
        photonView.RPC("SetParentRPC", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber-1);
       
    }

    [PunRPC]
    public void SetParentRPC(int index)
    {
        panelList[index].SetActive(true);
        panelList[index].GetComponent<VoteScenePlayerData>().voteButton.onClick.AddListener(() => { _voteManager.OnClickPlayerPanel(index); });
    }

    public void CountTime()
    {
        _voteData.ReportTimeCount -= (float)Time.deltaTime;
        //Debug.Log(_voteData.ReportTimeCount);

        if (_voteData.ReportTimeCount <= 0)
        {
            _reportTimeCountSlider.gameObject.SetActive(false); // 추후 수정할 것
            _voteData.VoteTimeCount -= (float)Time.deltaTime;
            //Debug.Log(_voteData.VoteTimeCount);

            if (_voteData.VoteTimeCount == 0)
            {
                PhotonNetwork.LocalPlayer.GetVoteResult();
            }
        }
    }
}
