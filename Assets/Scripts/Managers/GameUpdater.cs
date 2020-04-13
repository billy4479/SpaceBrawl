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

    private void Awake()
    {
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
                //MessageBox?
                Application.OpenURL(url1 + lastVersion + url2);
            }
        }
    }

    private void OnDestroy()
    {
        ConfigManager.FetchCompleted -= OnFetch;
    }

}
