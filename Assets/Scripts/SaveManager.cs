using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    static public SaveManager instance;
    private string savePath;
    private string optionPath;

    public class SavedScores
    {
        public string[] name;
        public int[] score;
        public string[] date;
    }

    public class Settings
    {
        public float VolumeSFX;
        public float VolumeMusic;
        public string name;
    }

    public SavedScores scores;
    public Settings settings;

    void Awake()
    {
        #region Create Instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
            Destroy(this.gameObject);
        #endregion

        if (Application.platform == RuntimePlatform.Android)
        {
            savePath = Application.persistentDataPath + "/save.json";
            optionPath = Application.persistentDataPath + "/options.json";
        }
        else
        {
            savePath = Application.dataPath + "/save.json";
            optionPath = Application.dataPath + "/options.json";
        }

        if (File.Exists(savePath)) //Il file esiste. Carico il salvataggio
        {
            scores = JsonUtility.FromJson<SavedScores>(File.ReadAllText(savePath));
        }
        else
        {   //Il file non esiste. Ne creo uno nuovo e carico i valori
            Debug.LogWarning("Save not found! Creaing a new one...");

            using (StreamWriter saveStream = new StreamWriter(savePath))
            {
                scores = new SavedScores
                {
                    score = new int[10],
                    name = new string[10],
                    date = new string[10]
                };
                for (int i = 0; i < 10; i++)
                {
                    scores.name[i] = "Empty";
                    scores.score[i] = 0;
                    scores.date[i] = "None";
                }
                string json = JsonUtility.ToJson(scores, false);

                saveStream.Write(json);
                saveStream.Flush();
                saveStream.Close();
            }
        }
        if (File.Exists(optionPath))
        {
            settings = JsonUtility.FromJson<Settings>(File.ReadAllText(optionPath));
        }
        else
        {
            Debug.LogWarning("Options not found! Creaing a new one...");
            using (StreamWriter optionsStream = new StreamWriter(optionPath))
            {
                settings = new Settings
                {
                    name = "Player",
                    VolumeMusic = 100f,
                    VolumeSFX = 100f
                };
                string json = JsonUtility.ToJson(settings, false);
                optionsStream.Write(json);
                optionsStream.Flush();
                optionsStream.Close();
            }
        }

    }

    public void WriteChanges()
    {
        File.WriteAllText(savePath, JsonUtility.ToJson(scores));
        File.WriteAllText(optionPath, JsonUtility.ToJson(settings));
    }
}
