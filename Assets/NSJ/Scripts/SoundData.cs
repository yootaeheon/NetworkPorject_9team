using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound")]
public class SoundData : ScriptableObject
{
    [System.Serializable]
    private struct Sound
    {
        public AudioClip ButtonClick;
        public AudioClip ButtonOff;
        public AudioClip Report;
        public AudioClip EmergencyCall;
        public AudioClip Dead;
        public AudioClip Kill;
        public AudioClip DuckIntro;
        public AudioClip GooseIntro;
        public AudioClip Vote;
        public AudioClip GooseStep;
    }
    [SerializeField] private Sound _sound;

    public AudioClip ButtonClick { get { return _sound.ButtonClick; } }
    public AudioClip Buttonoff { get { return _sound.ButtonOff; } }
    public AudioClip Report { get { return _sound.Report; } }
    public AudioClip Dead {  get { return _sound.Dead; } }
    public AudioClip Kill {  get { return _sound.Kill; } }
    public AudioClip EmergencyCall { get { return _sound.EmergencyCall; } }
    public AudioClip DuckIntro { get { return _sound.DuckIntro; } }
    public AudioClip GooseIntro { get {return _sound.GooseIntro; } }
    public AudioClip Vote { get { return _sound.Vote; } }
    public AudioClip GooseStep { get { return _sound.GooseStep; } }
}
