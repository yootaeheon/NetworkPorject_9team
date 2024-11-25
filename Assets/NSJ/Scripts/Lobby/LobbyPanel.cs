using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanel : BaseUI
{
    [SerializeField] GameObject _loadingBox;
    [SerializeField] RoomEntry _roomEntryPrefab;

    private RoomInfo _selectingRoomInfo;

    private Dictionary<string, RoomEntry> _roomDic = new Dictionary<string, RoomEntry>();
    #region private 필드
    enum Box { Lobby, Size }
    private GameObject[] _boxs = new GameObject[(int)Box.Size];
    private GameObject _lobbyStartButton => GetUI("LobbyStartButton");
    #endregion

    private void Awake()
    {
        Bind();
        Init();
    }

    private void Start()
    {
        SubscribeEvent();
    }

    private void OnEnable()
    {
        ChangeBox(Box.Lobby);
    }

    /// <summary>
    /// 선택한 방 업데이트
    /// </summary>
    public void UpdateSelectRoom(RoomInfo roomInfo)
    {
        // 선택된 방 캐싱
        // 같은 방을 두번 선택하면 캐싱 풀림
        if (_selectingRoomInfo == roomInfo)
        {
            _selectingRoomInfo = null;
            _lobbyStartButton.SetActive(false);
        }
        else
        {
            _selectingRoomInfo = roomInfo;
            _lobbyStartButton.SetActive(true);
        }

        // 전체 방 리스트 선택 체크
        foreach (RoomEntry roomEntry in _roomDic.Values)
        {
            roomEntry.CheckSelect(_selectingRoomInfo);
        }
    }

    /// <summary>
    /// 룸리스트 업데이트
    /// </summary>
    /// <param name="roomList"></param>
    private void UpdateRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            // 사라졌거나, 비공개거나, 열린방이 아닐때 (못들어가는 방)
            if (room.RemovedFromList || room.IsVisible == false || room.IsOpen == false)
            {
                // 해당방이 등록되지 않은 방이었을 때
                if (_roomDic.ContainsKey(room.Name) == false)
                    continue;
                Destroy(_roomDic[room.Name].gameObject);
                _roomDic.Remove(room.Name);
            }
            // 방이 새롭게 나왔을때
            else if (_roomDic.ContainsKey(room.Name) == false)
            {
                RoomEntry roomEntry = Instantiate(_roomEntryPrefab, GetUI("LobbyContent").transform);
                _roomDic.Add(room.Name, roomEntry);
                // TODO : 룸 엔트리 설정
                roomEntry.SetRoom(room);
                roomEntry.LobbyPanel = this;
            }
            // 기존 방이 변동사항이 있던 경우
            else if (_roomDic.ContainsKey(room.Name))
            {
                RoomEntry roomEntry = _roomDic[room.Name];
                // 룸 엔트리 설정
                roomEntry.SetRoom(room);
            }
        }
    }
    /// <summary>
    /// 방 입장하기
    /// </summary>
    private void JoinRoom()
    {
        ClearRoomEntry();
        ActivateLoadingBox(true);
        PhotonNetwork.JoinRoom(_selectingRoomInfo.Name);
    }

    /// <summary>
    /// 룸 리스트 비우기
    /// </summary>
    private void ClearRoomEntry()
    {
        foreach (RoomEntry roomEntry in _roomDic.Values)
        {
            Destroy(roomEntry.gameObject);
        }
        _roomDic.Clear();
    }


    /// <summary>
    /// 로비 떠나기
    /// </summary>
    private void LeftLobby()
    {
        PhotonNetwork.LeaveLobby();
    }

    #region 패널 조작

    /// <summary>
    /// UI 박스 변경
    /// </summary>
    private void ChangeBox(Box box)
    {
        ActivateLoadingBox(false);

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
            case Box.Lobby:
                ClearLobby(); 
                break;
            default:
                break;
        }
    }

    private void ClearLobby()
    {
        _lobbyStartButton.SetActive(false);
    }

    /// <summary>
    /// 로딩 화면 활성화 / 비활성화
    /// </summary>
    private void ActivateLoadingBox(bool isActive)
    {
        if (isActive)
        {
            _loadingBox.SetActive(true);
        }
        else
        {
            _loadingBox.SetActive(false);
        }
    }

    #endregion

    /// <summary>
    /// 초기 설정
    /// </summary>
    private void Init()
    {
        _boxs[(int)Box.Lobby] = GetUI("LobbyBox");
    }
    /// <summary>
    /// 이벤트 구독
    /// </summary>
    private void SubscribeEvent()
    {
        LobbyScene.Instance.OnRoomListUpdateEvent += UpdateRoomList;
        LobbyScene.Instance.OnLeftLobbyEvent += ClearRoomEntry;
        GetUI<Button>("LobbyBackButton").onClick.AddListener(LeftLobby);
        GetUI<Button>("LobbyStartButton").onClick.AddListener(JoinRoom);
        Debug.Log(GetUI<Button>("LobbyStartButton"));
    }

}
