using AppsFlyerSDK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AFInits : MonoBehaviour
{
    public Text installs;
    public Text attributes;
    public AppsFlyerObjectScript appsflyer;

    private void Start()
    {
        installs.text += "device ID " + AppsFlyer.getAppsFlyerId();
        appsflyer.OnConversionDataSuccess += ConversionDataSuccessHandler;
    }

    private void ConversionDataSuccessHandler(Dictionary<string, object> dictionary)
    {
        attributes.text += string.Join(", ", dictionary.Select(kv => kv.Key + "=" + kv.Value).ToArray());
    }
}
