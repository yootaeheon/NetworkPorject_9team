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
        if(PhotonNetwork.InRoom == true)
        {
            CreateSingleTon();
        }

        LobbyScene.Instance.OnJoinedRoomEvent += CreateSingleTon;
        LobbyScene.Instance.OnLeftRoomEvent += DeleteSingleTon;
    }




    private void CreateSingleTon()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if(GameLoadingScene.Instance == null)
            {
                PhotonNetwork.InstantiateRoomObject("GameLoadingScene", Vector3.zero, Quaternion.identity);
            }
            if (PlayerDataContainer.Instance == null)
            {
                PhotonNetwork.InstantiateRoomObject("PlayerDataContainer", Vector3.zero, Quaternion.identity);
            }                  
        }
    }

    private void DeleteSingleTon()
    {
        if(GameLoadingScene.Instance != null)
        {
            Destroy(GameLoadingScene.Instance.gameObject);
        }
        if(PlayerDataContainer.Instance != null)
        {
            Destroy(PlayerDataContainer.Instance.gameObject);
        }
    }
}
