using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerMover : MonoBehaviour
{
    [SerializeField] Speaker _speaker;

    private void Start()
    {
        Speaker speaker = GetComponentInChildren<Speaker>();
        _speaker = speaker;

        _speaker.transform.localPosition = Vector3.zero;
    }

  
}
