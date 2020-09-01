using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    public static ShopMenu instance;

    [Header("Shop Menu Tab & Panel")]
    public Sprite ingSprite;

    public Sprite availableSprite;
    public Image[] menutab;
    public Text[] menuText;
    public GameObject[] menupanel;

    public Button[] adremoveButtons;
    public Text[] adremoveTexts;
    public Button monthlyJewelButton;
    public Text monthlyJewelText;
    public GameObject speedBuffOutlineGO;
    public Button speedBuffButton;
    public Text speedBuffPriceText;

    [Header("Entrance Objects")]
    public Button[] buyButtons_Entrance;

    public Text[] priceTexts_Entrance;

    [Header("Token Objects")]
    public Button[] buyButtons_Token;

    public Text[] priceTexts_Token;
    public Button rebirthCouponBuyBtn;
    public Button lpPotionBuyBtn;
    public Text rebirthCoupon_dailyCountText;
    public Text lpPotion_dailyCountText;
    public RectTransform shopContents_jewel;
    public RectTransform shopContents_item;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    public void OnClickMenuTab(int index)
    {
        if (index.Equals(1))
        {
            var pos = shopContents_item.anchoredPosition;
            shopContents_item.anchoredPosition = new Vector2(pos.x, 0);
            BuyPanelSetting(buyButtons_Token, priceTexts_Token);
            BuyPanelSetting(buyButtons_Entrance, priceTexts_Entrance);

            if (SpeedManager.instance.speedDuration > 0)
            {
                speedBuffButton.interactable = false;
                speedBuffPriceText.text = "사용중";
                speedBuffButton.transform.GetChild(0).GetComponent<Image>().enabled = false;
                speedBuffPriceText.color = new Vector4(0.8941177f, 0.8901961f, 0.8078432f, 0.6f);
            }
            else
            {
                speedBuffButton.transform.GetChild(0).GetComponent<Image>().enabled = true;
                speedBuffPriceText.text = "100";
                speedBuffPriceText.color = new Vector4(0.9803922f, 0.4313726f, 0.427451f, 1);

                if (100 > CurrencyManager.instance.Jewel)
                    speedBuffButton.interactable = false;
                else
                    speedBuffButton.interactable = true;
            }
            speedBuffOutlineGO.SetActive(false);
        }
        else if (index.Equals(2))
            PetMenu.instance.OnClickPetScroll(0);
        else
        {
            var pos = shopContents_jewel.anchoredPosition;
            shopContents_jewel.anchoredPosition = new Vector2(pos.x, 0);
            for (int i = 0; i < CentralInfoManager.instance.isBuffAdRemoved.Count; i++)
            {
                if (CentralInfoManager.instance.isBuffAdRemoved[i].Equals(true))
                {
                    adremoveButtons[i].interactable = false;
                    adremoveTexts[i].color = new Vector4(0.8941177f, 0.8901961f, 0.8078432f, 0.6f);
                }
                else
                {
                    adremoveButtons[i].interactable = true;
                    adremoveTexts[i].color = new Vector4(0.8941177f, 0.8901961f, 0.8078432f, 1f);
                }
            }
        }

        for (int i = 0; i < menutab.Length; i++)
        {
            if (i.Equals(index))
            {
                menutab[i].sprite = ingSprite;
                menuText[i].color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 1);
                menupanel[i].SetActive(true);
            }
            else
            {
                menutab[i].sprite = availableSprite;
                menuText[i].color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 0.5f);
                menupanel[i].SetActive(false);
            }
        }
    }

    public void OnClickBuyJewel(int index)
    {
        switch (index)
        {
            case 0:
                // 25일간 보석 300개 지급
                // 즉시 2500개 지급
                CurrencyManager.instance.Jewel += ReturnShopGiftCount("SH100");
                CurrencyManager.instance.Jewel += CentralInfoManager.MonthlyProduct_DAILYREWARD;
                CentralInfoManager.instance.monthlyJewelProductBuyDate = System.DateTime.Now.ToString();
                MonthlyProductSetting("0", true);
                break;

            case 1:
                CurrencyManager.instance.Jewel += ReturnShopGiftCount("SH101");

                break;

            case 2:
                CurrencyManager.instance.Jewel += ReturnShopGiftCount("SH102");

                break;

            case 3:
                CurrencyManager.instance.Jewel += ReturnShopGiftCount("SH103");

                break;

            case 4:
                CurrencyManager.instance.Jewel += ReturnShopGiftCount("SH104");

                break;

            case 5:
                CentralInfoManager.instance.isBuffAdRemoved[0] = true;
                OnClickMenuTab(0);

                break;

            case 6:
                CentralInfoManager.instance.isBuffAdRemoved[1] = true;
                OnClickMenuTab(0);

                break;

            default:
                break;
        }
        SaveManager.instance.SaveData();
    }

    public void MonthlyProductSetting(string remainingDay, bool isProduct)
    {
        if (isProduct)
        {
            var ddd = CentralInfoManager.MonthlyProduct_PERIOD - int.Parse(remainingDay);
            monthlyJewelButton.interactable = false;
            monthlyJewelText.text = string.Format("보유중\n<size=20>({0}일 남음)</size>", ddd);
            monthlyJewelText.color = new Vector4(0.8941177f, 0.8901961f, 0.8078432f, 0.6f);
        }
        else
        {
            monthlyJewelButton.interactable = true;
            monthlyJewelText.text = "KRW\n25,000";
            monthlyJewelText.color = new Vector4(0.8941177f, 0.8901961f, 0.8078432f, 1f);
        }
    }

    public void OnClickBuyToken(int index)
    {
        switch (index)
        {
            case 0:
                CurrencyManager.instance.Jewel -= 100;
                CurrencyManager.instance.Token += 100;
                // GameManager.SaveLogToServer("증표 구매", string.Format("-100 / 이후 보석 갯수 : {0}", CurrencyManager.instance.Jewel), "보석 사용 현황");

                break;

            case 1:
                CurrencyManager.instance.Jewel -= 200;
                CurrencyManager.instance.Token += 210;
                //GameManager.SaveLogToServer("증표 구매", string.Format("-200 / 이후 보석 갯수 : {0}", CurrencyManager.instance.Jewel), "보석 사용 현황");

                break;

            case 2:
                CurrencyManager.instance.Jewel -= 500;
                CurrencyManager.instance.Token += 550;
                // GameManager.SaveLogToServer("증표 구매", string.Format("-500 / 이후 보석 갯수 : {0}", CurrencyManager.instance.Jewel), "보석 사용 현황");

                break;

            case 3:
                CurrencyManager.instance.Jewel -= 1000;
                CurrencyManager.instance.Token += 1200;
                // GameManager.SaveLogToServer("증표 구매", string.Format("-1000 / 이후 보석 갯수 : {0}", CurrencyManager.instance.Jewel), "보석 사용 현황");

                break;

            default:
                break;
        }
        BuyPanelSetting(buyButtons_Token, priceTexts_Token);
        BuyPanelSetting(buyButtons_Entrance, priceTexts_Entrance);
    }

    public void OnClickButEntranceTicket(int index)
    {
        switch (index)
        {
            case 0:
                CurrencyManager.instance.Jewel -= 50;
                CurrencyManager.instance.Ticket_BossDungeon += 1;
                //GameManager.SaveLogToServer("보스던전 티켓 1매 구매", string.Format("-50 / 이후 보석 갯수 : {0}", CurrencyManager.instance.Jewel), "보석 사용 현황");

                break;

            case 1:
                CurrencyManager.instance.Jewel -= 500;
                CurrencyManager.instance.Ticket_BossDungeon += 10;
                //GameManager.SaveLogToServer("보스던전 티켓 10매 구매", string.Format("-500 / 이후 보석 갯수 : {0}", CurrencyManager.instance.Jewel), "보석 사용 현황");

                break;

            default:
                break;
        }
        //다시 던전 입장쪽 보석 금액에 맞춰서 세팅.
        BuyPanelSetting(buyButtons_Token, priceTexts_Token);
        BuyPanelSetting(buyButtons_Entrance, priceTexts_Entrance);
    }

    private void BuyPanelSetting(Button[] buttons, Text[] priceTexts)
    {
        int tp;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (int.TryParse(priceTexts[i].text, out tp))
            {
                if (tp <= CurrencyManager.instance.Jewel)
                    buttons[i].interactable = true;
                else
                    buttons[i].interactable = false;
            }
            else
                buttons[i].interactable = false;
        }

        //여기서 하루 구매횟수가 0인 애들 컨트롤하기
        rebirthCoupon_dailyCountText.text = string.Format("일일 구매 갯수 ({0}/{1})", CentralInfoManager.rebirthCoupon_DailyCount, ReturnShopDaily("SH200"));
        lpPotion_dailyCountText.text = string.Format("일일 구매 갯수 ({0}/{1})", CentralInfoManager.lpPotion_DailyCount, ReturnShopDaily("SH202"));

        //버튼도 못눌리게해야함.
        if (CentralInfoManager.rebirthCoupon_DailyCount <= 0)
            rebirthCouponBuyBtn.interactable = false;
        if (CentralInfoManager.lpPotion_DailyCount <= 0)
            lpPotionBuyBtn.interactable = false;
    }

    public void OnClickItem(int index)
    {
        switch (index)
        {
            case 0:
                CurrencyManager.instance.Jewel -= 500;
                CentralInfoManager.ticket_nickNameChange += 1;
                // GameManager.SaveLogToServer("닉네임 변경권 구매", string.Format("-500 / 이후 보석 갯수 : {0}", CurrencyManager.instance.Jewel), "보석 사용 현황");

                break;

            case 1:
                CurrencyManager.instance.Jewel -= ReturnShopPrice("SH200");
                CentralInfoManager.rebirthCoupon += ReturnShopGiftCount("SH200");
                CentralInfoManager.rebirthCoupon_DailyCount--;
                GameManager.SaveLogToServer("전승권 구매 후 갯수", CentralInfoManager.rebirthCoupon.ToString(), "유료 상품 구매");
                //GameManager.SaveLogToServer("전승권 구매", string.Format("-{0} / 이후 보석 갯수 : {1}", ReturnShopPrice("SH200"), CurrencyManager.instance.Jewel), "보석 사용 현황");

                break;

            case 2:
                CurrencyManager.instance.Jewel -= ReturnShopPrice("SH201");
                CentralInfoManager.anyOpenKey += ReturnShopGiftCount("SH201");
                //GameManager.SaveLogToServer("만능열쇠 구매", string.Format("-{0} / 이후 보석 갯수 : {1}", ReturnShopPrice("SH201"), CurrencyManager.instance.Jewel), "보석 사용 현황");

                break;

            case 3:
                CurrencyManager.instance.Jewel -= ReturnShopPrice("SH202");
                CentralInfoManager.lpPotion += ReturnShopGiftCount("SH202");
                GameManager.SaveLogToServer("LP포션 구매 후 갯수", CentralInfoManager.lpPotion.ToString(), "유료 상품 구매");
                // GameManager.SaveLogToServer("LP포션 구매", string.Format("-{0} / 이후 보석 갯수 : {1}", ReturnShopPrice("SH202"), CurrencyManager.instance.Jewel), "보석 사용 현황");

                CentralInfoManager.lpPotion_DailyCount--;
                break;

            case 4:
                CurrencyManager.instance.Jewel -= 100;
                SpeedManager.instance.OnClickPayJewel();
                break;

            default:
                break;
        }
        BuyPanelSetting(buyButtons_Token, priceTexts_Token);
        BuyPanelSetting(buyButtons_Entrance, priceTexts_Entrance);
    }

    // Return Shop Table Data
    public string ReturnShopName(string shopIndex)
    {
        var res = "";
        for (int i = 0; i < TableData.Shop_Index.Count; i++)
        {
            if (TableData.Shop_Index[i].Equals(shopIndex))
                res = TableData.Shop_Name[i];
        }
        return res;
    }

    public string ReturnShopExplain(string shopIndex)
    {
        var res = "";
        for (int i = 0; i < TableData.Shop_Index.Count; i++)
        {
            if (TableData.Shop_Index[i].Equals(shopIndex))
                res = TableData.Shop_Explain[i];
        }
        return res;
    }

    public int ReturnShopGiftCount(string shopIndex)
    {
        var res = 0;
        for (int i = 0; i < TableData.Shop_Index.Count; i++)
        {
            if (TableData.Shop_Index[i].Equals(shopIndex))
                res = TableData.Shop_Gift_Count[i];
        }
        return res;
    }

    public int ReturnShopDaily(string shopIndex)
    {
        var res = 0;
        for (int i = 0; i < TableData.Shop_Index.Count; i++)
        {
            if (TableData.Shop_Index[i].Equals(shopIndex))
                res = TableData.Shop_Daily[i];
        }
        return res;
    }

    public int ReturnShopPrice(string shopIndex)
    {
        var res = 0;
        for (int i = 0; i < TableData.Shop_Index.Count; i++)
        {
            if (TableData.Shop_Index[i].Equals(shopIndex))
                res = TableData.Shop_Price[i];
        }
        return res;
    }
}