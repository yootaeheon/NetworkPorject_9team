using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MissionObj : MonoBehaviour
{
    private MissionState _missionState;

    public DraingeObjType type;
    private bool isComplete;
    public bool IsComplete
    {
        get
        {
            return isComplete;
        }
        set
        {
            if (isComplete != value)
            {
                _missionState = transform.parent.GetComponent<MissionState>();
                _missionState.ObjectCount--;
                isComplete = value;
            }
        }
    }

    private void OnDisable()
    {
        isComplete = false;
    }

}
