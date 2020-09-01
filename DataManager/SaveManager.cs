using BackEnd;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public GameObject restartPopUp_saving;

    [Header("character")]
    public CharacterStatus characterStatus;

    public TutorialManager tutorialManager;

    [Header("equip")]
    public EquipManager equipManager;

    public CentralInfoManager centralInfoManager;

    [Header("currency")]
    public CurrencyManager currencyManager;

    [Header("setting")]
    public BuffManager buffManager;         //

    public SpeedManager speedManager;       //
    public SettingManager settingManager;
    public PetMenu petMenu;                 //
    public PetManager petManager;           //

    public string nickName;
    public string indate_cha;
    public string indate_currency;
    public string indate_setting;

    public bool isInsert = false;
    public bool isInsert_item = false;
    public bool isInsert_rank = false;
    public bool isSaved = false;

    public bool saving = false;
    public bool notSaving = false;
    public int restartIndex = 0;

    private Coroutine retryPopUpCO;
    public GameObject retryPopUp;
    public Text retryText;

    // Start is called before the first frame update
    private void Start()
    {
        instance = this;
    }

    public IEnumerator InsertData()
    {
        //// character 정보 Update -> CentralinfoManager,
        Param param_character = new Param();

        param_character.Add("level", characterStatus.Level);
        param_character.Add("rebirth", characterStatus.Rebirth);
        param_character.Add("currExp", characterStatus.CurrExp);

        param_character.Add("lp", characterStatus.LP);
        param_character.Add("strength_auto", characterStatus.Strength_Auto);
        param_character.Add("agility_auto", characterStatus.Agility_Auto);
        param_character.Add("dex_auto", characterStatus.Dex_Auto);
        param_character.Add("lucky_auto", characterStatus.Lucky_Auto);

        param_character.Add("strength_lp", characterStatus.Strength_LP);
        param_character.Add("agility_lp", characterStatus.Agility_LP);
        param_character.Add("dex_lp", characterStatus.Dex_LP);
        param_character.Add("lucky_lp", characterStatus.Lucky_LP);

        param_character.Add("isBossDungeonEnter", centralInfoManager.isBossDungeonEnter);
        param_character.Add("isEquipUpgraded", centralInfoManager.isEquipUpgraded);
        param_character.Add("isAtkBuffTutorial", tutorialManager.isAtkBuffTutorial);
        param_character.Add("isGoldBuffTutorial", tutorialManager.isGoldBuffTutorial);

        //// equip 정보 Update-> CentralInfoManager, EnchantManager
        int[] equipLook = new int[CentralInfoManager.EQUIPKIND];

        var eachEquipCount = TableData.Item_Name.Count / CentralInfoManager.EQUIPKIND;

        int[] weapon_Level = new int[eachEquipCount];
        int[] head_Level = new int[eachEquipCount];
        int[] chest_Level = new int[eachEquipCount];
        int[] gloves_Level = new int[eachEquipCount];
        int[] pants_Level = new int[eachEquipCount];
        int[] shoes_Level = new int[eachEquipCount];
        int[] back_Level = new int[eachEquipCount];
        int[] face_Level = new int[eachEquipCount];

        int[] equipState_weapon = new int[eachEquipCount];
        int[] equipState_head = new int[eachEquipCount];
        int[] equipState_chest = new int[eachEquipCount];
        int[] equipState_gloves = new int[eachEquipCount];
        int[] equipState_pants = new int[eachEquipCount];
        int[] equipState_shoes = new int[eachEquipCount];
        int[] equipState_back = new int[eachEquipCount];
        int[] equipState_face = new int[eachEquipCount];

        double[] weaponStat = new double[eachEquipCount];
        double[] headStat = new double[eachEquipCount];
        double[] chestStat = new double[eachEquipCount];
        double[] glovesStat = new double[eachEquipCount];
        double[] pantsStat = new double[eachEquipCount];
        double[] shoesStat = new double[eachEquipCount];
        double[] backStat = new double[eachEquipCount];
        double[] faceStat = new double[eachEquipCount];

        for (int i = 0; i < CentralInfoManager.EQUIPKIND; i++)
        {
            equipLook[i] = centralInfoManager.equipLook[i];
        }
        for (int i = 0; i < eachEquipCount; i++)
        {
            weapon_Level[i] = centralInfoManager.weapon_Level[i];
            head_Level[i] = centralInfoManager.head_Level[i];
            chest_Level[i] = centralInfoManager.chest_Level[i];
            gloves_Level[i] = centralInfoManager.gloves_Level[i];
            pants_Level[i] = centralInfoManager.pants_Level[i];
            shoes_Level[i] = centralInfoManager.shoes_Level[i];
            back_Level[i] = centralInfoManager.back_Level[i];
            face_Level[i] = centralInfoManager.face_Level[i];

            equipState_weapon[i] = equipManager.equipState_weapon[i];
            equipState_head[i] = equipManager.equipState_head[i];
            equipState_chest[i] = equipManager.equipState_chest[i];
            equipState_gloves[i] = equipManager.equipState_gloves[i];
            equipState_pants[i] = equipManager.equipState_pants[i];
            equipState_shoes[i] = equipManager.equipState_shoes[i];
            equipState_back[i] = equipManager.equipState_back[i];
            equipState_face[i] = equipManager.equipState_face[i];

            weaponStat[i] = (double)equipManager.weaponStat[i];
            headStat[i] = (double)equipManager.headStat[i];
            chestStat[i] = (double)equipManager.chestStat[i];
            glovesStat[i] = (double)equipManager.glovesStat[i];
            pantsStat[i] = (double)equipManager.pantsStat[i];
            shoesStat[i] = (double)equipManager.shoesStat[i];
            backStat[i] = (double)equipManager.backStat[i];
            faceStat[i] = (double)equipManager.faceStat[i];
        }
        param_character.Add("equipLook", equipLook);

        param_character.Add("weapon_Level", weapon_Level);
        param_character.Add("head_Level", head_Level);
        param_character.Add("chest_Level", chest_Level);
        param_character.Add("gloves_Level", gloves_Level);
        param_character.Add("pants_Level", pants_Level);
        param_character.Add("shoes_Level", shoes_Level);
        param_character.Add("back_Level", back_Level);
        param_character.Add("face_Level", face_Level);

        param_character.Add("equipState_weapon", equipState_weapon);
        param_character.Add("equipState_head", equipState_head);
        param_character.Add("equipState_chest", equipState_chest);
        param_character.Add("equipState_gloves", equipState_gloves);
        param_character.Add("equipState_pants", equipState_pants);
        param_character.Add("equipState_shoes", equipState_shoes);
        param_character.Add("equipState_back", equipState_back);
        param_character.Add("equipState_face", equipState_face);

        param_character.Add("weaponStat", weaponStat);
        param_character.Add("headStat", headStat);
        param_character.Add("chestStat", chestStat);
        param_character.Add("glovesStat", glovesStat);
        param_character.Add("pantsStat", pantsStat);
        param_character.Add("shoesStat", shoesStat);
        param_character.Add("backStat", backStat);
        param_character.Add("faceStat", faceStat);

        // rank
        param_character.Add("accumulatedStage", RankManager.accumulatedStage);
        param_character.Add("bestStage", RankManager.bestStage);
        param_character.Add("myStat", RankManager.myStat);
        param_character.Add("bossDungeonStage", RankManager.bossDungeonStage);
        param_character.Add("rebirthCount", RankManager.rebirthCount);

        ////currency 정보 Update
        Param param_currency = new Param();
        param_currency.Add("gold", currencyManager.Gold.ToString());
        param_currency.Add("jewel", currencyManager.Jewel.ToString());
        param_currency.Add("token", currencyManager.Token.ToString());

        int[] boxCountList = new int[CentralInfoManager.NUMBEROFBOXKEYKINDS];
        int[] keyCountList = new int[CentralInfoManager.NUMBEROFBOXKEYKINDS];

        int[] petLevel = new int[PetManager.PETKINDS];
        double[] petStat = new double[PetManager.PETKINDS];
        string[] isPetBuy = new string[PetManager.PETKINDS];

        for (int i = 0; i < CentralInfoManager.NUMBEROFBOXKEYKINDS; i++)
        {
            boxCountList[i] = CentralInfoManager.boxCountList[i];
            keyCountList[i] = CentralInfoManager.keyCountList[i];
        }

        for (int i = 0; i < PetManager.PETKINDS; i++)
        {
            petLevel[i] = petManager.petLevel[i];
            petStat[i] = petManager.petStat[i];
            isPetBuy[i] = petManager.isPetBuy[i].ToString();
        }
        param_currency.Add("boxCountList", boxCountList);
        param_currency.Add("keyCountList", keyCountList);
        param_currency.Add("petLevel", petLevel);
        param_currency.Add("petStat", petStat);
        param_currency.Add("isPetBuy", isPetBuy);
        param_currency.Add("mountingPetIndex", petMenu.mountingPetIndex);
        param_currency.Add("ticket_nickNameChange", CentralInfoManager.ticket_nickNameChange);
        param_currency.Add("rebirthCoupon", CentralInfoManager.rebirthCoupon);
        param_currency.Add("anyOpenKey", CentralInfoManager.anyOpenKey);
        param_currency.Add("lpPotion", CentralInfoManager.lpPotion);
        param_currency.Add("rebirthCoupon_DailyCount", CentralInfoManager.rebirthCoupon_DailyCount);
        param_currency.Add("lpPotion_DailyCount", CentralInfoManager.lpPotion_DailyCount);
        param_currency.Add("monthlyJewelProductBuyDate", centralInfoManager.monthlyJewelProductBuyDate);
        param_currency.Add("monthlyJewelPayments", centralInfoManager.monthlyJewelPayments);
        param_currency.Add("boxOpenCount_ad", centralInfoManager.boxOpenCount_ad);
        param_currency.Add("boxOpenCoolTime_ad", centralInfoManager.boxOpenCoolTime_ad);

        ////setting 정보 Update
        Param param_setting = new Param();

        ////stage
        param_setting.Add("stageCount", CentralInfoManager.stageCount.ToString());
        param_setting.Add("rebirthStartStage", CentralInfoManager.rebirthStartStage.ToString());
        param_setting.Add("regionCount", CentralInfoManager.regionCount.ToString());
        param_setting.Add("bestStage", CentralInfoManager.bestStage.ToString());
        param_setting.Add("ticket_bossDungeon", currencyManager.Ticket_BossDungeon);
        param_setting.Add("boss_day", CentralInfoManager.boss_day);
        param_setting.Add("boss_adCount", CentralInfoManager.boss_adCount);
        param_setting.Add("bossStageCount", CentralInfoManager.bossStageCount);
        param_setting.Add("isHotTimePush_lunch", CentralInfoManager.isHotTimePush_lunch);
        param_setting.Add("isHotTimePush_afternoon", CentralInfoManager.isHotTimePush_afternoon);

        int[] buffCoolTime = new int[BuffManager.BUFF_KINDS];
        int[] buffCoolTimeStandard = new int[BuffManager.BUFF_KINDS];
        string[] isBuffing = new string[BuffManager.BUFF_KINDS];
        string[] isBuffAdRemoved = new string[BuffManager.BUFF_KINDS];

        for (int i = 0; i < BuffManager.BUFF_KINDS; i++)
        {
            buffCoolTime[i] = buffManager.buffCoolTime[i];
            buffCoolTimeStandard[i] = buffManager.buffCoolTimeStandard[i];
            isBuffing[i] = buffManager.isBuffing[i].ToString();
            isBuffAdRemoved[i] = centralInfoManager.isBuffAdRemoved[i].ToString();
        }
        param_setting.Add("speedDuration", speedManager.speedDuration);
        param_setting.Add("speed", speedManager.speed.ToString());
        param_setting.Add("buffCoolTime", buffCoolTime);
        param_setting.Add("buffCoolTimeStandard", buffCoolTimeStandard);
        param_setting.Add("isBuffing", isBuffing);
        param_setting.Add("isBuffAdRemoved", isBuffAdRemoved);
        param_setting.Add("isTheDayClose", NotificationManager.instance.isTheDayClose);

        Backend.GameInfo.Insert("character", param_character);
        LoadManager.instance.isCharacterLoaded = true;
        LoadManager.instance.isEquipLoaded = true;
        yield return null;
        BackendReturnObject cur = Backend.GameInfo.Insert("currency", param_currency);
        BackendReturnObject set = Backend.GameInfo.Insert("setting", param_setting);
        isInsert_item = true;
        isInsert_rank = true;

        BackendReturnObject bro_cha = Backend.GameInfo.GetPrivateContents("character");
        //Debug.Log(bro_cha.GetReturnValue());
        if (bro_cha.IsSuccess())
            indate_cha = bro_cha.GetReturnValuetoJSON()["rows"][0]["inDate"][0].ToString();

        BackendReturnObject bro_currency = Backend.GameInfo.GetPrivateContents("currency");
        //Debug.Log(bro_item.GetReturnValue());
        if (bro_currency.IsSuccess())
            indate_currency = bro_currency.GetReturnValuetoJSON()["rows"][0]["inDate"][0].ToString();

        BackendReturnObject bro_setting = Backend.GameInfo.GetPrivateContents("setting");
        //Debug.Log(bro_stage.GetReturnValue());
        if (bro_setting.IsSuccess())
            indate_setting = bro_setting.GetReturnValuetoJSON()["rows"][0]["inDate"][0].ToString();

        isInsert = true;
    }

    public void SaveData()
    {
        //// 1. character 정보 Update-> CentralInfoManager, EnchantManage
        Param param_character = new Param();
        param_character.Add("level", characterStatus.Level);
        param_character.Add("rebirth", characterStatus.Rebirth);
        param_character.Add("currExp", characterStatus.CurrExp);

        param_character.Add("lp", characterStatus.LP);
        param_character.Add("strength_auto", characterStatus.Strength_Auto);
        param_character.Add("agility_auto", characterStatus.Agility_Auto);
        param_character.Add("dex_auto", characterStatus.Dex_Auto);
        param_character.Add("lucky_auto", characterStatus.Lucky_Auto);

        param_character.Add("strength_lp", characterStatus.Strength_LP);
        param_character.Add("agility_lp", characterStatus.Agility_LP);
        param_character.Add("dex_lp", characterStatus.Dex_LP);
        param_character.Add("lucky_lp", characterStatus.Lucky_LP);

        param_character.Add("isBossDungeonEnter", centralInfoManager.isBossDungeonEnter);
        param_character.Add("isEquipUpgraded", centralInfoManager.isEquipUpgraded);
        param_character.Add("isAtkBuffTutorial", tutorialManager.isAtkBuffTutorial);
        param_character.Add("isGoldBuffTutorial", tutorialManager.isGoldBuffTutorial);

        //yield return GameManager.YieldInstructionCache.WaitForEndOfFrame;

        //// equip 정보 Update-> CentralInfoManager, EnchantManage
        int[] equipLook = new int[CentralInfoManager.EQUIPKIND];

        var eachEquipCount = TableData.Item_Name.Count / CentralInfoManager.EQUIPKIND;

        int[] weapon_Level = new int[eachEquipCount];
        int[] head_Level = new int[eachEquipCount];
        int[] chest_Level = new int[eachEquipCount];
        int[] gloves_Level = new int[eachEquipCount];
        int[] pants_Level = new int[eachEquipCount];
        int[] shoes_Level = new int[eachEquipCount];
        int[] back_Level = new int[eachEquipCount];
        int[] face_Level = new int[eachEquipCount];

        int[] equipState_weapon = new int[eachEquipCount];
        int[] equipState_head = new int[eachEquipCount];
        int[] equipState_chest = new int[eachEquipCount];
        int[] equipState_gloves = new int[eachEquipCount];
        int[] equipState_pants = new int[eachEquipCount];
        int[] equipState_shoes = new int[eachEquipCount];
        int[] equipState_back = new int[eachEquipCount];
        int[] equipState_face = new int[eachEquipCount];

        double[] weaponStat = new double[eachEquipCount];
        double[] headStat = new double[eachEquipCount];
        double[] chestStat = new double[eachEquipCount];
        double[] glovesStat = new double[eachEquipCount];
        double[] pantsStat = new double[eachEquipCount];
        double[] shoesStat = new double[eachEquipCount];
        double[] backStat = new double[eachEquipCount];
        double[] faceStat = new double[eachEquipCount];

        for (int i = 0; i < CentralInfoManager.EQUIPKIND; i++)
        {
            equipLook[i] = centralInfoManager.equipLook[i];
        }
        for (int i = 0; i < eachEquipCount; i++)
        {
            weapon_Level[i] = centralInfoManager.weapon_Level[i];
            head_Level[i] = centralInfoManager.head_Level[i];
            chest_Level[i] = centralInfoManager.chest_Level[i];
            gloves_Level[i] = centralInfoManager.gloves_Level[i];
            pants_Level[i] = centralInfoManager.pants_Level[i];
            shoes_Level[i] = centralInfoManager.shoes_Level[i];
            back_Level[i] = centralInfoManager.back_Level[i];
            face_Level[i] = centralInfoManager.face_Level[i];

            equipState_weapon[i] = equipManager.equipState_weapon[i];
            equipState_head[i] = equipManager.equipState_head[i];
            equipState_chest[i] = equipManager.equipState_chest[i];
            equipState_gloves[i] = equipManager.equipState_gloves[i];
            equipState_pants[i] = equipManager.equipState_pants[i];
            equipState_shoes[i] = equipManager.equipState_shoes[i];
            equipState_back[i] = equipManager.equipState_back[i];
            equipState_face[i] = equipManager.equipState_face[i];

            weaponStat[i] = (double)equipManager.weaponStat[i];
            headStat[i] = (double)equipManager.headStat[i];
            chestStat[i] = (double)equipManager.chestStat[i];
            glovesStat[i] = (double)equipManager.glovesStat[i];
            pantsStat[i] = (double)equipManager.pantsStat[i];
            shoesStat[i] = (double)equipManager.shoesStat[i];
            backStat[i] = (double)equipManager.backStat[i];
            faceStat[i] = (double)equipManager.faceStat[i];
        }
        param_character.Add("equipLook", equipLook);

        param_character.Add("weapon_Level", weapon_Level);
        param_character.Add("head_Level", head_Level);
        param_character.Add("chest_Level", chest_Level);
        param_character.Add("gloves_Level", gloves_Level);
        param_character.Add("pants_Level", pants_Level);
        param_character.Add("shoes_Level", shoes_Level);
        param_character.Add("back_Level", back_Level);
        param_character.Add("face_Level", face_Level);

        param_character.Add("equipState_weapon", equipState_weapon);
        param_character.Add("equipState_head", equipState_head);
        param_character.Add("equipState_chest", equipState_chest);
        param_character.Add("equipState_gloves", equipState_gloves);
        param_character.Add("equipState_pants", equipState_pants);
        param_character.Add("equipState_shoes", equipState_shoes);
        param_character.Add("equipState_back", equipState_back);
        param_character.Add("equipState_face", equipState_face);

        param_character.Add("weaponStat", weaponStat);
        param_character.Add("headStat", headStat);
        param_character.Add("chestStat", chestStat);
        param_character.Add("glovesStat", glovesStat);
        param_character.Add("pantsStat", pantsStat);
        param_character.Add("shoesStat", shoesStat);
        param_character.Add("backStat", backStat);
        param_character.Add("faceStat", faceStat);

        ////rank 정보 Insert -> RankManager
        param_character.Add("accumulatedStage", RankManager.accumulatedStage);
        param_character.Add("bestStage", RankManager.bestStage);
        param_character.Add("myStat", RankManager.myStat);
        param_character.Add("bossDungeonStage", RankManager.bossDungeonStage);
        param_character.Add("rebirthCount", RankManager.rebirthCount);

        //yield return GameManager.YieldInstructionCache.WaitForEndOfFrame;

        //// 2. currency 정보 Update
        Param param_currency = new Param();
        param_currency.Add("gold", currencyManager.Gold.ToString());
        param_currency.Add("jewel", currencyManager.Jewel.ToString());
        param_currency.Add("token", currencyManager.Token.ToString());

        ////item 정보
        int[] boxCountList = new int[CentralInfoManager.NUMBEROFBOXKEYKINDS];
        int[] keyCountList = new int[CentralInfoManager.NUMBEROFBOXKEYKINDS];

        int[] petLevel = new int[PetManager.PETKINDS];
        double[] petStat = new double[PetManager.PETKINDS];
        string[] isPetBuy = new string[PetManager.PETKINDS];

        for (int i = 0; i < CentralInfoManager.NUMBEROFBOXKEYKINDS; i++)
        {
            boxCountList[i] = CentralInfoManager.boxCountList[i];
            keyCountList[i] = CentralInfoManager.keyCountList[i];
        }

        for (int i = 0; i < PetManager.PETKINDS; i++)
        {
            petLevel[i] = petManager.petLevel[i];
            petStat[i] = petManager.petStat[i];
            isPetBuy[i] = petManager.isPetBuy[i].ToString();
        }
        param_currency.Add("boxCountList", boxCountList);
        param_currency.Add("keyCountList", keyCountList);
        param_currency.Add("petLevel", petLevel);
        param_currency.Add("petStat", petStat);
        param_currency.Add("isPetBuy", isPetBuy);
        param_currency.Add("mountingPetIndex", petMenu.mountingPetIndex);
        param_currency.Add("ticket_nickNameChange", CentralInfoManager.ticket_nickNameChange);
        param_currency.Add("rebirthCoupon", CentralInfoManager.rebirthCoupon);
        param_currency.Add("anyOpenKey", CentralInfoManager.anyOpenKey);
        param_currency.Add("lpPotion", CentralInfoManager.lpPotion);
        param_currency.Add("rebirthCoupon_DailyCount", CentralInfoManager.rebirthCoupon_DailyCount);
        param_currency.Add("lpPotion_DailyCount", CentralInfoManager.lpPotion_DailyCount);
        param_currency.Add("boxOpenCount_ad", centralInfoManager.boxOpenCount_ad);
        param_currency.Add("boxOpenCoolTime_ad", centralInfoManager.boxOpenCoolTime_ad);
        param_currency.Add("monthlyJewelProductBuyDate", centralInfoManager.monthlyJewelProductBuyDate);
        param_currency.Add("monthlyJewelPayments", centralInfoManager.monthlyJewelPayments);

        //yield return GameManager.YieldInstructionCache.WaitForEndOfFrame;

        //// 3. setting 정보 Update
        Param param_setting = new Param();
        int[] buffCoolTime = new int[BuffManager.BUFF_KINDS];
        int[] buffCoolTimeStandard = new int[BuffManager.BUFF_KINDS];
        string[] isBuffing = new string[BuffManager.BUFF_KINDS];
        string[] isBuffAdRemoved = new string[BuffManager.BUFF_KINDS];

        for (int i = 0; i < BuffManager.BUFF_KINDS; i++)
        {
            buffCoolTime[i] = buffManager.buffCoolTime[i];
            buffCoolTimeStandard[i] = buffManager.buffCoolTimeStandard[i];
            isBuffing[i] = buffManager.isBuffing[i].ToString();
            isBuffAdRemoved[i] = centralInfoManager.isBuffAdRemoved[i].ToString();
        }
        param_setting.Add("speedDuration", speedManager.speedDuration);
        param_setting.Add("speed", speedManager.speed.ToString());
        param_setting.Add("buffCoolTime", buffCoolTime);
        param_setting.Add("buffCoolTimeStandard", buffCoolTimeStandard);
        param_setting.Add("isBuffing", isBuffing);
        param_setting.Add("isBuffAdRemoved", isBuffAdRemoved);
        param_setting.Add("isTheDayClose", NotificationManager.instance.isTheDayClose);

        ////stage 정보 Update -> SpeedManager, CentralInfoManager
        param_setting.Add("stageCount", CentralInfoManager.stageCount.ToString());
        param_setting.Add("rebirthStartStage", CentralInfoManager.rebirthStartStage.ToString());
        param_setting.Add("regionCount", CentralInfoManager.regionCount.ToString());
        param_setting.Add("bestStage", CentralInfoManager.bestStage.ToString());
        param_setting.Add("ticket_bossDungeon", currencyManager.Ticket_BossDungeon);
        param_setting.Add("boss_day", CentralInfoManager.boss_day);
        param_setting.Add("boss_adCount", CentralInfoManager.boss_adCount);
        param_setting.Add("bossStageCount", CentralInfoManager.bossStageCount);   ////
        param_setting.Add("isHotTimePush_lunch", CentralInfoManager.isHotTimePush_lunch);
        param_setting.Add("isHotTimePush_afternoon", CentralInfoManager.isHotTimePush_afternoon);
        param_setting.Add("isDungeonConquer", CentralInfoManager.isDungeonConquer);

        // yield return GameManager.YieldInstructionCache.WaitForEndOfFrame;

        saving = true;
        StartCoroutine(SaveCHKCO());
        try
        {
            SendQueue.Enqueue(Backend.GameInfo.Update, "character", indate_cha, param_character, updateComplete =>
            {
                if (updateComplete.IsSuccess())
                    saving = true;
                else if (!updateComplete.IsSuccess() || updateComplete.IsServerError())
                    notSaving = true;
            });
            SendQueue.Enqueue(Backend.GameInfo.Update, "currency", indate_currency, param_currency, updateComplete =>
            {
                if (updateComplete.IsSuccess())
                    saving = true;
                else if (!updateComplete.IsSuccess() || updateComplete.IsServerError())
                    notSaving = true;
            });
            SendQueue.Enqueue(Backend.GameInfo.Update, "setting", indate_setting, param_setting, updateComplete =>
            {
                if (updateComplete.IsSuccess())
                    saving = false;
                else if (!updateComplete.IsSuccess() || updateComplete.IsServerError())
                    notSaving = true;
            });
        }
        catch (System.Exception e)
        {
            Debug.Log("저장 catch ##########################");
            //RestartSaving();
            GameManager.SaveLogToServer("정보 저장 에러 발생", e.ToString(), "에러");
        }
    }

    public void SaveData_WhenExit()
    {
        isSaved = false;

        Param param_character = new Param();
        param_character.Add("level", characterStatus.Level);
        param_character.Add("rebirth", characterStatus.Rebirth);
        param_character.Add("currExp", characterStatus.CurrExp);

        param_character.Add("lp", characterStatus.LP);
        param_character.Add("strength_auto", characterStatus.Strength_Auto);
        param_character.Add("agility_auto", characterStatus.Agility_Auto);
        param_character.Add("dex_auto", characterStatus.Dex_Auto);
        param_character.Add("lucky_auto", characterStatus.Lucky_Auto);

        param_character.Add("strength_lp", characterStatus.Strength_LP);
        param_character.Add("agility_lp", characterStatus.Agility_LP);
        param_character.Add("dex_lp", characterStatus.Dex_LP);
        param_character.Add("lucky_lp", characterStatus.Lucky_LP);

        param_character.Add("isBossDungeonEnter", centralInfoManager.isBossDungeonEnter);
        param_character.Add("isEquipUpgraded", centralInfoManager.isEquipUpgraded);
        param_character.Add("isAtkBuffTutorial", tutorialManager.isAtkBuffTutorial);
        param_character.Add("isGoldBuffTutorial", tutorialManager.isGoldBuffTutorial);

        //// equip 정보 Update-> CentralInfoManager, EnchantManage
        int[] equipLook = new int[CentralInfoManager.EQUIPKIND];

        var eachEquipCount = TableData.Item_Name.Count / CentralInfoManager.EQUIPKIND;

        int[] weapon_Level = new int[eachEquipCount];
        int[] head_Level = new int[eachEquipCount];
        int[] chest_Level = new int[eachEquipCount];
        int[] gloves_Level = new int[eachEquipCount];
        int[] pants_Level = new int[eachEquipCount];
        int[] shoes_Level = new int[eachEquipCount];
        int[] back_Level = new int[eachEquipCount];
        int[] face_Level = new int[eachEquipCount];

        int[] equipState_weapon = new int[eachEquipCount];
        int[] equipState_head = new int[eachEquipCount];
        int[] equipState_chest = new int[eachEquipCount];
        int[] equipState_gloves = new int[eachEquipCount];
        int[] equipState_pants = new int[eachEquipCount];
        int[] equipState_shoes = new int[eachEquipCount];
        int[] equipState_back = new int[eachEquipCount];
        int[] equipState_face = new int[eachEquipCount];

        double[] weaponStat = new double[eachEquipCount];
        double[] headStat = new double[eachEquipCount];
        double[] chestStat = new double[eachEquipCount];
        double[] glovesStat = new double[eachEquipCount];
        double[] pantsStat = new double[eachEquipCount];
        double[] shoesStat = new double[eachEquipCount];
        double[] backStat = new double[eachEquipCount];
        double[] faceStat = new double[eachEquipCount];

        for (int i = 0; i < CentralInfoManager.EQUIPKIND; i++)
        {
            equipLook[i] = centralInfoManager.equipLook[i];
        }
        for (int i = 0; i < eachEquipCount; i++)
        {
            weapon_Level[i] = centralInfoManager.weapon_Level[i];
            head_Level[i] = centralInfoManager.head_Level[i];
            chest_Level[i] = centralInfoManager.chest_Level[i];
            gloves_Level[i] = centralInfoManager.gloves_Level[i];
            pants_Level[i] = centralInfoManager.pants_Level[i];
            shoes_Level[i] = centralInfoManager.shoes_Level[i];
            back_Level[i] = centralInfoManager.back_Level[i];
            face_Level[i] = centralInfoManager.face_Level[i];

            equipState_weapon[i] = equipManager.equipState_weapon[i];
            equipState_head[i] = equipManager.equipState_head[i];
            equipState_chest[i] = equipManager.equipState_chest[i];
            equipState_gloves[i] = equipManager.equipState_gloves[i];
            equipState_pants[i] = equipManager.equipState_pants[i];
            equipState_shoes[i] = equipManager.equipState_shoes[i];
            equipState_back[i] = equipManager.equipState_back[i];
            equipState_face[i] = equipManager.equipState_face[i];

            weaponStat[i] = (double)equipManager.weaponStat[i];
            headStat[i] = (double)equipManager.headStat[i];
            chestStat[i] = (double)equipManager.chestStat[i];
            glovesStat[i] = (double)equipManager.glovesStat[i];
            pantsStat[i] = (double)equipManager.pantsStat[i];
            shoesStat[i] = (double)equipManager.shoesStat[i];
            backStat[i] = (double)equipManager.backStat[i];
            faceStat[i] = (double)equipManager.faceStat[i];
        }
        param_character.Add("equipLook", equipLook);

        param_character.Add("weapon_Level", weapon_Level);
        param_character.Add("head_Level", head_Level);
        param_character.Add("chest_Level", chest_Level);
        param_character.Add("gloves_Level", gloves_Level);
        param_character.Add("pants_Level", pants_Level);
        param_character.Add("shoes_Level", shoes_Level);
        param_character.Add("back_Level", back_Level);
        param_character.Add("face_Level", face_Level);

        param_character.Add("equipState_weapon", equipState_weapon);
        param_character.Add("equipState_head", equipState_head);
        param_character.Add("equipState_chest", equipState_chest);
        param_character.Add("equipState_gloves", equipState_gloves);
        param_character.Add("equipState_pants", equipState_pants);
        param_character.Add("equipState_shoes", equipState_shoes);
        param_character.Add("equipState_back", equipState_back);
        param_character.Add("equipState_face", equipState_face);

        param_character.Add("weaponStat", weaponStat);
        param_character.Add("headStat", headStat);
        param_character.Add("chestStat", chestStat);
        param_character.Add("glovesStat", glovesStat);
        param_character.Add("pantsStat", pantsStat);
        param_character.Add("shoesStat", shoesStat);
        param_character.Add("backStat", backStat);
        param_character.Add("faceStat", faceStat);

        ////rank 정보 Insert -> RankManager
        param_character.Add("accumulatedStage", RankManager.accumulatedStage);
        param_character.Add("bestStage", RankManager.bestStage);
        param_character.Add("myStat", RankManager.myStat);
        param_character.Add("bossDungeonStage", RankManager.bossDungeonStage);
        param_character.Add("rebirthCount", RankManager.rebirthCount);

        //// 2. currency 정보 Update
        Param param_currency = new Param();
        param_currency.Add("gold", currencyManager.Gold.ToString());
        param_currency.Add("jewel", currencyManager.Jewel.ToString());
        param_currency.Add("token", currencyManager.Token.ToString());

        ////item 정보
        int[] boxCountList = new int[CentralInfoManager.NUMBEROFBOXKEYKINDS];
        int[] keyCountList = new int[CentralInfoManager.NUMBEROFBOXKEYKINDS];

        int[] petLevel = new int[PetManager.PETKINDS];
        double[] petStat = new double[PetManager.PETKINDS];
        string[] isPetBuy = new string[PetManager.PETKINDS];

        for (int i = 0; i < CentralInfoManager.NUMBEROFBOXKEYKINDS; i++)
        {
            boxCountList[i] = CentralInfoManager.boxCountList[i];
            keyCountList[i] = CentralInfoManager.keyCountList[i];
        }

        for (int i = 0; i < PetManager.PETKINDS; i++)
        {
            petLevel[i] = petManager.petLevel[i];
            petStat[i] = petManager.petStat[i];
            isPetBuy[i] = petManager.isPetBuy[i].ToString();
        }
        param_currency.Add("boxCountList", boxCountList);
        param_currency.Add("keyCountList", keyCountList);
        param_currency.Add("petLevel", petLevel);
        param_currency.Add("petStat", petStat);
        param_currency.Add("isPetBuy", isPetBuy);
        param_currency.Add("mountingPetIndex", petMenu.mountingPetIndex);
        param_currency.Add("ticket_nickNameChange", CentralInfoManager.ticket_nickNameChange);
        param_currency.Add("rebirthCoupon", CentralInfoManager.rebirthCoupon);
        param_currency.Add("anyOpenKey", CentralInfoManager.anyOpenKey);
        param_currency.Add("lpPotion", CentralInfoManager.lpPotion);
        param_currency.Add("rebirthCoupon_DailyCount", CentralInfoManager.rebirthCoupon_DailyCount);
        param_currency.Add("lpPotion_DailyCount", CentralInfoManager.lpPotion_DailyCount);
        param_currency.Add("boxOpenCount_ad", centralInfoManager.boxOpenCount_ad);
        param_currency.Add("boxOpenCoolTime_ad", centralInfoManager.boxOpenCoolTime_ad);
        param_currency.Add("monthlyJewelProductBuyDate", centralInfoManager.monthlyJewelProductBuyDate);
        param_currency.Add("monthlyJewelPayments", centralInfoManager.monthlyJewelPayments);

        //// 3. setting 정보 Update
        Param param_setting = new Param();
        int[] buffCoolTime = new int[BuffManager.BUFF_KINDS];
        int[] buffCoolTimeStandard = new int[BuffManager.BUFF_KINDS];
        string[] isBuffing = new string[BuffManager.BUFF_KINDS];
        string[] isBuffAdRemoved = new string[BuffManager.BUFF_KINDS];

        for (int i = 0; i < BuffManager.BUFF_KINDS; i++)
        {
            buffCoolTime[i] = buffManager.buffCoolTime[i];
            buffCoolTimeStandard[i] = buffManager.buffCoolTimeStandard[i];
            isBuffing[i] = buffManager.isBuffing[i].ToString();
            isBuffAdRemoved[i] = centralInfoManager.isBuffAdRemoved[i].ToString();
        }
        param_setting.Add("speedDuration", speedManager.speedDuration);
        param_setting.Add("speed", speedManager.speed.ToString());
        param_setting.Add("buffCoolTime", buffCoolTime);
        param_setting.Add("buffCoolTimeStandard", buffCoolTimeStandard);
        param_setting.Add("isBuffing", isBuffing);
        param_setting.Add("isBuffAdRemoved", isBuffAdRemoved);
        param_setting.Add("isTheDayClose", NotificationManager.instance.isTheDayClose);

        ////stage 정보 Update -> SpeedManager, CentralInfoManager
        param_setting.Add("stageCount", CentralInfoManager.stageCount.ToString());
        param_setting.Add("rebirthStartStage", CentralInfoManager.rebirthStartStage.ToString());
        param_setting.Add("regionCount", CentralInfoManager.regionCount.ToString());
        param_setting.Add("bestStage", CentralInfoManager.bestStage.ToString());
        param_setting.Add("ticket_bossDungeon", currencyManager.Ticket_BossDungeon);
        param_setting.Add("boss_day", CentralInfoManager.boss_day);
        param_setting.Add("boss_adCount", CentralInfoManager.boss_adCount);
        param_setting.Add("bossStageCount", CentralInfoManager.bossStageCount);   ////
        param_setting.Add("isHotTimePush_lunch", CentralInfoManager.isHotTimePush_lunch);
        param_setting.Add("isHotTimePush_afternoon", CentralInfoManager.isHotTimePush_afternoon);
        param_setting.Add("isDungeonConquer", CentralInfoManager.isDungeonConquer);

        Backend.GameInfo.Update("character", indate_cha, param_character);
        Backend.GameInfo.Update("currency", indate_currency, param_currency);
        Backend.GameInfo.Update("setting", indate_setting, param_setting);

        isSaved = true;
    }

    public void RestartSaving()
    {
        RestartSavingCO();
    }

    private void RestartSavingCO()
    {
        if (restartIndex < 3)
        {
            restartIndex++;
            Debug.Log("저장 재시작 : " + restartIndex);
            if (retryPopUpCO == null)
                retryPopUpCO = StartCoroutine(RetryTextCO());
            SaveData();
        }
        else
        {
            restartIndex = 0;
            restartPopUp_saving.SetActive(true);
            retryPopUpCO = null;
        }
    }

    private IEnumerator RetryTextCO()
    {
        retryPopUp.SetActive(true);
        while (saving)
        {
            yield return new WaitForSeconds(1f);
            retryText.text = string.Format("게임 정보 저장 재시도중.({0})", restartIndex);
            yield return new WaitForSeconds(1f);
            retryText.text = string.Format("게임 정보 저장 재시도중..({0})", restartIndex);
            yield return new WaitForSeconds(1f);
            retryText.text = string.Format("게임 정보 저장 재시도중...({0})", restartIndex);
        }
        retryPopUp.SetActive(false);
    }

    private IEnumerator SaveCHKCO()
    {
        yield return new WaitForSecondsRealtime(5f);
        if (saving)
        {
            StartCoroutine(GameManager.instance.ChkTokenAndRefresh());
            yield return GameManager.YieldInstructionCache.WaitForEndOfFrame;

            RestartSaving();
        }
    }
}