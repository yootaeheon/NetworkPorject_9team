using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : BaseMission
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioMixer _audioMixer; 
    [SerializeField] private Slider _sfxSlider;

    private AudioSource _sfxSource;
    private AudioSource _bgmSource;

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
        _sfxSlider.onValueChanged.AddListener(SetVolumeSFX);
    }

    //SFX 셋팅 진행

    public void SetVolumeSFX(float volume)
    {
        _audioMixer.SetFloat("SFX", volume * 20f); 
    }

    public void SetVolumeBGM(float volume)
    {
        _audioMixer.SetFloat("BGM", volume * 20f);
    }


    public void BGMPlay(AudioClip clip)
    {
        _bgmSource.clip = clip;
        _bgmSource.Play();
    }

    public void SFXPlay(AudioClip clip)
    {
        _sfxSource.clip = clip;
        _sfxSource.Play();
    }
}
