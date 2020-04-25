using System.IO;
using UnityEngine;
using BillyUtils;

public class SaveManager : MonoBehaviour
{
    static public SaveManager instance;
    private string savePath;
    private string oldSavePath;

    #region Classes and Types

    public enum ControllMethod { Finger, Joystick, ToggledJoystick, KeyBoard };

    [System.Serializable]
    public class SavedScores
    {
        public string name;
        public int score;
        public string date;

        public SavedScores()
        {
            name = "Empty";
            score = 0;
            date = "None";
        }
    }

    [System.Serializable]
    public class Settings
    {
        public float VolumeSFX;
        public float VolumeMusic;
        public string name;
        public ControllMethod controllMethod;
        public int selectedCharacter;
        public Settings()
        {
            selectedCharacter = 0;
            VolumeMusic = 100f;
            VolumeSFX = 100f;
            name = "Player";
            controllMethod = ControllMethod.Finger;
        }
    }

    [System.Serializable]
    private class SaveCollection
    {
        public SavedScores[] savedScores;
        public Settings settings;
        public SaveCollection()
        {
            savedScores = new SavedScores[10];
            for (int i = 0; i < 10; i++)
                savedScores[i] = new SavedScores();
            settings = new Settings();
        }
    }

    #endregion Classes and Types

    private SaveCollection saveCollection;
    public SavedScores[] scores;
    public Settings settings;
    private const string passphrase = "sP@c3Br@wL";

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
            savePath = Application.persistentDataPath + "/save.dat";
            oldSavePath = Application.persistentDataPath + "/save.json";
        }
        else
        {
            savePath = Application.dataPath + "/save.dat";
            oldSavePath = Application.persistentDataPath + "/save.json";
        }

        #endregion Select Path

        ReadFile();
    }

    public void WriteChanges()
    {
        if (saveCollection == null)
        {
            saveCollection = new SaveCollection();
            scores = saveCollection.savedScores;
            settings = saveCollection.settings;
        }
        else
        {
            saveCollection.savedScores = scores;
            saveCollection.settings = settings;
        }
        using (StreamWriter saveStream = new StreamWriter(File.Open(savePath, FileMode.OpenOrCreate)))
        {
            string json = JsonUtility.ToJson(saveCollection, false);
            string encrypted = StringEncryptor.Encrypt(json, passphrase);

            saveStream.Write(encrypted);
        }
    }

    public GameInfo GetGameInfo()
    {
        return new GameInfo() { selectedPlayerID = settings.selectedCharacter};
    }

    public void ReadFile()
    {
        if (File.Exists(oldSavePath))
            File.Delete(oldSavePath);
        if (File.Exists(savePath))
        {
            string raw = File.ReadAllText(savePath);
            saveCollection = JsonUtility.FromJson<SaveCollection>(StringEncryptor.Decrypt(raw, passphrase));
            if (saveCollection != null)
            {
                scores = saveCollection.savedScores;
                settings = saveCollection.settings;
            }
            else
                WriteChanges();
        }
        else
            WriteChanges();
    }
}