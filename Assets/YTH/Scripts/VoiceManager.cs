using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;

public class VoiceManager : MonoBehaviourPunCallbacks
{
    public const string RoomName = "playerpaneltest";
    public static VoiceManager Instance { get; private set; }

    [SerializeField] PlayerController _controller;

    [SerializeField] PunVoiceClient _voiceClient;

    [SerializeField] Photon.Voice.Unity.Recorder _recorder;

    [SerializeField] GameObject[] _speakingSigns;
   // [SerializeField] PhotonVoiceView[] _voiceViews;

    private Speaker _speaker;

    private const byte LIVING_GROUP = 1;
    private const byte DEAD_GROUP = 2;

    public override void OnEnable()
    {
        // _speaker = GetComponent<Speaker>();
        //// _voiceView = GetComponent<PhotonVoiceView>();
        //
        // if (_voiceView != null && PunVoiceClient.Instance != null)
        // {
        //     // Recorder와 Speaker 초기화 확인
        //     if (!_voiceView.IsRecording )
        //     {
        //         Debug.LogWarning("Recorder 제대로 설정 X.");
        //     }
        //     if (!_voiceView.IsSpeaking)
        //     {
        //         Debug.LogWarning("Speaker가 제대로 설정 X.");
        //     }
        // }
    }
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

        //TODO : 플레이어컨트롤러 겟컴포넌트로 참조 시킬 것
    }

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void Update()
    {
        Debug.Log(PhotonNetwork.NetworkClientState);

    }

    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.IsConnectedAndReady && !PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnJoinedLobby()
    {
        Util.GetDelay(3f);
        Debug.Log("방 입장 중  ` ` ` ");
        PhotonNetwork.JoinRoom(RoomName);
    }


    public override void OnJoinedRoom()
    {
        Debug.Log("룸에 입장했습니다. Voice 설정을 확인합니다.");
    }


    // 플레이어 컨트롤에서 호출할 것
    public void SetVoiceChannel(bool isGhost)
    {
        // IsGhohst 변경 시 이벤트로 하는게 좋을듯
        // 살아있음 => 그룹 1 사용
        if (!_controller.isGhost)
        {
            _recorder.InterestGroup = LIVING_GROUP;
            _recorder.TransmitEnabled = false;
            Debug.Log("살아있음 : 1번 채널 이용중");
        }
        // 죽은 플레이어 => 그룹 2번 사용
        else
        {
            _recorder.InterestGroup = DEAD_GROUP;
            Debug.Log("사망하여 채널 전환 : 2번 채널");
        }
    }

    //public void IsSpeakingImageEnable(int index)
    //{
    //    if (_recorder.IsCurrentlyTransmitting) // 포톤보이스뷰.isSpeaking 으로 수정해야함 각가 플레이어가 포톤보이스뷰 들고있어야할듯
    //    {
    //        _image[].SetActive(true);
    //    }
    //    else
    //    {
    //        _image.SetActive(false);
    //    }
    //}
}
