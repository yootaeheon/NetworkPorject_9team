using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevPlan : MonoBehaviour
{
    ///
    /// 1. 투표에 필요한 데이터 생성 (VoteData stream 이용하여 변수 동기화 & 커스텀 프로퍼티 이용 각 플레이어의 투표 받은 수 동기화)
    ///(커스텀 프로퍼티 : 각 플레이어 투표 받은 수, 그 투표받은 수 불러오기 생성 예정)
    /// 
    /// 2.투표 씬 입장 시 각 플레이어 패널들을 생성  PlayerNumbering을 이용해 플레이어 Panel 주인과 연동이 될 것임
    /// 
    /// 3. RPC 이용 상대 플레이어 패널을 눌르면 그 플레이어 패널 주인 (2번 과정 이용)에게 투표 받은 수 +1
    /// 
    /// 4. 투표 시간이 끝나면 (2번 과정 이용) 모든 customProperty.ContainsKey[] 이용하여 투표받은 수 집계 제일 득표수가 많은 플레이어 추방
    ///



    
}
