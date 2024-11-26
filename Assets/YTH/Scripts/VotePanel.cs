using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class VotePanel : MonoBehaviourPunCallbacks
{
    public const string RoomName = "playerpaneltest";

    [SerializeField] VoteManager _voteManager;

    [SerializeField] VoteSceneData _voteData;

    private PlayerType _playerType; // Duck이 마피아

    [SerializeField] GameObject[] _panelList; // PlayerActorNumber 인덱스로 미리 생성해둔 패널들을 리스트에 담아 연결

    [SerializeField] GameObject[] _SkipanonymImage; // 스킵 수 만큼 익명 이미지 생성

    [SerializeField] VoteScenePlayerData[] playerData;

    #region UI Property
    [Header("UI")]
    // [SerializeField] GameObject _characterImage; // 투표창에서 각 플레이어 캐릭터 이미지

    //  [SerializeField] Image _voteSignImage; // 투표한 플레이어 표시 이미지

    //  [SerializeField] Image _deadSignImage; // 죽은 상태 표시 이미지

    [SerializeField] TMP_Text _nickNameText; // 각 플레이어 닉네임 텍스트

    [SerializeField] GameObject _votePanel; // 투표창 전체 패널

    [SerializeField] GameObject _playerPanel; // 각 플레이어 패널

    [SerializeField] Button _skipButton;

    [SerializeField] Slider _reportTimeCountSlider; // 신고자만 말할 수 있는 시간 카운트

    [SerializeField] Slider _voteTimeCountSlider; // 투표 가능 시간 카운트
    #endregion

    private void Awake()
    {
        Init();
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnSkipAnonymImage();
        }
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
        SpawnPlayerPanel();
        SetPlayerPanel(PhotonNetwork.LocalPlayer); // 모든 플레이어를 업데이트하게 수정하기
        foreach (Player player in PhotonNetwork.PlayerList) // 수정 필요
        {
            GetComponent<VoteScenePlayerData>();
        }

        // 투표 씬 입장 때마다 모든 플레이어 _playerData.DidVote == false 해주기
    }

    // 각 플레이어 패널을 세팅하는 함수
    public void SetPlayerPanel(Player player)
    {
        // _nickNameText.text = player.NickName; // 닉네임 불러오기
        //TODO : _characterImage = ""; // 캐릭터 이미지 불러오기
        //TODO : 죽은 캐릭터에 사망 표시 띄워놓기
        //TODO : 플레이어가 죽은 상태라면 그 플레이어 투표 버튼 비활성화 // button.interatable == false
    }

    // 플레이어 패널 생성 함수
    public void SpawnPlayerPanel()
    {   //ActorNumber 1 부터 시작 
        // 인덱스 번호로 매개변수를 받기 위해서 -1
        photonView.RPC("SpawnPlayerPanelRPC", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber - 1);
    }

    [PunRPC]
    public void SpawnPlayerPanelRPC(int index)
    {
        _panelList[index].SetActive(true);
        _panelList[index].GetComponent<VoteScenePlayerData>().VoteButton.onClick.AddListener(() => { _voteManager.Vote(index); });
    }


    //투표 종료 후 스킵 수 만큼 익명 이미지 생성
    public void SpawnSkipAnonymImage()
    {
        photonView.RPC("SpawnSkipAnonymImageRPC", RpcTarget.All, _voteData.SkipCount);
    }

    [PunRPC]
    public void SpawnSkipAnonymImageRPC(int index)
    {
        for (int i = 0; i < index; i++)
        {
            _SkipanonymImage[i].SetActive(true);
        }
    }

    //투표 종료 후 득표 수 만큼 플레이어 패널에 익명 이미지 생성
    public void SpawnAnonymImage()
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
                //TODO : 득표수만큼 활성화
            }
        }
    }

    public void CountTime()
    {
        _voteData.ReportTimeCount -= (float)Time.deltaTime; // Time.deltaTime 수정 필요 시 수정
        //Debug.Log(_voteData.ReportTimeCount);
        _reportTimeCountSlider.value = _voteData.ReportTimeCount;
        if (_voteData.ReportTimeCount <= 0)
        {
            _reportTimeCountSlider.gameObject.SetActive(false); // 추후 수정할 것
            _voteData.VoteTimeCount -= (float)Time.deltaTime;
            _voteTimeCountSlider.value = _voteData.VoteTimeCount;
            //Debug.Log(_voteData.VoteTimeCount);

            if (_voteData.VoteTimeCount <= 0)
            {
              //  SpawnAnonymImage();
                SpawnSkipAnonymImage();
                _voteManager.GetVoteResult();
            }
        }
    }
}
