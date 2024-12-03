using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerInfo : MonoBehaviour
{
    [Header("Sabotage Ability")]
    [SerializeField] private UnityEngine.UI.Image _armImage;   //게임 스폰 시 컬러 값 전달
    [SerializeField] private GameObject _abilityPrefab;
    private PlayerType _playerType;
    private Coroutine _setCo;

    private void Start()
    {
        _setCo = StartCoroutine(SetPlayerCoroutine());
    }

    private void Update()
    {
        if (_playerType.Equals(PlayerType.Duck))
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                _abilityPrefab.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Ability 팝업창 정보 셋팅 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetPlayerCoroutine()
    {
        yield return Util.GetDelay(3f);
        SetPlayer();
    }

    /// <summary>
    /// 오리 플레이어 Ability 팝업창 정보 셋팅
    /// </summary>
    private void SetPlayer()
    {
        _playerType = PlayerDataContainer.Instance.GetPlayerJob(PhotonNetwork.LocalPlayer.GetPlayerNumber());
        PlayerData data = PlayerDataContainer.Instance.GetPlayerData(PhotonNetwork.LocalPlayer.GetPlayerNumber());
        _armImage.color = data.PlayerColor;
    }

}
