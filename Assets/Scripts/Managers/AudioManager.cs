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

    private float overrideVolume;

    public void SetSource(AudioSource source)
    {
        this.source = source;
        this.source.clip = clip;
        this.source.loop = loop;
    }

    public void Play()
    {
        source.volume = volume * (1 + Random.Range(-RandomVolume / 2, RandomVolume / 2)) * overrideVolume;
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
    {
        overrideVolume = volume;
        source.volume = this.volume * overrideVolume * (1 + Random.Range(-RandomVolume / 2, RandomVolume / 2));
    }
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

        SetVolume(SaveManager.instance.settings.VolumeMusic, true);
        SetVolume(SaveManager.instance.settings.VolumeSFX, false);


        PlaySound("Music");
    }

    public void SetVolume(float volume, bool isMusic)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].isMusic == isMusic)
            {
                sounds[i].SetVolume(volume * 0.01f);
            }
        }
    }

    public void PlaySound(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
            {
                //if (sounds[i].isMusic) sounds[i].SetVolume(SaveManager.instance.settings.VolumeMusic);
                //if (!sounds[i].isMusic) sounds[i].SetVolume(SaveManager.instance.settings.VolumeSFX);
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