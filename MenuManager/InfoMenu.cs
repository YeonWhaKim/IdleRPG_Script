using BackEnd;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoMenu : MonoBehaviour
{
    [Serializable]
    public class UI
    {
        public FormulaCollection formulacollection;
        public Text expText;
        public Image expBar;

        [Header("Nick Name")]
        public Text nickNameText;

        public Text totalStatsAndRankText;
        public Text ticket_nickNameChangeText;
        public Button nickNameChangeButton;

        [Header("My Info")]
        public Text levelText;

        public Button replayButton;
        public Text replayExpText;
        public Text replayGOText;
        public Text replayCouponText;
        public Text replayCountText;
        public Text lpText;
        public Text[] statusTexts_Auto;   // 나중에 각 스테이터스와 능력치별로.
        public Text[] statusTexts_Lp;
        public Text[] requiredLpPoints;
        public Text[] sumOfStats;
        public Button[] lpButtons;
        public Button lpPotionButton;
        public Text lpPotionCountText;

        [Header("Equipment")]
        public List<Image> equipIconSR;

        public List<GameObject> selectedImage;
        public Sprite[] defaultSprites;
        public Sprite[] defaultFrameSprites;
        public Image defaultFrame;
        public Text equipInfoText;
        public Text equipRank;
        public Text equipNextLevelInfoText;
        public List<Button> equipLevelUpButtons;
        public List<Text> equipLevelUpGoldText;

        public void ReplayExpSetting(int rebirthCount, bool cheaterDetected)
        {
            ReplayCouponSetting();
            if (rebirthCount >= TableData.Replay_On_Level.Count)
            {
                replayButton.interactable = false;
                replayExpText.text = string.Format("전승가능 Lv.--");
                replayCountText.text = string.Format("전승횟수Max\n({0})", GameManager.NumberNotation_comma((decimal)TableData.Replay_On_Level.Count));

                replayGOText.color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 0.6f);
                replayCouponText.color = new Vector4(0.4039216f, 0.8274511f, 0.7333333f, 0.6f);
                replayCountText.color = new Vector4(0.937255f, 0.7725491f, 0.3019608f, 0.6f);
                replayCountText.alignment = TextAnchor.LowerCenter;
            }
            else
            {
                replayButton.interactable = true;
                replayExpText.text = string.Format("전승가능 Lv.{0}", RebirthManager.instance.ReturnAvailableRebrithLevel(rebirthCount));
                replayCountText.text = string.Format("전승횟수 : {0}", CharacterStatus.instance.Rebirth);

                replayGOText.color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 1f);
                replayCouponText.color = new Vector4(0.4039216f, 0.8274511f, 0.7333333f, 1f);
                replayCountText.color = new Vector4(0.937255f, 0.7725491f, 0.3019608f, 1f);
                replayCountText.alignment = TextAnchor.MiddleCenter;
            }
            if (cheaterDetected) GameManager.SaveLogToServer("전승 Cheater", rebirthCount.ToString(), "CheaterDetector");
        }

        public void ReplayCouponSetting()
        {
            replayCouponText.text = string.Format("(전승권:{0})", CentralInfoManager.rebirthCoupon);
            if (CentralInfoManager.cheaterDetected) GameManager.SaveLogToServer("전승권 Cheater", CentralInfoManager.rebirthCoupon.ToString(), "CheaterDetector");
        }

        public void LPSetting(int lp, bool cheaterDetected)
        {
            lpText.text = lp.ToString();
            LPButtonSetting();
            if (cheaterDetected) GameManager.SaveLogToServer("LP Cheater", lp.ToString(), "CheaterDetector");
        }

        public void LPPotionSetting()
        {
            lpPotionCountText.text = CentralInfoManager.lpPotion.ToString();
            if (CentralInfoManager.cheaterDetected) GameManager.SaveLogToServer("LP포션 Cheater", CentralInfoManager.rebirthCoupon.ToString(), "CheaterDetector");

            if (CentralInfoManager.lpPotion <= 0)
                lpPotionButton.interactable = false;
            else
                lpPotionButton.interactable = true;
        }

        public void LevelSetting(bool isMaxLevel, int level, bool cheaterDetected)
        {
            if (isMaxLevel)
                levelText.text = string.Format("Lv.Max\n<size=20>({0})</size>", level);
            else
                levelText.text = string.Format("Lv.{0}", level);

            if (level >= RebirthManager.instance.ReturnAvailableRebrithLevel(CharacterStatus.instance.Rebirth))
                replayButton.GetComponent<Animator>().SetTrigger("IsRebirth");
            else
                replayButton.GetComponent<Animator>().Rebind();

            if (cheaterDetected) GameManager.SaveLogToServer("레벨 Cheater", level.ToString(), "CheaterDetector");
        }

        public void ExpSetting(bool isMaxLevel, long exp, long maxExp)
        {
            if (isMaxLevel)
            {
                expText.text = "Lv. Max";
                expBar.fillAmount = 1;
            }
            else
            {
                expText.text = string.Format("XP {0}/{1}", exp, maxExp);
                expBar.fillAmount = (float)exp / (float)maxExp;
            }
        }

        public void StatusSetting_Auto(int index, long statusValue, bool cheaterDetected)
        {
            statusTexts_Auto[index].text = statusValue.ToString();
            if (cheaterDetected) GameManager.SaveLogToServer("스테이터스 AUTO 수치 Cheater", string.Format("{0}-{1}", index, statusValue), "CheaterDetector");
        }

        public void StatusSetting_LP(int index, long statusValue, bool cheaterDetected)
        {
            statusTexts_Lp[index].text = string.Format("+{0}", statusValue.ToString());

            var lpvalue = FormulaCollection.RequiredLpPoint(statusValue);
            requiredLpPoints[index].text = lpvalue.ToString();

            LPButtonSetting();
            if (cheaterDetected) GameManager.SaveLogToServer("스테이터스 수동 수치 Cheater", string.Format("{0}-{1}", index, statusValue), "CheaterDetector");
        }

        public void LPButtonSetting()
        {
            for (int i = 0; i < lpButtons.Length; i++)
            {
                if (int.Parse(requiredLpPoints[i].text) <= instance.characterStatus.LP)
                    lpButtons[i].interactable = true;
                else
                    lpButtons[i].interactable = false;
            }
            if (CharacterStatus.instance.Strength_LP >= TableData.Status_Max[TableData.Status_Max.Count - 1])
                lpButtons[0].interactable = false;
            if (CharacterStatus.instance.Agility_LP >= TableData.Status_Max[TableData.Status_Max.Count - 1])
                lpButtons[1].interactable = false;
            if (CharacterStatus.instance.Dex_LP >= TableData.Status_Max[TableData.Status_Max.Count - 1])
                lpButtons[2].interactable = false;
            if (CharacterStatus.instance.Lucky_LP >= TableData.Status_Max[TableData.Status_Max.Count - 1])
                lpButtons[3].interactable = false;
        }

        public void SumOfSetting(int index, double statValue)
        {
            var res = GameManager.NumberNotation_comma(statValue);
            //공격력을 제외한 나머지는 %로
            if (index == 3
                || index == 2)
                res = string.Format("{0}%", GameManager.NumberNotation_comma(statValue * 100d));
            else if (index == 1)
                res = string.Format("{0}", GameManager.NumberNotation_comma(statValue * 100d));

            sumOfStats[index].text = res;
            totalStatsAndRankText.text = string.Format("전투력 : {0}", GameManager.NumberNotation_comma(formulacollection.ReturnTotalStats()));
        }

        public void EquipInfoChange(int whatkind, int equipIndex)
        {
            var level = CentralInfoManager.instance.ReturnEquipLevel(whatkind, equipIndex);
            var maxlevel = EquipManager.instance.Return_EquipMaxLevel(whatkind, equipIndex);
            var status = EquipManager.instance.Return_EquipStatus(whatkind, equipIndex);
            equipRank.text = EquipManager.instance.Return_EquipGrade(whatkind, equipIndex);

            if (level >= maxlevel)
            {
                equipInfoText.text = string.Format("<size=30>{0}</size>\n<color=#E4E3CE>Lv.Max({1})</color>\n<color=#6EE3C8>{2} : {3}</color>",
                   EquipManager.instance.Return_EquipName(whatkind, equipIndex),
                   maxlevel,
                   status,
                   EquipManager.instance.ReturnEquipStatValue(whatkind, equipIndex, 0));
                equipNextLevelInfoText.text = "Max";

                for (int i = 0; i < equipLevelUpGoldText.Count; i++)
                {
                    equipLevelUpGoldText[i].text = "---";
                    equipLevelUpButtons[i].interactable = false;
                }
            }
            else
            {
                equipInfoText.text = string.Format("<size=30>{0}</size>\n<color=#E4E3CE>Lv.{1}</color>\n<color=#6EE3C8>{2} : {3}</color>",
                    EquipManager.instance.Return_EquipName(whatkind, equipIndex),
                    level,
                    status,
                    EquipManager.instance.ReturnEquipStatValue(whatkind, equipIndex, 0));
                var price = EquipManager.instance.Return_EquipLevelUpPrice(whatkind, equipIndex, level);
                equipNextLevelInfoText.text = string.Format("<color=#F0DEB8>Lv.{0} → </color><color=#6EE3C8><b>{1}</b></color>\n<color=#F0DEB8>{2} : {3} → </color><color=#6EE3C8><b>{4}</b></color>",
                    level,
                    ++level,
                    status,
                     EquipManager.instance.ReturnEquipStatValue(whatkind, equipIndex, 0),
                     EquipManager.instance.ReturnEquipStatValue(whatkind, equipIndex, 1));

                equipLevelUpGoldText[0].text = GameManager.NumberNotation_Korean(price, true);

                if (price <= CurrencyManager.instance.Gold)
                    equipLevelUpButtons[0].interactable = true;
                else
                    equipLevelUpButtons[0].interactable = false;
            }
        }
    }

    [SerializeField] public UI ui;
    public static InfoMenu instance;
    public CentralInfoManager centralInfoManager;
    public CharacterStatus characterStatus;
    public FormulaCollection formulacollection;
    public LevelUpButton_InfoMenu levelupButton_infomenu;
    public ATKUpButton_LP atklp;
    public AGIUpButton_LP agilp;
    public DEXUpButton_LP dexlp;
    public LUKUpButton_LP luklp;
    public GameObject nickNameChangePopUp;
    public GameObject nickNameErrorGO;
    public GameObject nickNameAvailableGO;
    public InputField nickNameField;
    public GameObject equipInfo;
    public GameObject statInfo;
    public Text errorText;
    public Text availableText;
    public int selectedEquipIndex;
    public GameObject notAvailableRebirthPopUp;
    public Animator infoStatButtonAnimator;
    public GameObject infoStateNewGO;

    // Start is called before the first frame update
    private void OnEnable()
    {
        CentralInfoManager.instance.startSetting += StartSetting;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(ButtonPressedCO());
    }

    private void StartSetting()
    {
        InfoMenuSetting();
    }

    public void InfoMenuSetting()
    {
        ui.nickNameText.text = SaveManager.instance.nickName;
        ui.ticket_nickNameChangeText.text = string.Format("닉네임 변경하기\n({0}/1)", CentralInfoManager.ticket_nickNameChange);
        if (CentralInfoManager.ticket_nickNameChange < 1)
            ui.nickNameChangeButton.interactable = false;
        else
            ui.nickNameChangeButton.interactable = true;

        ui.totalStatsAndRankText.text = string.Format("전투력 : {0}", GameManager.NumberNotation_comma(formulacollection.ReturnTotalStats()));

        ui.LPPotionSetting();
        // equip 세팅
        EquipSetting();
        OnClickEquip(0);
        ui.LevelSetting(CharacterStatus.instance.isMaxLevel, CharacterStatus.instance.Level, false);
        ui.ReplayExpSetting(CharacterStatus.instance.Rebirth, false);
        statInfo.SetActive(false);
    }

    public void EquipSetting()
    {
        // 착용한 장비 보여주기
        for (int i = 0; i < ui.equipIconSR.Count - 1; i++)
        {
            ui.equipIconSR[i].sprite = CentralInfoManager.instance.ReturnEquipSprite(i, centralInfoManager.equipLook[i]);
        }
    }

    public void OnClickRebirth()
    {
        if (DungeonManager.instance.dungeon_state == DungeonManager.DUNGEON_STATE.BOSS)
        {
            notAvailableRebirthPopUp.SetActive(true);
            return;
        }
        if (CharacterStatus.instance.Level < RebirthManager.instance.ReturnAvailableRebrithLevel(CharacterStatus.instance.Rebirth))
        {
            if (CentralInfoManager.rebirthCoupon <= 0)
            {
                MenuManager.instance.OnClickMenuButton(6);
                ShopMenu.instance.OnClickMenuTab(1);
            }
            else
                RebirthManager.instance.RebirhPopUp(true);
        }
        else
        {
            RebirthManager.instance.RebirhPopUp(false);
            if (TutorialManager.instance.isRebirthTutorial == true)
                TutorialManager.instance.OnClickTutorialPanel_Rebirth();
        }
    }

    public void OnClickLpPotion()
    {
        var reward = UnityEngine.Random.Range(1, 11);
        CharacterStatus.instance.LP += reward;
        CentralInfoManager.lpPotion--;
        GameManager.SaveLogToServer("LP포션 사용 후 갯수", CentralInfoManager.lpPotion.ToString(), "유료 상품 사용");

        ui.LPPotionSetting();
    }

    public void OnClickEquip(int index)
    {
        if (index.Equals(8))
        {
            statInfo.SetActive(true);
            infoStateNewGO.SetActive(false);
            //ui.defaultFrame.sprite = ui.defaultFrameSprites[1];
            //ui.equipIconSR[ui.equipIconSR.Count - 1].sprite = ui.defaultSprites[1];

            //for (int i = 0; i < ui.selectedImage.Count; i++)
            //{
            //    ui.selectedImage[i].SetActive(false);
            //}
        }
        else
        {
            ui.defaultFrame.sprite = ui.defaultFrameSprites[0];
            ui.equipIconSR[ui.equipIconSR.Count - 1].sprite = ui.defaultSprites[0];
            selectedEquipIndex = index;
            ui.EquipInfoChange(index, centralInfoManager.equipLook[index]);
            equipInfo.SetActive(true);

            for (int i = 0; i < ui.selectedImage.Count; i++)
            {
                if (i.Equals(index))
                    ui.selectedImage[i].SetActive(true);
                else
                    ui.selectedImage[i].SetActive(false);
            }
        }
    }

    public void OnClickEquipLevelUp()
    {
        var level = CentralInfoManager.instance.ReturnEquipLevel(selectedEquipIndex, centralInfoManager.equipLook[selectedEquipIndex]);
        decimal price = EquipManager.instance.Return_EquipLevelUpPrice(selectedEquipIndex, centralInfoManager.equipLook[selectedEquipIndex], level);
        if (CurrencyManager.instance.Gold < price)
            return;

        CurrencyManager.instance.Gold -= price;
        EquipManager.instance.EquipLevelUp_FirstBuy(false, selectedEquipIndex, centralInfoManager.equipLook[selectedEquipIndex], 1);
        ui.EquipInfoChange(selectedEquipIndex, centralInfoManager.equipLook[selectedEquipIndex]);
    }

    //public void OnClickEquipLevelUp_10()
    //{
    //    var level = CentralInfoManager.instance.ReturnEquipLevel(selectedEquipIndex, centralInfoManager.equipLook[selectedEquipIndex]);
    //    decimal price = EquipManager.instance.Return_EquipLevelUpPrice(selectedEquipIndex, centralInfoManager.equipLook[selectedEquipIndex], level + instance.ReturnHowLevelUp(10));
    //    CurrencyManager.instance.Gold -= price;
    //    EquipManager.instance.EquipLevelUp_FirstBuy(false, selectedEquipIndex, centralInfoManager.equipLook[selectedEquipIndex], ReturnHowLevelUp(10));
    //    ui.EquipInfoChange(selectedEquipIndex, centralInfoManager.equipLook[selectedEquipIndex]);
    //}

    //public void OnClickEquipLevelUp_100()
    //{
    //    var level = CentralInfoManager.instance.ReturnEquipLevel(selectedEquipIndex, centralInfoManager.equipLook[selectedEquipIndex]);
    //    var price = EquipManager.instance.Return_EquipLevelUpPrice(selectedEquipIndex, centralInfoManager.equipLook[selectedEquipIndex], level + instance.ReturnHowLevelUp(100));
    //    CurrencyManager.instance.Gold -= price;
    //    EquipManager.instance.EquipLevelUp_FirstBuy(false, selectedEquipIndex, centralInfoManager.equipLook[selectedEquipIndex], ReturnHowLevelUp(100));
    //    ui.EquipInfoChange(selectedEquipIndex, centralInfoManager.equipLook[selectedEquipIndex]);
    //}

    public int ReturnHowLevelUp(int defaultUpValue)
    {
        var level = CentralInfoManager.instance.ReturnEquipLevel(selectedEquipIndex, centralInfoManager.equipLook[selectedEquipIndex]);
        var maxLevel = EquipManager.instance.Return_EquipMaxLevel(selectedEquipIndex, centralInfoManager.equipLook[selectedEquipIndex]);
        var jud = level + defaultUpValue - maxLevel;
        if (jud <= 0)
            return defaultUpValue;
        else
            return maxLevel - level;
    }

    public void OnClickGoEquipMenu()
    {
        MenuManager.instance.OnClickMenuButton(1);
        EquipMenu.instance.OnClickEquipTab(selectedEquipIndex);
    }

    public void OnClickLpUpButton(int index)
    {
        if (CharacterStatus.instance.LP < FormulaCollection.RequiredLpPoint(CharacterStatus.instance.ReturnStatusLP(index)))
            return;
        CharacterStatus.instance.IncreasedStatusWithLp(index);

        if (TutorialManager.instance.isRebirthTutorial)
            TutorialManager.instance.OnClickTutorialPanel_Rebirth();
    }

    public void OnClickNickNameChange()
    {
        nickNameChangePopUp.SetActive(true);
    }

    public void OnClickNickNameChange_Confirm()
    {
        BackendReturnObject cNickName = Backend.BMember.UpdateNickname(nickNameField.text.Trim());

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
            availableText.text = string.Format("'{0}'으로 닉네임이 변경되었습니다.", nickNameField.text.Trim());
            SaveManager.instance.nickName = nickNameField.text.Trim();
            nickNameAvailableGO.SetActive(true);
        }
    }

    public void OnClickNickNameChange_Success()
    {
        CentralInfoManager.ticket_nickNameChange -= 1;
        InfoMenuSetting();
    }

    public void OnClickNickNameTicketBuy()
    {
        MenuManager.instance.OnClickMenuButton(6);
        ShopMenu.instance.OnClickMenuTab(1);
    }

    private IEnumerator ButtonPressedCO()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.3f);
            if (levelupButton_infomenu.isBtnDown && ui.equipLevelUpButtons[0].interactable.Equals(true))
                OnClickEquipLevelUp();
            else if (atklp.isBtnDown)
                OnClickLpUpButton(0);
            else if (agilp.isBtnDown)
                OnClickLpUpButton(1);
            else if (dexlp.isBtnDown)
                OnClickLpUpButton(2);
            else if (luklp.isBtnDown)
                OnClickLpUpButton(3);
        }
    }
}