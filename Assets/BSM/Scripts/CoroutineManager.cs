using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    public static CoroutineManager Instance { get; private set; }

    private Dictionary<MonoBehaviour, Coroutine> _routineDict = new Dictionary<MonoBehaviour, Coroutine>();

    private void Awake() => SetSingleton();

    private void SetSingleton()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    /// <summary>
    /// 시작 코루틴을 받아와서 관리하는 기능
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public IEnumerator ManagerStartCoroutine(MonoBehaviour key, Coroutine value)
    {
        if (_routineDict.ContainsKey(key))
        {
            //키와 값이 이미 존재한다면 재생중인 코루틴 중지
            if (_routineDict.TryGetValue(key, out Coroutine routine))
            {
                StopCoroutine(routine);
                _routineDict.Remove(key);
            }
        }

        
        _routineDict.TryAdd(key, value);
        IEnumerator enumerator = _routineDict.GetEnumerator();

        //다음 동작이 있을 동안 반복
        while (enumerator.MoveNext())
        {
            //해당 코루틴의 대기 시간만큼 지연
            yield return enumerator.Current; 
        } 
    } 

    /// <summary>
    /// 코루틴 중지를 관리하는 기능
    /// </summary>
    /// <param name="key"></param>
    public void ManagerStopCoroutine(MonoBehaviour key)
    {
        if (!_routineDict.ContainsKey(key))
            return;

        StopCoroutine(_routineDict[key]);
        _routineDict.Remove(key);
    }

}
