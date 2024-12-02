using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VotePanel : MonoBehaviourPunCallbacks
{
    public const string RoomName = "playerpaneltest";
    PlayerDataContainer _playerDataContainer => PlayerDataContainer.Instance;

    [SerializeField] VoteManager _voteManager;

    [SerializeField] VoteSceneData _voteData;

    [SerializeField] VoteScenePlayerData[] _playerData;

    [SerializeField] GameObject[] _panelList; // PlayerActorNumber 인덱스로 미리 생성해둔 패널들을 리스트에 담아 연결

    #region UI
    [Header("UI")]

    [SerializeField] GameObject[] _SkipAnonymImage; // 스킵 수 만큼 익명 이미지 생성

    [SerializeField] GameObject[] _panelAnonymImage; // 2차원 배열 이용하여 구현 계획

    [SerializeField] Button[] _voteButtons; // 투표하기 위한 버튼들
  
    [SerializeField] GameObject[] _deadSignImage; // 죽은 상태 표시 이미지

    [SerializeField] GameObject[] _voteSignImage; // 죽은 상태 표시 이미지

    [SerializeField] TMP_Text[] _nickNameText; // 각 플레이어 닉네임 텍스트

    [SerializeField] Image[] _playerColor;

    [SerializeField] TMP_Text _stateText;

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
            SetPlayerPanel();
        }
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
        StartCoroutine(SetPlayerPanel());
    }

    private void Update()
    {
        Debug.Log(PhotonNetwork.NetworkClientState);
        CountTime();
    }

    // 각 플레이어 패널을 세팅하는 함수
    private IEnumerator SetPlayerPanel()
    {
        yield return 1f.GetDelay();
        photonView.RPC(nameof(SetPlayerPanelRPC), RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void SetPlayerPanelRPC()
    {

        for (int i = 0; i < 12; i++)
        {
            _nickNameText[i].text = _playerDataContainer.GetPlayerData(PhotonNetwork.LocalPlayer.GetPlayerNumber()).PlayerName;
            _voteSignImage[i].SetActive(false);
            _deadSignImage[i].SetActive(_playerDataContainer.GetPlayerData(PhotonNetwork.LocalPlayer.GetPlayerNumber()).IsGhost);
            _playerColor[i].color = _playerDataContainer.GetPlayerData(PhotonNetwork.LocalPlayer.GetPlayerNumber()).PlayerColor;
            //_panelAnonymImage[i].SetActive(false);
            _playerData[i].DidVote = false;

            if (_playerDataContainer.GetPlayerJob(PhotonNetwork.LocalPlayer.GetPlayerNumber()) != PlayerType.Duck)
                return;

            _nickNameText[PhotonNetwork.LocalPlayer.GetPlayerNumber()].color = Color.red;
        }
    }

    // 플레이어 패널 생성 함수
    private void SpawnPlayerPanel()
    {
        photonView.RPC("SpawnPlayerPanelRPC", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.GetPlayerNumber());
    }

    [PunRPC]
    public void SpawnPlayerPanelRPC(int index)
    {
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
           //TODO : 
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
                //SpawnAnonymImage();
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
