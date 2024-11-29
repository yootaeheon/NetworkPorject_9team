using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VotePanel : MonoBehaviourPunCallbacks
{
    public const string RoomName = "playerpaneltest";

    [SerializeField] VoteManager _voteManager;

    [SerializeField] VoteSceneData _voteData;

    private PlayerType _playerType; // Duck이 마피아

    [SerializeField] GameObject[] _panelList; // PlayerActorNumber 인덱스로 미리 생성해둔 패널들을 리스트에 담아 연결

    [SerializeField] GameObject[] _SkipAnonymImage; // 스킵 수 만큼 익명 이미지 생성

    [SerializeField] GameObject[] _panelAnonymImage; // 2차원 배열 이용하여 구현 계획

    [SerializeField] VoteScenePlayerData[] _playerData;

    [SerializeField] Button[] _voteButtons; // 투표하기 위한 버튼들

    #region UI
    [Header("UI")]
    // [SerializeField] GameObject _characterImage; // 투표창에서 각 플레이어 캐릭터 이미지

    //  [SerializeField] Image _voteSignImage; // 투표한 플레이어 표시 이미지

    //  [SerializeField] Image _deadSignImage; // 죽은 상태 표시 이미지

    [SerializeField] TMP_Text _nickNameText; // 각 플레이어 닉네임 텍스트

    [SerializeField] TMP_Text _stateText;

    [SerializeField] GameObject _votePanel; // 투표창 전체 패널

    [SerializeField] GameObject _playerPanel; // 각 플레이어 패널

    [SerializeField] public Button _skipButton;

    [SerializeField] Slider _reportTimeCountSlider; // 신고자만 말할 수 있는 시간 카운트

    [SerializeField] Slider _voteTimeCountSlider; // 투표 가능 시간 카운트
    #endregion

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _reportTimeCountSlider.maxValue = _voteData.ReportTimeCount;
        _voteTimeCountSlider.maxValue = _voteData.VoteTimeCount;

        _reportTimeCountSlider.value = _voteData.ReportTimeCount;
        _voteTimeCountSlider.value = _voteData.VoteTimeCount;
    }

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            SpawnPlayerPanel();
        }
     

        // 투표씬 입장 시 투표 여부 false로 초기화
        //for (int i = 0; i < 12; i++)
        //{
        //    _playerData[i].DidVote = false;
        //}
    }
    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 8;
        options.IsVisible = false; // 비공개 방

        PhotonNetwork.JoinOrCreateRoom(RoomName, options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        SpawnPlayerPanel();
    }


    private void Update()
    {
        Debug.Log(PhotonNetwork.NetworkClientState);

        CountTime();
    }

    // 각 플레이어 패널을 세팅하는 함수
    private void SetPlayerPanel(GameObject[] panelList)
    {
        // _nickNameText.text = player.NickName; // 닉네임 불러오기
        //TODO : _characterImage = ""; // 캐릭터 이미지 불러오기
        //TODO : 죽은 캐릭터에 사망 표시 띄워놓기
        //TODO : 플레이어가 죽은 상태라면 그 플레이어 투표 버튼 비활성화 // button.interatable == false
    }

    // 플레이어 패널 생성 함수
    private void SpawnPlayerPanel()
    {
        photonView.RPC("SpawnPlayerPanelRPC", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber - 1);
    }

    [PunRPC]
    public void SpawnPlayerPanelRPC(int index)
    {
        Debug.Log("11111111111111111111111111111");
        _panelList[index].SetActive(true);
        _panelList[index].GetComponent<VoteScenePlayerData>().VoteButton.onClick.AddListener(() => { _voteManager.Vote(index); });
    }


    //투표 종료 후 스킵 수 만큼 익명 이미지 생성
    private void SpawnSkipAnonymImage()
    {
        photonView.RPC("SpawnSkipAnonymImageRPC", RpcTarget.All, _voteData.SkipCount);
    }



    [PunRPC]
    public void SpawnSkipAnonymImageRPC(int index)
    {
        for (int i = 0; i < index; i++)
        {
            _SkipAnonymImage[i].SetActive(true);
        }
    }

    //투표 종료 후 득표 수 만큼 플레이어 패널에 익명 이미지 생성
    private void SpawnAnonymImage()
    {
        photonView.RPC("SpawnAnonymImageRPC", RpcTarget.All, _voteManager._voteCounts);
    }

    [PunRPC]
    public void SpawnAnonymImageRPC(int index)
    {
        for (int i = 0; i < 12; i++)
        {
            for (int j = 0; j < index; j++)
            {
                //TODO : 득표수만큼 활성화, 2차원 배열 사용
            }
        }
    }

    // 시간 측정 함수
    private void CountTime()
    {
        foreach (Button button in _voteButtons)
        {
            button.interactable = false;
            _skipButton.interactable = false;
        }


        _voteData.ReportTimeCount -= (float)Time.deltaTime; // Time.deltaTime 수정 필요 시 수정
        _reportTimeCountSlider.value = _voteData.ReportTimeCount;
        if (_voteData.ReportTimeCount <= 0) // 리포트 타임 종료 시 투표, 스킵 버튼 활성화
        {
            _stateText.text = "VOTE!";
            foreach (Button button in _voteButtons)
            {
                button.interactable = true;
                _skipButton.interactable = true;
            }
            _reportTimeCountSlider.gameObject.SetActive(false); // 추후 수정할 것
            _voteData.VoteTimeCount -= (float)Time.deltaTime;
            _voteTimeCountSlider.value = _voteData._voteTimeCount;
            if (_voteData.VoteTimeCount <= 0) // 투표 시간 종료 시 투표, 스킵 버튼 비활성화
            {
                DisableButton();
                //  SpawnAnonymImage();
                SpawnSkipAnonymImage();
                _voteManager.GetVoteResult();
            }
        }
    }

    // 투표 버튼 비활성화 함수
    public void DisableButton()
    {
        foreach (var button in _voteButtons)
        {
            button.enabled = false;
            Debug.Log("투표버튼 비활성화");
        }
        _skipButton.enabled = false;
    }

    IEnumerator SpawnPlayerPanelRoutine()
    {
        yield return 2f.GetDelay();
        SpawnPlayerPanel();
    }
}
