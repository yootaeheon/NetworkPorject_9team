using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Voice.Unity;
using System.Collections;
using UnityEngine;

public class SeparationVoice : MonoBehaviour
{
    PlayerDataContainer _playerDataContainer => PlayerDataContainer.Instance;

    private Speaker _speaker;

    IEnumerator Start()
    {
        yield return null;
        Speaker speaker = GetComponentInChildren<Speaker>();
        _speaker = speaker;
    }

    private void Update()
    {
        if (_playerDataContainer == null)
            return;

        if (LobbyScene.Instance != null)
            return ;

        // 플레이어 사망 시 스피커 위치 변경하여 보이스 분리
        if (_playerDataContainer.GetPlayerData(PhotonNetwork.LocalPlayer.GetPlayerNumber()).IsGhost)
        {
            _speaker.transform.position = new Vector3(0, 0, _speaker.transform.position.z - 500);
        }
        else
        {
            _speaker.transform.localPosition = Vector3.zero;
        }

        // 게임중
        if (VoteScene.Instance == null)
        {
            _speaker.gameObject.SetActive(true);
        }
        // 투표중
        else
        {
            _speaker.gameObject.SetActive(false);
        }
    }
}

