using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Voice.Unity;
using System.Collections;
using UnityEngine;

public class SeparationVoice : MonoBehaviourPun
{
    PlayerDataContainer PlayerDataContainer => PlayerDataContainer.Instance;

    private Speaker _speaker;

    private PhotonView _playerView;

    IEnumerator Start()
    {
        yield return null;
        Speaker speaker = GetComponentInChildren<Speaker>();
        _speaker = speaker;

        StartCoroutine(SeparateVoice());
    }

    private void Awake()
    {
       // PhotonView playerView = GetComponentInParent<PhotonView>();
       // _playerView = playerView;
    }

    private void Update()
    {
        // 게임중
        //  if (VoteScene.Instance == null)
        //  {
        //      _speaker.gameObject.SetActive(true);
        //  }
        //  // 투표중
        //  else
        //  {
        //      _speaker.gameObject.SetActive(false);
        //  }
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
                yield return 0.5f.GetDelay();
                photonView.RPC(nameof(SeparateVoiceRpc), RpcTarget.AllBuffered);
            }
        }

    }

    // 플레이어 사망 시 스피커 위치 변경하여 보이스 분리
    [PunRPC]
    public void SeparateVoiceRpc()
    {
        if (PlayerDataContainer.GetPlayerData(PhotonNetwork.LocalPlayer.GetPlayerNumber()).IsGhost)
        {
            _speaker.transform.position = new Vector3(0, 0, 30);
        }
        else
        {
            _speaker.transform.localPosition = Vector3.zero;
        }
    }
}

