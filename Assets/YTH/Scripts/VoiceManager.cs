using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;

public class VoiceManager : MonoBehaviourPunCallbacks
{
    public static VoiceManager Instance { get; private set; }

    [SerializeField] PlayerController _controller;

    public PunVoiceClient _voiceClient;

    public Photon.Voice.Unity.Recorder _recorder;

    public VoiceConnection _voiceConnection;

    private const byte LIVING_GROUP = 1;
    private const byte DEAD_GROUP = 2;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        PhotonNetwork.ConnectUsingSettings();
    }

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        Debug.Log(PhotonNetwork.NetworkClientState);

        //// 서버 연결되어 룸에 있으면 보이스 활성화
        //if (PhotonNetwork.InRoom)
        //{
        //_recorder.TransmitEnabled = true;
        //}
        //else
        //{
        //_recorder.TransmitEnabled = false;
        //}
    }

    public override void OnJoinedRoom()
    {
        // 펀 보이스
        if (_voiceClient == null)
        {
            _voiceClient = PunVoiceClient.Instance;
        }

        if (_recorder == null)
        {
            Debug.LogError("Recorder is not assigned!");
            return;
        }
    }

    // 플레이어 컨트롤에서 호출할 것
    public void SetVoiceChannel(bool isGhost)
    {
        // 살아있음 => 그룹 1 사용
        if (!_controller.isGhost)
        {
            _recorder.InterestGroup = LIVING_GROUP;
            Debug.Log("살아있음 : 1번 채널 이용중");
        }
        // 죽은 플레이어 => 그룹 2번 사용
        else
        {
            _recorder.InterestGroup = DEAD_GROUP;
            Debug.Log("사망하여 채널 전환 : 2번 채널");
        }
    }
}
