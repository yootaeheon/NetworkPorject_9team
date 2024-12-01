using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class ServerCallback : MonoBehaviourPunCallbacks
{
    public static ServerCallback Instance;

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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ¸¶½ºÅÍ ¼­¹ö ¿¬°á½Ã Äİ¹é
    /// </summary>
    public override void OnConnectedToMaster()
    {
        OnConnectedEvent?.Invoke();
    }

    /// <summary>
    /// ¼­¹ö Á¢¼Ó ÇØÁ¦ ½Ã Äİ¹é
    /// </summary>
    public override void OnDisconnected(DisconnectCause cause)
    {
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
        OnJoinedRoomEvent?.Invoke();
    }
    /// <summary>
    /// ¹æ ÀÔÀå ½ÇÆĞ ½Ã Äİ¹é
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRoomFailed(short returnCode, string message)
    {

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
        OnJoinedLobbyEvent?.Invoke();
    }
    /// <summary>
    /// ·Îºñ ÅğÀå ½Ã Äİ¹é
    /// </summary>
    public override void OnLeftLobby()
    {
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
    }
}
