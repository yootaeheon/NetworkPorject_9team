using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.UI;

public class VoiceManager : MonoBehaviourPunCallbacks
{
    // public const string RoomName = "playerpaneltest";
    public static VoiceManager Instance { get; private set; }

    [SerializeField] Image[] _speakingSigns;

    [SerializeField] PhotonVoiceView[] _voiceViews;

    [SerializeField] Recorder _recorder;

    PlayerDataContainer _playerDataContainer => PlayerDataContainer.Instance;

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


    // 말할 때 표시 기능
    public void IsSpeakingImageEnable()
    {
        _speakingSigns[PhotonNetwork.LocalPlayer.ActorNumber - 1].enabled = _voiceViews[PhotonNetwork.LocalPlayer.ActorNumber - 1].IsSpeaking;
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

    public void DisableVoice()
    {
        //사망 시 보이스 off 기능
        // if (_playerDataContainer.GetPlayerData(PhotonNetwork.LocalPlayer.GetPlayerNumber()).IsGhost)
        // {
        //     _recorder.TransmitEnabled = false;
        // }
    }
}
