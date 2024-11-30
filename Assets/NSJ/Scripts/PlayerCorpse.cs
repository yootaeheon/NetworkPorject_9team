using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCorpse : MonoBehaviour
{
    Coroutine _lifeRoutine;
    private void OnEnable()
    {
        if( _lifeRoutine == null )
        {
            _lifeRoutine = StartCoroutine(LifeRoutine());
        }
    }
    private void OnDisable()
    {
        if(_lifeRoutine != null)
        {
            StopCoroutine( _lifeRoutine );
            _lifeRoutine = null;
        }
    }

    /// <summary>
    /// 투표씬 넘어가기 전까지 씬에 남아있음
    /// </summary>
    /// <returns></returns>
    IEnumerator LifeRoutine()
    {
        while (true)
        {
            // 투표씬 진입 시
            if (VoteScene.Instance != null)
            {
                yield return 1f.GetDelay();
               DeleteCorpse();
            }
            yield return 0.1f.GetDelay();
        }
    }

    /// <summary>
    /// 시체 삭제
    /// </summary>
    private void DeleteCorpse()
    {
        if (_lifeRoutine != null)
        {
            StopCoroutine(_lifeRoutine);
            _lifeRoutine = null;
        }

        ReportingObject reportingObject = GetComponent<ReportingObject>();
        reportingObject.Reporting();
    }

}
