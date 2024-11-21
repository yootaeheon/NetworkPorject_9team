using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class MissionState : MonoBehaviour
{
    [field: SerializeField] public int ObjectCount { get; set; }
    [field: SerializeField] public bool IsDetect { get; set; }
    [field: SerializeField] public Vector2 MousePos { get; set; }
    [SerializeField] public List<AudioClip> _clips = new List<AudioClip>();
    [HideInInspector] public Animator _anim;

    public int _openHash;
    public int _closeHash; 
      
    private void Awake()
    {
        _anim = GetComponent<Animator>();

        _openHash = Animator.StringToHash("OpenPopup");
        _closeHash = Animator.StringToHash("ClosePopup"); 
    }

    private void OnEnable()
    {
        _anim.Play(_openHash);
    }

    public void ClosePopAnim()
    {
        _anim.Play(_closeHash);
    }
     
}
