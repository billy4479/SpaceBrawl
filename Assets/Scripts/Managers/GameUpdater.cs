using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.RemoteConfig;

public class GameUpdater : MonoBehaviour
{
    struct userAttributes { }
    struct appAttributes { }

    string lastVersion;
    const string androidUrl1 = "http://ftp.spacebrawlreal.altervista.org/versioni/Android/SpaceBrawl.";
    const string androidUrl2 = ".apk";
    const string windowsUrl1 = "http://ftp.spacebrawlreal.altervista.org/versioni/PC/Windows/";
    const string windowsUrl2 = ".exe";
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
                if (Application.platform == RuntimePlatform.WindowsPlayer) url = windowsUrl1 + lastVersion + windowsUrl2;
                else url = androidUrl1 + lastVersion + androidUrl2;
                DialogueSystem.instance.ShowDialogue(1);
            }
        }
    }

    private void OnDestroy()
    {
        ConfigManager.FetchCompleted -= OnFetch;
    }

}
