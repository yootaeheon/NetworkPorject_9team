using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SoundManager : BaseMission
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _bgmSlider;


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
        _sfxSource = GetMissionComponent<AudioSource>("SFX");
        _bgmSource = GetMissionComponent<AudioSource>("BGM");
        _masterSource = GetMissionComponent<AudioSource>("Master");
        _loopSfxSource = GetMissionComponent<AudioSource>("LoopSFX");
        _sfxSlider.onValueChanged.AddListener(SetVolumeSFX);
        _bgmSlider.onValueChanged.AddListener(SetVolumeBGM);
        //_masterSlider.onValueChanged.AddListener(SetVolumeMaster);
    }

    /// <summary>
    /// SFX 볼륨 조절
    /// </summary>
    /// <param name="volume"></param>
    public void SetVolumeSFX(float volume)
    {
        _audioMixer.SetFloat("SFX", volume * 20f); 
    }

    /// <summary>
    /// BGM 볼륨 조절
    /// </summary>
    /// <param name="volume"></param>
    public void SetVolumeBGM(float volume)
    {
        _audioMixer.SetFloat("BGM", volume * 20f);
    }

    /// <summary>
    /// Master 볼륨 조절
    /// </summary>
    /// <param name="volume"></param>
    public void SetVolumeMaster(float volume)
    {
        _audioMixer.SetFloat("MASTER", volume * 20f);
    }


    /// <summary>
    /// BGM 교체 후 재생
    /// </summary>
    /// <param name="clip"></param>
    public void BGMPlay(AudioClip clip)
    {
        _bgmSource.clip = clip;
        _bgmSource.Play();
    }

    /// <summary>
    /// SFX 교체 후 재생 
    /// </summary>
    /// <param name="clip"></param>
    public void SFXPlay(AudioClip clip)
    {
        _sfxSource.clip = clip;
        _sfxSource.Play(); 
    }

    public void LoopSFXPlay(AudioClip clip)
    {
        _loopSfxSource.clip = clip;
        _loopSfxSource.Play();
    }

}
