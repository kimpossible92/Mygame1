using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WebLNK : MonoBehaviour
{
    public Text webviewLink;
    public ScriptableString url;

    private void Start()
    {
        webviewLink.text += url.value;
        Debug.Log("wv " + url.value);
    }
}
