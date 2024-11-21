using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class MissionState : MonoBehaviour
{
    [field: HideInInspector] public string MissionName {  get; set; }
    [field: HideInInspector] public int ObjectCount { get; set; }
    [field: HideInInspector] public bool IsDetect { get; set; }
    [field: HideInInspector] public Vector2 MousePos { get; set; }
    [SerializeField] public List<AudioClip> _clips = new List<AudioClip>();
    [HideInInspector] public Animator _anim;

    [HideInInspector] public int _openHash;
    [HideInInspector] public int _closeHash; 
      
    private void Awake()
    {
        _anim = GetComponent<Animator>();

        _openHash = Animator.StringToHash("OpenPopup");
        _closeHash = Animator.StringToHash("ClosePopup"); 
    }

    private void OnEnable()
    {
        _anim.Play(_openHash);
        Debug.Log($"현재 미션 내용 : {MissionName}");
    }

    public void ClosePopAnim()
    {
        _anim.Play(_closeHash);
    }
     
}
