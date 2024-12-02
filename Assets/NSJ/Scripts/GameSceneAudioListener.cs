using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneAudioListener : MonoBehaviour
{
    private AudioListener _audioListener;

    private void Awake()
    {
        _audioListener = GetComponent<AudioListener>();
    }

    private void Start()
    {
        StartCoroutine(UpdateRoutine());
    }
    IEnumerator UpdateRoutine()
    {
        while (true) 
        {
            if(VoteScene.Instance == null) // ≈ı«• æ¿ æ∆¥‘
            {
                _audioListener.enabled = true;

            }
            else // ≈ı«•æ¿
            {
                _audioListener.enabled = false;
            }
            yield return 0.05f.GetDelay();
        }
    }
}
