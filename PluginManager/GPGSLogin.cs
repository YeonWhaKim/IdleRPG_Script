using BackEnd;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using LitJson;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GPGSLogin : MonoBehaviour
{
    public Text progressText;
    public GameObject nickNameGO;
    public InputField nickNameField;
    public GameObject nickNameErrorGO;
    public Text errorText;
    public GameObject nickNameAvailableGO;
    public Text availableText;
    public Text verText;
    public GameObject termsPopUp;
    private string id;
    private string nickName;
    public GameObject serverShotDownPopUp;
    public GameObject versionUpdatePopUp;
    public GameObject blockPopUp;
    public Text blockText;

    // Use this for initialization
    private void Awake()
    {
        verText.text = string.Format("ver.{0}", Application.version);
    }

    private void Start()
    {
        //yield return new WaitUntil(() => BackendAsyncClass.backendAsyncQueueState == BackendAsyncClass.BackendAsyncQueueState.Running);
        progressText.text = "구글 로그인 준비중";
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
            .Builder()
            .RequestServerAuthCode(false)
            .RequestIdToken()
            .RequestEmail()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        // recommended for debugging:
        PlayGamesPlatform.DebugLogEnabled = true;
        // Activate the Google Play Games platform
        PlayGamesPlatform.Activate();
#if UNITY_EDITOR
        BackendReturnObject bro = Backend.BMember.CustomLogin("rladusghk55", "2017dlstkrhk!@");
        ConfirmUser();
#elif UNITY_ANDROID
        //if (!Backend.Utils.GetGoogleHash().Equals(""))
        //    Debug.Log(Backend.Utils.GetGoogleHash());

        StartCoroutine(Login());
#endif
    }

    // GPGS 로그인 
    private IEnumerator Login()
    {
        progressText.text = "구글 로그인 성공";
        yield return GameManager.YieldInstructionCache.WaitForEndOfFrame;
        // 이미 로그인 된 경우
        if (Social.localUser.authenticated == true)
        {
            progressText.text = "구글 로그인 성공";
            BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs");
            id = Social.localUser.id;

            if (BRO.GetStatusCode().Equals("403") && BRO.GetMessage().Equals("forbidden blocked user, 금지된 blocked user 입니다"))
            {
                blockText.text = BRO.GetErrorCode();
                blockPopUp.SetActive(true);
                yield break;
            }
            ConfirmUser();
        }
        else
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    progressText.text = "구글 로그인 성공";
                    // 로그인 성공 -> 뒤끝 서버에 획득한 구글 토큰으로 가입요청
                    BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs");
                    id = Social.localUser.id;

                    if (BRO.GetStatusCode().Equals("403") && BRO.GetMessage().Equals("Forbidden blocked user, 금지된 blocked user"))
                    {
                        blockText.text = BRO.GetErrorCode();
                        blockPopUp.SetActive(true);
                        return;
                    }
                    ConfirmUser();
                }
                else
                {
                    progressText.text = "구글 로그인 실패";
                    // 로그인 실패
                    Debug.Log("Login failed for some reason");
                }
            });
        }

        Backend.Initialize(() =>
        {
            if (Backend.IsInitialized)
                Debug.Log(Backend.Utils.GetGoogleHash());
            else
                Debug.Log("초기화실패");
        });
    }

    // 구글 토큰 받아옴
    private string GetTokens()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            Backend.BMember.LoginWithTheBackendToken();
            // 유저 토큰 받기 첫번째 방법
            string _IDtoken = PlayGamesPlatform.Instance.GetIdToken();
            // 두번째 방법
            // string _IDtoken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
            return _IDtoken;
        }
        else
        {
            Debug.Log("접속되어있지 않습니다. PlayGamesPlatform.Instance.localUser.authenticated :  fail");
            return null;
        }
    }

    private void ConfirmUser()
    {
        StartCoroutine(LoadManager.instance.LoadingTextCO());

        BackendReturnObject state = Backend.Utils.GetServerStatus();
        var serverStatus = state.GetReturnValuetoJSON()["serverStatus"].ToString();
        if (serverStatus.Equals("2"))
        {
            serverShotDownPopUp.SetActive(true);
            return;
        }

        var appVer = Application.version;
        var str = appVer.Split('.');
        var realAppVer = int.Parse(str[str.Length - 1]);
#if UNITY_EDITOR
#elif UNITY_ANDROID
        BackendReturnObject version = Backend.Utils.GetLatestVersion();
        var serverVer = int.Parse(version.GetReturnValuetoJSON()["version"].ToString());
        var type = version.GetReturnValuetoJSON()["type"].ToString();
        if (realAppVer < serverVer)
        {
            versionUpdatePopUp.SetActive(true);
            return;
        }
#endif

        BackendReturnObject userinfo = Backend.BMember.GetUserInfo();
        if (userinfo.IsSuccess())
        {
            JsonData nickname = userinfo.GetReturnValuetoJSON()["row"]["nickname"];
            if (nickname == null)
            {
                termsPopUp.SetActive(true);
            }
            else
            {
                SaveManager.instance.nickName = nickname.ToString();
                StartCoroutine(TableDataLoadManager.instance.Load(false));
            }

            JsonData email = userinfo.GetReturnValuetoJSON()["row"]["emailForFindPassword"];

#if ANDROID
            if (email == null)
                bro = Backend.BMember.UpdateFederationEmail(GetTokens(), FederationType.Google);
#endif
        }
    }

    public void CreateNickname()
    {
        nickNameGO.SetActive(true);
    }

    public void OnClickRanomCreate()
    {
        nickNameField.text = id;
    }

    public void OnClickCreateNicname()
    {
        BackendReturnObject cNickName = Backend.BMember.CreateNickname(nickNameField.text.Trim());

        if (cNickName.GetStatusCode().Equals("409"))
        {
            errorText.text = "이미 사용중인 닉네임 입니다.";
            nickNameErrorGO.SetActive(true);
        }
        else if (cNickName.GetStatusCode().Equals("400"))
        {
            errorText.text = "닉네임이 20자를 초과하였습니다.";
            nickNameErrorGO.SetActive(true);
        }
        else
        {
            availableText.text = string.Format("'{0}'님 반갑습니다", nickNameField.text.Trim());
            SaveManager.instance.nickName = nickNameField.text.Trim();
            BackendReturnObject userinfo = Backend.BMember.GetUserInfo();
            nickNameAvailableGO.SetActive(true);
        }
    }

    public void OnClickCreateNickname_Confirm()
    {
        nickNameGO.SetActive(false);
        nickNameAvailableGO.SetActive(false);
        GameStart_TermsPopUp();
    }

    public void GameStart_TermsPopUp()
    {
        StartCoroutine(LoadManager.instance.AtFirstDataSetting(true));
    }

    public void OnUpdateButton()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.CheekyGames.idleRPG");
    }
}