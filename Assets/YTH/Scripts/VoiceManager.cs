using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Voice.Unity;
using UnityEngine;

public class VoiceManager : MonoBehaviourPunCallbacks
{
    public static VoiceManager Instance { get; private set; }

    PlayerDataContainer PlayerDataContainer => PlayerDataContainer.Instance;

    [SerializeField] Recorder _recorder;

    private bool _initTarget = false;

    [SerializeField] PlayerController _playerController;

    private int[] _aliveTargetPlayers;

    private int[] _deadTargetPlayers;


    private void Start()
    {
        _aliveTargetPlayers = new int[12];
        _deadTargetPlayers = new int[12];

        _recorder = FindAnyObjectByType<Recorder>();

        SetAliveTargetPlayers();
        SetDeadTargetPlayers();
    }

    public void DisableVoice()
    {
        //보이스 off 기능
        // if (PlayerDataContainer.GetPlayerData(PhotonNetwork.LocalPlayer.GetPlayerNumber()).IsGhost)
        // {
        //     _recorder.TransmitEnabled = false;
        // }
    }

    private void SetAliveTargetPlayers()
    {

        for (int i = 0; i < _aliveTargetPlayers.Length; i++)
        {
            if (i != PhotonNetwork.LocalPlayer.GetPlayerNumber())
            {
                _aliveTargetPlayers[i] = i + 1;
            }
        }
        _recorder.TargetPlayers = _aliveTargetPlayers;
    }

    private void SetDeadTargetPlayers()
    {
        for (int i = 0; i < _deadTargetPlayers.Length; i++)
        {
            _deadTargetPlayers[i] = 0;
        }
    }

    public void MeDead()
    {
        photonView.RPC(nameof(MeDeadRpc), RpcTarget.All, PhotonNetwork.LocalPlayer.GetPlayerNumber());
    }

    [PunRPC]
    public void MeDeadRpc(int index)
    {
        _aliveTargetPlayers[index] = 0;

        if (index != PhotonNetwork.LocalPlayer.GetPlayerNumber())
        {
            _deadTargetPlayers[index] = index + 1;
        }

        if (_playerController.isGhost == true)
        {
            _recorder.TargetPlayers = _deadTargetPlayers;
        }
    }
}



