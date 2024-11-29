using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class VoteManager : MonoBehaviourPunCallbacks
{
    [SerializeField] VoteSceneData _voteData;

    [SerializeField] VotePanel _votePanel;

    [SerializeField] VoteScenePlayerData[] _playerData;

    public int[] _voteCounts; // �� �÷��̾���(ActorNumber�� ����� �ε��� ��ȣ)�� ��ǥ���� �迭�� ����

    [SerializeField] Button[] _voteButtons;

    // IsDead == false && DidVote == false �϶��� ��ŵ �����ϰ� ���� �߰�
    public void Vote(int index) // �÷��̾� �г��� ���� ��ǥ
    {
        photonView.RPC("VotePlayerRPC", RpcTarget.All, index);
        _votePanel.DisableButton();
    }

    [PunRPC]
    public void VotePlayerRPC(int index)
    {
        _voteCounts[index]++;
        Debug.Log($"{index}�� �÷��̾� ��ǥ�� {_voteCounts[index]} ");
    }

    // IsDead == false && DidVote == false �϶��� ��ŵ �����ϰ� ���� �߰�
    public void OnClickSkip() // ��ŵ ��ư ���� ��
    {
        photonView.RPC("OnClickSkipRPC", RpcTarget.AllBuffered);
        _votePanel.DisableButton();
    }

    [PunRPC]
    public void OnClickSkipRPC()
    {
        _voteData.SkipCount++;
        Debug.Log($" ��ŵ �� : {_voteData.SkipCount}");
    }

    // ��ǥ ���� �� ���� ���
    public void GetVoteResult()
    {
        // �ִ� ��ǥ�� ã�� ���
        bool isKick = false;
        int top = -1;
        int top2 = -1;
        int playerIndex = -1;

        for (int i = 0; i < 12; i++)
        {
            if (_voteCounts[i] > top)
            {
                top = _voteCounts[i];
                playerIndex = i;

                isKick = true;
            }
            else if (_voteCounts[i] == top)
            {
                top2 = _voteCounts[i];
                Debug.Log("����ǥ�� ���� ��~");
                isKick = false;
                return;
            }
        }
        Debug.Log($"{_voteData.SkipCount}ǥ ���!");
        Debug.Log($"{playerIndex}�� �÷��̾� �缱 {top}ǥ : �߹�˴ϴ�");


        PlayerData playerData =  PlayerDataContainer.Instance.GetPlayerData(playerIndex);
        StartCoroutine(ShowVoteResultRoutine(playerData.PlayerColor, playerData.PlayerName,playerData.Type));

        //TODO : ����Ʈ�� �Ǵ� ���
        return;
    }
    
    IEnumerator ShowVoteResultRoutine(Color playerColor , string name, PlayerType type)
    {
        yield return 3f.GetDelay();
        GameUI.ShowVoteResult(playerColor, name, type);
        yield return 7f.GetDelay();
        SceneChanger.UnLoadScene("VoteScene");
    }
}
