using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using System;
using UnityEngine.UI;
public class FBInitial : MonoBehaviour
{
    //public static FBInitial facebook;
    public Text deeplink;

    public ScriptableString url;

    private void Awake()
    {
        
    }
    public void viewLinks(string link)
    {
        url.value = link;
        if (!FB.IsInitialized)
        {
            FB.Init(InitCallback, OnHideUnity);
            //FB.LogAppEvent("starting");
            Debug.Log("awake not init");
        }
        else
        {
            FB.ActivateApp();
            FB.Mobile.FetchDeferredAppLinkData(DeepLinkCallback);
            Debug.Log("awake active app");
        }

        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        //FB.LogAppEvent("starting");
        AppsFlyerSDK.AppsFlyer.sendEvent("starting", null);
    }
    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
            Debug.Log("FB is initialized unit call");
            FB.Mobile.FetchDeferredAppLinkData(DeepLinkCallback);
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void FacebookEvent()
    {
        var samplParams = new Dictionary<string, object>();
        samplParams[AppEventParameterName.ContentID] = "sample_step_1";
        samplParams[AppEventParameterName.Description] = "First step, clicking the first button!";
        samplParams[AppEventParameterName.Success] = "1";

        FB.LogAppEvent(
           AppEventName.CompletedTutorial,
           parameters: samplParams
        );
    }

    void DeepLinkCallback(IAppLinkResult result)
    {
        Debug.Log("result " + result.Url);
        if (!String.IsNullOrEmpty(result.Url))
        {
            Debug.Log("scriptable " + url.value);
            url.value = result.Url;
            deeplink.text += result.Url;
            Application.OpenURL(result.Url); print(result.Url);
            Debug.Log(deeplink.text);
        }
    }
}
