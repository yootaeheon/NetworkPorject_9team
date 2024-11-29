using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroySingleTon : MonoBehaviour
{
    [SerializeField] GameLoadingScene gameLoadingScenePrefab;
    [SerializeField] PlayerDataContainer playerDataContainerPrefab;

    GameObject gameLoadingScene;
    GameObject playerDataContainer;


    private void Start()
    {
        LobbyScene.Instance.OnJoinedRoomEvent += CreateSingleTon;
        LobbyScene.Instance.OnLeftRoomEvent += DeleteSingleTon;
    }




    private void CreateSingleTon()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            gameLoadingScene = PhotonNetwork.InstantiateRoomObject("GameLoadingScene", Vector3.zero, Quaternion.identity);
            playerDataContainer = PhotonNetwork.InstantiateRoomObject("PlayerDataContainer", Vector3.zero, Quaternion.identity);
        }
    }

    private void DeleteSingleTon()
    {
        if(gameLoadingScene != null)
        {
            Destroy(gameLoadingScene);
        }
        if(playerDataContainer != null)
        {
            Destroy(playerDataContainer);
        }
    }
}
