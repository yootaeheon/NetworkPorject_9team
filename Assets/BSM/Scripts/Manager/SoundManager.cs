using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : BaseMission
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private SoundData _data;
    public static SoundData Data { get { return Instance._data; } }

    [Header("Sound Setting UI")]
    [SerializeField] private AudioMixer _audioMixer;
    public static AudioMixer AudioMixer { get { return Instance._audioMixer; } }
    public static AudioSource SFX { get { return Instance._sfxSource; } }
    public static AudioSource BGM { get { return Instance._bgmSource; } }
    public static AudioSource LoopSFX { get { return Instance._loopSfxSource; } }
    public static AudioSource Master { get { return Instance._masterSource; } }

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
    }

    /// <summary>
    /// SFX 볼륨 조절
    /// </summary>
    /// <param name="volume"></param>
    public static void SetVolumeSFX(float volume)
    {
        AudioMixer.SetFloat("SFX", volume * 20f);
    }

    /// <summary>
    /// SFX 볼륨값 가져오기
    /// </summary>
    /// <returns></returns>
    public static float GetVolumeSFX()
    {
        AudioMixer.GetFloat("SFX", out float volume);
        return volume / 20f;
    }
    /// <summary>
    /// BGM 볼륨 조절
    /// </summary>
    /// <param name="volume"></param>
    public static void SetVolumeBGM(float volume)
    {
        AudioMixer.SetFloat("BGM", volume * 20f);
    }
    /// <summary>
    /// BGM 볼륨값 가져오기
    /// </summary>
    /// <param name="volume"></param>
    public static float GetVolumeBGM()
    {
        AudioMixer.GetFloat("BGM", out float volume);
        return volume / 20f;
    }

    /// <summary>
    /// Master 볼륨 조절
    /// </summary>
    /// <param name="volume"></param>
    public static void SetVolumeMaster(float volume)
    {
        AudioMixer.SetFloat("MASTER", volume * 20f);
    }

    /// <summary>
    /// Master 볼륨값 가져오기
    /// </summary>
    /// <returns></returns>
    public static float GetVolumeMaster()
    {
        AudioMixer.GetFloat("MASTER", out float volume);
        return volume / 20f;
    }

    /// <summary>
    /// BGM 교체 후 재생
    /// </summary>
    /// <param name="clip"></param>
    public static void BGMPlay(AudioClip clip)
    {
        if (clip == null)
            return;

        BGM.clip = clip;
        BGM.Play();
    }

    /// <summary>
    /// SFX 교체 후 재생 
    /// </summary>
    /// <param name="clip"></param>
    public static void SFXPlay(AudioClip clip)
    {
        if (clip == null)
            return;

        SFX.clip = clip;
        SFX.PlayOneShot(clip);
    }

    public static void LoopSFXPlay(AudioClip clip)
    {
        if (clip == null)
            return;

        LoopSFX.clip = clip;
        LoopSFX.PlayOneShot(clip);
    }

}
