using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioConfig", menuName = "Configs/AudioConfig", order = 1)]
public class AudioConfig : BaseConfig
{
    public List<Sound> audiosInGame;
}

[System.Serializable]
public class Sound
{
    public string audioName;
    public AudioClip audioClip;
    [Range(0,1)]
    public float volume = 1;
    [Range(0.1f, 3f)]
    public float pitch = 1;
    [HideInInspector]
    public AudioSource source;
    public bool loop = false;
}