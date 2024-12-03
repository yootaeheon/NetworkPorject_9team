using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SoundManager : BaseMission
{
    public static SoundManager Instance { get; private set; }

    [Header("Sound Setting UI")]
    [SerializeField] private AudioMixer _audioMixer;
    private static AudioMixer s_audioMixer { get { return Instance._audioMixer; } }
    //[SerializeField] private Slider _masterSlider;
    //[SerializeField] private Slider _sfxSlider;
    //[SerializeField] private Slider _bgmSlider;
    private static AudioSource s_sfx {  get { return Instance._sfxSource; } }
    private static AudioSource s_bgm { get { return Instance._bgmSource; } }
    private static AudioSource s_loopSfx { get { return Instance._loopSfxSource; } }
    private static AudioSource s_master { get { return Instance._masterSource; } }

    private AudioSource _sfxSource;
    private AudioSource _bgmSource;
    private AudioSource _loopSfxSource;
    private AudioSource _masterSource;

    private void Start()
    {
        SetSingleton();
        SetObject();
    }

    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void SetObject()
    {
        //if (_bgmSlider == null || _sfxSlider == null || _masterSlider == null) return;


        _sfxSource = GetMissionComponent<AudioSource>("SFX");
        _bgmSource = GetMissionComponent<AudioSource>("BGM");
        _masterSource = GetMissionComponent<AudioSource>("Master");
        _loopSfxSource = GetMissionComponent<AudioSource>("LoopSFX");
        //_sfxSlider.onValueChanged.AddListener(SetVolumeSFX);
        //_bgmSlider.onValueChanged.AddListener(SetVolumeBGM);
        //_masterSlider.onValueChanged.AddListener(SetVolumeMaster);
    }

    /// <summary>
    /// SFX 볼륨 조절
    /// </summary>
    /// <param name="volume"></param>
    public static void SetVolumeSFX(float volume)
    {
        s_audioMixer.SetFloat("SFX", volume * 20f); 
        
    }

    public static float GetVolumeSFX()
    {
        s_audioMixer.GetFloat("SFX", out float volume);
        return volume;
    }
    /// <summary>
    /// BGM 볼륨 조절
    /// </summary>
    /// <param name="volume"></param>
    public static void SetVolumeBGM(float volume)
    {
        s_audioMixer.SetFloat("BGM", volume * 20f);
    }

    /// <summary>
    /// Master 볼륨 조절
    /// </summary>
    /// <param name="volume"></param>
    public static void SetVolumeMaster(float volume)
    {
        s_audioMixer.SetFloat("MASTER", volume * 20f);
    }


    /// <summary>
    /// BGM 교체 후 재생
    /// </summary>
    /// <param name="clip"></param>
    public static  void BGMPlay(AudioClip clip)
    {
        s_bgm.clip = clip;
        s_bgm.Play();
    }

    /// <summary>
    /// SFX 교체 후 재생 
    /// </summary>
    /// <param name="clip"></param>
    public static void SFXPlay(AudioClip clip)
    {
        s_sfx.clip = clip;
        s_sfx.Play(); 
    }

    public static void LoopSFXPlay(AudioClip clip)
    {
        s_loopSfx.clip = clip;
        s_loopSfx.Play();
    }

}
