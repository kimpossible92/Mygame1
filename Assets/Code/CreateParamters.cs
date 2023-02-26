using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class CreateParamters : MonoBehaviour
{
    public UnityEngine.UI.Button button;
    public bool shouldAnonymizeUser = false;
    public UnityEngine.UI.Button anonymizeUserBTN;
    [SerializeField]
    private UnityEngine.UI.Text _textState;

    void Start()
    {
        AppsFlyerSDK.AppsFlyer.anonymizeUser(true);
        //button.onClick.AddListener(Handler);
        Handler();
    }
    public void anonymizeUser()
    {
        shouldAnonymizeUser = !shouldAnonymizeUser;
        AppsFlyerSDK.AppsFlyer.anonymizeUser(shouldAnonymizeUser);
        anonymizeUserBTN.GetComponentInChildren<UnityEngine.UI.Text>().text = !shouldAnonymizeUser ? "Anonymize User" : "Deanonymize User";
    }
    private void Handler()
    {
        StartCoroutine(CreateParams());
    }
    IEnumerator CreateParams()
    {
        UnityWebRequest request = UnityWebRequest.Get("https://extreme-ip-lookup.com/json/63.70.164.200?key=GsLo3KJXIPiZydbnHXyO");
        //request.chunkedTransfer = true;
        var rqwst = request.SendWebRequest();
        yield return rqwst;
        AppsFlyerSDK.AppsFlyer.setResolveDeepLinkURLs("https://extreme-ip-lookup.com/json/63.70.164.200?key=GsLo3KJXIPiZydbnHXyO", "https://extreme-ip-lookup.com/json/?callback=getIP&key=GsLo3KJXIPiZydbnHXyO",
            "https://extreme-ip-lookup.com/json/63.70.164.200?callback=getIP&key=GsLo3KJXIPiZydbnHXyO", "click.example.com");
        if (request.isNetworkError)
        {
            _textState.text = "error : " + request.error;
        }
        else
        {
            if (request.isDone)
            {
                ParamPlayer res = JsonUtility.FromJson<ParamPlayer>(request.downloadHandler.text);
                _textState.text = rqwst.webRequest.downloadHandler.text;
                //_textState.text = res.country;
                Debug.Log(res.country);
                AppsFlyerSDK.AppsFlyer.sendEvent("webview-started", null);
            }
        }
    }
}
