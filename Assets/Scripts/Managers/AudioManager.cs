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
    public bool isMusic;

    public void SetSource(AudioSource source)
    {
        this.source = source;
        this.source.clip = clip;
        this.source.loop = loop;
    }

    public void Play(float volume)
    {
        source.volume = this.volume * (1 + Random.Range(-RandomVolume / 2, RandomVolume / 2)) * (volume / 100f);
        source.pitch = pitch * (1 + Random.Range(-RandomPitch / 2, RandomPitch / 2));
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }

    public void Pause()
    {
        source.Pause();
    }

    public void Unpause()
    {
        source.UnPause();
    }

    public void SetVolume(float volume)
    { this.source.volume = this.volume * (1 + Random.Range(-RandomVolume / 2, RandomVolume / 2)) * (volume / 100f); }
}

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private Sound[] sounds;

    private float musicVolume;
    private float sfxVolume;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _aud = new GameObject("Sound_" + i + "_" + sounds[i].name);
            sounds[i].SetSource(_aud.AddComponent<AudioSource>());
            _aud.transform.SetParent(transform);
        }

        musicVolume = SaveManager.instance.settings.VolumeMusic;
        sfxVolume = SaveManager.instance.settings.VolumeSFX;

        PlaySound("Music");
    }

    public void SetMusicVolume(float volume)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == "Music")
            {
                sounds[i].SetVolume(volume);
                return;
            }
        }
        Debug.LogError("No matching sound for " + name);
    }

    public void PlaySound(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
            {
                float volume;
                if (sounds[i].isMusic)
                    volume = musicVolume;
                else
                    volume = sfxVolume;
                sounds[i].Play(volume);
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