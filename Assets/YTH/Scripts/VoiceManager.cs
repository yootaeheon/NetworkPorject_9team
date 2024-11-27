using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice.Unity;

public class VoiceManager : MonoBehaviour
{
    public static VoiceManager Instance { get; private set; }

    [SerializeField] PlayerController _playerController;

    [SerializeField] Recorder _recorder;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            _recorder.TransmitEnabled = true;
        }
        else
        {
             _recorder.TransmitEnabled = false;
        }
    }
}
