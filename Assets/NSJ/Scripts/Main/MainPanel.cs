using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : BaseUI
{
    [SerializeField] int _minPlayer;
    [SerializeField] int _maxPlayer;

    #region private 필드

    enum Box { Main, Quick, Join, Create, Size }
    private GameObject[] _boxs = new GameObject[(int)Box.Size];

    // MainBox
    private TMP_Text _mainNameText => GetUI<TMP_Text>("MainNameText");
    private TMP_Text _mainNickNameText => GetUI<TMP_Text>("MainNickNameText");
    //JoinBox
    private TMP_InputField _joinRoomInput => GetUI<TMP_InputField>("JoinRoomInput");
    private TMP_InputField _joinNickNameInput => GetUI<TMP_InputField>("JoinNickNameInput");
    private GameObject _joinInvisibleOnImage => GetUI("JoinInvisibleOnImage");

    // CreateRoomBox
    private TMP_InputField _createNickNameInput => GetUI<TMP_InputField>("CreateNickNameInput");
    private TMP_Text _createPlayerCountText => GetUI<TMP_Text>("CreatePlayerCountText");
    private Slider _createPlayerCountSlider => GetUI<Slider>("CreatePlayerCountSlider");
    private Slider _createRoomOpenSlider => GetUI<Slider>("CreateRoomOpenSlider");
    private TMP_Text _createRoomOpenText => GetUI<TMP_Text>("CreateRoomOpenText");
    private GameObject _createPrivacyCheck => GetUI("CreatePrivacyCheck");

    // QuickBox
    private TMP_InputField _quickNickNameInput => GetUI<TMP_InputField>("QuickNickNameInput");
    private GameObject _quickColorBox => GetUI("QuickColorBox");


    private StringBuilder _sb = new StringBuilder();
    #endregion

    private void Awake()
    {
        Bind();
        Init();
    }

    private void Start()
    {
        SubscribesEvent();
    }

    private void OnEnable()
    {
        ChangeBox(Box.Main);
    }

    #region JoinBox 버튼
    /// <summary>
    /// 로비 입장
    /// </summary>
    private void JoinLobby()
    {
        string nickName = _joinNickNameInput.text; // 닉네임 캐싱


        LobbyScene.ActivateLoadingBox(true); // 로딩창 활성화

        if (nickName != string.Empty) // 닉네임 변경점 있으면 닉네임 변경
        {
            ChangeNickName(nickName); // 닉네임 변경(포톤네트워크 닉네임 변경, 데이터베이스 닉네임 변경      
        }

        LobbyScene.ActivateLoadingBox(true);
        PhotonNetwork.JoinLobby(); // 로비 입장
    }
    /// <summary>
    /// 방 코드 자동 대문자 변환
    /// </summary>
    private void ChangeRoomCodeToUpper(string value)
    {
        _joinRoomInput.text = value.ToUpper();
    }

    /// <summary>
    /// 방코드 숨기기/보이기
    /// </summary>
    private void ChangeRoomCodeInvisible()
    {
        if (_joinRoomInput.contentType == TMP_InputField.ContentType.Password) //안보이는 상태
        {
            _joinRoomInput.contentType = TMP_InputField.ContentType.Standard;
            _joinInvisibleOnImage.SetActive(false);
        }
        else
        {
            _joinRoomInput.contentType = TMP_InputField.ContentType.Password;
            _joinInvisibleOnImage.SetActive(true);
        }
        StartCoroutine(ChangeRoomCodeInvisibleRoutine());
    }

    IEnumerator ChangeRoomCodeInvisibleRoutine()
    {
        string temp = _joinRoomInput.text;
        _joinRoomInput.text = string.Empty;
        yield return null;
        _joinRoomInput.text = temp;
    }


    /// <summary>
    /// 방 코드로 입장
    /// </summary>
    private void JoinRoomCode()
    {
        string roomCode = _joinRoomInput.text; //방 코드 캐싱
        if (roomCode == string.Empty)
            return;

        string nickName = _joinNickNameInput.text; // 닉네임 캐싱

        LobbyScene.ActivateLoadingBox(true); // 로딩창 활성화

        if (nickName != string.Empty) // 닉네임 변경점 있으면 닉네임 변경
        {
            ChangeNickName(nickName); // 닉네임 변경(포톤네트워크 닉네임 변경, 데이터베이스 닉네임 변경      
        }

        PhotonNetwork.JoinRoom(roomCode); // 방 코드로 방 입장
    }
    #endregion

    #region 방 생성

    /// <summary>
    /// 방 생성
    /// </summary>
    private void CreateRoom()
    {
        string nickName = _createNickNameInput.text;
        if (nickName != string.Empty) // 닉네임 변경 시
        {
            ChangeNickName(nickName);
        }

        string roomCode = GetRandomRoomCode(6); // 랜덤 방코드 획득
        int maxPlayer = (int)_createPlayerCountSlider.value; // 최대 인원 획득
        bool isVisible = (int)_createRoomOpenSlider.value == 0 ? true : false; // 공개 여부 획득

        // 방 옵션 세팅
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayer;
        options.IsVisible = isVisible;
        options.SetPrivacy(_createPrivacyCheck.activeSelf);

        LobbyScene.ActivateLoadingBox(true);
        PhotonNetwork.CreateRoom(roomCode, options);
    }

    /// <summary>
    /// 플레이어 카운트 업데이트
    /// </summary>
    private void UpdatePlayerCount(float value)
    {
        _createPlayerCountText.SetText(value.GetText());
    }

    /// <summary>
    /// 공개방 비공개방 업데이트
    /// </summary>
    private void UpdateIsVisible(float value)
    {
        if (value == 1f) // 슬라이더 값 1은 비공개, 0은 공개
        {
            _createRoomOpenText.SetText("비공개".GetText());
        }
        else
        {
            _createRoomOpenText.SetText("공개".GetText());
        }
    }

    /// <summary>
    /// 프라이버시 모드
    /// </summary>
    private void UpdatePrivacyMode()
    {
        if(_createPrivacyCheck.activeSelf == false)
        {
            // 프라이버시 모드 활성화
            _createPrivacyCheck.SetActive(true);
        }
        else
        {
            // 비활성화
            _createPrivacyCheck.SetActive(false);
        }
    }

    #endregion

    #region 빠른 시작
    /// <summary>
    /// 빠른 시작
    /// </summary>
    private void StartRandomMatch()
    {
        string nickName = _quickNickNameInput.text;
        if (nickName != string.Empty) // 닉네임 변동 사항 있을 시에 닉네임 변경
        {
            ChangeNickName(nickName);
        }

        LobbyScene.ActivateLoadingBox(true);
        PhotonNetwork.JoinRandomRoom();
    }
    /// <summary>
    /// 빠른 시작 매칭 실패 시 새로운 방 자동 생성
    /// </summary>
    private void CreateRandomRoom(short returnCode, string message)
    {
        string roomCode = GetRandomRoomCode(6); // 랜덤 방코드 획득
        int maxPlayer = 10;
        bool isVisible = true;

        // 방 옵션 세팅
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayer;
        options.IsVisible = isVisible;
        options.SetPrivacy(true); // 빠른시작 방이니까 프라이버시 모드

        PhotonNetwork.CreateRoom(roomCode, options);
    }

    #endregion

    #region 로그아웃
    /// <summary>
    /// 로그아웃
    /// </summary>
    private void LogOut()
    {
        LobbyScene.ActivateLoadingBox(true);
        BackendManager.Auth.SignOut(); // 로그아웃
        PhotonNetwork.Disconnect(); // 서버 연결 끊기
    }

    #endregion

    #region 패널 조작

    /// <summary>
    /// UI 박스 변경
    /// </summary>
    private void ChangeBox(Box box)
    {
        LobbyScene.ActivateLoadingBox(false);

        for (int i = 0; i < _boxs.Length; i++)
        {
            if (_boxs[i] == null)
                return;

            if (i == (int)box) // 바꾸고자 하는 박스만 활성화
            {
                _boxs[i].SetActive(true);
                ClearBox(box); // 초기화 작업도 진행
            }
            else
            {
                _boxs[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// UI 박스 초기화 작업
    /// </summary>
    /// <param name="box"></param>
    private void ClearBox(Box box)
    {
        switch (box)
        {
            case Box.Main:
                ClearMainBox();
                break;
            case Box.Create:
                ClearCreateRoomBox();
                break;
            case Box.Join:
                ClearJoinBox();
                break;
            case Box.Quick:
                ClearQuickBox();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 메인 화면 초기화
    /// </summary>
    private void ClearMainBox()
    {
        if (BackendManager.Instance == null)
            return;
        if (BackendManager.User == null)
            return;
        _mainNameText.SetText($"{BackendManager.User.FirstName} {BackendManager.User.SecondName} 님이 로그인했습니다".GetText());
        _mainNickNameText.SetText($"[{BackendManager.User.NickName}]".GetText());
    }
    /// <summary>
    /// 방 입장(게임 하기 버튼) 포기화
    /// </summary>
    private void ClearJoinBox()
    {
        _joinNickNameInput.text = string.Empty;
        _joinRoomInput.text = string.Empty;
        _joinInvisibleOnImage.SetActive(false);
    }

    /// <summary>
    /// 방 생성 화면 초기화
    /// </summary>
    private void ClearCreateRoomBox()
    {
        _createNickNameInput.text = string.Empty;
        _createPlayerCountSlider.value = (int)((_createPlayerCountSlider.maxValue + _createPlayerCountSlider.minValue) / 2); // 절반 수치만큼
        _createRoomOpenSlider.value = 1f; // 기본 비공개 방
        _createPrivacyCheck.SetActive(false);
    }
    /// <summary>
    /// 빠른 시작 초기화
    /// </summary>
    private void ClearQuickBox()
    {
        _quickNickNameInput.text = string.Empty;
        _quickColorBox.SetActive(false);
    }


    #endregion


    #region 초기 설정
    /// <summary>
    /// 초기 설정
    /// </summary>
    private void Init()
    {
        #region box 배열 설정

        _boxs[(int)Box.Main] = GetUI("MainBox");
        _boxs[(int)Box.Quick] = GetUI("QuickBox");
        _boxs[(int)Box.Join] = GetUI("JoinBox");
        _boxs[(int)Box.Create] = GetUI("CreateRoomBox");

        #endregion

        #region CreateRoomBox
        // TODO : 추후 게임매니저 같은곳에서 최대최소 인원 연동해서 가져와야할 필요가 있음
        _createPlayerCountSlider.minValue = _minPlayer;
        _createPlayerCountSlider.maxValue = _maxPlayer;

        #endregion
    }

    /// <summary>
    /// 이벤트 구독
    /// </summary>
    private void SubscribesEvent()
    {


        #region MainBox

        GetUI<Button>("MainLogOutButton").onClick.AddListener(LogOut);
        GetUI<Button>("MainQuickMatchButton").onClick.AddListener(() => ChangeBox(Box.Quick));
        GetUI<Button>("MainJoinButton").onClick.AddListener(() => ChangeBox(Box.Join));

        #endregion

        #region QuickBox

        GetUI<Button>("QuickBackButton").onClick.AddListener(() => ChangeBox(Box.Main));

        #endregion

        #region JoinBox

        GetUI<Button>("JoinBackButton").onClick.AddListener(() => ChangeBox(Box.Main));
        GetUI<Button>("JoinCreateRoomButton").onClick.AddListener(() => ChangeBox(Box.Create));
        _joinRoomInput.onValueChanged.AddListener(ChangeRoomCodeToUpper);
        GetUI<Button>("JoinInvisibleButton").onClick.AddListener(ChangeRoomCodeInvisible);
        GetUI<Button>("JoinLobbyButton").onClick.AddListener(JoinLobby);
        GetUI<Button>("JoinRoomButton").onClick.AddListener(JoinRoomCode);

        #endregion

        #region CreateRoomBox

        _createPlayerCountSlider.onValueChanged.AddListener(UpdatePlayerCount);
        _createRoomOpenSlider.onValueChanged.AddListener(UpdateIsVisible);
        GetUI<Button>("CreateBackButton").onClick.AddListener(() => ChangeBox(Box.Join));
        GetUI<Button>("CreateRoomButton").onClick.AddListener(CreateRoom);
        GetUI<Button>("CreatePrivacyButton").onClick.AddListener(UpdatePrivacyMode);

        #endregion

        #region QuickBox

        LobbyScene.Instance.OnJoinRandomFailedEvent += CreateRandomRoom;
        GetUI<Button>("QuickColorButton").onClick.AddListener(() => { _quickColorBox.SetActive(!_quickColorBox.activeSelf); });
        GetUI<Button>("QuickStartButton").onClick.AddListener(StartRandomMatch);
        #endregion

        GetUI<Button>("SettingButton").onClick.AddListener(() => LobbyScene.ActivateOptionBox(true));
    }
    #endregion

    /// <summary>
    /// 닉네임 변경
    /// </summary>
    /// <param name="nickName"></param>
    private void ChangeNickName(string nickName)
    {
        BackendManager.User.NickName = nickName; // 유저 정보에 닉네임 변경
        // 닉네임 데이터 베이스에 일부 쓰기 저장
        BackendManager.SettingDic.Clear();
        BackendManager.SettingDic.Add(UserDate.NICKNAME, nickName);
        BackendManager.Auth.CurrentUser.UserId.GetUserDataRef().UpdateChildrenAsync(BackendManager.SettingDic);
    }

    /// <summary>
    /// 랜덤 방 코드 얻기
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    private string GetRandomRoomCode(int length)
    {
        _sb.Clear();
        for (int i = 0; i < length; i++)
        {
            int numberOrAlphabet = Random.Range(0, 2);
            if (numberOrAlphabet == 0) // 숫자형
            {
                int numberASKII = Random.Range(48, 58); // 아스키코드 48~57번까지(0~9)
                _sb.Append((char)numberASKII);
            }
            else // 문자형
            {
                int alphabetASKII = Random.Range(65, 91); // 아스키코드 65~91번까지 (A~Z)
                _sb.Append((char)alphabetASKII);
            }
        }
        return _sb.ToString();
    }
}
