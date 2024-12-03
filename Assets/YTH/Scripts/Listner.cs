using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Listner : MonoBehaviourPun
{
    private GameLoadingScene _scene;

   [SerializeField] Vector3 offset;

    private void Update()
    {
        if (PlayerDataContainer.Instance.GetPlayerData(PhotonNetwork.LocalPlayer.GetPlayerNumber()).IsGhost)
        {
            gameObject.transform.position = GameLoadingScene.MyPlayer.transform.position + offset;
        }
        else
        {
            gameObject.transform.position = GameLoadingScene.MyPlayer.transform.position;
        }
    }
}
