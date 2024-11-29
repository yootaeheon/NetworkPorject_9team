using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class PlayerDataContainer : MonoBehaviourPun
{
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

        LobbyScene.Instance.OnPlayerEnteredRoomEvent += SetEnterPlayerData;
        LobbyScene.Instance.OnPlayerLeftRoomEvent += SetExitPlayerData;
    }
    public void SetPlayerData(int actorNumber, string playerName, PlayerType type, float Rcolor, float Gcolor, float Bcolor, bool isGhost)
    {

        StartCoroutine(SetPlayerDataRoutine(actorNumber, playerName, type, Rcolor, Gcolor, Bcolor, isGhost));

    }

    IEnumerator SetPlayerDataRoutine(int actorNumber, string playerName, PlayerType type, float Rcolor, float Gcolor, float Bcolor, bool isGhost)
    {
        yield return 0.1f.GetDelay();

        photonView.RPC("RpcSetPlayerData", RpcTarget.AllBuffered, actorNumber, playerName, type, Rcolor, Gcolor, Bcolor, isGhost);
    }

    private void SetEnterPlayerData(Player newPlayer)
    {
        StartCoroutine(SetEnterPlayerDataRoutine(newPlayer));
    }

    IEnumerator SetEnterPlayerDataRoutine(Player newPlayer)
    {
        yield return 0.1f.GetDelay();

        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        PlayerData data = GetPlayerData(actorNumber);
        photonView.RPC(nameof(RpcSetEnterPlayerData), RpcTarget.AllBuffered,
           newPlayer,
           actorNumber,
           data.PlayerName,
           data.Type,
           data.PlayerColor.r,
           data.PlayerColor.g,
           data.PlayerColor.b,
           data.IsGhost);
    }

    private void SetExitPlayerData(Player exitPlayer)
    {
        StartCoroutine(SetExitPlayerDataRoutine(exitPlayer));
    }

    IEnumerator SetExitPlayerDataRoutine(Player exitPlayer)
    {
        yield return 0.1f.GetDelay();
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        PlayerData data = GetPlayerData(actorNumber);
        photonView.RPC("RpcSetPlayerData", RpcTarget.AllBuffered, actorNumber, "None", PlayerType.Goose, Color.white.r, Color.white.g, Color.white.b, true);
    }

    public PlayerData GetPlayerData(int actorNumber)
    {
        return playerDataArray[actorNumber - 1];
    }
    public PlayerType GetPlayerJob(int actorNumber)
    {
        return playerDataArray[actorNumber - 1].Type;
    }
    public void RandomSetjob()
    {
        photonView.RPC("RpcRandomSetjob", RpcTarget.All);
    }

    public void UpdatePlayerGhostList(int actorNumber)
    {
        photonView.RPC("RpcUpdatePlayerGhostList", RpcTarget.All, actorNumber);
    }

    [PunRPC]
    private void RpcUpdatePlayerGhostList(int actorNumber)
    {
        playerDataArray[actorNumber].IsGhost = true;
    }

    [PunRPC]
    private void RpcSetPlayerData(int actorNumber, string playerName, PlayerType type, float Rcolor, float Gcolor, float Bcolor, bool isGhost)
    {

        int index = actorNumber - 1;
        Color color = new Color(Rcolor, Gcolor, Bcolor, 255f);

        Debug.Log("셋 디버깅");
        Debug.Log(playerDataArray[index]);

        if (playerDataArray[index] == null)
        {
            playerDataArray[index] = new PlayerData(playerName, type, color, isGhost);
        }
        else
        {

            playerDataArray[index].PlayerName = playerName;
            playerDataArray[index].Type = type;
            playerDataArray[index].PlayerColor = color;
            playerDataArray[index].IsGhost = isGhost;
        }
    }

    [PunRPC]
    private void RpcSetEnterPlayerData(Player enterPlayer, int actorNumber, string playerName, PlayerType type, float Rcolor, float Gcolor, float Bcolor, bool isGhost)
    {
        if (enterPlayer != PhotonNetwork.LocalPlayer)
            return;

        int index = actorNumber - 1;
        Color color = new Color(Rcolor, Gcolor, Bcolor, 255f);

        Debug.Log("엔터 디버깅");
        Debug.Log(playerDataArray[index]);
        if (playerDataArray[index] == null)
        {
            playerDataArray[index] = new PlayerData(playerName, type, color, isGhost);
        }
        else
        {

            playerDataArray[index].PlayerName = playerName;
            playerDataArray[index].Type = type;
            playerDataArray[index].PlayerColor = color;
            playerDataArray[index].IsGhost = isGhost;
        }
    }


    [PunRPC]
    private void RpcRandomSetjob()
    {
        int count = 0;
        for (int i = 0; i < playerDataArray.Length; i++)
        {
            if (playerDataArray[i].IsGhost == false)
            {
                count++;
            }
        }
        for (int i = 0; i < count; i++)
        {
            int x = Random.Range(0, count);
            playerDataArray[x].Type = PlayerType.Duck;
        }

        int index = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        PlayerType type = playerDataArray[index].Type;
        Color color = playerDataArray[index].PlayerColor;
        GameUI.ShowGameStart(type, color);
    }
}
