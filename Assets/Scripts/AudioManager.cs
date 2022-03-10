using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AudioManager : SerializeSingleton<AudioManager>
{
    [SerializeField] private AudioConfig audioConfig;

    private void Awake()
    {
        CreateAudioSources();
    }

    //Creates audio sources to play audio
    private void CreateAudioSources()
    {
        foreach (var sound in audioConfig.audiosInGame)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.audioClip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;    
        }
    }

    public void PlaySound(string audioName, bool excludeAllSounds = false, List<string> soundsToExclude = null)
    {
        Sound soundToPlay = audioConfig.audiosInGame.Where(t => t.audioName == audioName).FirstOrDefault();

        if(soundToPlay == null)
        {
            Debug.LogError("Sound Not Found");
            return;
        }

        if (excludeAllSounds)
        {
            StartCoroutine(ResumePlayingStoppedAudios(soundToPlay, StopAllAudios()));
        }

        if(soundsToExclude != null)
        {
            StartCoroutine(ResumePlayingStoppedAudios(soundToPlay, StopAudios(soundsToExclude)));
        }

        soundToPlay.source.Play();
    }

    private IEnumerator ResumePlayingStoppedAudios(Sound source, List<string> audiosToResume)
    {
        yield return new WaitForSeconds(source.audioClip.length + 1);
        foreach (var song in audiosToResume)
        {
            PlaySound(song);
        }
    }

    public List<string> StopAllAudios()
    {
        List<string> audiosStoppedPlaying = new List<string>();
        foreach (var item in audioConfig.audiosInGame)
        {
            if (item.source.isPlaying)
            {
                item.source.Stop();
                audiosStoppedPlaying.Add(item.audioName);
            }
        }

        return audiosStoppedPlaying;
    }

    public List<string> StopAudios(List<string> soundsToStop)
    {
        List<string> audiosStoppedPlaying = new List<string>();
        foreach (var item in audioConfig.audiosInGame)
        {
            if (soundsToStop.Contains(item.audioName))
            {
                item.source.Stop();
                audiosStoppedPlaying.Add(item.audioName);
            }
        }

        return audiosStoppedPlaying;
    }

    public List<string> StopAllAudios(List<string> audioToExclude)
    {
        List<string> audiosStoppedPlaying = new List<string>();
        foreach (var item in audioConfig.audiosInGame)
        {
            if (!audioToExclude.Contains(item.audioName))
            {
                item.source.Stop();
                audiosStoppedPlaying.Add(item.audioName);
            }
        }

        return audiosStoppedPlaying;
    }

    public List<string> StopAllAudios(string audioToExclude)
    {
        return StopAllAudios(new List<string>() { audioToExclude });
    }
}
