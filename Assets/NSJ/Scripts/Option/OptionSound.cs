using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionSound : BaseUI
{
    private Slider _masterSlider => GetUI<Slider>("SoundMasterSlider");
    private Slider _playerSlider => GetUI<Slider>("SoundPlayerSlider");
    private Slider _bgmSlider => GetUI<Slider>("SoundBGMSlider");
    private Slider _sfxSlider => GetUI<Slider>("SoundSFXSlider");

    private void Awake()
    {
        Bind();
        Init();
    }
    private void Start()
    {
        InitSliderValue();
        SubscribesEvent();
    }



    /// <summary>
    /// 마스터 볼륨 조절
    /// </summary>
    /// <param name="volume"></param>
    private void SetMaster(float volume)
    {
        SoundManager.SetVolumeMaster(volume);
    }

    /// <summary>
    /// BGM 볼륨 조절
    /// </summary>
    /// <param name="volume"></param>
    private void SetBGM(float volume)
    {
        SoundManager.SetVolumeBGM(volume);
    }

    private void SetSFX(float volume)
    {
        SoundManager.SetVolumeSFX(volume);
    }

    private void SetPlayerVolume(float volume)
    {
        // TODO : 플레이어 전용 사운드 볼륨 조절
    }

    private void Init()
    {
        
    }

    private void InitSliderValue()
    {
        _masterSlider.value = SoundManager.GetVolumeMaster();
        _bgmSlider.value = SoundManager.GetVolumeBGM();
        _sfxSlider.value = SoundManager.GetVolumeSFX();
    }
    private void SubscribesEvent()
    {
        _masterSlider.onValueChanged.AddListener(SetMaster);
        _bgmSlider.onValueChanged.AddListener(SetBGM);
        _sfxSlider.onValueChanged.AddListener(SetSFX);
    }

}
