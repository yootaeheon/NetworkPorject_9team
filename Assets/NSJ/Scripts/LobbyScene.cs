using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyScene : MonoBehaviourPunCallbacks
{
    public static LobbyScene Instance;
    public static GameObject Loading { get { return Instance._popUp.Loading; } }
    public static GameObject Option { get { return Instance._popUp.Option; } }
  
    public static bool IsLoginCancel { get { return  Instance._isLoginCancel; } set { Instance._isLoginCancel = value; } }
    public static bool IsJoinRoomCancel { get { return Instance._isJoinRoomCancel; } set { Instance._isJoinRoomCancel = value; } }

    #region ¿Ã∫•∆Æ
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
    public event UnityAction<List<RoomInfo>> OnRoomListUpdateEvent;
    #endregion 

    #region private « µÂ
    public enum Panel { Login, Main, Lobby, Room, Size }

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
    private static GameObject s_backGroundImage { get { return Instance._panelStruct.BackGroundImage; } }
    private static GameObject s_loginPanel { get { return Instance._panelStruct.LoginPanel; } }
    private static GameObject s_mainPanel { get { return Instance._panelStruct.MainPanel; } }
    private static GameObject s_lobbyPanel { get { return Instance._panelStruct.LobbyPanel; } }
    private static GameObject s_roomPanel { get { return Instance._panelStruct.RoomPanel; } }

    private GameObject[] _panels = new GameObject[(int)Panel.Size];
    private GameObject _curPanel;
    private static GameObject s_curPanel { get { return Instance._curPanel; } }

    [System.Serializable]
    struct PopUpUI
    {
        public GameObject Loading;
        public GameObject Option;
    }
    [SerializeField] PopUpUI _popUp;

    private bool _isLoginCancel;
    private bool _isJoinRoomCancel;
    #endregion

    private void Awake()
    {
        InitSingleTon(); // ΩÃ±€≈Ê
        Init(); // √ ±‚ º≥¡§

    }
    private void Start()
    {
        SubscribesEvent();
        ChangePanel(Panel.Login);
    }

    #region ∆˜≈Ê ≥◊∆Æøˆ≈© ƒ›πÈ «‘ºˆ
    /// <summary>
    /// ∏∂Ω∫≈Õ º≠πˆ ø¨∞·Ω√ ƒ›πÈ
    /// </summary>
    public override void OnConnectedToMaster()
    {
        ChangePanel(Panel.Main);
        OnConnectedEvent?.Invoke();
    }

    /// <summary>
    /// º≠πˆ ¡¢º” «ÿ¡¶ Ω√ ƒ›πÈ
    /// </summary>
    public override void OnDisconnected(DisconnectCause cause)
    {
        ChangePanel(Panel.Login);
        OnDisconnectedEvent?.Invoke(cause);
    }

    /// <summary>
    /// πÊ ª˝º∫ Ω√ ƒ›πÈ
    /// </summary>
    public override void OnCreatedRoom()
    {
        OnCreateRoomEvent?.Invoke();
    }

    /// <summary>
    /// πÊ ¿‘¿Â Ω√ ƒ›πÈ
    /// </summary>
    public override void OnJoinedRoom()
    {
        ChangePanel(Panel.Room);
        OnJoinedRoomEvent?.Invoke();
    }

    /// <summary>
    /// ∑£¥˝∏≈ƒ™ ¿‘¿Â Ω«∆– Ω√ ƒ›πÈ
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        OnJoinRandomFailedEvent?.Invoke(returnCode, message);
    }

    /// <summary>
    /// πÊ ≈¿Â Ω√ ƒ›πÈ
    /// </summary>
    public override void OnLeftRoom()
    {
        ChangePanel(Panel.Main);
        OnLeftRoomEvent?.Invoke();
    }
    /// <summary>
    /// «√∑π¿ÃæÓ ¿‘¿Â Ω√ ƒ›πÈ
    /// </summary>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        OnPlayerEnteredRoomEvent?.Invoke(newPlayer);
    }
    /// <summary>
    /// «√∑π¿ÃæÓ ≈¿Â Ω√ ƒ›πÈ
    /// </summary>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        OnPlayerLeftRoomEvent?.Invoke(otherPlayer);
    }
    /// <summary>
    /// «√∑π¿ÃæÓ «¡∑Œ∆€∆º ∫Ø∞Ê Ω√ ƒ›πÈ
    /// </summary>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        OnPlayerPropertiesUpdateEvent?.Invoke(targetPlayer, changedProps);
    }
    /// <summary>
    /// ∏∂Ω∫≈Õ ≈¨∂Û¿Ãæ∆Æ ∫Ø∞Ê Ω√ ƒ›πÈ
    /// </summary>
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        OnMasterClientSwitchedEvent?.Invoke(newMasterClient);
    }

    /// <summary>
    /// ∑Œ∫Ò ¿‘¿Â Ω√ ƒ›πÈ
    /// </summary>
    public override void OnJoinedLobby()
    {
        ChangePanel(Panel.Lobby);
        OnJoinedLobbyEvent?.Invoke();
    }
    /// <summary>
    /// ∑Œ∫Ò ≈¿Â Ω√ ƒ›πÈ
    /// </summary>
    public override void OnLeftLobby()
    {
        ChangePanel(Panel.Main);
        OnLeftLobbyEvent?.Invoke();
    }
    /// <summary>
    /// ∑Î ∏ÆΩ∫∆Æ æ˜µ•¿Ã∆Æ
    /// </summary>
    /// <param name="roomList"></param>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        StartCoroutine(OnRoomListUpdateRoutine(roomList));
    }
    IEnumerator OnRoomListUpdateRoutine(List<RoomInfo> roomList)
    {
        yield return null;
        OnRoomListUpdateEvent?.Invoke(roomList);
        // ∑Œ∫Ò∆–≥Œ »£≠ÅΩ√ ¿Ã∫•∆Æ ±∏µ∂Ω√∞£¿ª ¿ß«— 1«¡∑π¿” ¥ ¥¬ ƒ⁄∑Á∆æ
        // ¡®¿Â ∂« ƒ⁄∑Á∆æ¿Ãæﬂ. æ∆æ∆ ƒ⁄∑Á∆æ ≥™¿« Ω≈, ≥™¿« ∫˚.
    }


    #endregion

    /// <summary>
    /// ∑Œµ˘ »≠∏È »∞º∫»≠ / ∫Ò»∞º∫»≠
    /// </summary>
    public static void ActivateLoadingBox(bool isActive)
    {
        Loading.SetActive(isActive);
    }

    /// <summary>
    /// ø…º« √¢ »∞º∫»≠ / ∫Ò»∞º∫»≠
    /// </summary>
    /// <param name="isActive"></param>
    public static void ActivateOptionBox(bool isActive)
    {
        Option.SetActive(isActive);
    }

    /// <summary>
    /// ∆–≥Œ ±≥√º
    /// </summary>
    /// <param name="panel"></param>
    private void ChangePanel(Panel panel)
    {

        for (int i = 0; i < _panels.Length; i++)
        {
            if (i == (int)panel) // ∏≈∞≥∫ØºˆøÕ ¿œƒ°«œ¥¬ ∆–≥Œ¿Ã∏È »∞º∫»≠
            {
                if (_panels[i] == null)
                    return;
                _panels[i].SetActive(true);
                _curPanel = _panels[i];
                if (panel == Panel.Room) // ∆–≥Œ¿Ã ∑Î¿Ã∏È µﬁπË∞Ê ∫Ò»∞º∫»≠
                {
                    s_backGroundImage.SetActive(false);
                }
                else
                {
                    s_backGroundImage.SetActive(true);
                }
            }
            else // æ∆¥œ∏È ∫Ò»∞º∫»≠
            {
                _panels[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// ∑Œµ˘ ƒµΩΩ ºº∆√
    /// </summary>
   public static void SetIsLoadingCancel()
    {
        if(s_curPanel == s_loginPanel)
        {
            IsLoginCancel = true;
        }
        else if(s_curPanel == s_mainPanel)
        {
            IsJoinRoomCancel = true;
        }
    }



    #region √ ±‚ º≥¡§

    /// <summary>
    /// ΩÃ±€≈Ê ¡ˆ¡§
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
    /// √ ±‚ º≥¡§
    /// </summary>
    private void Init()
    {
        _panels[(int)Panel.Login] = s_loginPanel;
        _panels[(int)Panel.Main] = s_mainPanel;
        _panels[(int)Panel.Lobby] = s_lobbyPanel;
        _panels[(int)Panel.Room] = s_roomPanel;

        ActivateLoadingBox(false);
        ActivateOptionBox(false);
    }

    /// <summary>
    /// ¿Ã∫•∆Æ ±∏µ∂
    /// </summary>
    private void SubscribesEvent()
    {

    }
    #endregion
}
