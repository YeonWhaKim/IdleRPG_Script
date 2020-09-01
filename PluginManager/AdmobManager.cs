using Firebase.Analytics;
using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class AdmobManager : MonoBehaviour
{
    public static AdmobManager instance;
    public RewardedAd rewardedAd;

    public enum CallObject { DEFAULT, SPEEDUP, BUFF_DOUBLEGOLD, BUFF_DOUBLEATK, DUNGEON_BOSS, DUNGEON_ENCHANT, BOX_OPEN };

    public Enum callEnum = CallObject.DEFAULT;
    private bool imNotSkip = false;

    // Start is called before the first frame update
    private
    void Start()
    {
        instance = this;

#if UNITY_ANDROID
        string appId = "ca-app-pub-2751188796370643~9891201922";

#elif UNITY_IPHONE
            string appId = "unexpected_platform";
#else
            string appId = "unexpected_platform";
#endif
        //Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);
        RequestRewardBasedVideo();
    }

    public void RequestRewardBasedVideo()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
                   //string adUnitId =  "ca-app-pub-3940256099942544/5224354917"; //테스트용
                    string adUnitId = "ca-app-pub-2751188796370643/8578120252";
#elif UNITY_IPHONE
                    string adUnitId = GameConfig.instance.admob.iosRewarded.Trim();
#else
                    string adUnitId = "unexpected_platform";
#endif

        // Get singleton reward based video ad reference.
        this.rewardedAd = new RewardedAd(adUnitId);
        // RewardBasedVideoAd is a singleton, so handlers should only be registered once.
        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        this.rewardedAd.LoadAd(this.CreateAdRequest());
    }

    private AdRequest CreateAdRequest()
    {
        //return new AdRequest.Builder()
        //        .AddTestDevice(AdRequest.TestDeviceSimulator)
        //        .AddTestDevice("0123456789ABCDEF0123456789ABCDEF")
        //        .AddKeyword("game")
        //        .TagForChildDirectedTreatment(false)
        //        .AddExtra("color_bg", "9B30FF")
        //        .Build();
        return new AdRequest.Builder()
            .TagForChildDirectedTreatment(false)
            .Build();
    }

    public void ShowRewardBasedVided(CallObject callObject)
    {
        callEnum = callObject;
        if (this.rewardedAd.IsLoaded())
            this.rewardedAd.Show();
        else
            Debug.Log("Reward based video ad is not ready yet");
    }

    public void RequestRewardVideo()
    {
        switch (callEnum)
        {
            case CallObject.SPEEDUP:
                //배속 시작 - 1.5
                SpeedManager.instance.selectSpeedPopUP.GetComponent<Animator>().SetTrigger("False");
                StartCoroutine(SpeedManager.instance.SpeedUp(1.5f, SpeedManager.speedDurationStandard));
                GameManager.instance.TokenRefresh(true);
                FirebaseAnalytics.LogEvent("AD_SpeedUp");
                break;

            case CallObject.BUFF_DOUBLEGOLD:
                BuffManager.instance.BuffStart_Real(0);
                GameManager.instance.TokenRefresh(true);

                FirebaseAnalytics.LogEvent("AD_DoubleGoldBuff");
                break;

            case CallObject.BUFF_DOUBLEATK:
                BuffManager.instance.BuffStart_Real(1);
                GameManager.instance.TokenRefresh(true);

                FirebaseAnalytics.LogEvent("AD_DoubleAtkBuff");
                break;

            case CallObject.DUNGEON_BOSS:
                BossDungeonMenu.instance.OnClickBossDungeonEntrance(1);
                GameManager.instance.TokenRefresh(true);

                FirebaseAnalytics.LogEvent("AD_BossDungeon");
                break;

            case CallObject.DUNGEON_ENCHANT:
                break;

            case CallObject.BOX_OPEN:
                GameManager.instance.TokenRefresh(true);

                BoxOpenReward();
                FirebaseAnalytics.LogEvent("AD_BoxOpen");
                break;

            default:
                break;
        }
    }

    private void BoxOpenReward()
    {
        StartCoroutine(BoxMenu.instance.Open_Ad());
        CentralInfoManager.instance.boxOpenCount_ad++;
        //쿨타임 가게하기.
        BoxMenu.instance.StartBoxOpenADCoolTime();
    }

    #region RewardBasedVided callback handlers

    private void HandleRewardedAdClosed(object sender, EventArgs e)
    {
        RequestRewardBasedVideo();
        MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
        if (imNotSkip)
        {
            RequestRewardVideo();
            imNotSkip = false;
        }
        callEnum = CallObject.DEFAULT;
    }

    private void HandleUserEarnedReward(object sender, Reward e)
    {
        imNotSkip = true;
        string type = e.Type;
        double amount = e.Amount;
        MonoBehaviour.print(
            "HandleRewardBasedVideoRewarded event received for " + amount.ToString() + " " + type);
    }

    private void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs e)
    {
        Debug.Log("HandleRewardedAdFailedToShow");
    }

    private void HandleRewardedAdOpening(object sender, EventArgs e)
    {
        Debug.Log("HandleRewardedAdOpening");
    }

    private void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs e)
    {
        Debug.Log("HandleRewardedAdFailedToLoad");
    }

    private void HandleRewardedAdLoaded(object sender, EventArgs e)
    {
        Debug.Log("HandleRewardedAdLoaded");
    }

    #endregion RewardBasedVided callback handlers
}