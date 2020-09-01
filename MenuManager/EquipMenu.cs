using System;
using UnityEngine;
using UnityEngine.UI;

public class EquipMenu : MonoBehaviour
{
    public static EquipMenu instance;
    public const int EquipSetCount = 30;
    private int whatKind_equipTab = 0;

    [Header("Tab")]
    public Image[] tabImages;

    public Text[] tabTexts;
    public Sprite ingSprite;
    public Sprite availableSprite;

    [Header("Panel")]
    public Transform equipParent;

    public Transform[] equipParentChild;
    public Sprite goldSprite;
    public Sprite tokenSprite;
    public Button[] mountButtons = new Button[EquipSetCount];
    public Text[] mountTexts = new Text[EquipSetCount];
    public Button[] levelUpButtons = new Button[EquipSetCount];
    public Text[] levelUpExpTexts = new Text[EquipSetCount];
    public Text[] levelUpPriceTexts = new Text[EquipSetCount];
    public Button[] buyButtons = new Button[EquipSetCount];
    public Image[] buyTypeImages = new Image[EquipSetCount];
    public Text[] buyPriceTexts = new Text[EquipSetCount];
    public Text[] nameAndStatTexts = new Text[EquipSetCount];
    public Text[] itemLevelTexts = new Text[EquipSetCount];
    public GameObject[] rockImages = new GameObject[EquipSetCount];
    public GameObject[] openJewelButton = new GameObject[EquipSetCount];
    public Text[] openJewelCountText = new Text[EquipSetCount];
    public Text[] rockTexts = new Text[EquipSetCount];
    public Image[] itemImages = new Image[EquipSetCount];
    public GameObject[] selectedImages = new GameObject[EquipSetCount];

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        EquipMenuUISetting();
    }

    private void Start()
    {
    }

    private void EquipMenuUISetting()
    {
        for (int i = 0; i < EquipSetCount; i++)
        {
            var mountButton = equipParent.GetChild(i).GetChild(0).GetChild(0);
            mountButtons[EquipSetCount - 1 - i] = mountButton.GetComponent<Button>();
            mountTexts[EquipSetCount - 1 - i] = mountButton.GetChild(0).GetComponent<Text>();

            var levelUpButton = equipParent.GetChild(i).GetChild(0).GetChild(1);
            levelUpButtons[EquipSetCount - 1 - i] = levelUpButton.GetComponent<Button>();
            levelUpExpTexts[EquipSetCount - 1 - i] = levelUpButton.GetChild(0).GetComponent<Text>();
            levelUpPriceTexts[EquipSetCount - 1 - i] = levelUpButton.GetChild(1).GetChild(0).GetComponent<Text>();

            var buyButton = equipParent.GetChild(i).GetChild(0).GetChild(2);
            buyButtons[EquipSetCount - 1 - i] = buyButton.GetComponent<Button>();
            buyTypeImages[EquipSetCount - 1 - i] = buyButton.GetChild(1).GetComponent<Image>();
            buyPriceTexts[EquipSetCount - 1 - i] = buyButton.GetChild(1).GetChild(0).GetComponent<Text>();

            nameAndStatTexts[EquipSetCount - 1 - i] = equipParent.GetChild(i).GetChild(0).GetChild(3).GetComponent<Text>();

            itemLevelTexts[EquipSetCount - 1 - i] = equipParent.GetChild(i).GetChild(0).GetChild(4).GetComponent<Text>();

            rockImages[EquipSetCount - 1 - i] = equipParent.GetChild(i).GetChild(0).GetChild(5).gameObject;
            rockTexts[EquipSetCount - 1 - i] = equipParent.GetChild(i).GetChild(0).GetChild(5).GetChild(0).GetComponent<Text>();
            openJewelButton[EquipSetCount - 1 - i] = equipParent.GetChild(i).GetChild(0).GetChild(5).GetChild(1).gameObject;
            openJewelCountText[EquipSetCount - 1 - i] = equipParent.GetChild(i).GetChild(0).GetChild(5).GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>();

            itemImages[EquipSetCount - 1 - i] = equipParent.GetChild(i).GetChild(1).GetChild(0).GetComponent<Image>();
            selectedImages[EquipSetCount - 1 - i] = equipParent.GetChild(i).GetChild(1).GetChild(1).gameObject;

            var index = EquipSetCount - 1 - i;
            mountButtons[index].onClick.AddListener(delegate { OnClickEquipMount(index); });
            levelUpButtons[index].onClick.AddListener(delegate { OnClickEquipLevelUp(index); });
            buyButtons[index].onClick.AddListener(delegate { OnClickEquipBuy(index); });
            buyButtons[index].onClick.AddListener(delegate { OnClickEquipBuy(index); });
            openJewelButton[index].GetComponent<Button>().onClick.AddListener(delegate { OnClickEquipUnLock(index); });
        }
    }

    public void OnClickEquipTab(int index)
    {
        whatKind_equipTab = index;
        //메뉴 텝 세팅하고
        for (int i = 0; i < tabImages.Length; i++)
        {
            if (i.Equals(index))
            {
                tabImages[i].sprite = ingSprite;
                tabTexts[i].color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 1);
            }
            else
            {
                tabImages[i].sprite = availableSprite;
                tabTexts[i].color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 0.5f);
            }
        }
        //해당 장비 패널 세팅
        PanelInfoSetting(index);
    }

    public void PanelInfoSetting(int index)
    {
        var etcIndex = 0;
        for (int i = 0; i < EquipSetCount; i++)
        {
            if (EquipManager.instance.ReturnEquipStateList(index)[i] >= 0)        ///////////////////////////////
            {
                itemImages[i].sprite = CentralInfoManager.instance.ReturnEquipSprite_equipMennu(index, int.Parse(TableData.Item_Look_Number[i]) - 1);
                nameAndStatTexts[i].text = string.Format("{0} {1}\n{2} : {3}",
                    EquipManager.instance.Return_EquipGrade(index, i),
                    EquipManager.instance.Return_EquipName(index, i),
                    EquipManager.instance.Return_EquipStatus(index, i),
                    EquipManager.instance.ReturnEquipStatValue(index, i, 0));
                EquipLevelSetting(index, i);
                EquipMountSetting(index, i);
                EquipLevelUpSetting(index, i);
                EquipBuySetting(index, i);
                EquipRockSetting(index, i);           /////////////////////////////
                etcIndex = i;
            }
            else
                break;
        }
        // 기본장비 세팅하기
        //try
        //{
        itemImages[etcIndex + 1].sprite = CentralInfoManager.instance.ReturnEquipSprite_equipMennu(index, int.Parse(TableData.Item_Look_Number[etcIndex + 1]) - 1);
        nameAndStatTexts[etcIndex + 1].text = string.Format("{0} {1}\n{2} : {3}",
            EquipManager.instance.Return_EquipGrade(index, etcIndex + 1),
            EquipManager.instance.Return_EquipName(index, etcIndex + 1),
            EquipManager.instance.Return_EquipStatus(index, etcIndex + 1),
            EquipManager.instance.ReturnEquipStatValue(index, etcIndex + 1, 0));
        EquipLevelSetting(index, etcIndex + 1);
        EquipMountSetting(index, etcIndex + 1);
        EquipLevelUpSetting(index, etcIndex + 1);
        EquipBuySetting(index, etcIndex + 1);
        EquipRockSetting(index, etcIndex + 1);
        //}
        //catch (Exception)
        //{
        //    throw;
        //}
        ///////////////////////////
        for (int i = 0; i < equipParent.childCount; i++)
        {
            if (i < equipParent.childCount - etcIndex - 2)
                equipParent.GetChild(i).gameObject.SetActive(false);
            else
                equipParent.GetChild(i).gameObject.SetActive(true);
        }
    }

    //잠금관련
    public void EquipRockSetting(int whatKind, int index)
    {
        if (EquipManager.instance.ReturnEquipState(whatKind, index) < 0)
            rockImages[index].SetActive(true);
        else
            rockImages[index].SetActive(false);
        rockTexts[index].text = EquipManager.instance.Return_EquipOpenCondition(whatKind, index);

        var gemOpenValue = EquipManager.instance.Return_EquipGemOpenValue(whatKind, index);
        if (gemOpenValue > 0)
        {
            openJewelButton[index].SetActive(true);
            openJewelCountText[index].text = gemOpenValue.ToString();
            if (CurrencyManager.instance.Jewel < gemOpenValue)
                openJewelButton[index].GetComponent<Button>().interactable = false;
            else
                openJewelButton[index].GetComponent<Button>().interactable = true;
        }
        else
            openJewelButton[index].SetActive(false);
    }

    public void OnClickEquipUnLock(int index)
    {
        var gemOpenValue = EquipManager.instance.Return_EquipGemOpenValue(whatKind_equipTab, index);
        EquipManager.instance.EquipUnLockJewel(whatKind_equipTab, index, gemOpenValue);
        PanelInfoSetting(whatKind_equipTab);
    }

    //장착관련
    private void OnClickEquipMount(int index)
    {
        CentralInfoManager.instance.equipLook[whatKind_equipTab] = index;
        CharacterLookSetting.instance.ChracterEquipSetting(false, whatKind_equipTab, int.Parse(TableData.Item_Look_Number[index]));
        for (int i = 0; i < EquipSetCount; i++)
        {
            EquipMountSetting(whatKind_equipTab, i);
        }
    }

    private void EquipMountSetting(int whatKind, int index)
    {
        if (CentralInfoManager.instance.equipLook[whatKind].Equals(index))
        {
            mountButtons[index].interactable = false;
            mountTexts[index].text = "장착중";
            selectedImages[index].SetActive(true);
        }
        else
        {
            mountButtons[index].interactable = true;
            mountTexts[index].text = "장착하기";
            selectedImages[index].SetActive(false);
        }
    }

    //레벨업 관련
    private void OnClickEquipLevelUp(int index)
    {
        if (TutorialManager.instance.isEquipTutorial)
            TutorialManager.instance.OnClickTutorialPanel_Equip();

        //금액 빼가고
        CurrencyManager.instance.Gold -= EquipManager.instance.Return_EquipLevelUpPrice(whatKind_equipTab, index, CentralInfoManager.instance.ReturnEquipLevel(whatKind_equipTab, index));
        //레벨업하고
        EquipManager.instance.EquipLevelUp_FirstBuy(false, whatKind_equipTab, index, 1);
        //메뉴판 다시 세팅
        EquipLevelUpSetting(whatKind_equipTab, index);
        EquipBuyButtonSetting(whatKind_equipTab);

        nameAndStatTexts[index].text = string.Format("{0} {1}\n{2} : {3}",
    EquipManager.instance.Return_EquipGrade(whatKind_equipTab, index),
    EquipManager.instance.Return_EquipName(whatKind_equipTab, index),
    EquipManager.instance.Return_EquipStatus(whatKind_equipTab, index),
    EquipManager.instance.ReturnEquipStatValue(whatKind_equipTab, index, 0));
        EquipLevelSetting(whatKind_equipTab, index);
    }

    private void EquipLevelUpSetting(int whatKind, int index)
    {
        var equipLevel = CentralInfoManager.instance.ReturnEquipLevel(whatKind, index);
        var maxLevel = EquipManager.instance.Return_EquipMaxLevel(whatKind, index);
        if (equipLevel >= maxLevel)
        {
            levelUpButtons[index].interactable = false;
            levelUpPriceTexts[index].text = "---";
            levelUpExpTexts[index].text = "Max";
        }
        else
        {
            var levelupPrice = EquipManager.instance.Return_EquipLevelUpPrice(whatKind, index, CentralInfoManager.instance.ReturnEquipLevel(whatKind, index));
            if (levelupPrice <= CurrencyManager.instance.Gold)
                levelUpButtons[index].interactable = true;
            else
                levelUpButtons[index].interactable = false;
            levelUpPriceTexts[index].text = GameManager.NumberNotation_Korean(levelupPrice, true);

            var upgrade = TableData.Item_Status_Upgrade[EquipManager.instance.ReturnRealIndex(whatKind, index)];
            if (!whatKind.Equals(0) && !whatKind.Equals(4))
            {
                upgrade *= 100;
                upgrade = Math.Round(upgrade, 1);
                if (whatKind.Equals(1) || whatKind.Equals(2) ||
                    whatKind.Equals(5) || whatKind.Equals(6))
                    upgrade = Math.Truncate(upgrade);
            }

            var upgrade_str = string.Format("{0}%", upgrade);
            if (whatKind.Equals(0)
                || whatKind.Equals(4)
                || whatKind.Equals(1)
                || whatKind.Equals(5))
                upgrade_str = upgrade.ToString();

            levelUpExpTexts[index].text = string.Format("{0}\n+{1}",
                 EquipManager.instance.Return_EquipStatus(whatKind, index), upgrade_str);
        }
    }

    private void EquipBuyButtonSetting(int whatKind)
    {
        for (int i = 0; i < EquipSetCount; i++)
        {
            if (EquipManager.instance.ReturnEquipStateList(whatKind)[i] >= 0)        ///////////////////////////////
            {
                var equipLevel = CentralInfoManager.instance.ReturnEquipLevel(whatKind, i);
                var maxLevel = EquipManager.instance.Return_EquipMaxLevel(whatKind, i);
                if (equipLevel >= maxLevel)
                {
                    levelUpButtons[i].interactable = false;
                    levelUpPriceTexts[i].text = "---";
                    levelUpExpTexts[i].text = "Max";
                }
                else
                {
                    var levelupPrice = EquipManager.instance.Return_EquipLevelUpPrice(whatKind, i, CentralInfoManager.instance.ReturnEquipLevel(whatKind, i));
                    if (levelupPrice <= CurrencyManager.instance.Gold)
                        levelUpButtons[i].interactable = true;
                    else
                        levelUpButtons[i].interactable = false;
                    levelUpPriceTexts[i].text = GameManager.NumberNotation_Korean(levelupPrice, true);

                    var upgrade = TableData.Item_Status_Upgrade[EquipManager.instance.ReturnRealIndex(whatKind, i)];
                    if (!whatKind.Equals(0) && !whatKind.Equals(4))
                    {
                        upgrade *= 100;
                        upgrade = Math.Round(upgrade, 1);
                        if (whatKind.Equals(1) || whatKind.Equals(2) ||
                            whatKind.Equals(5) || whatKind.Equals(6))
                            upgrade = Math.Truncate(upgrade);
                    }

                    var upgrade_str = string.Format("{0}%", upgrade);
                    if (whatKind.Equals(0)
                       || whatKind.Equals(4)
                       || whatKind.Equals(1)
                       || whatKind.Equals(5))
                        upgrade_str = upgrade.ToString();

                    levelUpExpTexts[i].text = string.Format("{0}\n+{1}",
                         EquipManager.instance.Return_EquipStatus(whatKind, i), upgrade_str);
                }

                var buyValue = 0;
                var buyType = EquipManager.instance.Return_EquipBuyInfo(whatKind, i, out buyValue);
                if (EquipManager.instance.ReturnEquipState(whatKind, i) > 0)
                    buyButtons[i].gameObject.SetActive(false);
                else
                {
                    buyButtons[i].gameObject.SetActive(true);
                    if (buyType.Equals("Token"))
                    {
                        buyTypeImages[i].sprite = tokenSprite;

                        if (buyValue <= CurrencyManager.instance.Token)
                            buyButtons[i].interactable = true;
                        else
                            buyButtons[i].interactable = false;
                        buyPriceTexts[i].text = GameManager.NumberNotation_comma((decimal)buyValue);
                    }
                    else
                    {
                        buyTypeImages[i].sprite = goldSprite;
                        if (buyValue <= CurrencyManager.instance.Gold)
                            buyButtons[i].interactable = true;
                        else
                            buyButtons[i].interactable = false;
                        buyPriceTexts[i].text = GameManager.NumberNotation_Korean(buyValue, true);
                    }
                }
            }
            else
                break;
        }
    }

    private void EquipLevelSetting(int whatKind, int index)
    {
        var equipLevel = CentralInfoManager.instance.ReturnEquipLevel(whatKind, index);
        var maxLevel = EquipManager.instance.Return_EquipMaxLevel(whatKind, index);
        if (equipLevel >= maxLevel)
            itemLevelTexts[index].text = string.Format("Lv.Max\n<size=20>({0})</size>", maxLevel);
        else
            itemLevelTexts[index].text = string.Format("Lv.{0}", CentralInfoManager.instance.ReturnEquipLevel(whatKind, index));
    }

    //구입하기 관련
    private void OnClickEquipBuy(int index)
    {
        //금액 빼가고]
        var buyValue = 0;
        var buyType = EquipManager.instance.Return_EquipBuyInfo(whatKind_equipTab, index, out buyValue);
        if (buyType.Equals("Token"))
            CurrencyManager.instance.Token -= buyValue;
        else
            CurrencyManager.instance.Gold -= buyValue;
        //구입하고
        EquipManager.instance.EquipLevelUp_FirstBuy(true, whatKind_equipTab, index, 1);
        //메뉴판 다시 세팅
        itemImages[index].sprite = CentralInfoManager.instance.ReturnEquipSprite_equipMennu(whatKind_equipTab, int.Parse(TableData.Item_Look_Number[index]) - 1);
        nameAndStatTexts[index].text = string.Format("{0} {1}\n{2} : {3}",
            EquipManager.instance.Return_EquipGrade(whatKind_equipTab, index),
            EquipManager.instance.Return_EquipName(whatKind_equipTab, index),
            EquipManager.instance.Return_EquipStatus(whatKind_equipTab, index),
            EquipManager.instance.ReturnEquipStatValue(whatKind_equipTab, index, 0));
        EquipLevelSetting(whatKind_equipTab, index);
        EquipMountSetting(whatKind_equipTab, index);
        EquipLevelUpSetting(whatKind_equipTab, index);
        EquipBuyButtonSetting(whatKind_equipTab);
        EquipBuySetting(whatKind_equipTab, index);
        EquipRockSetting(whatKind_equipTab, index);
    }

    private void EquipBuySetting(int whatKind, int index)
    {
        var buyValue = 0;
        var buyType = EquipManager.instance.Return_EquipBuyInfo(whatKind, index, out buyValue);
        if (EquipManager.instance.ReturnEquipState(whatKind, index) > 0)
            buyButtons[index].gameObject.SetActive(false);
        else
        {
            buyButtons[index].gameObject.SetActive(true);
            if (buyType.Equals("Token"))
            {
                buyTypeImages[index].sprite = tokenSprite;

                if (buyValue <= CurrencyManager.instance.Token)
                    buyButtons[index].interactable = true;
                else
                    buyButtons[index].interactable = false;
                buyPriceTexts[index].text = GameManager.NumberNotation_comma((decimal)buyValue);
            }
            else
            {
                buyTypeImages[index].sprite = goldSprite;
                if (buyValue <= CurrencyManager.instance.Gold)
                    buyButtons[index].interactable = true;
                else
                    buyButtons[index].interactable = false;
                buyPriceTexts[index].text = GameManager.NumberNotation_Korean(buyValue, true);
            }
        }
    }
}