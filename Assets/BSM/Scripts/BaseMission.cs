using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMission : MonoBehaviour
{

    private Dictionary<string, GameObject> _goDict;
    private Dictionary<(string, System.Type), Component> _coDict;
 
    protected void Bind()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>(true);

        _goDict = new Dictionary<string, GameObject>(transforms.Length << 2);

        foreach(Transform child in transforms)
        {
            _goDict.TryAdd(child.gameObject.name, child.gameObject);
        }

        _coDict = new Dictionary<(string, System.Type), Component>(); 
    }

    protected GameObject GetMissionObject(string name)
    { 
        _goDict.TryGetValue(name, out GameObject go); 
        return go;
    }

    protected T GetMissionComponent<T>(string name) where T : Component
    {
        (string, System.Type) _key = (name, typeof(T));

        _coDict.TryGetValue(_key, out Component co);

        if (co != null)
            return co as T;

        _goDict.TryGetValue(name, out GameObject go);

        if(go != null)
        {
            co = go.GetComponent<T>();

            if(co != null)
            {
                _coDict.TryAdd(_key, co);
                return co as T;
            } 
        }

        return null; 
    }


}
