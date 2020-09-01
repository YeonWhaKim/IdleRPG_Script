using Firebase;
using System.Collections;
using UnityEngine;

public class PushManager : MonoBehaviour
{
    private FirebaseApp app;

    public IEnumerator Start()
    {
        yield return new WaitUntil(() => TableDataLoadManager.instance.isLoadDone);

        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            { // Create and hold a reference to your FirebaseApp,
              // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;
                Debug.Log("파이어 베이스 앱 초기화 완료");
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
#if UNITY_EDITOR
#elif UNITY_ANDROID
            BackEnd.BackendReturnObject bro = BackEnd.Backend.Android.PutDeviceToken();
            Debug.Log("뒤끝 푸시 : " + bro.GetMessage());
#endif
    }

    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
    }
}