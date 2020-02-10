using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    private AudioSource source;

    [Range(0f, 1f)]
    public float volume = .7f;
    [Range(.5f, 1.5f)]
    public float pitch = 1f;

    [Range(0f, .5f)]
    public float RandomVolume = .1f;
    [Range(0f, .5f)]
    public float RandomPitch = .1f;
    public bool loop;

    public void SetSource(AudioSource source)
    {
        this.source = source;
        this.source.clip = clip;
        this.source.loop = loop;
    }

    public void Play()
    {
        source.volume = volume * (1 + Random.Range(-RandomVolume / 2, RandomVolume / 2));
        source.pitch = pitch * (1 + Random.Range(-RandomPitch / 2, RandomPitch / 2));
        source.Play();
    }

    public void Stop() { source.Stop(); }

    public void Pause() { source.Pause(); }
    public void Unpause() { source.UnPause(); }
}

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    Sound[] sounds;

    public static AudioManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _aud = new GameObject("Sound_" + i + "_" + sounds[i].name);
            sounds[i].SetSource(_aud.AddComponent<AudioSource>());
            _aud.transform.SetParent(transform);
        }

        PlaySound("Music");
    }

    public void PlaySound(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
            {
                sounds[i].Play();
                return;
            }
        }
        Debug.LogError("No matching sound for " + name);
    }
    
    public void StopSound(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
            {
                sounds[i].Stop();
                return;
            }
        }
        Debug.LogError("No matching sound for " + name);
    }
    
    public void PauseSound(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
            {
                sounds[i].Pause();
                return;
            }
        }
        Debug.LogError("No matching sound for " + name);
    }
    
    public void UnpauseSound(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
            {
                sounds[i].Unpause();
                return;
            }
        }
        Debug.LogError("No matching sound for " + name);
    }
}
