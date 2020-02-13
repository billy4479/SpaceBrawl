﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Options : MonoBehaviour
{
    public TMP_InputField nameInput;
    public Slider sfxSlider;
    public Slider musicSlider;

    private SaveManager sm;
    private AudioManager am;

    private void Start()
    {
        sm = SaveManager.instance;
        am = AudioManager.instance;

        sfxSlider.value = sm.settings.VolumeSFX;
        musicSlider.value = sm.settings.VolumeMusic;
        nameInput.text = sm.settings.name;
    }

    public void OnButtonClick()
    {
        sm.settings.VolumeMusic = musicSlider.value;
        sm.settings.VolumeSFX = sfxSlider.value;

        if (nameInput.text.Length > 10 || nameInput.text.Length == 0)
            sm.settings.name = "Player";
        else
            sm.settings.name = nameInput.text;

        sm.WriteChanges();
        SceneManager.LoadScene("Menu");
    }

    public void OnMusicChange()
    {
        sm.settings.VolumeMusic = musicSlider.value;
        am.SetMusicVolume(musicSlider.value);
    }
    public void OnSFXChange()
    {
        sm.settings.VolumeSFX = sfxSlider.value;
    }

}
