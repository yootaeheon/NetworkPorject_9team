using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPun
{
    [Header("사보타지 능력 사용 관련 변수")]
    [SerializeField] private AudioClip _sirenClip;
    [SerializeField] private AudioClip _bgmClip;
    [SerializeField] private Image _sirenPanelImage;
    [SerializeField] private Button _fireBtn;
    [SerializeField] private Button _lifeBtn;
    [SerializeField] private Button _breakerBtn;

    [Header("글로벌 미션 Task Text")]
    [SerializeField] private TextMeshProUGUI _globalTaskText;

    private Light2D _light2D;
    private Coroutine _globalTaskCo;

    [HideInInspector] public bool GlobalMissionState;
    [HideInInspector] public bool TheWin;
    private string _globalTaskName;
    private float CountDown;

    private bool _globalState;
    public bool GlobalState
    {
        get => _globalState;
        set
        {
            _globalState = GlobalMissionState;

            if (_globalState)
            {
                SetTaskText();
            }
            else
            {
                if (_globalTaskCo != null)
                {
                    StopCoroutine(_globalTaskCo);
                    _globalTaskText.transform.parent.gameObject.SetActive(false);
                    _globalTaskCo = null;
                }
            }
        }
    }

    public static GameManager Instance { get; private set; }

    [Header("미션 게이지 슬라이더")]
    [SerializeField] public Slider _missionScoreSlider;

    private int _totalMissionScore = 30;
    private int _clearMissionScore = 0;

    //글로벌 미션 팝업창 종료 조건 변수
    [HideInInspector] public bool GlobalMissionClear = true;

    //불지르기 미션 클리어 조건
    [HideInInspector] public bool FirstGlobalFire;
    [HideInInspector] public bool SecondGlobalFire;
    [HideInInspector] public int GlobalFireCount = 2;
    [HideInInspector] public SabotageType CurAbility;
    private SabotageType UseAbility;

    private bool _sabotageFire;
    public bool SabotageFire
    {
        get { return _sabotageFire; }

        set
        {
            _sabotageFire = value;
            _fireBtn.interactable = _sabotageFire;
        }
    }

    private bool _sabotageLife;
    public bool SabotageLife
    {
        get
        {
            return _sabotageLife;
        }
        set
        {
            _sabotageLife = value;
            _lifeBtn.interactable = _sabotageLife;
        }
    }


    private bool _sabotageBreaker;
    public bool SabotageBreaker
    {
        get
        {
            return _sabotageBreaker;
        }
        set
        {
            _sabotageBreaker = value;
            _breakerBtn.interactable = _sabotageBreaker;
        }
    }

    private void Awake()
    {
        SetSingleton();
        _fireBtn.onClick.AddListener(DuckFireAbilityInvoke);
        _lifeBtn.onClick.AddListener(DuckLifeAbilityInvoke);
        _breakerBtn.onClick.AddListener(DuckBreakerAbilityInvoke);
    }
     
    private void OnEnable()
    {
        TheWin = false;
    }

    private void Start()
    {
        _light2D = Camera.main.transform.GetChild(0).GetComponent<Light2D>();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 글로벌 미션 Text 코루틴
    /// </summary>
    private void SetTaskText()
    {
        _globalTaskText.transform.parent.gameObject.SetActive(true);
        _globalTaskCo = StartCoroutine(TaskTextCoroutine());
    }

    /// <summary>
    /// 글로벌 미션 Text 변화 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator TaskTextCoroutine()
    {
        float _elapsedTime = 0f;
        float _limitTime = 60f;
        CountDown = _limitTime;

        while (_elapsedTime < _limitTime)
        {
            _elapsedTime += Time.deltaTime;
            CountDown -= Time.deltaTime;
            _globalTaskText.text = string.Concat(_globalTaskName + " [" + ((int)CountDown).ToString()) + "]";

            yield return null;
        }

        TheWin = CountDown <= 0f ? true : false;

        yield break;
    }

    private void DuckFireAbilityInvoke()
    {
        UseAbility = SabotageType.Fire;
        photonView.RPC(nameof(DuckAbilityRPC), RpcTarget.AllViaServer, true, UseAbility);
    }

    private void DuckLifeAbilityInvoke()
    {
        UseAbility = SabotageType.OxygenBlock;
        photonView.RPC(nameof(DuckAbilityRPC), RpcTarget.AllViaServer, true, UseAbility);
    }

    private void DuckBreakerAbilityInvoke()
    {
        UseAbility = SabotageType.BlackOut;
        photonView.RPC(nameof(DuckAbilityRPC), RpcTarget.AllViaServer, true, UseAbility);
    }

    /// <summary>
    /// Duck 유저 사보타지 능력 기능 동기화
    /// </summary>
    public void DuckAbilityInvoke()
    {
        photonView.RPC(nameof(DuckAbilityRPC), RpcTarget.AllViaServer, true);
    }

    [PunRPC]
    public void DuckAbilityRPC(bool value, SabotageType type)
    {
        CurAbility = type;

        switch (type)
        {
            case SabotageType.Fire:
                SabotageFire = false;
                _globalTaskName = "화재 진압하기";
                break;

            case SabotageType.BlackOut:
                SabotageBreaker = false;
                _globalTaskName = "전등 고치기";
                _light2D.pointLightInnerRadius = 2.5f;
                _light2D.pointLightOuterRadius = 3f;
                break;

            case SabotageType.OxygenBlock:
                SabotageLife = false;
                _globalTaskName = "생명 유지 장치 재설정하기";
                break;
        }

        GlobalMissionClear = false;
        GlobalMissionState = value;
        GlobalState = GlobalMissionState;
        SoundManager.BGMPlay(_sirenClip);
        StartCoroutine(SirenCoroutine());
    }

    /// <summary>
    /// 사이렌 점멸 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator SirenCoroutine()
    {
        float value = 0f;

        while (!GlobalMissionClear)
        {
            if (!_sirenPanelImage.gameObject.activeSelf)
            {
                _sirenPanelImage.gameObject.SetActive(true);
            }

            Color alpha = _sirenPanelImage.color;
            value = alpha.a;

            if (alpha.a > 0f)
            {
                while (true)
                {
                    value -= Time.deltaTime * 0.5f;

                    _sirenPanelImage.color = new Color(1f, 0, 0, value);

                    if (_sirenPanelImage.color.a < 0f) break;

                    yield return null;
                }
            }


            if (alpha.a < 0f)
            {

                while (true)
                {
                    value += Time.deltaTime * 0.5f;

                    _sirenPanelImage.color = new Color(1f, 0, 0, value);

                    if (_sirenPanelImage.color.a > 0.43f) break;

                    yield return null;
                }
            }

        }

        UseAbility = SabotageType.None;
        yield break;
    }

    //------ 거위들이 깨야할 미션

    /// <summary>
    /// 각 클라이언트에서 미션 클리어 시마다 점수 증가
    /// </summary>
    public void AddMissionScore()
    {
        photonView.RPC(nameof(MissionTotalScore), RpcTarget.AllViaServer, 1);
    }

    /// <summary>
    /// 점수 동기화
    /// </summary>
    /// <param name="score"></param>
    [PunRPC]
    public void MissionTotalScore(int score)
    {
        _clearMissionScore += score;
        _missionScoreSlider.value = (float)_clearMissionScore / (float)_totalMissionScore;
    }

    /// <summary>
    /// 불끄기 미션 개수
    /// </summary>
    public void GlobalFire()
    {
        photonView.RPC(nameof(FireCountSync), RpcTarget.AllViaServer, 1);
    }

    public void FirstFire()
    {
        photonView.RPC(nameof(FirstFireRPC), RpcTarget.AllViaServer, true);
    }

    [PunRPC]
    public void FirstFireRPC(bool value)
    {
        FirstGlobalFire = value;
    }

    public void SecondFire()
    {
        photonView.RPC(nameof(SecondFireRPC), RpcTarget.AllViaServer, true);
    }

    [PunRPC]
    public void SecondFireRPC(bool value)
    {
        SecondGlobalFire = value;
    }

    /// <summary>
    /// 불끄기 미션 카운트 동기화
    /// </summary>
    /// <param name="value"></param>
    [PunRPC]
    public void FireCountSync(int value)
    {
        GlobalFireCount -= value;

        if (GlobalFireCount < 1)
        {
            photonView.RPC(nameof(GlobalMissionRPC), RpcTarget.AllViaServer, true);
        }
    }

    /// <summary>
    /// 각 클라이언트에서 사보타지 능력 사용한 미션 클리어 여부
    /// </summary>
    public void CompleteGlobalMission()
    {
        photonView.RPC(nameof(GlobalMissionRPC), RpcTarget.AllViaServer, true);
    }

    /// <summary>
    /// 사보타지 능력 미션 클리어 여부 동기화
    /// </summary>
    /// <param name="value"></param>
    [PunRPC]
    public void GlobalMissionRPC(bool value)
    {
        GlobalMissionClear = value;
        GlobalMissionState = false;
        GlobalState = GlobalMissionState;
        _sirenPanelImage.gameObject.SetActive(false);
        SoundManager.BGMPlay(_bgmClip);
        
        if(CurAbility.Equals(SabotageType.BlackOut))
        {
            _light2D.pointLightInnerRadius = 0f;
            _light2D.pointLightOuterRadius = 26.7f;
        }
    }

}
