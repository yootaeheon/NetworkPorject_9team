using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class LobbyScene : MonoBehaviourPunCallbacks
{
    public static LobbyScene Instance;

    #region ÀÌº¥Æ®
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

    #region private ÇÊµå
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
        InitSingleTon(); // ½Ì±ÛÅæ
        Init(); // ÃÊ±â ¼³Á¤
        
    }
    private void Start()
    {
        SubscribesEvent();
        ChangePanel(Panel.Login);
    }

    #region Æ÷Åæ ³×Æ®¿öÅ© Äİ¹é ÇÔ¼ö
    /// <summary>
    /// ¸¶½ºÅÍ ¼­¹ö ¿¬°á½Ã Äİ¹é
    /// </summary>
    public override void OnConnectedToMaster()
    {
        ChangePanel(Panel.Main);
        OnConnectedEvent?.Invoke();
    }

    /// <summary>
    /// ¼­¹ö Á¢¼Ó ÇØÁ¦ ½Ã Äİ¹é
    /// </summary>
    public override void OnDisconnected(DisconnectCause cause)
    {
        ChangePanel(Panel.Login);
        OnDisconnectedEvent?.Invoke(cause);
    }

    /// <summary>
    /// ¹æ »ı¼º ½Ã Äİ¹é
    /// </summary>
    public override void OnCreatedRoom()
    {
        OnCreateRoomEvent?.Invoke();
    }

    /// <summary>
    /// ¹æ ÀÔÀå ½Ã Äİ¹é
    /// </summary>
    public override void OnJoinedRoom()
    {
        ChangePanel(Panel.Room);
        OnJoinedRoomEvent?.Invoke();
    }

    /// <summary>
    /// ·£´ı¸ÅÄª ÀÔÀå ½ÇÆĞ ½Ã Äİ¹é
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        OnJoinRandomFailedEvent?.Invoke(returnCode, message);
    }

    /// <summary>
    /// ¹æ ÅğÀå ½Ã Äİ¹é
    /// </summary>
    public override void OnLeftRoom()
    {
        ChangePanel(Panel.Main);
        OnLeftRoomEvent?.Invoke();
    }
    /// <summary>
    /// ÇÃ·¹ÀÌ¾î ÀÔÀå ½Ã Äİ¹é
    /// </summary>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        OnPlayerEnteredRoomEvent?.Invoke(newPlayer);
    }
    /// <summary>
    /// ÇÃ·¹ÀÌ¾î ÅğÀå ½Ã Äİ¹é
    /// </summary>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        OnPlayerLeftRoomEvent?.Invoke(otherPlayer);
    }
    /// <summary>
    /// ÇÃ·¹ÀÌ¾î ÇÁ·ÎÆÛÆ¼ º¯°æ ½Ã Äİ¹é
    /// </summary>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        OnPlayerPropertiesUpdateEvent?.Invoke(targetPlayer, changedProps);
    }
    /// <summary>
    /// ¸¶½ºÅÍ Å¬¶óÀÌ¾ğÆ® º¯°æ ½Ã Äİ¹é
    /// </summary>
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        OnMasterClientSwitchedEvent?.Invoke(newMasterClient);
    }

    /// <summary>
    /// ·Îºñ ÀÔÀå ½Ã Äİ¹é
    /// </summary>
    public override void OnJoinedLobby()
    {
        ChangePanel(Panel.Lobby);
        OnJoinedLobbyEvent?.Invoke();
    }
    /// <summary>
    /// ·Îºñ ÅğÀå ½Ã Äİ¹é
    /// </summary>
    public override void OnLeftLobby()
    {
        ChangePanel(Panel.Main);
        OnLeftLobbyEvent?.Invoke();
    }
    /// <summary>
    /// ·ë ¸®½ºÆ® ¾÷µ¥ÀÌÆ®
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
        // ·ÎºñÆĞ³Î È£­½Ã ÀÌº¥Æ® ±¸µ¶½Ã°£À» À§ÇÑ 1ÇÁ·¹ÀÓ ´Ê´Â ÄÚ·çÆ¾
        // Á¨Àå ¶Ç ÄÚ·çÆ¾ÀÌ¾ß. ¾Æ¾Æ ÄÚ·çÆ¾ ³ªÀÇ ½Å, ³ªÀÇ ºû.
    }

    #endregion

    /// <summary>
    /// ÆĞ³Î ±³Ã¼
    /// </summary>
    /// <param name="panel"></param>
    private void ChangePanel(Panel panel)
    {

        for (int i = 0; i < _panels.Length; i++)
        {
            if (i == (int)panel) // ¸Å°³º¯¼ö¿Í ÀÏÄ¡ÇÏ´Â ÆĞ³ÎÀÌ¸é È°¼ºÈ­
            {
                if (_panels[i] == null)
                    return;         
                _panels[i].SetActive(true);

                if (panel == Panel.Room) // ÆĞ³ÎÀÌ ·ëÀÌ¸é µŞ¹è°æ ºñÈ°¼ºÈ­
                {
                    _backGroundImage.SetActive(false);
                }
                else
                {
                    _backGroundImage.SetActive(true);
                }
            }
            else // ¾Æ´Ï¸é ºñÈ°¼ºÈ­
            {
                _panels[i].SetActive(false);
            }
        }
    }


    #region ÃÊ±â ¼³Á¤

    /// <summary>
    /// ½Ì±ÛÅæ ÁöÁ¤
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
    /// ÃÊ±â ¼³Á¤
    /// </summary>
    private void Init()
    {
        _panels[(int)Panel.Login] = _loginPanel;
        _panels[(int)Panel.Main] = _mainPanel;
        _panels[(int)Panel.Lobby] = _lobbyPanel;
        _panels[(int)Panel.Room] = _roomPanel;
    }

    /// <summary>
    /// ÀÌº¥Æ® ±¸µ¶
    /// </summary>
    private void SubscribesEvent()
    {

    }
    #endregion
}
