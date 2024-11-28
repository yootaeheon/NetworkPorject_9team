using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PhotonHashTable = ExitGames.Client.Photon.Hashtable;

public class PlayerSpawner : MonoBehaviourPun
{
    [SerializeField] PlayerReadyUI _readyUI;
    private PlayerController _myPlayer;

    private void Start()
    {
        SubscribesEvents();
    }

    private void SubscribesEvents()
    {
        LobbyScene.Instance.OnJoinedRoomEvent += SpawnPlayer;
        LobbyScene.Instance.OnPlayerEnteredRoomEvent += SetPlayerToNewPlayer;
        LobbyScene.Instance.OnPlayerPropertiesUpdateEvent += SetPlayerPropertiesUpdate;
        LobbyScene.Instance.OnMasterClientSwitchedEvent += SetMasterClientSwitched;
    }

    /// <summary>
    /// 플레이어 스폰
    /// </summary>
    private void SpawnPlayer()
    {
        // 랜덤위치 결정
        Vector2 randomPos = new Vector2(Random.Range(-8, 8), Random.Range(1, 4));

        // 해당 랜덤 위치에 스폰
        GameObject playerInstance = PhotonNetwork.Instantiate("LJH_Player",randomPos, Quaternion.identity);
        // 본인 플레이어 캐싱
        _myPlayer = playerInstance.GetComponent<PlayerController>();
        SetPlayerToOrigin();
    }

    /// <summary>
    /// 원래 있던 플레이어들에게 본인 플레이어 동기화
    /// </summary>
    private void SetPlayerToOrigin()
    {
        int id = _myPlayer.photonView.ViewID;
        photonView.RPC(nameof(RPCSetPlayerToOrigin), RpcTarget.All, id, PhotonNetwork.LocalPlayer);
    }

    /// <summary>
    /// 새로 들어온 플레이어에게 본인 플레이어 동기화
    /// </summary>
    private void SetPlayerToNewPlayer(Player newPlayer)
    {
        int id = _myPlayer.photonView.ViewID;
        photonView.RPC(nameof(RPCSetPlayerToNewPlayer), RpcTarget.All, id, PhotonNetwork.LocalPlayer, newPlayer);
    }


    /// <summary>
    /// 원래 있던 플레이어들에게 본인 플레이어 동기화 RPC
    /// </summary>
    [PunRPC]
    private void RPCSetPlayerToOrigin(int id, Player player)
    {
        PhotonView playerView = PhotonView.Find(id);
        SetPlayer(playerView, player);
    }

    /// <summary>
    ///  새로 들어온 플레이어에게 본인 플레이어 동기화 RPC
    /// </summary>
    [PunRPC]
    private void RPCSetPlayerToNewPlayer(int id, Player player, Player newPlayer)
    {
        // 새로운 플레이어가 본인이 아니면 함수를 돌리지 않음
        if (newPlayer != PhotonNetwork.LocalPlayer)
            return;
        PhotonView playerView = PhotonView.Find(id);
        SetPlayer(playerView, player);
    }

    /// <summary>
    /// 플레이어 설정
    /// </summary>
    private void SetPlayer(PhotonView photonView, Player player)
    {
        TMP_Text nickNameText = photonView.GetComponentInChildren<TMP_Text>();
        nickNameText.SetText(player.NickName);

        // 레디 UI 설정
        PlayerReadyUI readyUI = Instantiate(_readyUI, photonView.transform);
        if (player.IsMasterClient == true)
        {
            readyUI.ChangeImage(PlayerReadyUI.Image.Master);
        }
        else
        {
            bool ready = player.GetReady();
            if (ready == true)
            {
                readyUI.ChangeImage(PlayerReadyUI.Image.Ready);
            }
            else
            {
                readyUI.ChangeImage(PlayerReadyUI.Image.UnReady);
            }
        }
    }

    /// <summary>
    ///  플레이어 프로퍼티 변경
    /// </summary>
    private void SetPlayerPropertiesUpdate(Player player, PhotonHashTable arg1)
    {
        if (player != PhotonNetwork.LocalPlayer)
            return;

        int id = _myPlayer.photonView.ViewID;
        photonView.RPC(nameof(RPCSetPlayerProperty), RpcTarget.All, id, player);
    }

    /// <summary>
    /// 마스터 클라이언트 변경
    /// </summary>
    private void SetMasterClientSwitched(Player newMaster)
    {
        if (newMaster != PhotonNetwork.LocalPlayer)
            return;

        int id = _myPlayer.photonView.ViewID;
        photonView.RPC(nameof(RPCSetPlayerProperty), RpcTarget.All, id, newMaster);
    }

    [PunRPC]
    private void RPCSetPlayerProperty(int id, Player player)
    {
        PhotonView playerView = PhotonView.Find(id);

        PlayerReadyUI readyUI = playerView.GetComponentInChildren<PlayerReadyUI>();
        if (player.IsMasterClient == true)
        {
            readyUI.ChangeImage(PlayerReadyUI.Image.Master);
        }
        else
        {
            bool ready = player.GetReady();
            if (ready == true)
            {
                readyUI.ChangeImage(PlayerReadyUI.Image.Ready);
            }
            else
            {
                readyUI.ChangeImage(PlayerReadyUI.Image.UnReady);
            }
        }

    }

}
