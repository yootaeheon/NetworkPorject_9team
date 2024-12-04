using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCamera : MonoBehaviour
{
    [SerializeField] AudioListener _audioListener;

    private void Awake()
    {
        _audioListener = GetComponent<AudioListener>();
    }

    private void Update()
    {
        if(PhotonNetwork.InRoom == true)
        {
            _audioListener.enabled = false;
        }
        else
        {
            _audioListener.enabled =true;
        }
    }

}
