using UnityEngine.Audio;
using System;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public Sound[] sounds;

    public static Audio instance;

    public float chosenVolume = 1;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        Init();
    }

    public void Init()
    {
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume * chosenVolume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    public void VolumeUpdate()
    {
        foreach (Sound sound in sounds)
        {
            sound.source.volume = sound.volume * chosenVolume;
        }
    }

    public void Play(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);

        if (sound == null)
        {
            Debug.LogWarning(name + " sound has not been found!");
            return;
        }
        Debug.Log("Playing " + sound.name);
        sound.source.Play();
    }

    public void Stop(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);

        if (sound == null)
        {
            Debug.LogWarning(name + " sound has not been found!");
            return;
        }
        Debug.Log("Stopping " + sound.name);
        sound.source.Stop();
    }

    public void StopAll()
    {
        foreach (Sound sound in sounds)
        {
            sound.source.Stop();
        }
    }
}