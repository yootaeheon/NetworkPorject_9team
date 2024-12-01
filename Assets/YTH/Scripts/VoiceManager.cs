using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.UI;

public class VoiceManager : MonoBehaviourPunCallbacks
{
   // public const string RoomName = "playerpaneltest";
    public static VoiceManager Instance { get; private set; }

    [SerializeField] PlayerController _controller;

    [SerializeField] PunVoiceClient _voiceClient;

    [SerializeField] Photon.Voice.Unity.Recorder _recorder;

    [SerializeField] Image[] _speakingSigns;
    [SerializeField] PhotonVoiceView[] _voiceViews;

    private Speaker _speaker;

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
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("룸에 입장했습니다. Voice 설정을 확인합니다.");
    }


   public void IsSpeakingImageEnable()
   {
        _speakingSigns[PhotonNetwork.LocalPlayer.ActorNumber-1].enabled = _voiceViews[PhotonNetwork.LocalPlayer.ActorNumber - 1].IsSpeaking;
   }

    public void FindAndLogConnectedSpeakers()
    {
        // Scene에 있는 모든 Speaker를 검색
        Speaker[] speakers = FindObjectsOfType<Speaker>();

        int index = 0;
        foreach (var speaker in speakers)
        {
            // Speaker에 연결된 PhotonVoiceView 가져오기
            PhotonVoiceView voiceView = speaker.GetComponentInParent<PhotonVoiceView>();
            _voiceViews[index] = voiceView;
            index++;
        }
    }
}
