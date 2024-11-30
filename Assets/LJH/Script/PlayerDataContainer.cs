using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PlayerDataContainer : MonoBehaviourPun
{
    [SerializeField] public int GooseCount = 0;
    [SerializeField] public int DuckCount = 0;
    [SerializeField] public PlayerData[] playerDataArray;

    private int MaxPlayers = 15;
    public static PlayerDataContainer Instance;


    private void Start()
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

        // 배열  초기화
        playerDataArray = new PlayerData[MaxPlayers];
        for (int i = 0; i < MaxPlayers; i++)
        {
            playerDataArray[i] = new PlayerData("None", PlayerType.Goose, Color.white, true);
        }

        StartCoroutine(SubscribesEventLobbySceneRoutine());
    }
    /// <summary>
    /// 플레이어 데이터 세팅
    /// </summary>
    public void SetPlayerData(int playerNumber, string playerName, PlayerType type, float Rcolor, float Gcolor, float Bcolor, bool isGhost)
    {

        StartCoroutine(SetPlayerDataRoutine(playerNumber, playerName, type, Rcolor, Gcolor, Bcolor, isGhost));

    }
    IEnumerator SetPlayerDataRoutine(int playerNumber, string playerName, PlayerType type, float Rcolor, float Gcolor, float Bcolor, bool isGhost)
    {
        yield return 0.1f.GetDelay();

        photonView.RPC("RpcSetPlayerData", RpcTarget.AllBuffered, playerNumber, playerName, type, Rcolor, Gcolor, Bcolor, isGhost);
    }

    /// <summary>
    /// 입장 플레이어 데이터 세팅
    /// </summary>
    private void SetEnterPlayerData(Player newPlayer)
    {
        StartCoroutine(SetEnterPlayerDataRoutine(newPlayer));
    }

    IEnumerator SetEnterPlayerDataRoutine(Player newPlayer)
    {
        yield return 0.1f.GetDelay();

        int playerNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber();
        PlayerData data = GetPlayerData(playerNumber);
        photonView.RPC(nameof(RpcSetEnterPlayerData), RpcTarget.AllBuffered,
           newPlayer,
           playerNumber,
           data.PlayerName,
           data.Type,
           data.PlayerColor.r,
           data.PlayerColor.g,
           data.PlayerColor.b,
           data.IsGhost);
    }

    /// <summary>
    /// 퇴장 플레이어 세팅
    /// </summary>
    private void SetExitPlayerData(Player exitPlayer)
    {
        StartCoroutine(SetExitPlayerDataRoutine(exitPlayer));
    }

    IEnumerator SetExitPlayerDataRoutine(Player exitPlayer)
    {
        yield return 0.1f.GetDelay();
        int playerNumber = exitPlayer.GetPlayerNumber();
        PlayerData data = GetPlayerData(playerNumber);
        photonView.RPC(nameof(RpcSetExitPlayerData), RpcTarget.AllBuffered, playerNumber, "None", PlayerType.Goose, Color.white.r, Color.white.g, Color.white.b, true);
    }
    public void SetPlayerTypeCounts()
    {
        Debug.Log("타입별 카운팅");
        Debug.Log($"{PhotonNetwork.PlayerList.Length}현재 방 인원");
        GooseCount = 0;
        DuckCount = 0;
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (playerDataArray[i].IsGhost == false)
            {
                if (playerDataArray[i].Type == PlayerType.Goose)
                {
                    GooseCount++;
                }
                else
                {
                    DuckCount++;
                }

            }

        }

    }
    public PlayerData GetPlayerData(int playerNumber)
    {
        return playerDataArray[playerNumber];
    }
    public PlayerType GetPlayerJob(int playerNumber)
    {
        return playerDataArray[playerNumber].Type;
    }
    public void RandomSetjob()
    {
        photonView.RPC(nameof(RpcRandomSetjob), RpcTarget.MasterClient);

    }

    public void UpdatePlayerGhostList(int playerNumber)
    {
        photonView.RPC("RpcUpdatePlayerGhostList", RpcTarget.All, playerNumber);
    }

    [PunRPC]
    private void RpcUpdatePlayerGhostList(int playerNumber)
    {
        playerDataArray[playerNumber].IsGhost = true;
    }

    [PunRPC]
    private void RpcSetPlayerData(int playerNumber, string playerName, PlayerType type, float Rcolor, float Gcolor, float Bcolor, bool isGhost)
    {

        int index = playerNumber;
        Color color = new Color(Rcolor, Gcolor, Bcolor, 255f);

        Debug.Log("셋 디버깅");
        Debug.Log(playerDataArray[index]);

        if (playerDataArray[index] == null)
        {
            playerDataArray[index] = new PlayerData(playerName, type, color, isGhost);
        }
        else
        {
            playerDataArray[index].IsNone = false;
            playerDataArray[index].PlayerName = playerName;
            //playerDataArray[ix].Type = type;
            playerDataArray[index].PlayerColor = color;
            playerDataArray[index].IsGhost = isGhost;
        }
    }

    [PunRPC]
    private void RpcSetEnterPlayerData(Player enterPlayer, int playerNumber, string playerName, PlayerType type, float Rcolor, float Gcolor, float Bcolor, bool isGhost)
    {
        if (enterPlayer != PhotonNetwork.LocalPlayer)
            return;

        int index = playerNumber;
        Color color = new Color(Rcolor, Gcolor, Bcolor, 255f);

        Debug.Log("엔터 디버깅");
        Debug.Log(playerDataArray[index]);
        if (playerDataArray[index] == null)
        {
            playerDataArray[index] = new PlayerData(playerName, type, color, isGhost);
        }
        else
        {
            playerDataArray[index].IsNone = false;
            playerDataArray[index].PlayerName = playerName;
            //playerDataArray[ix].Type = type;
            playerDataArray[index].PlayerColor = color;
            playerDataArray[index].IsGhost = isGhost;
        }
    }

    [PunRPC]
    private void RpcSetExitPlayerData(int playerNumber, string playerName, PlayerType type, float Rcolor, float Gcolor, float Bcolor, bool isGhost)
    {

        int index = playerNumber;
        Color color = new Color(Rcolor, Gcolor, Bcolor, 255f);

        if (playerDataArray[index] == null)
        {
            playerDataArray[index] = new PlayerData(playerName, type, color, isGhost);
        }
        else
        {
            playerDataArray[index].IsNone = false;
            playerDataArray[index].PlayerName = playerName;
            //playerDataArray[ix].Type = type;
            playerDataArray[index].PlayerColor = color;
            playerDataArray[index].IsGhost = isGhost;
        }
    }

    [PunRPC]
    private void RpcRandomSetjob()
    {
        int count = 0;
        //for (int i = 0; i < playerDataArray.Length; i++)
        //{
        //    if (playerDataArray[i].IsNone == false)
        //    {
        //        // None이 아닌 인덱스만 추가
        //        count++;
        //    }
        //}

        // 5명당 오리 한명씩, 5의 배수 초과부터 ex) 6명 2명
        count = (PhotonNetwork.CurrentRoom.MaxPlayers % 5) != 0 ? PhotonNetwork.CurrentRoom.MaxPlayers / 5 + 1 : PhotonNetwork.CurrentRoom.MaxPlayers / 5;
        for (int i = 0; i < count; i++)
        {
            // 랜덤 인덱스 오리 선정
            int x = Random.Range(0, PhotonNetwork.CurrentRoom.MaxPlayers);
            // 정해진 인덱스를 모든 플레이어와 동기화
            photonView.RPC(nameof(RpcSetDuckPlayer), RpcTarget.All, x);
        }

        // 직업 선정 UI 표시
        photonView.RPC(nameof(RpcShowGameStartUI), RpcTarget.All);
    }
    /// <summary>
    /// 오리 플레이어 동기화
    /// </summary>
    [PunRPC]
    private void RpcSetDuckPlayer(int index)
    {
        // 해당 인덱스를 모두가 오리 플레이어라고 저장
        playerDataArray[index].Type = PlayerType.Duck;
    }
    /// <summary>
    /// 게임 시작 UI 실행
    /// </summary>
    [PunRPC]
    private void RpcShowGameStartUI()
    {
        int index = PhotonNetwork.LocalPlayer.GetPlayerNumber();
        PlayerType type = playerDataArray[index].Type;
        Color color = playerDataArray[index].PlayerColor;
        GameUI.ShowGameStart(type, color);
    }

    IEnumerator SubscribesEventLobbySceneRoutine()
    {
        // 구독함?
        bool isSubscribe = false;

        while (true)
        {
            // 구독안했음?
            if (isSubscribe == false)
            {
                //로비씬임?
                if (LobbyScene.Instance != null)
                {
                    // 구독
                    SubscribesEventLobbyScene();
                    isSubscribe = true;
                }
                yield return null;
            }
            // 구독했음?
            else
            {
                // 로비씬 아님?
                if (LobbyScene.Instance == null)
                {
                    // 로비씬가면 다시 구독 ㄱㄱ
                    isSubscribe = false;
                }
                yield return null;
            }
        }
    }
    private void SubscribesEventLobbyScene()
    {
        LobbyScene.Instance.OnPlayerEnteredRoomEvent += SetEnterPlayerData;
        LobbyScene.Instance.OnPlayerLeftRoomEvent += SetExitPlayerData;
    }
}
