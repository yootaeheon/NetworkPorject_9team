using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MissionState : MonoBehaviour
{
    [field: SerializeField] public string MissionName {  get; set; }
    [field: HideInInspector] public int ObjectCount { get; set; }
    [field: HideInInspector] public bool IsDetect { get; set; }
    [field: HideInInspector] public Vector2 MousePos { get; set; } 
    [field: HideInInspector] public PlayerType MyPlayerType { get; set; }
    [field: HideInInspector] public bool IsPerform { get; set; }
    [field: HideInInspector] public bool IsAssign { get; set; }

    [field: HideInInspector] public int TextIndex { get; set; } = -1;

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

    private bool isAnimCheck;

    private void Awake()
    {
        //_anim = GetComponent<Animator>();
        isAnimCheck = TryGetComponent<Animator>(out _anim);

        _openHash = Animator.StringToHash("OpenPopup");
        _closeHash = Animator.StringToHash("ClosePopup"); 
    }

    private void OnEnable()
    {
        if(isAnimCheck)
            _anim.Play(_openHash); 
    }

    public void ClosePopAnim()
    {
        if(isAnimCheck)
            _anim.Play(_closeHash);
    }

}
