using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;

public class MissionState : MonoBehaviour
{
    [field: HideInInspector] public string MissionName {  get; set; }
    [field: HideInInspector] public int ObjectCount { get; set; }
    [field: HideInInspector] public bool IsDetect { get; set; }
    [field: HideInInspector] public Vector2 MousePos { get; set; }

    public PlayerType MyPlayerType { get; set; }

    //오리의 컬러 값
    public static Color PlayerColor { get; set; }

    [Header("미션 브금 리스트")]
    [Tooltip
        ("[0] 미션 상호작용 사운드\n" +
        "[1] 미션 클리어 사운드\n" +
        "[2] X 버튼 클릭 사운드")]
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
