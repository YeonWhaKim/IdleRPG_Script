using BackEnd;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public GameObject settingPanel;

    [Header("BGM")]
    public Image bgmButton;

    public GameObject onText_bgm;
    public GameObject offText_bgm;

    [Header("SoundEffect")]
    public Image soundEffectButton;

    public GameObject onText_se;
    public GameObject offText_se;
    public AudioSource boxGetse;
    public List<AudioSource> hitSoundList = new List<AudioSource>();

    [Header("Coupon")]
    public InputField couponNumberIF;

    public GameObject popUpGOErrorGO;
    public Text popUpExpText;
    public GameObject popUpGO_jewel600;
    public Text couponNameText_600;
    public GameObject popUpGO_jewel1300;
    public Text couponNameText_1300;
    public GameObject popUpGO_vip;

    private string couponName;

    // Start is called before the first frame update

    public void OnClickSettingButton()
    {
        settingPanel.SetActive(true);
    }

    public void OnClickBgm()
    {
        if (onText_bgm.activeSelf)
        {
            offText_bgm.SetActive(true);
            onText_bgm.SetActive(false);
            bgmButton.color = Color.black;
            SoundManager.instance.bgmSource.mute = true;
        }
        else
        {
            offText_bgm.SetActive(false);
            onText_bgm.SetActive(true);
            bgmButton.color = Color.white;
            SoundManager.instance.bgmSource.mute = false;
        }
    }

    public void OnClickSoundEffect()
    {
        if (onText_se.activeSelf)
        {
            offText_se.SetActive(true);
            onText_se.SetActive(false);
            soundEffectButton.color = Color.black;

            for (int i = 0; i < hitSoundList.Count; i++)
            {
                hitSoundList[i].mute = true;
            }
            SoundManager.instance.charcaterRunSource.mute = true;
            SoundManager.instance.bossAppearSource.mute = true;
            SoundManager.instance.goldGetSource.mute = true;
            SoundManager.instance.effectSource_UI.mute = true;
            SoundManager.instance.menuButtonSource.mute = true;
            boxGetse.mute = true;
        }
        else
        {
            offText_se.SetActive(false);
            onText_se.SetActive(true);
            soundEffectButton.color = Color.white;

            for (int i = 0; i < hitSoundList.Count; i++)
            {
                hitSoundList[i].mute = false;
            }

            SoundManager.instance.charcaterRunSource.mute = false;
            SoundManager.instance.bossAppearSource.mute = false;
            SoundManager.instance.goldGetSource.mute = false;
            SoundManager.instance.effectSource_UI.mute = false;
            SoundManager.instance.menuButtonSource.mute = false;

            boxGetse.mute = false;
        }
    }

    public void OnClickCouponRewardGet()
    {
        var couponNumber = couponNumberIF.text;
        couponNumber = couponNumber.Trim();
        //var test = "82757b7ffe8028fcca";

        if (couponNumber.Equals(null) || couponNumber.Equals(""))
            return;

        BackendReturnObject bro = Backend.Coupon.UseCoupon(couponNumber);
        //Debug.Log(bro.GetReturnValue());
        //Debug.Log(bro.GetStatusCode());
        //Debug.Log(bro.GetMessage());

        if (bro.GetStatusCode().Equals("200"))
        {
            var itemCount = bro.GetReturnValuetoJSON()["itemsCount"].ToString();
            var uuid = bro.GetReturnValuetoJSON()["uuid"].ToString();
            CouponKindJudge(uuid, itemCount);
        }
        else
        {
            popUpExpText.text = "이미 사용되었거나,\n틀린번호 입니다.";
            popUpGOErrorGO.SetActive(true);
        }
    }

    private void CouponKindJudge(string uuid, string itemCount)
    {
        switch (uuid)
        {
            case "764":
            case "771":
            case "772":
            case "773":
            case "774":
            case "798":
            case "796":
            case "803":
                couponNameText_600.text = "사전가입 쿠폰";
                popUpGO_jewel600.SetActive(true);
                CurrencyManager.instance.Jewel += 600;
                break;

            case "765":
            case "799":
                couponNameText_1300.text = "스페셜 쿠폰";
                popUpGO_jewel1300.SetActive(true);
                CurrencyManager.instance.Jewel += 1300;
                break;

            case "766":
            case "800":
                couponNameText_600.text = "유니크 쿠폰";
                popUpGO_jewel600.SetActive(true);
                CurrencyManager.instance.Jewel += 600;

                break;

            case "767":
            case "801":
                couponNameText_1300.text = "플러스 쿠폰";
                popUpGO_jewel1300.SetActive(true);
                CurrencyManager.instance.Jewel += 1300;
                break;

            case "768":
            case "802":
            case "804":
                popUpGO_vip.SetActive(true);
                CurrencyManager.instance.Jewel += 8000;
                CurrencyManager.instance.Token += 1200;
                PetManager.instance.PetBuy(0);
                break;

            case "769":
                popUpExpText.text = "테스트 사전가입\n보석 : 100개";
                CurrencyManager.instance.Jewel += 100;
                popUpGOErrorGO.SetActive(true);
                break;

            default:
                break;
        }
    }

    public void OnClickServiceButton()
    {
        string mailto = "cheeky@thebackend.io";
        string subject = EscapeURL("버그 리포트 / 기타 문의사항");
        string body = EscapeURL
            (string.Format("이 곳에 내용을 구체적으로 작성해주세요.(버그,궁금한점,결제관련,로그인관련,게임방법 등)\n\n\n\n________\n\nDevice Model : {0}\n\nDevice OS : {1}\n\n________"
           , SystemInfo.deviceModel, SystemInfo.operatingSystem));

        Application.OpenURL("mailto:" + mailto + "?subject=" + subject + "&body=" + body);
    }

    //string EscapeURL정의
    private string EscapeURL(string url)
    {
        return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
    }
}