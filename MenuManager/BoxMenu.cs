using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BoxMenu : MonoBehaviour
{
    public static BoxMenu instance;
    public const int BOXOPENADCOOLTIME_STANDARD = 300;
    public Sprite[] keySprites;
    public Sprite[] rewardSprites;

    [Header("Box Menu Panel")]
    public Image[] boxImages;

    public Text[] boxNames;
    public Text[] boxExps;
    public Text[] boxCountTexts;
    public CanvasGroup[] openButtonCanvasGroups;
    public CanvasGroup[] openButtonCanvasGroups_jewel;
    public Button[] openButtons;
    public Button[] openButtons_jewel;
    public Image[] keyImages;
    public Text[] keyCountTexts;

    [Header("Box Open Panel")]
    public GameObject boxOpenPanel;

    public Button adOpenButton;
    public Image adImage;
    public Text adOpenText;
    public Text adOpenCountText;

    public Image oneOpenKeyImege;
    public Button oneOpenButton;

    public Button oneOpenButton_Jewel;
    public Image oneOpenImage_jewel;
    public Text oneOpenCountText_jewel;
    public Text oneOpenText_jewel;

    public Button allOpenButton;
    public Image allOpenKeyImage;
    public Text allOpenKeyCountText;
    public Text allOpenKeyText;

    public Button allOpenButton_jewel;
    public Image allOpenImage_jewel;
    public Text allOpenCountText_jewel;
    public Text allOpenText_jewel;
    public SkeletonAnimation boxSkeletonAnimation_open;

    [Header("Box Result")]
    public GameObject boxResultPanel;

    public SkeletonAnimation boxSkeletonAnimation_result;
    public Skeleton boxSkeleton;
    public Transform rewardParent;
    public GameObject BoxOpenEffectGO;

    private int openBoxIndex;
    private int maxCount;
    private int rewardSpriteIndex;
    private bool isJewelOpen;
    private BoxManager.OPENTYPE opentype;
    private Spine.EventData eventData_open;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => LoadManager.instance.isDataLoaded);
        if (CentralInfoManager.instance.boxOpenCoolTime_ad < BOXOPENADCOOLTIME_STANDARD)
            StartCoroutine(BoxOpenCoolTime());
    }

    private void HandleAnimationStateEvent(Spine.TrackEntry entry, Spine.Event e)
    {
        eventData_open = boxSkeletonAnimation_result.Skeleton.Data.FindEvent("open");
        if (e.Data == eventData_open)
            SoundManager.instance.UISoundPlay(2);
    }

    public void BoxMenuSetting()
    {
        var boxMaxCount = RebirthManager.instance.ReturnMaxLevel_DependingOnRebirthCount(CharacterStatus.instance.Rebirth);
        for (int i = 0; i < boxCountTexts.Length; i++)
        {
            boxCountTexts[i].text = string.Format("{0}개", CentralInfoManager.boxCountList[i]);
            if (CentralInfoManager.boxCountList[i] >= boxMaxCount)
                boxCountTexts[i].text = string.Format("{0}개(Max)", CentralInfoManager.boxCountList[i]);
            keyCountTexts[i].text = string.Format("열기\nx {0}", CentralInfoManager.keyCountList[i]);
            keyImages[i].sprite = keySprites[i];
            boxNames[i].text = TableData.Box_Name[i];

            openButtons_jewel[i].gameObject.SetActive(false);

            //상자가 없을때
            if (CentralInfoManager.boxCountList[i] == 0)
            {
                boxImages[i].color = new Vector4(1, 1, 1, 0.6f);
                boxNames[i].color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 0.6f);
                boxExps[i].color = new Vector4(0.8117648f, 0.7882354f, 0.7333333f, 0.6f);
                boxCountTexts[i].color = new Vector4(0.4039216f, 0.8274511f, 0.7333333f, 0.6f);

                openButtonCanvasGroups[i].alpha = 0.6f;
                openButtons[i].interactable = false;
            }
            else
            {
                boxImages[i].color = new Vector4(1, 1, 1, 1f);
                boxNames[i].color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 1f);
                boxExps[i].color = new Vector4(0.8117648f, 0.7882354f, 0.7333333f, 1f);
                boxCountTexts[i].color = new Vector4(0.4039216f, 0.8274511f, 0.7333333f, 1f);

                openButtonCanvasGroups[i].alpha = 1f;
                openButtons[i].interactable = true;

                //키 세팅
                KeySetting(i);
            }
        }
    }

    private void KeySetting(int index)
    {
        // 키 없을 때
        if (CentralInfoManager.keyCountList[index] <= 0)
        {
            // 만능키 없을 때
            if (CentralInfoManager.anyOpenKey <= 0)
            {
                // 보석으로 열기
                openButtons_jewel[index].gameObject.SetActive(true);
                if (CurrencyManager.instance.Jewel <= 0)
                {
                    //openButtonCanvasGroups_jewel[index].alpha = 0.6f;
                    //openButtons_jewel[index].interactable = false;

                    openButtons_jewel[index].gameObject.SetActive(false);

                    openButtonCanvasGroups[index].alpha = 0.6f;
                    openButtons[index].interactable = false;
                }
            }
            // 만능키 있을 때
            else
            {
                keyCountTexts[index].text = string.Format("열기\nx {0}", CentralInfoManager.anyOpenKey);
                keyImages[index].sprite = keySprites[keySprites.Length - 1];
            }
        }
        // 키 있을 때
        else
            openButtons_jewel[index].gameObject.SetActive(false);
    }

    public void OnClickOpenButton(int index)
    {
        openBoxIndex = index;

        oneOpenButton_Jewel.gameObject.SetActive(false);
        allOpenButton_jewel.gameObject.SetActive(false);

        // 보석으로 열기
        if (openButtons_jewel[index].gameObject.activeSelf)
        {
            opentype = BoxManager.OPENTYPE.JEWEL;

            isJewelOpen = true;
            maxCount = CentralInfoManager.boxCountList[index];

            oneOpenButton_Jewel.gameObject.SetActive(true);
            allOpenButton_jewel.gameObject.SetActive(true);

            oneOpenButton_Jewel.interactable = true;
            oneOpenCountText_jewel.text = TableData.Box_Open_Gem[index].ToString();

            allOpenButton_jewel.interactable = true;
            allOpenCountText_jewel.text = (TableData.Box_Open_Gem[index] * maxCount).ToString();

            if (CurrencyManager.instance.Jewel < TableData.Box_Open_Gem[index])
            {
                oneOpenButton_Jewel.interactable = false;
                oneOpenImage_jewel.color = new Vector4(1, 1, 1, 0.6f);
                oneOpenCountText_jewel.color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 0.6f);
                oneOpenText_jewel.color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 0.6f);
            }
            else
            {
                oneOpenButton_Jewel.interactable = true;
                oneOpenImage_jewel.color = new Vector4(1, 1, 1, 1f);
                oneOpenCountText_jewel.color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 1f);
                oneOpenText_jewel.color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 1f);
            }

            if (CurrencyManager.instance.Jewel < TableData.Box_Open_Gem[index] * maxCount)
            {
                allOpenButton_jewel.interactable = false;
                allOpenImage_jewel.color = new Vector4(1, 1, 1, 0.6f);
                allOpenCountText_jewel.color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 0.6f);
                allOpenText_jewel.color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 0.6f);
            }
            else
            {
                allOpenButton_jewel.interactable = true;
                allOpenImage_jewel.color = new Vector4(1, 1, 1, 1f);
                allOpenCountText_jewel.color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 1f);
                allOpenText_jewel.color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 1f);
            }
        }
        // 키로 열기
        else
        {
            isJewelOpen = false;
            // 만능키일때
            if (keyImages[index].sprite.Equals(keySprites[keySprites.Length - 1]))
            {
                opentype = BoxManager.OPENTYPE.ANYOPENKEY;
                oneOpenKeyImege.sprite = keySprites[keySprites.Length - 1];
                allOpenKeyImage.sprite = keySprites[keySprites.Length - 1];

                allOpenButton.interactable = false;
                allOpenKeyImage.color = new Vector4(1, 1, 1, 0.6f);
                allOpenKeyCountText.color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 0.6f);
                allOpenKeyText.color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 0.6f);

                allOpenKeyCountText.text = string.Format("---");
            }
            else
            {
                opentype = BoxManager.OPENTYPE.KEY;
                oneOpenKeyImege.sprite = keySprites[index];
                allOpenKeyImage.sprite = keySprites[index];

                // 키도 갯수에 맞춰서 세팅하기
                allOpenButton.interactable = true;
                allOpenKeyImage.color = Color.white;
                allOpenKeyCountText.color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 1f);
                allOpenKeyText.color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 1f);

                maxCount = CentralInfoManager.boxCountList[index] > CentralInfoManager.keyCountList[index] ? CentralInfoManager.keyCountList[index] : CentralInfoManager.boxCountList[index];
                allOpenKeyCountText.text = string.Format("x {0}", maxCount);
            }
        }

        boxSkeletonAnimation_open.Skeleton.SetSkin(string.Format("box{0}", string.Format("{0:D2}", index + 1)));
        boxSkeletonAnimation_open.AnimationState.SetAnimation(0, "idle", true);

        // 광고 쿨타임 세팅
        if (CentralInfoManager.instance.boxOpenCoolTime_ad < BOXOPENADCOOLTIME_STANDARD)
        {
            adOpenButton.interactable = false;
            adImage.color = new Vector4(1, 1, 1, 0.6f);
            adOpenText.color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 0.6f);
            adOpenCountText.color = new Vector4(0.9803922f, 0.4313726f, 0.427451f, 1);
        }
        else
        {
            if (CentralInfoManager.instance.boxOpenCount_ad.Equals(CentralInfoManager.ADOPENMAXCOUNT))
            {
                adOpenButton.interactable = false;
                adImage.color = new Vector4(1, 1, 1, 0.6f);
                adOpenText.color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 0.6f);
                adOpenCountText.color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 0.6f);
                adOpenCountText.text = string.Format("({0}/{1})", CentralInfoManager.instance.boxOpenCount_ad, CentralInfoManager.ADOPENMAXCOUNT);
            }
            else
            {
                adOpenButton.interactable = true;
                adImage.color = Color.white;
                adOpenText.color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 1);
                adOpenCountText.color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 1);
                adOpenCountText.text = string.Format("({0}/{1})", CentralInfoManager.instance.boxOpenCount_ad, CentralInfoManager.ADOPENMAXCOUNT);
            }
        }
        boxOpenPanel.SetActive(true);
    }

    public void OnClickOpen_ad()
    {
        AdmobManager.instance.ShowRewardBasedVided(AdmobManager.CallObject.BOX_OPEN);
    }

    public IEnumerator Open_Ad()
    {
        yield return GameManager.YieldInstructionCache.WaitForEndOfFrame;
        boxOpenPanel.SetActive(false);

        BoxManager.instance.ListReset(BoxManager.instance.boxRewardType, BoxManager.instance.boxRewardValue);
        BoxManager.instance.BoxOpenReward(opentype, openBoxIndex, true);

        ResultPanel();
    }

    public void OnClickOpen_one()
    {
        boxOpenPanel.SetActive(false);

        BoxManager.instance.ListReset(BoxManager.instance.boxRewardType, BoxManager.instance.boxRewardValue);
        BoxManager.instance.BoxOpenReward(opentype, openBoxIndex, false);

        ResultPanel();
    }

    public void OnClickOpen_many()
    {
        boxOpenPanel.SetActive(false);

        BoxManager.instance.ListReset(BoxManager.instance.boxRewardType, BoxManager.instance.boxRewardValue);
        for (int i = 0; i < maxCount; i++)
        {
            BoxManager.instance.BoxOpenReward(opentype, openBoxIndex, false);
        }

        ResultPanel();
    }

    public void StartBoxOpenADCoolTime()
    {
        StartCoroutine(BoxOpenCoolTime());
    }

    private IEnumerator BoxOpenCoolTime()
    {
        adOpenCountText.text = GameManager.MM__SS(CentralInfoManager.instance.boxOpenCoolTime_ad);
        while (CentralInfoManager.instance.boxOpenCoolTime_ad > 0)
        {
            yield return new WaitForSecondsRealtime(1f);
            CentralInfoManager.instance.boxOpenCoolTime_ad--;
            adOpenCountText.text = GameManager.MM__SS(CentralInfoManager.instance.boxOpenCoolTime_ad);
        }
        CentralInfoManager.instance.boxOpenCoolTime_ad = BOXOPENADCOOLTIME_STANDARD;
    }

    private void ResultPanel()
    {
        boxSkeletonAnimation_result.Skeleton.SetSkin(string.Format("box{0}", string.Format("{0:D2}", openBoxIndex + 1)));
        boxSkeletonAnimation_result.AnimationState.SetAnimation(0, "open4", false);
        BoxOpenEffectGO.SetActive(false);
        BoxOpenEffectGO.SetActive(true);

        for (int i = 0; i < BoxManager.instance.boxRewardType.Count; i++)
        {
            if (BoxManager.instance.boxRewardValue[i] <= 0)
                rewardParent.GetChild(i).gameObject.SetActive(false);
            else
            {
                var rewardValue = BoxManager.instance.boxRewardValue[i];
                var rewardText = ReturnRewardText(BoxManager.instance.boxRewardType[i], BoxManager.instance.boxRewardValue[i]);
                var rewardGO = rewardParent.GetChild(i);
                rewardGO.GetChild(0).GetComponent<Image>().sprite = rewardSprites[rewardSpriteIndex];
                rewardGO.GetChild(1).GetComponent<Text>().text = string.Format("{0} + {1}", rewardText, rewardValue);
                rewardGO.gameObject.SetActive(true);
            }
        }
        boxResultPanel.SetActive(true);
        boxSkeletonAnimation_result.AnimationState.Event += HandleAnimationStateEvent;
        BoxMenuSetting();
    }

    public void ResultPanelYes()
    {
        StartCoroutine(CloseCo());
    }

    private IEnumerator CloseCo()
    {
        boxSkeletonAnimation_result.AnimationState.SetAnimation(0, "idle", false);

        yield return new WaitForSecondsRealtime(0.3f);
        boxResultPanel.SetActive(false);
    }

    private string ReturnRewardText(string rewardType, int addedValue)
    {
        var result = "";
        switch (rewardType)
        {
            case "Gold":
                result = "골드";
                rewardSpriteIndex = 0;
                CurrencyManager.instance.AddGold(addedValue);
                break;

            case "Token":
                result = "증표";
                rewardSpriteIndex = 1;
                CurrencyManager.instance.Token += addedValue;
                break;

            case "B01":
                result = "나무 상자";
                rewardSpriteIndex = 11;
                CentralInfoManager.boxCountList[0] += addedValue;
                break;

            case "B02":
                result = "철 상자";
                rewardSpriteIndex = 12;
                CentralInfoManager.boxCountList[1] += addedValue;
                break;

            case "B03":
                result = "주석 상자";
                rewardSpriteIndex = 13;
                CentralInfoManager.boxCountList[2] += addedValue;
                break;

            case "B04":
                result = "은 상자";
                rewardSpriteIndex = 14;
                CentralInfoManager.boxCountList[3] += addedValue;
                break;

            case "B05":
                result = "금 상자";
                rewardSpriteIndex = 15;
                CentralInfoManager.boxCountList[4] += addedValue;
                break;

            case "B06":
                result = "백금 상자";
                rewardSpriteIndex = 16;
                CentralInfoManager.boxCountList[5] += addedValue;
                break;

            case "B07":
                result = "다이아 상자";
                rewardSpriteIndex = 17;
                CentralInfoManager.boxCountList[6] += addedValue;
                break;

            case "K01":
                result = "나무 열쇠";
                rewardSpriteIndex = 2;
                CentralInfoManager.keyCountList[0] += addedValue;
                break;

            case "K02":
                result = "철 열쇠";
                rewardSpriteIndex = 3;
                CentralInfoManager.keyCountList[1] += addedValue;
                break;

            case "K03":
                result = "주석 열쇠";
                rewardSpriteIndex = 4;
                CentralInfoManager.keyCountList[2] += addedValue;
                break;

            case "K04":
                result = "은 열쇠";
                rewardSpriteIndex = 5;
                CentralInfoManager.keyCountList[3] += addedValue;
                break;

            case "K05":
                result = "금 열쇠";
                rewardSpriteIndex = 6;
                CentralInfoManager.keyCountList[4] += addedValue;
                break;

            case "K06":
                result = "백금 열쇠";
                rewardSpriteIndex = 7;
                CentralInfoManager.keyCountList[5] += addedValue;
                break;

            case "K07":
                result = "다이아 열쇠";
                rewardSpriteIndex = 8;
                CentralInfoManager.keyCountList[6] += addedValue;
                break;

            case "K100":
                result = "만능 열쇠";
                rewardSpriteIndex = 9;
                CentralInfoManager.anyOpenKey += addedValue;
                break;

            case "LP_Potion":
                result = "LP 포션";
                rewardSpriteIndex = 10;
                CentralInfoManager.lpPotion += addedValue;
                GameManager.SaveLogToServer("LP포션 획득", CentralInfoManager.lpPotion.ToString(), "유료 상품 구매");

                break;

            default:
                break;
        }
        return result;
    }
}