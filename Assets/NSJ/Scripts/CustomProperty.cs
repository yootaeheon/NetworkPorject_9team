using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

/// <summary>
/// 플레이어 커스텀 프로퍼티
/// </summary>
public static class CustomPlayerProperty 
{
    private const string READY = "Ready";
    private static PhotonHashtable _customProperty = new PhotonHashtable();

    /// <summary>
    /// 레디 프로퍼티 세팅
    /// </summary>
    /// <param name="player"></param>
    /// <param name="isReady"></param>
    public static void SetReady(this Player player, bool isReady)
    {
        _customProperty.Clear();
        _customProperty.Add(READY, isReady);
        player.SetCustomProperties(_customProperty);
    }
    /// <summary>
    /// 레디 프로퍼티 가져오기
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public static bool GetReady(this Player player)
    {
        PhotonHashtable customProperty = player.CustomProperties;
        if (customProperty.ContainsKey(READY)) 
        {
            return (bool)customProperty[READY];
        }
        return false;
    }
}

/// <summary>
/// 방 커스텀 프로퍼티
/// </summary>
public static class CustomRoomProperty
{
    private const string PRIVACY = "privacy";

    private static PhotonHashtable _customProperty = new PhotonHashtable();

    /// <summary>
    /// 방 공개 여부 프로퍼티 세팅
    /// </summary>
    /// <param name="room"></param>
    /// <param name="isPrivacy"></param>
    public static void SetPrivacy(this Room room, bool isPrivacy)
    {
        _customProperty.Clear();
        _customProperty.Add(PRIVACY, isPrivacy);
        room.SetCustomProperties(_customProperty);
    }

    /// <summary>
    /// 방 공개 여부 프로퍼티 가져오기
    /// </summary>
    /// <param name="room"></param>
    /// <returns></returns>
    public static bool GetPrivacy(this Room room) 
    {
        PhotonHashtable customProperty = room.CustomProperties;
        if (customProperty.ContainsKey(PRIVACY)) 
        {
            return (bool)customProperty[PRIVACY];
        }
        return false;
    }

    /// <summary>
    /// 방 공개 여부 프로퍼티 세팅
    /// </summary>
    /// <param name="room"></param>
    /// <param name="isPrivacy"></param>
    public static void SetPrivacy(this RoomOptions roomOptions, bool isPrivacy)
    {
        _customProperty.Clear();
        _customProperty.Add(PRIVACY, isPrivacy);
        roomOptions.CustomRoomProperties = _customProperty;
    }
}
