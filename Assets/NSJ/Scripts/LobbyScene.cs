using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyScene : MonoBehaviourPunCallbacks
{
    public static LobbyScene Instance;

    #region 이벤트
    public event UnityAction OnConnectedEvent;
    public event UnityAction<DisconnectCause> OnDisconnectedEvent;
    public event UnityAction OnCreateRoomEvent;
    public event UnityAction OnJoinedRoomEvent;
    public event UnityAction<short, string> OnJoinRandomFailedEvent;
    public event UnityAction OnLeftRoomEvent;
    public event UnityAction<Player> OnPlayerEnteredRoomEvent;
    public event UnityAction<Player> OnPlayerLeftRoomEvent;
    public event UnityAction<Player, PhotonHashtable> OnPlayerPropertiesUpdateEvent;
    public event UnityAction OnJoinedLobbyEvent;
    public event UnityAction OnLeftLobbyEvent;
    public event UnityAction<Player> OnMasterClientSwitchedEvent;
    #endregion

    #region private 필드
    enum Panel { Login, Main, Lobby, Room, Size }

    [System.Serializable]
    struct PanelStruct
    {
        public GameObject BackGroundImage;
        public GameObject LoginPanel;
        public GameObject MainPanel;
        public GameObject LobbyPanel;
        public GameObject RoomPanel;
    }
    [SerializeField] private PanelStruct _panelStruct;
    private GameObject _backGroundImage { get { return _panelStruct.BackGroundImage; } }
    private GameObject _loginPanel { get { return _panelStruct.LoginPanel; } }
    private GameObject _mainPanel { get { return _panelStruct.MainPanel; } }
    private GameObject _lobbyPanel { get { return _panelStruct.LobbyPanel; } }
    private GameObject _roomPanel { get { return _panelStruct.RoomPanel; } }

    private GameObject[] _panels = new GameObject[(int)Panel.Size];
    #endregion

    private void Awake()
    {
        InitSingleTon(); // 싱글톤
        Init(); // 초기 설정
        
    }
    private void Start()
    {
        SubscribesEvent();
        ChangePanel(Panel.Login);
    }

    #region 포톤 네트워크 콜백 함수
    /// <summary>
    /// 마스터 서버 연결시 콜백
    /// </summary>
    public override void OnConnectedToMaster()
    {
        ChangePanel(Panel.Main);
        OnConnectedEvent?.Invoke();
    }

    /// <summary>
    /// 서버 접속 해제 시 콜백
    /// </summary>
    public override void OnDisconnected(DisconnectCause cause)
    {
        ChangePanel(Panel.Login);
        OnDisconnectedEvent?.Invoke(cause);
    }

    /// <summary>
    /// 방 생성 시 콜백
    /// </summary>
    public override void OnCreatedRoom()
    {
        OnCreateRoomEvent?.Invoke();
    }

    /// <summary>
    /// 방 입장 시 콜백
    /// </summary>
    public override void OnJoinedRoom()
    {
        ChangePanel(Panel.Room);
        OnJoinedRoomEvent?.Invoke();
    }

    /// <summary>
    /// 랜덤매칭 입장 실패 시 콜백
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("1");
        OnJoinRandomFailedEvent?.Invoke(returnCode, message);
    }

    /// <summary>
    /// 방 퇴장 시 콜백
    /// </summary>
    public override void OnLeftRoom()
    {
        ChangePanel(Panel.Main);
        OnLeftRoomEvent?.Invoke();
    }
    /// <summary>
    /// 플레이어 입장 시 콜백
    /// </summary>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        OnPlayerEnteredRoomEvent?.Invoke(newPlayer);
    }
    /// <summary>
    /// 플레이어 퇴장 시 콜백
    /// </summary>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        OnPlayerLeftRoomEvent?.Invoke(otherPlayer);
    }
    /// <summary>
    /// 플레이어 프로퍼티 변경 시 콜백
    /// </summary>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        OnPlayerPropertiesUpdateEvent?.Invoke(targetPlayer, changedProps);
    }
    /// <summary>
    /// 마스터 클라이언트 변경 시 콜백
    /// </summary>
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        OnMasterClientSwitchedEvent?.Invoke(newMasterClient);
    }

    /// <summary>
    /// 로비 입장 시 콜백
    /// </summary>
    public override void OnJoinedLobby()
    {
        ChangePanel(Panel.Lobby);
        OnJoinedLobbyEvent?.Invoke();
    }
    /// <summary>
    /// 로비 퇴장 시 콜백
    /// </summary>
    public override void OnLeftLobby()
    {
        ChangePanel(Panel.Main);
        OnLeftLobbyEvent?.Invoke();
    }
    #endregion

    /// <summary>
    /// 패널 교체
    /// </summary>
    /// <param name="panel"></param>
    private void ChangePanel(Panel panel)
    {

        for (int i = 0; i < _panels.Length; i++)
        {
            if (i == (int)panel) // 매개변수와 일치하는 패널이면 활성화
            {
                if (_panels[i] == null)
                    return;         
                _panels[i].SetActive(true);

                if (panel == Panel.Room) // 패널이 룸이면 뒷배경 비활성화
                {
                    _backGroundImage.SetActive(false);
                }
                else
                {
                    _backGroundImage.SetActive(true);
                }
            }
            else // 아니면 비활성화
            {
                _panels[i].SetActive(false);
            }
        }
    }


    #region 초기 설정

    /// <summary>
    /// 싱글톤 지정
    /// </summary>
    private void InitSingleTon()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 초기 설정
    /// </summary>
    private void Init()
    {
        _panels[(int)Panel.Login] = _loginPanel;
        _panels[(int)Panel.Main] = _mainPanel;
        _panels[(int)Panel.Lobby] = _lobbyPanel;
        _panels[(int)Panel.Room] = _roomPanel;
    }

    /// <summary>
    /// 이벤트 구독
    /// </summary>
    private void SubscribesEvent()
    {

    }
    #endregion
}
