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
    private UiFollowingPlayer _myNamePanel;

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
        StartCoroutine(SpawnPlayerRoutine());
    }

    IEnumerator SpawnPlayerRoutine()
    {
        yield return null;
        // 랜덤위치 결정
        Vector2 randomPos = new Vector2(Random.Range(-8, 8), Random.Range(1, 4));

        // 해당 랜덤 위치에 스폰
        GameObject playerInstance = PhotonNetwork.Instantiate("LJH_Player", randomPos, Quaternion.identity);
        GameObject namePanel = PhotonNetwork.Instantiate("NamePanel", randomPos, Quaternion.identity);
        // 본인 플레이어 캐싱
        _myPlayer = playerInstance.GetComponent<PlayerController>();
        _myNamePanel = namePanel.GetComponent<UiFollowingPlayer>();
        _myNamePanel.setTarget(playerInstance);
        SetPlayerToOrigin();
    }

    /// <summary>
    /// 원래 있던 플레이어들에게 본인 플레이어 동기화
    /// </summary>
    private void SetPlayerToOrigin()
    {
        int playerId = _myPlayer.photonView.ViewID;
        int namePanelID = _myNamePanel.photonView.ViewID;
        photonView.RPC(nameof(RPCSetPlayerToOrigin), RpcTarget.All, playerId,namePanelID, PhotonNetwork.LocalPlayer);
    }

    /// <summary>
    /// 새로 들어온 플레이어에게 본인 플레이어 동기화
    /// </summary>
    private void SetPlayerToNewPlayer(Player newPlayer)
    {
        int playerID = _myPlayer.photonView.ViewID;
        int namePanelID = _myNamePanel.photonView.ViewID;
        photonView.RPC(nameof(RPCSetPlayerToNewPlayer), RpcTarget.All, playerID, namePanelID, PhotonNetwork.LocalPlayer, newPlayer);
    }


    /// <summary>
    /// 원래 있던 플레이어들에게 본인 플레이어 동기화 RPC
    /// </summary>
    [PunRPC]
    private void RPCSetPlayerToOrigin(int playerId, int namePanelID, Player player)
    {
        PhotonView playerView = PhotonView.Find(playerId);
        PhotonView namePanelView = PhotonView.Find(namePanelID);
        SetPlayer(playerView, namePanelView,player);
    }

    /// <summary>
    ///  새로 들어온 플레이어에게 본인 플레이어 동기화 RPC
    /// </summary>
    [PunRPC]
    private void RPCSetPlayerToNewPlayer(int playerId, int namePanelID, Player player, Player newPlayer)
    {
        // 새로운 플레이어가 본인이 아니면 함수를 돌리지 않음
        if (newPlayer != PhotonNetwork.LocalPlayer)
            return;
        PhotonView playerView = PhotonView.Find(playerId);
        PhotonView namePanelView = PhotonView.Find(namePanelID);
        SetPlayer(playerView, namePanelView, player);
    }

    /// <summary>
    /// 플레이어 설정
    /// </summary>
    private void SetPlayer(PhotonView playerView,PhotonView namePanelView, Player player)
    {
        TMP_Text nickNameText = namePanelView.GetComponentInChildren<TMP_Text>();
        nickNameText.SetText(player.NickName);

        // 레디 UI 설정
        PlayerReadyUI readyUI = Instantiate(_readyUI, namePanelView.transform);
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

        int playerId = _myPlayer.photonView.ViewID;
        int namePanelID = _myNamePanel.photonView.ViewID;
        photonView.RPC(nameof(RPCSetPlayerProperty), RpcTarget.All, playerId, namePanelID, player);
    }

    /// <summary>
    /// 마스터 클라이언트 변경
    /// </summary>
    private void SetMasterClientSwitched(Player newMaster)
    {
        if (newMaster != PhotonNetwork.LocalPlayer)
            return;

        int playerId = _myPlayer.photonView.ViewID;
        int namePanelID = _myNamePanel.photonView.ViewID;
        photonView.RPC(nameof(RPCSetPlayerProperty), RpcTarget.All, playerId, namePanelID, newMaster);
    }

    [PunRPC]
    private void RPCSetPlayerProperty(int playerId, int namePanelID,Player player)
    {
        PhotonView playerView = PhotonView.Find(playerId);
        PhotonView namePanelView = PhotonView.Find(namePanelID);

        PlayerReadyUI readyUI = namePanelView.GetComponentInChildren<PlayerReadyUI>();
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
