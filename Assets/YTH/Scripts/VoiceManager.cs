using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.Profiling;

public class VoiceManager : MonoBehaviourPunCallbacks
{
    public static VoiceManager Instance { get; private set; }

    PlayerDataContainer PlayerDataContainer => PlayerDataContainer.Instance;

    [SerializeField] TargetPlayersController _targetPlayersController;

    [SerializeField] Photon.Voice.Unity.Recorder _recorder;

    [SerializeField] PlayerController _playerController;

    [SerializeField] VoiceConnection _connection;

    [SerializeField] byte[] ALIVE;

    [SerializeField] byte[] DEAD;


    private void Start()
    {
        //  _targetPlayersController = FindAnyObjectByType<TargetPlayersController>();
        _recorder = FindAnyObjectByType<Photon.Voice.Unity.Recorder>();
        _connection = FindAnyObjectByType<VoiceConnection>();
        _connection.Client.OpChangeGroups(null, ALIVE);
     
      // SetAliveTargetPlayers();
      // SetDeadTargetPlayers();
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

        for (int i = 0; i < _targetPlayersController._aliveTargetPlayers.Length; i++)
        {
            if (i != PhotonNetwork.LocalPlayer.GetPlayerNumber())
            {
                _targetPlayersController._aliveTargetPlayers[i] = i + 1;
            }
            else
            {
                _targetPlayersController._aliveTargetPlayers[i] = 0;
            }
        }
        _recorder.TargetPlayers = _targetPlayersController._aliveTargetPlayers;
    }

    private void SetDeadTargetPlayers()
    {
        for (int i = 0; i < _targetPlayersController._deadTargetPlayers.Length; i++)
        {
            _targetPlayersController._deadTargetPlayers[i] = 0;
        }
    }

    public void MeDead()
    {
        // photonView.RPC(nameof(MeDeadRpc), RpcTarget.All, playerNumber);
        // Debug.Log($"겟플레이어넘버 = {PhotonNetwork.LocalPlayer.GetPlayerNumber()}");
        // Debug.Log($"액터넘버 = {PhotonNetwork.LocalPlayer.ActorNumber}");
        _recorder.InterestGroup = 2;
        _connection.Client.OpChangeGroups(ALIVE, DEAD);
    }

    [PunRPC]
    public void MeDeadRpc(int index)
    {
       

        _targetPlayersController._aliveTargetPlayers[index] = 0;
        Debug.Log($"인덱스 = {index}");

        if (index != PhotonNetwork.LocalPlayer.GetPlayerNumber())
        {
            _targetPlayersController._deadTargetPlayers[index] = index + 1;
        }

        if (PlayerDataContainer.GetPlayerData(PhotonNetwork.LocalPlayer.GetPlayerNumber()).IsGhost == true)
        {
            Debug.Log("이즈 고스트 true");
            _recorder.TargetPlayers = _targetPlayersController._deadTargetPlayers;
        }
        else
        {
            Debug.Log("이즈 고스트 falsse");
            _recorder.TargetPlayers = _targetPlayersController._aliveTargetPlayers;
        }
    }
}



