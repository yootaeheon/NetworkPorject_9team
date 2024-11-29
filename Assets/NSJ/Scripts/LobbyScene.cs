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
    public enum Panel { Login, Main, Lobby, Room, Loading, Option, Size }

    [System.Serializable]
    struct PanelStruct
    {
        public GameObject BackGroundImage;
        public GameObject LoginPanel;
        public GameObject MainPanel;
        public GameObject LobbyPanel;
        public GameObject RoomPanel;
        public GameObject LoadingPanel;
        public GameObject OptionPanel;
    }
    [SerializeField] private PanelStruct _panelStruct;
    private static GameObject s_backGroundImage { get { return Instance._panelStruct.BackGroundImage; } }
    private static GameObject s_loginPanel { get { return Instance._panelStruct.LoginPanel; } }
    private static GameObject s_mainPanel { get { return Instance._panelStruct.MainPanel; } }
    private static GameObject s_lobbyPanel { get { return Instance._panelStruct.LobbyPanel; } }
    private static GameObject s_roomPanel { get { return Instance._panelStruct.RoomPanel; } }
    private static GameObject s_loadingPanel { get { return Instance._panelStruct.LoadingPanel;} }
    private static GameObject s_optionPanel { get { return Instance._panelStruct.OptionPanel;} }

    private List<GameObject> _panels = new List<GameObject>((int)Panel.Size);
    private GameObject _curPanel;
    private static GameObject s_curPanel { get { return Instance._curPanel; } }

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

        if (OptionPanel.Instance != null)
        {
           
            _panelStruct.OptionPanel = OptionPanel.Instance.gameObject;
            _panels.Add(OptionPanel.Instance.gameObject);
        }

        InitPanel();
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
    /// πÊ ¿‘¿Â Ω«∆– Ω√ ƒ›πÈ
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        ActivateLoadingBox(false);
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
    }


    #endregion

    /// <summary>
    /// ∑Œµ˘ »≠∏È »∞º∫»≠ / ∫Ò»∞º∫»≠
    /// </summary>
    public static void ActivateLoadingBox(bool isActive)
    { 
        s_loadingPanel.SetActive(isActive);
    }

    /// <summary>
    /// ø…º« √¢ »∞º∫»≠ / ∫Ò»∞º∫»≠
    /// </summary>
    /// <param name="isActive"></param>
    public static void ActivateOptionBox(bool isActive)
    {
        s_optionPanel.SetActive(isActive);
    }

    /// <summary>
    /// ∆–≥Œ ±≥√º
    /// </summary>
    /// <param name="panel"></param>
    private void ChangePanel(Panel panel)
    {
        for (int i = 0; i < _panels.Count; i++)
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
    /// √ ±‚ ∆–≥Œ º≥¡§
    /// </summary>
    private void InitPanel()
    {
        if (PhotonNetwork.InRoom) // πÊø° ¬¸∞°«—ªÛ≈¬ø¥¿ª∂ß
        {
            ChangePanel(Panel.Room);
        }
        else if (PhotonNetwork.IsConnected) // æ∆¥œ∏È º≠πˆø° ø¨∞·µ«¿÷¥¯ ªÛ≈¬ø¥¿ª ∂ß
        {
            ChangePanel(Panel.Main);
        }
        else
        {
            ChangePanel(Panel.Login);
        }
    }

    /// <summary>
    /// √ ±‚ º≥¡§
    /// </summary>
    private void Init()
    {
        _panels.Add(s_loginPanel);
        _panels.Add(s_mainPanel);
        _panels.Add(s_lobbyPanel);
        _panels.Add(s_roomPanel);
        _panels.Add(s_loadingPanel);
        //_panels[(int)Panel.Option] = s_optionPanel;

        //ActivateLoadingBox(false);
        //ActivateOptionBox(false);

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    /// <summary>
    /// ¿Ã∫•∆Æ ±∏µ∂
    /// </summary>
    private void SubscribesEvent()
    {

    }
    #endregion
}
