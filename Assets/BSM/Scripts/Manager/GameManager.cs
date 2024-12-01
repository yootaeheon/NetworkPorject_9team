using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    private Coroutine _globalTaskCo;
    [SerializeField] private TextMeshProUGUI _globalTaskText;
    public bool GlobalMissionState;
    private string _globalTaskName;

    private bool _globalState;
    public bool GlobalState
    {
        get => _globalState;
        set
        {
            _globalState = GlobalMissionState;
            Debug.Log("값이 바뀌나");

            if (_globalState)
            {
                SetTaskText();
            }
            else
            {
                if (_globalTaskCo != null)
                {
                    StopCoroutine(_globalTaskCo);
                    _globalTaskText.gameObject.SetActive(false);
                    _globalTaskCo = null;
                }
            }
        }
    }

    private void SetTaskText()
    {
        _globalTaskText.gameObject.SetActive(true);
        _globalTaskCo = StartCoroutine(TaskTextCoroutine());
    }

    private IEnumerator TaskTextCoroutine()
    {
        float _elapsedTime = 0f;
        float _limitTime = 60f;
        float countDown = _limitTime;

        while (_elapsedTime < _limitTime)
        {
            _elapsedTime += Time.deltaTime;
            countDown -= Time.deltaTime;
            _globalTaskText.text = string.Concat(_globalTaskName + ((int)countDown).ToString());

            yield return null;
        }

        yield break;
    }


    public static GameManager Instance { get; private set; }

    [SerializeField] public Slider _missionScoreSlider;

    private int _totalMissionScore = 30;
    private int _clearMissionScore = 0;


    [Header("글로벌 미션 팝업창 종료 조건 변수")]
    public bool GlobalMissionClear = true;

    [Header("불 지르기 미션 클리어 조건")]
    public bool FirstGlobalFire;
    public bool SecondGlobalFire;
    public int GlobalFireCount = 2;


    private SabotageType UseAbility;
    public SabotageType CurAbility;


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

    private void Update()
    {
        Debug.Log($"현재 사용 능력 :{UseAbility}");
        Debug.Log($"이름 :{_globalTaskName}");
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
                break;

            case SabotageType.OxygenBlock:
                SabotageLife = false;
                _globalTaskName = "생명 유지 장치 재설정하기";
                break;
        }
        Debug.Log($"발동된 능력 :{type}");

        GlobalMissionClear = false;
        GlobalMissionState = value;
        GlobalState = GlobalMissionState;
        SoundManager.Instance.BGMPlay(_sirenClip);
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
        SoundManager.Instance.BGMPlay(_bgmClip);
    }

}
