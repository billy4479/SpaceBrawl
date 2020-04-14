using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RemoteConfig;

public class GameUpdater : MonoBehaviour
{
    struct userAttributes { }
    struct appAttributes { }

    string lastVersion;
    const string url1 = "http://ftp.spacebrawlreal.altervista.org/versioni/Android/SpaceBrawl.";
    const string url2 = ".apk";
    string url;
    public static GameUpdater instance { get; private set; }
    public string GetURL()
    {
        return url;
    }

    private void Awake()
    {
        instance = this;
        ConfigManager.FetchCompleted += OnFetch;
        ConfigManager.FetchConfigs(new userAttributes(), new appAttributes());
    }

    private void OnFetch(ConfigResponse response)
    {
        if (response.requestOrigin == ConfigOrigin.Remote)
        {
            lastVersion = ConfigManager.appConfig.GetString("lastVersion");
            if (lastVersion != null && lastVersion != Application.version)
            {
                url = url1 + lastVersion + url2;
                DialogueSystem.instance.ShowDialogue(1);
            }
        }
    }

    private void OnDestroy()
    {
        ConfigManager.FetchCompleted -= OnFetch;
    }

}
