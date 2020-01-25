using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    static public SaveManager instance;
    private string path;

    public class Scores
    {
        public string[] name;
        public int[] score;
    }

    public static Scores scores;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
            Destroy(this.gameObject);

        path = Application.dataPath + "/save.json";

        if (scores == null)
        {
            if (File.Exists(path)) //Il file esiste. Carico il salvataggio
            {
                scores = new Scores();
                scores = JsonUtility.FromJson<Scores>(File.ReadAllText(path));
            }
            else
            {   //Il file non esiste. Ne creo uno nuovo e carico i valori
                Debug.LogWarning("Save not found! Creaing a new one...");
                StreamWriter sw = new StreamWriter(path);
                scores = new Scores{
                    score = new int[10],
                    name = new string[10]
                };
                for (int i = 0; i < 10; i++)
                {
                    scores.name[i] = "Empty";
                    scores.score[i] = 0;
                }
                string json = JsonUtility.ToJson(scores, false);
                Debug.Log(json);

                sw.Write(json);
                sw.Flush();
                sw.Close();
            }
        }
    }

    public void WriteScoreToFile()
    {
        File.WriteAllText(path, JsonUtility.ToJson(scores));
    }
}
