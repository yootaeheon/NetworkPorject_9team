using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyScene : MonoBehaviourPunCallbacks, IPunObservable
{
    TMP_Text _tmpText;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       int value = stream.SendAndReceiveStruct(1); // 변수 동기화
       GameObject gameObject = stream.SendAndReceiveClass(this.gameObject); // 컴포넌트 동기화
       float lack = info.GetLack(); // 지연 시간 가져오기
        _tmpText.SetText("텍스트".GetText()); // 텍스트 메쉬 프로 사용시 
    }
    IEnumerator TestRoutine()
    {
        yield return 0.751f.GetDelay(); // 딜레이 가져올 때
    }
}
