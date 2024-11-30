using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteScene : MonoBehaviour
{
    public static VoteScene Instance;

    private void Awake()
    {
        InitSingleTon();
    }

    private void InitSingleTon()
    {
       if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
