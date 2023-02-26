using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class WebView : MonoBehaviour
{
    [SerializeField]UnityEngine.UI.Text _text;
    [SerializeField] private string[] uris;
    private bool InternetAccess=false;
    public void Play()
    {
        if(!InternetAccess)UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    public IEnumerator TextConn(Action<bool> callback)
    {
        foreach(string uri in uris)
        {
            UnityWebRequest request = UnityWebRequest.Get(uri);

            yield return request.SendWebRequest();
            Debug.Log(message: "{ GameLog}=>[InternetAccess]-TestConnect \n +URI: " + uri + "\n Network Error" + request.isNetworkError);
            if (request.isNetworkError == false)
            {
                InternetAccess = true;
                callback(obj:true);
                yield break;
            }
        }
        InternetAccess = false;
        callback(obj: false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        StartCoroutine(routine: TextConn(result =>
         {
             Debug.Log(message: "{ GameLog}=>[Demo] - Awake - InternetAccess:" + result);

         }));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
