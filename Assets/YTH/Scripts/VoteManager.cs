using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VoteManager : MonoBehaviourPunCallbacks
{
    [SerializeField] VoteSceneData _voteData;

    [SerializeField] VoteScenePlayerData _playerData;

    public const string RoomName = "playerpaneltest";

    private List<GameObject> playerPanels = new List<GameObject>();

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
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
        SpawnPanelWithSetParent();
    }

    public void SpawnPanelWithSetParent()
    {
        GameObject myPanel = PhotonNetwork.Instantiate("PlayerPanel", Vector2.zero, Quaternion.identity);

        myPanel.transform.SetParent(_voteData.PlayerPanelParent, false);
        
        
        //photonView.RPC("SetParentRPC", RpcTarget.All, myPanel, _voteData.PlayerPanelParent);
    }

    [PunRPC]
    public void SetParentRPC(GameObject panel, Transform parentTransform)
    {
        panel.transform.SetParent(parentTransform, false);
    }


}
