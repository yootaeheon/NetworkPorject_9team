using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerSetting : MonoBehaviour
{
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponentInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.InRoom == false)
        {
            Destroy(gameObject);
        }

        StartCoroutine(SetSpeaker());
    }

    IEnumerator SetSpeaker()
    {
        yield return null;
        // VoteScene 전환 시 스피커 SpecialBlend 0(2D)으로 설정 
        if (VoteScene.Instance != null)
        {
            _audioSource.spatialBlend = 0;
        }
        // GameScene 전환 시 스피커 SpecialBlend 1(3D)으로 설정 
        else
        {
            _audioSource.spatialBlend = 1;
        }
    }
}
