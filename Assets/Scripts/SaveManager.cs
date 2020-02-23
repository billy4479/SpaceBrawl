using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    static public SaveManager instance;
    private string savePath;
    private string oldSavePath;

    #region Classes and Types

    public enum ControllMethod { Finger, Joystick, ToggledJoystick };

    [System.Serializable]
    public class SavedScores
    {
        public string name;
        public int score;
        public string date;
    }

    [System.Serializable]
    public class Settings
    {
        public float VolumeSFX;
        public float VolumeMusic;
        public string name;
        public ControllMethod controllMethod;
    }

    [System.Serializable]
    private class SaveCollection
    {
        public SavedScores[] savedScores;
        public Settings settings;
    }

    #endregion Classes and Types

    private SaveCollection saveCollection;
    public SavedScores[] scores;
    public Settings settings;

    private void Awake()
    {
        #region Singletone

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
            Destroy(this.gameObject);

        #endregion Singletone

        #region Select Path

        if (Application.platform == RuntimePlatform.Android)
        {
            savePath = Application.persistentDataPath + "/save.json";
            oldSavePath = Application.persistentDataPath + "/options.json";
        }
        else
        {
            savePath = Application.dataPath + "/save.json";
            oldSavePath = Application.persistentDataPath + "/options.json";
        }

        #endregion Select Path

        if (File.Exists(savePath))
        {
            saveCollection = JsonUtility.FromJson<SaveCollection>(File.ReadAllText(savePath));
            if (saveCollection != null)
            {
                scores = saveCollection.savedScores;
                settings = saveCollection.settings;
            }
            else
            {
                Debug.LogWarning("Il file esiste ma non viene letto correttamente...");
                File.Delete(savePath);
                if (File.Exists(oldSavePath))
                    File.Delete(oldSavePath);
                CreateSaveFile();
            }
        }
        else
        {
            Debug.LogWarning("Save not found! Creaing a new one...");
            CreateSaveFile();
        }
    }

    private void CreateSaveFile()
    {
        using (StreamWriter saveStream = new StreamWriter(savePath))
        {
            scores = new SavedScores[10];
            for (int i = 0; i < 10; i++)
            {
                scores[i] = new SavedScores();
                scores[i].name = "Empty";
                scores[i].score = 0;
                scores[i].date = "None";
            }

            settings = new Settings
            {
                name = "Player",
                VolumeMusic = 100f,
                VolumeSFX = 100f
            };

            saveCollection = new SaveCollection
            {
                savedScores = scores,
                settings = settings
            };

            string json = JsonUtility.ToJson(saveCollection, true);

            saveStream.Write(json);
            saveStream.Flush();
            saveStream.Close();
        }
    }
    public void WriteChanges()
    {
        saveCollection = new SaveCollection
        {
            savedScores = scores,
            settings = settings
        };
        File.WriteAllText(savePath, JsonUtility.ToJson(saveCollection, true));
    }
}