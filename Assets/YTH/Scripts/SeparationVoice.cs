using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Voice.Unity;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SeparationVoice : MonoBehaviourPun
{
    PlayerDataContainer PlayerDataContainer => PlayerDataContainer.Instance;

    [SerializeField] Speaker _speaker;

    private PhotonView _playerView;

    IEnumerator Start()
    {
        yield return null;
        Speaker speaker = GetComponentInChildren<Speaker>();
        _speaker = speaker;

        StartCoroutine(SeparateVoice());
    }

    public IEnumerator SeparateVoice()
    {
        while (true)
        {
          

            if (PlayerDataContainer == null || LobbyScene.Instance != null || _speaker == null)
            {
                yield return null;
            }
            else
            {
                if (photonView.IsMine == false)
                    yield break;

                yield return 0.5f.GetDelay();
                photonView.RPC(nameof(SeparateVoiceRpc), RpcTarget.All, PhotonNetwork.LocalPlayer.GetPlayerNumber());
            }
        }

    }

    // 플레이어 사망 시 스피커 위치 변경하여 보이스 분리
    [PunRPC]
    public void SeparateVoiceRpc(int index)
    {
        if (PlayerDataContainer.GetPlayerData(index).IsGhost)
        {
            _speaker.transform.position = new Vector3(0, 0, 30);
        }
        else
        {
            _speaker.transform.localPosition = Vector3.zero;
        }
    }
}

