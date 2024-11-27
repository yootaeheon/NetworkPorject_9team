using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalButton : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _powerClips = new List<AudioClip>();

    private int _powerCount;
    public int PowerCount
    {
        get
        {
            return _powerCount;
        }
        set
        {
            _powerCount = value;

            if(_powerCount < 1)
            {
                //껐다 킬 수 있게 Bool 변수 셋팅
            }


        }
    }

    private void OnEnable()
    {
        _powerCount = Random.Range(1, 15);
        Debug.Log($"{gameObject.name} : {_powerCount}");
    }


    //각 버튼 별 클릭해야 할 횟수 > Random.Range로 부여
    //각 버튼 별 스크립트로 넘겨주는게 좋을듯
}
