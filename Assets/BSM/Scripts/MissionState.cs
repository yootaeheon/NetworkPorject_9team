using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionState : MonoBehaviour
{
    [field: SerializeField] public int ObjectCount { get; set; }
    [field: SerializeField] public bool IsDetect { get; set; }
    [SerializeField] public List<AudioClip> _clips = new List<AudioClip>();

}
