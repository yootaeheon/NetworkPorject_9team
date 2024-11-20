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

    public IEnumerator ManagerStartCoroutine(MonoBehaviour key, Coroutine value)
    {
        if (_routineDict.ContainsKey(key))
        {
            if (_routineDict.TryGetValue(key, out Coroutine routine))
            {
                StopCoroutine(routine);
            }
        }

        _routineDict.TryAdd(key, value);
        IEnumerator enumerator = _routineDict.GetEnumerator();

        while (enumerator.MoveNext())
        {
            yield return enumerator.Current; 
        } 
    } 

    public void ManagerStopCoroutine(MonoBehaviour key)
    {
        if (!_routineDict.ContainsKey(key))
            return;

        StopCoroutine(_routineDict[key]);
        _routineDict.Remove(key);
    }

}
