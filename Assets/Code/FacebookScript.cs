using UnityEngine;
using System.Collections;
using Facebook.Unity;
using Facebook.MiniJSON;
using GameSparks.Api.Requests;
using System.Collections.Generic;
using GameSparks.Core;
using System;
using System.Linq;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;
using Firebase.Platform;
//using PlayFab;
//using PlayFab.ClientModels;
//using LoginResult = PlayFab.ClientModels.LoginResult;
[FirestoreData]
public class CustomMetadata
{
    [FirestoreProperty]
    public string id_string { get; set; }
    [FirestoreProperty]
    public string Location { get; set; }
    [FirestoreProperty]
    public int score { get; set; }
}
public class FacebookScript : MonoBehaviour
{
    [SerializeField] Button GetButtonFB;
    public static FacebookScript THIS;
    private string lastResponse = string.Empty;
    [SerializeField] private string _location;
    protected string LastResponse
    {
        get
        {
            return this.lastResponse;
        }

        set
        {
            this.lastResponse = value;
        }
    }

    private string status = "Ready";

    protected string Status
    {
        get
        {
            return this.status;
        }

        set
        {
            this.status = value;
        }
    }
    public void LoginWithFB2(string accessToken)
    {
        //FindObjectOfType<move2>().Name = "me";//
        //GetButtonFB.gameObject.SetActive(false);
        //PortalNetwork.THIS.UserID = response.UserId; 
		var firestore = FirebaseFirestore.DefaultInstance;
        //GetPicture(AccessToken.CurrentAccessToken.TokenString); 
        //Tournament.joined = true; Tournament.tournament.MenuTounamentClick();
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Firebase.Auth.Credential credential = Firebase.Auth.FacebookAuthProvider.GetCredential(accessToken);
        //var newMetadata = new MetadataChanges() { };
        //newMetadata.ToString();
       
        // UpdateMetadataAsync
        //var childData = new CustomMetadata
        //{
        //    id_string = "",
        //    Location = "ru",
        //    score = 1
        //};
        //string parentid = "07775000";
        //firestore.Document($"children/{parentid}/childs/3").SetAsync(childData);
        //firestore.Collection("children").Document("parentid").Collection("childs").Document().SetAsync(childData);

    }

    public void LoginWithFB(string accessToken)
    {
        new FacebookConnectRequest().SetSwitchIfPossible(true).SetAccessToken(accessToken).Send((response) => {
            if (!response.HasErrors)
            {
                FindObjectOfType<move2>().Name = response.DisplayName.ToString();//
                GetButtonFB.gameObject.SetActive(false);
                PortalNetwork.THIS.UserID = response.UserId;
                GetPicture(AccessToken.CurrentAccessToken.TokenString);Tournament.joined = true;Tournament.tournament.MenuTounamentClick();
                Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                Firebase.Auth.Credential credential = Firebase.Auth.FacebookAuthProvider.GetCredential(accessToken);
                var newMetadata = new MetadataChanges() {};
                newMetadata.ToString();
                var firestore = FirebaseFirestore.DefaultInstance;
                // UpdateMetadataAsync
                var childData = new CustomMetadata
                {
                    id_string = response.UserId,
                    Location = "ru", 
                    score = 1
                };
                string parentid = "07775000";
                //firestore.Document($"children/{parentid}/childs/3").SetAsync(childData);
                firestore.Collection("children").Document("parentid").Collection("childs").Document().SetAsync(childData);


                auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
                    if (task.IsCanceled)
                    {
                        Debug.LogError("SignInWithCredentialAsync was canceled.");
                        return;
                    }
                    if (task.IsFaulted)
                    {
                        Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                        return;
                    }

                    Firebase.Auth.FirebaseUser newUser = task.Result;
                    Debug.LogFormat("User signed in successfully: {0} ({1})",
                        newUser.DisplayName, newUser.UserId);
                });

            }
            else
            {
                IDictionary<string, object> errors = response.Errors.BaseData;
                Debug.Log("Authentification error:");
                foreach (var item in errors)
                {
                    if (item.Key == "error" && item.Value.ToString() == "timeout")
                    {
                        if (GS.Available)
                        {
                            GS.Reset();
                        }
                    }
                }
            }
        });
    }
    public void CallFBInit()
    {
        FB.Init(OnInitComplete, OnHideUnity);
    }
    private string _message;

    public void SetMessage(string message, bool error = false)
    {
        _message = message;
        if (error)
            Debug.LogError(_message);
        else
            Debug.Log(_message);
    }

    private void Awake()
    {
        THIS = this;
    }
    public void NewInit() { OnFacebookInitialized(); }
    private void OnFacebookInitialized()
    {
        SetMessage("Logging into Facebook...");

        // Once Facebook SDK is initialized, if we are logged in, we log out to demonstrate the entire authentication cycle.
        if (FB.IsLoggedIn)
            FB.LogOut();

        // We invoke basic login procedure and pass in the callback to process the result
        FB.LogInWithReadPermissions(null, OnFacebookLoggedIn);
    }
    private void OnFacebookLoggedIn(ILoginResult result)
    {
        // If result has no errors, it means we have authenticated in Facebook successfully
        if (result == null || string.IsNullOrEmpty(result.Error))
        {
            SetMessage("Facebook Auth Complete! Access Token: " + AccessToken.CurrentAccessToken.TokenString + "\nLogging into PlayFab...");

            /*
             * We proceed with making a call to PlayFab API. We pass in current Facebook AccessToken and let it create
             * and account using CreateAccount flag set to true. We also pass the callback for Success and Failure results
             */
            //PlayFabClientAPI.LoginWithFacebook(new LoginWithFacebookRequest { CreateAccount = true, AccessToken = AccessToken.CurrentAccessToken.TokenString },
            //    OnPlayfabFacebookAuthComplete, OnPlayfabFacebookAuthFailed);
        }
        else
        {
            // If Facebook authentication failed, we stop the cycle with the message
            SetMessage("Facebook Auth Failed: " + result.Error + "\n" + result.RawResult, true);
        }
    }
    // When processing both results, we just set the message, explaining what's going on.
    private void OnPlayfabFacebookAuthComplete()//(LoginResult result)
    {
        //SetMessage("PlayFab Facebook Auth Complete. Session ticket: " + result.SessionTicket);
    }

    private void OnPlayfabFacebookAuthFailed()//(PlayFabError error)
    {
        //SetMessage("PlayFab Facebook Auth Failed: " + error.GenerateErrorReport(), true);
    }
    private void OnInitComplete()
    {
        Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);

    }
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetPicture(AccessToken.CurrentAccessToken.TokenString);
        }
    }
    private void OnHideUnity(bool isGameShown)
    {
        Debug.Log("Is game showing? " + isGameShown);
    }
    protected void HandleResult(IResult result)
    {
        if (result == null)
        {
            this.LastResponse = "Null Response\n";
            Debug.Log(this.LastResponse);
            return;
        }
        if (!string.IsNullOrEmpty(result.Error))
        {
            this.Status = "Error - Check log for details";
            this.LastResponse = "Error Response:\n" + result.Error;
            Debug.Log(result.Error);
        }
        else if (result.Cancelled)
        {
            this.Status = "Cancelled - Check log for details";
            this.LastResponse = "Cancelled Response:\n" + result.RawResult;
            Debug.Log(result.RawResult);
        }
        else if (!string.IsNullOrEmpty(result.RawResult))
        {
            this.Status = "Success - Check log for details";
            this.LastResponse = "Success Response:\n" + result.RawResult;
			if(FB.IsLoggedIn){GetButtonFB.gameObject.SetActive(false);}
            //PlayFabClientAPI.LoginWithFacebook(new LoginWithFacebookRequest { CreateAccount = true, AccessToken = AccessToken.CurrentAccessToken.TokenString },OnPlayfabFacebookAuthComplete, OnPlayfabFacebookAuthFailed);
            //LoginWithFB(AccessToken.CurrentAccessToken.TokenString);
            //LoginWithFB2(AccessToken.CurrentAccessToken.TokenString);
        }
        else
        {
            this.LastResponse = "Empty Response\n";
            Debug.Log(this.LastResponse);
        }
    }
    public void GetPicture(string id)
    {
        FB.API("/" + id + "/picture?g&width=128&height=128&redirect=false", HttpMethod.GET, this.ProfilePhotoCallback);//2.1.4		
    }
    private void ProfilePhotoCallback(IGraphResult result)
    {
        if (string.IsNullOrEmpty(result.Error))//2.1.4
        {
            var dic = result.ResultDictionary["data"] as Dictionary<string, object>;
            string url = dic.Where(i => i.Key == "url").First().Value as string;
            print(url);
            FindObjectOfType<move2>().urlOnTournament = url;
            StartCoroutine(loadPicture(url));
        }

    }
    IEnumerator loadPicture(string url)
    {
        WWW www = new WWW(url);
        yield return www;
        var texture = www.texture;
        var sprite = Sprite.Create(texture, new Rect(0, 0, 128, 128), new Vector2(0, 0), 1f);print(url);
        string url1 = url.Replace("https://platform-lookaside.fbsbx.com/platform/profilepic/?", string.Empty);//
        PortalNetwork.THIS.Addpicture(url1, PortalNetwork.THIS.UserID);
        yield return new WaitForSeconds(0.5f);//
        PortalNetwork.THIS.LoadPicture();
    }
    public void fBvoid()//
    {
        FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, this.HandleResult);
    }
    // Use this for initialization
    void Start()
    {
        CallFBInit();
        //SetMessage("Initializing Facebook..."); // logs the given message and displays it on the screen using OnGUI method

        // This call is required before any other calls to the Facebook API. We pass in the callback to be invoked once initialization is finished
        //FB.Init(OnFacebookInitialized);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
