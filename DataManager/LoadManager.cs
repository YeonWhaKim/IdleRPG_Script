using BackEnd;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{
    public static LoadManager instance;
    public GPGSLogin gPGSLogin;
    public bool isDataLoaded = false;
    public bool isCharacterLoaded = false;
    public bool isEquipLoaded = false;
    public GameObject fade;
    public Text progressText;
    private string textst;
    private string characterIndate;

    public SaveManager saveManager;

    [Header("character")]
    public CharacterStatus characterStatus;

    public TutorialManager tutorialManager;

    [Header("equip")]
    public EquipManager equipManager;

    public CentralInfoManager centralInfoManager;

    [Header("currency")]
    public CurrencyManager currencyManager;

    [Header("item")]
    public PetManager petManager;

    public PetMenu petMenu;
    public ShopMenu shopMenu;

    [Header("setting")]
    public BuffManager buffManager;         //

    public SpeedManager speedManager;       //
    public SettingManager settingManager;

    private List<bool> isReadyDone = new List<bool>();

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        if (isReadyDone.Count > 0)
            isReadyDone.Clear();

        for (int i = 0; i < 10; i++)
        {
            isReadyDone.Add(false);
        }
        isDataLoaded = false;
    }

    public IEnumerator LoadData()
    {
        ProgressTextSet("모험 준비 중(8%)");

        Backend.GameInfo.GetPrivateContents("character", callback =>
       {
           if (callback.IsSuccess())
               GetGameInfo(0, callback.GetReturnValuetoJSON());
       });
        yield return new WaitUntil(() => isReadyDone[0]);
        ProgressTextSet("모험 준비 중(15%)");

        Backend.GameInfo.GetPrivateContents("currency", callback =>
       {
           if (callback.IsSuccess())
               GetGameInfo(1, callback.GetReturnValuetoJSON());
       });
        yield return new WaitUntil(() => isReadyDone[1]);
        ProgressTextSet("모험 준비 중(55%)");

        Backend.GameInfo.GetPrivateContents("setting", callback =>
       {
           if (callback.IsSuccess())
               GetGameInfo(2, callback.GetReturnValuetoJSON());
       });
        yield return new WaitUntil(() => isReadyDone[2]);
        ProgressTextSet("모험 준비 중(98%)");
    }

    private void GetGameInfo(int index, JsonData returnData)
    {
        if (returnData != null && returnData["rows"].Count > 0)
        {
            if (returnData.Keys.Contains("rows"))
            {
                JsonData row = returnData["rows"];
                switch (index)
                {
                    case 0:
                        GetData_character(row[0]);
                        break;

                    case 1:
                        GetData_currency(row[0]);
                        break;

                    case 2:
                        StartCoroutine(GetData_setting(row[0]));
                        break;

                    default:
                        break;
                }
            }
        }
        else
        {
            StartCoroutine(AtFirstDataSetting(false));
        }
    }

    public IEnumerator AtFirstDataSetting(bool isLoad)
    {
        yield return new WaitWhile(() => gPGSLogin.nickNameGO.activeSelf);
        yield return new WaitWhile(() => gPGSLogin.nickNameAvailableGO.activeSelf);

        if (isLoad)
            StartCoroutine(TableDataLoadManager.instance.Load(true));

        yield return new WaitUntil(() => TableDataLoadManager.instance.isLoadDone);

        SetGameData_Character();
        SetGameData_Currency();

        MessageManager.instance.OpenChatUI();
        SetGameData_Setting();
        StartCoroutine(saveManager.InsertData());
        MessageManager.instance.ChatHandlers();
        yield return new WaitUntil(() => saveManager.isInsert);

        yield return new WaitUntil(() => MessageManager.instance.message_allSetting);
        yield return GameManager.YieldInstructionCache.WaitForEndOfFrame;

        if (!NotificationManager.instance.isTheDayClose)
        {
            NotificationManager.instance.LoadNotification(true);
            yield return new WaitUntil(() => NotificationManager.instance.isLoadDone);
        }

        fade.SetActive(true);
        //isDataLoaded = true;
        CartoonManager.instance.CartoonOn();
    }

    // character 불러오기 & 세팅
    private void GetData_character(JsonData data)
    {
        for (int i = 0; i < CentralInfoManager.EQUIPKIND; i++)
        {
            centralInfoManager.equipLook.Add(int.Parse(data["equipLook"]["L"][i]["N"].ToString()));
        }
        for (int i = 0; i < TableData.Item_Name.Count / CentralInfoManager.EQUIPKIND; i++)
        {
            centralInfoManager.weapon_Level.Add(int.Parse(data["weapon_Level"]["L"][i]["N"].ToString()));
            centralInfoManager.head_Level.Add(int.Parse(data["head_Level"]["L"][i]["N"].ToString()));
            centralInfoManager.chest_Level.Add(int.Parse(data["chest_Level"]["L"][i]["N"].ToString()));
            centralInfoManager.gloves_Level.Add(int.Parse(data["gloves_Level"]["L"][i]["N"].ToString()));
            centralInfoManager.pants_Level.Add(int.Parse(data["pants_Level"]["L"][i]["N"].ToString()));
            centralInfoManager.shoes_Level.Add(int.Parse(data["shoes_Level"]["L"][i]["N"].ToString()));
            centralInfoManager.back_Level.Add(int.Parse(data["back_Level"]["L"][i]["N"].ToString()));
            centralInfoManager.face_Level.Add(int.Parse(data["face_Level"]["L"][i]["N"].ToString()));

            equipManager.equipState_weapon.Add(int.Parse(data["equipState_weapon"]["L"][i]["N"].ToString()));
            equipManager.equipState_head.Add(int.Parse(data["equipState_head"]["L"][i]["N"].ToString()));
            equipManager.equipState_chest.Add(int.Parse(data["equipState_chest"]["L"][i]["N"].ToString()));
            equipManager.equipState_gloves.Add(int.Parse(data["equipState_gloves"]["L"][i]["N"].ToString()));
            equipManager.equipState_pants.Add(int.Parse(data["equipState_pants"]["L"][i]["N"].ToString()));
            equipManager.equipState_shoes.Add(int.Parse(data["equipState_shoes"]["L"][i]["N"].ToString()));
            equipManager.equipState_back.Add(int.Parse(data["equipState_back"]["L"][i]["N"].ToString()));
            equipManager.equipState_face.Add(int.Parse(data["equipState_face"]["L"][i]["N"].ToString()));

            equipManager.weaponStat.Add(decimal.Parse(data["weaponStat"]["L"][i]["N"].ToString()));
            equipManager.headStat.Add(decimal.Parse(data["headStat"]["L"][i]["N"].ToString()));
            equipManager.chestStat.Add(decimal.Parse(data["chestStat"]["L"][i]["N"].ToString()));
            equipManager.glovesStat.Add(decimal.Parse(data["glovesStat"]["L"][i]["N"].ToString()));
            equipManager.pantsStat.Add(decimal.Parse(data["pantsStat"]["L"][i]["N"].ToString()));
            equipManager.shoesStat.Add(decimal.Parse(data["shoesStat"]["L"][i]["N"].ToString()));
            equipManager.backStat.Add(decimal.Parse(data["backStat"]["L"][i]["N"].ToString()));
            equipManager.faceStat.Add(decimal.Parse(data["faceStat"]["L"][i]["N"].ToString()));
        }
        isEquipLoaded = true;
        characterStatus.Rebirth = int.Parse(data["rebirth"][0].ToString());
        characterStatus.Level = int.Parse(data["level"][0].ToString());
        //characterStatus.Rebirth = 0;
        //characterStatus.Level = 1;

        characterStatus.MaxExpForLevelUp = TableData.Level_Exp[characterStatus.Level - 1];
        characterStatus.CurrExp = long.Parse(data["currExp"][0].ToString());
        //characterStatus.CurrExp = 0;
        characterStatus.LP = int.Parse(data["lp"][0].ToString());
        characterStatus.Strength_Auto = long.Parse(data["strength_auto"][0].ToString());
        characterStatus.Agility_Auto = long.Parse(data["agility_auto"][0].ToString());
        characterStatus.Dex_Auto = long.Parse(data["dex_auto"][0].ToString());
        characterStatus.Lucky_Auto = long.Parse(data["lucky_auto"][0].ToString());

        characterStatus.Strength_LP = long.Parse(data["strength_lp"][0].ToString());
        characterStatus.Agility_LP = long.Parse(data["agility_lp"][0].ToString());
        characterStatus.Dex_LP = long.Parse(data["dex_lp"][0].ToString());
        characterStatus.Lucky_LP = long.Parse(data["lucky_lp"][0].ToString());

        centralInfoManager.isBossDungeonEnter = bool.Parse(data["isBossDungeonEnter"][0].ToString());
        centralInfoManager.isEquipUpgraded = bool.Parse(data["isEquipUpgraded"][0].ToString());
        tutorialManager.isAtkBuffTutorial = bool.Parse(data["isAtkBuffTutorial"][0].ToString());
        tutorialManager.isGoldBuffTutorial = bool.Parse(data["isGoldBuffTutorial"][0].ToString());
        saveManager.indate_cha = data["inDate"][0].ToString();

        isCharacterLoaded = true;
        RankManager.myStat = double.Parse(data["myStat"][0].ToString());
        RankManager.accumulatedStage = double.Parse(data["accumulatedStage"][0].ToString());
        RankManager.bestStage = double.Parse(data["bestStage"][0].ToString());
        RankManager.bossDungeonStage = double.Parse(data["bossDungeonStage"][0].ToString());
        RankManager.rebirthCount = double.Parse(data["rebirthCount"][0].ToString());

        isReadyDone[0] = true;
    }

    private void SetGameData_Character()
    {
        for (int i = 0; i < CentralInfoManager.EQUIPKIND; i++)
        {
            centralInfoManager.equipLook.Add(0);
        }

        for (int i = 0; i < TableData.Item_Name.Count / CentralInfoManager.EQUIPKIND; i++)
        {
            centralInfoManager.weapon_Level.Add(1);
            centralInfoManager.head_Level.Add(1);
            centralInfoManager.chest_Level.Add(1);
            centralInfoManager.gloves_Level.Add(1);
            centralInfoManager.pants_Level.Add(1);
            centralInfoManager.shoes_Level.Add(1);
            centralInfoManager.back_Level.Add(1);
            centralInfoManager.face_Level.Add(1);

            equipManager.equipState_weapon.Add(-1);
            equipManager.equipState_head.Add(-1);
            equipManager.equipState_chest.Add(-1);
            equipManager.equipState_gloves.Add(-1);
            equipManager.equipState_pants.Add(-1);
            equipManager.equipState_shoes.Add(-1);
            equipManager.equipState_back.Add(-1);
            equipManager.equipState_face.Add(-1);

            equipManager.weaponStat.Add(TableData.Item_Status_First[equipManager.ReturnRealIndex(0, i)]);
            equipManager.headStat.Add(TableData.Item_Status_First[equipManager.ReturnRealIndex(1, i)]);
            equipManager.chestStat.Add(TableData.Item_Status_First[equipManager.ReturnRealIndex(2, i)]);
            equipManager.glovesStat.Add(TableData.Item_Status_First[equipManager.ReturnRealIndex(3, i)]);
            equipManager.pantsStat.Add(TableData.Item_Status_First[equipManager.ReturnRealIndex(4, i)]);
            equipManager.shoesStat.Add(TableData.Item_Status_First[equipManager.ReturnRealIndex(5, i)]);
            equipManager.backStat.Add(TableData.Item_Status_First[equipManager.ReturnRealIndex(6, i)]);
            equipManager.faceStat.Add(TableData.Item_Status_First[equipManager.ReturnRealIndex(7, i)]);
        }

        equipManager.equipState_weapon[0] = 1;
        equipManager.equipState_head[0] = 1;
        equipManager.equipState_chest[0] = 1;
        equipManager.equipState_gloves[0] = 1;
        equipManager.equipState_pants[0] = 1;
        equipManager.equipState_shoes[0] = 1;
        equipManager.equipState_back[0] = 1;
        equipManager.equipState_face[0] = 1;

        characterStatus.Rebirth = 0;
        characterStatus.Level = 1;
        characterStatus.MaxExpForLevelUp = TableData.Level_Exp[characterStatus.Level - 1];
        characterStatus.CurrExp = 0;

        characterStatus.LP = 0;
        characterStatus.Strength_Auto = 1;
        characterStatus.Agility_Auto = 0;
        characterStatus.Dex_Auto = 0;
        characterStatus.Lucky_Auto = 0;

        characterStatus.Strength_LP = 0;
        characterStatus.Agility_LP = 0;
        characterStatus.Dex_LP = 0;
        characterStatus.Lucky_LP = 0;

        centralInfoManager.isBossDungeonEnter = false;
        centralInfoManager.isEquipUpgraded = false;
        tutorialManager.isAtkBuffTutorial = true;
        tutorialManager.isGoldBuffTutorial = true;

        RankManager.myStat = 0;
        RankManager.accumulatedStage = 1;
        RankManager.bestStage = 1;
        RankManager.bossDungeonStage = 1;
        RankManager.rebirthCount = 0;
    }

    //// currency 불러오기 & 세팅
    private void GetData_currency(JsonData data)
    {
        currencyManager.Gold = decimal.Parse(data["gold"][0].ToString());
        currencyManager.Jewel = long.Parse(data["jewel"][0].ToString());
        currencyManager.Token = long.Parse(data["token"][0].ToString());
        /////// 각 재화 뻥튀기
        //currencyManager.Gold += 100000000000;
        //currencyManager.Jewel += 100000;            /////////////////////////////////////////////////////
        //currencyManager.Token += 100000;
        if (currencyManager.Jewel <= 0)
            currencyManager.Jewel = 0;
        if (currencyManager.Token <= 0)
            currencyManager.Token = 0;

        for (int i = 0; i < CentralInfoManager.NUMBEROFBOXKEYKINDS; i++)
        {
            CentralInfoManager.boxCountList.Add(int.Parse(data["boxCountList"]["L"][i]["N"].ToString()));
            CentralInfoManager.keyCountList.Add(int.Parse(data["keyCountList"]["L"][i]["N"].ToString()));
            if (CentralInfoManager.keyCountList[i] < 0)
                CentralInfoManager.keyCountList[i] = 0;
        }

        CentralInfoManager.keyCountList[1] = 5;     ////////////////////////////////////////

        for (int i = 0; i < PetManager.PETKINDS; i++)
        {
            petManager.petLevel.Add(int.Parse(data["petLevel"]["L"][i]["N"].ToString()));
            petManager.petStat.Add(float.Parse(data["petStat"]["L"][i]["N"].ToString()));
            petManager.isPetBuy.Add(bool.Parse(data["isPetBuy"]["L"][i]["S"].ToString()));
        }

        petMenu.mountingPetIndex = int.Parse(data["mountingPetIndex"][0].ToString());
        CentralInfoManager.ticket_nickNameChange = int.Parse(data["ticket_nickNameChange"][0].ToString());
        CentralInfoManager.rebirthCoupon = int.Parse(data["rebirthCoupon"][0].ToString());
        CentralInfoManager.anyOpenKey = int.Parse(data["anyOpenKey"][0].ToString());
        CentralInfoManager.lpPotion = int.Parse(data["lpPotion"][0].ToString());

        CentralInfoManager.rebirthCoupon_DailyCount = int.Parse(data["rebirthCoupon_DailyCount"][0].ToString());
        CentralInfoManager.lpPotion_DailyCount = int.Parse(data["lpPotion_DailyCount"][0].ToString());
        centralInfoManager.monthlyJewelProductBuyDate = data["monthlyJewelProductBuyDate"][0].ToString();
        centralInfoManager.boxOpenCoolTime_ad = int.Parse(data["boxOpenCoolTime_ad"][0].ToString());
        centralInfoManager.boxOpenCount_ad = int.Parse(data["boxOpenCount_ad"][0].ToString());

        var dic = data as IDictionary;
        if (!dic.Contains("monthlyJewelPayments"))
            centralInfoManager.monthlyJewelPayments = false;
        else
            centralInfoManager.monthlyJewelPayments = bool.Parse(data["monthlyJewelPayments"][0].ToString());

        if (!centralInfoManager.monthlyJewelProductBuyDate.Equals(""))
        {
            var savedDay = Convert.ToDateTime(centralInfoManager.monthlyJewelProductBuyDate);
            TimeSpan dateDiff = Convert.ToDateTime(DateTime.Now) - savedDay;
            var diffDay = dateDiff.Days;

            if (diffDay > CentralInfoManager.MonthlyProduct_PERIOD)
            {
                centralInfoManager.monthlyJewelProductBuyDate = "";
                shopMenu.MonthlyProductSetting("", false);
            }
            else
            {
                shopMenu.MonthlyProductSetting(diffDay.ToString(), true);
                if (centralInfoManager.monthlyJewelPayments.Equals(false))
                {
                    currencyManager.Jewel += CentralInfoManager.MonthlyProduct_DAILYREWARD;
                    centralInfoManager.monthlyJewelPayments = true;
                    GameManager.SaveLogToServer("받은 날 정보", string.Format("{0}일차 / {1} 구매", diffDay, centralInfoManager.monthlyJewelProductBuyDate), "월간 상품 보석 획득");
                }
            }
        }
        else
            shopMenu.MonthlyProductSetting("", false);

        saveManager.indate_currency = data["inDate"][0].ToString();
        isReadyDone[1] = true;
    }

    private void SetGameData_Currency()
    {
        currencyManager.Gold = 0;
        currencyManager.Jewel = 0;
        currencyManager.Token = 0;

        for (int i = 0; i < CentralInfoManager.NUMBEROFBOXKEYKINDS; i++)
        {
            CentralInfoManager.boxCountList.Add(0);
            CentralInfoManager.keyCountList.Add(0);
        }
        for (int i = 0; i < PetManager.PETKINDS; i++)
        {
            petManager.petLevel.Add(0);
            petManager.petStat.Add(0);
            petManager.isPetBuy.Add(false);
        }

        petMenu.mountingPetIndex = -1;
        CentralInfoManager.ticket_nickNameChange = 0;
        CentralInfoManager.rebirthCoupon = 0;
        CentralInfoManager.anyOpenKey = 0;
        CentralInfoManager.lpPotion = 0;
        centralInfoManager.boxOpenCount_ad = 0;
        centralInfoManager.boxOpenCoolTime_ad = CentralInfoManager.BoxOpenCoolTime_STANDARD;
        CentralInfoManager.rebirthCoupon_DailyCount = shopMenu.ReturnShopDaily("SH200");
        CentralInfoManager.lpPotion_DailyCount = shopMenu.ReturnShopDaily("SH202");
        centralInfoManager.monthlyJewelProductBuyDate = "";
        centralInfoManager.monthlyJewelPayments = false;
    }

    //// setting 불러오기 & 세팅
    private IEnumerator GetData_setting(JsonData data)
    {
        speedManager.speedDuration = int.Parse(data["speedDuration"][0].ToString());
        speedManager.speed = float.Parse(data["speed"][0].ToString());
        for (int i = 0; i < BuffManager.BUFF_KINDS; i++)
        {
            buffManager.buffCoolTime.Add(int.Parse(data["buffCoolTime"]["L"][i]["N"].ToString()));
            buffManager.buffCoolTimeStandard.Add(int.Parse(data["buffCoolTimeStandard"]["L"][i]["N"].ToString()));
            buffManager.isBuffing.Add(bool.Parse(data["isBuffing"]["L"][i]["S"].ToString()));
            centralInfoManager.isBuffAdRemoved.Add(bool.Parse(data["isBuffAdRemoved"]["L"][i]["S"].ToString()));
        }

        saveManager.indate_setting = data["inDate"][0].ToString();

        CentralInfoManager.stageCount = long.Parse(data["stageCount"][0].ToString());
        CentralInfoManager.regionCount = long.Parse(data["regionCount"][0].ToString());
        CentralInfoManager.bestStage = long.Parse(data["bestStage"][0].ToString());
        var dic = data as IDictionary;
        if (!dic.Contains("bossStageCount"))
            CentralInfoManager.bossStageCount = 1;
        else
            CentralInfoManager.bossStageCount = int.Parse(data["bossStageCount"][0].ToString());

        if (!dic.Contains("isTheDayClose"))
            NotificationManager.instance.isTheDayClose = false;
        else
            NotificationManager.instance.isTheDayClose = bool.Parse(data["isTheDayClose"][0].ToString());

        if (!dic.Contains("rebirthStartStage"))
            CentralInfoManager.rebirthStartStage = 1;
        else
            CentralInfoManager.rebirthStartStage = long.Parse(data["rebirthStartStage"][0].ToString());

        if (!dic.Contains("isHotTimePush_lunch"))
            CentralInfoManager.isHotTimePush_lunch = false;
        else
            CentralInfoManager.isHotTimePush_lunch = bool.Parse(data["isHotTimePush_lunch"][0].ToString());

        // CentralInfoManager.isHotTimePush_lunch = false;  /////////////////////////////////////////////////////////////////////////

        if (!dic.Contains("isHotTimePush_afternoon"))
            CentralInfoManager.isHotTimePush_afternoon = false;
        else
            CentralInfoManager.isHotTimePush_afternoon = bool.Parse(data["isHotTimePush_afternoon"][0].ToString());

        currencyManager.Ticket_BossDungeon = double.Parse(data["ticket_bossDungeon"][0].ToString());
        CentralInfoManager.boss_adCount = int.Parse(data["boss_adCount"][0].ToString());
        CentralInfoManager.boss_day = int.Parse(data["boss_day"][0].ToString());
        if (!dic.Contains("isDungeonConquer"))
            CentralInfoManager.isDungeonConquer = false;
        else
            CentralInfoManager.isDungeonConquer = bool.Parse(data["isDungeonConquer"][0].ToString());

        if (CentralInfoManager.boss_day != int.Parse(DateTime.Now.ToString("dd")))
        {
            CentralInfoManager.boss_adCount = 0;
            CentralInfoManager.rebirthCoupon_DailyCount = shopMenu.ReturnShopDaily("SH200");
            CentralInfoManager.lpPotion_DailyCount = shopMenu.ReturnShopDaily("SH202");
            centralInfoManager.boxOpenCount_ad = 0;
            centralInfoManager.monthlyJewelPayments = false;
            CentralInfoManager.bossStageCount = 1;
            CentralInfoManager.isHotTimePush_lunch = false;
            CentralInfoManager.isHotTimePush_afternoon = false;
            CentralInfoManager.isDungeonConquer = false;
            NotificationManager.instance.isTheDayClose = false;
        }
        isReadyDone[2] = true;
#if UNITY_EDITOR
        fade.SetActive(true);
        if (!NotificationManager.instance.isTheDayClose)
            NotificationManager.instance.LoadNotification(true);
        isDataLoaded = true;
        yield return GameManager.YieldInstructionCache.WaitForEndOfFrame;
#elif UNITY_ANDROID
        MessageManager.instance.OpenChatUI();
        yield return new WaitUntil(() => MessageManager.instance.message_allSetting);

        MessageManager.instance.ChatHandlers();
        yield return GameManager.YieldInstructionCache.WaitForEndOfFrame;

        if (!NotificationManager.instance.isTheDayClose)
        {
            NotificationManager.instance.LoadNotification(true);
            yield return new WaitUntil(() => NotificationManager.instance.isLoadDone);
        }
        fade.SetActive(true);
        isDataLoaded = true;
#endif
    }

    private void SetGameData_Setting()
    {
        speedManager.speedDuration = 0;
        speedManager.speed = 0;
        for (int i = 0; i < BuffManager.BUFF_KINDS; i++)
        {
            buffManager.buffCoolTime.Add(0);
            buffManager.buffCoolTimeStandard.Add(BuffManager.BUFFDURATION);
            buffManager.isBuffing.Add(false);
            centralInfoManager.isBuffAdRemoved.Add(false);
        }

        CentralInfoManager.stageCount = 1;
        CentralInfoManager.rebirthStartStage = 1;
        CentralInfoManager.regionCount = 1;
        CentralInfoManager.bestStage = 1;

        currencyManager.Ticket_BossDungeon = 0;
        CentralInfoManager.boss_day = int.Parse(DateTime.Now.ToString("dd"));
        CentralInfoManager.boss_adCount = 0;
        CentralInfoManager.bossStageCount = 1;
        CentralInfoManager.isHotTimePush_lunch = false;
        CentralInfoManager.isHotTimePush_afternoon = false;
        CentralInfoManager.isDungeonConquer = false;
        NotificationManager.instance.isTheDayClose = false;
    }

    public void ProgressTextSet(string str)
    {
        textst = str;
    }

    public IEnumerator LoadingTextCO()
    {
        textst = "데이터 불러오는 중";
        while (isDataLoaded.Equals(false))
        {
            progressText.text = string.Format("{0}", textst);
            yield return GameManager.YieldInstructionCache.WaitForSeconds(0.5f);
            progressText.text = string.Format("{0}.", textst);
            yield return GameManager.YieldInstructionCache.WaitForSeconds(0.5f);

            progressText.text = string.Format("{0}..", textst);
            yield return GameManager.YieldInstructionCache.WaitForSeconds(0.5f);

            progressText.text = string.Format("{0}...", textst);
            yield return GameManager.YieldInstructionCache.WaitForSeconds(0.5f);
        }
    }
}