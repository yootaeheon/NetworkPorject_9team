using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour 
{
    public static MainPanel Instance;

    [SerializeField] private Button _settingButton;

    public enum Box { Main, Quick, Join, Create, Size }

    [System.Serializable]
    struct BoxStruct
    {
        public GameObject Main;
        public GameObject Quick; 
        public GameObject Join;
        public GameObject Create;
    }
    [SerializeField] BoxStruct _boxStruct;

    private GameObject[] _boxs = new GameObject[(int)Box.Size];
    private static GameObject[] s_boxs { get { return Instance._boxs; } set { Instance._boxs = value; } }

    private StringBuilder _sb = new StringBuilder();


    private void Awake()
    {
        InitSingleTon();
        Init();
    }

    private void Start()
    {
        SubscribesEvent();
    }

    private void OnEnable()
    { 
        if (LobbyScene.Instance != null &&LobbyScene.IsJoinRoomCancel == true) // 로딩 캔슬 초기화 및 UI 변경 금지
        {
            LobbyScene.IsJoinRoomCancel = false;
            return;
        }

        ChangeBox(Box.Main);
    }

    private void OnDisable()
    {
        if (LobbyScene.IsJoinRoomCancel == true) // 로딩 캔슬 시
        {
            CancelJoinRoom();
        }
    }

    /// <summary>
    /// 방 입장 캔슬
    /// </summary>
    private void CancelJoinRoom()
    {
        LoadingBox.StartLoading();
        PhotonNetwork.LeaveRoom();
    }


    /// <summary>
    /// UI 박스 변경
    /// </summary>
    public static void ChangeBox(Box box)
    {
        LoadingBox.StopLoading();

        for (int i = 0; i < s_boxs.Length; i++)
        {
            if (s_boxs[i] == null)
                return;

            if (i == (int)box) // 바꾸고자 하는 박스만 활성화
            {
                s_boxs[i].SetActive(true);
                //ClearBox(box); // 초기화 작업도 진행
            }
            else
            {
                s_boxs[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// 초기 설정
    /// </summary>
    private void Init()
    {
        _boxs[(int)Box.Main] = _boxStruct.Main;
        _boxs[(int)Box.Quick] = _boxStruct.Quick;
        _boxs[(int)Box.Join] = _boxStruct.Join;
        _boxs[(int)Box.Create] = _boxStruct.Create;
    }

    /// <summary>
    /// 이벤트 구독
    /// </summary>
    private void SubscribesEvent()
    {
        _settingButton.onClick.AddListener(() => OptionPanel.SetActiveOption(true));
        _settingButton.onClick.AddListener(() => SoundManager.SFXPlay(SoundManager.Data.ButtonClick));
    }

    /// <summary>
    /// 싱글톤 지정
    /// </summary>
    private void InitSingleTon()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
