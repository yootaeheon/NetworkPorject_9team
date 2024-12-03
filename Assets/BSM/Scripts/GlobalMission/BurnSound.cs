using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnSound : MonoBehaviour
{
    [SerializeField] private AudioClip _burnClip;


    private void OnDisable()
    {
        SoundManager.SFXPlay(_burnClip);
    }

}
