using CodeStage.AntiCheat.Detectors;
using CodeStage.AntiCheat.ObscuredTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralInfoManager : MonoBehaviour
{
    public static CentralInfoManager instance;
    public const int DEFAULTEQUIPINDEX = 0;
    public const int REINFORCEMENT_MINUS_VALUE = 2;
    public List<Sprite> weapon_Sprite = new List<Sprite>();
    public List<Sprite> head_Sprite = new List<Sprite>();
    public List<Sprite> chest_Sprite = new List<Sprite>();
    public List<Sprite> gloves_Sprite = new List<Sprite>();
    public List<Sprite> pants_Sprite = new List<Sprite>();
    public List<Sprite> shoes_Sprite = new List<Sprite>();
    public List<Sprite> back_Sprite = new List<Sprite>();
    public List<Sprite> face_Sprite = new List<Sprite>();

    public List<Sprite> weapon_Sprite_equip = new List<Sprite>();
    public List<Sprite> head_Sprite_equip = new List<Sprite>();
    public List<Sprite> chest_Sprite_equip = new List<Sprite>();
    public List<Sprite> gloves_Sprite_equip = new List<Sprite>();
    public List<Sprite> pants_Sprite_equip = new List<Sprite>();
    public List<Sprite> shoes_Sprite_equip = new List<Sprite>();
    public List<Sprite> back_Sprite_equip = new List<Sprite>();
    public List<Sprite> face_Sprite_equip = new List<Sprite>();

    [Header("Stage Info")]
    public const int MaxRegion = 10;

    public static ObscuredLong bestStage;
    public static ObscuredLong stageCount;
    public static long rebirthStartStage;
    public static ObscuredLong regionCount;

    public static int ticket_nickNameChange;

    [Header("Equip Info")]
    public const int EQUIPKIND = 8;

    public List<int> equipLook = new List<int>();

    public List<int> weapon_Level = new List<int>();
    public List<int> head_Level = new List<int>();
    public List<int> chest_Level = new List<int>();  // 얘네들 다 세팅하고 EquipManager에 각 장비 Stat리스트(기본 0)와 sum 정의하기
    public List<int> gloves_Level = new List<int>();
    public List<int> pants_Level = new List<int>();
    public List<int> shoes_Level = new List<int>();
    public List<int> back_Level = new List<int>();
    public List<int> face_Level = new List<int>();
    public bool isEquipUpgraded;

    [Header("Boss Dungeon")]
    public static ObscuredInt bossStageCount;

    public static bool isDungeonConquer;        //
    public static int boss_adCount;
    public static int boss_day;
    public bool isBossDungeonEnter;

    [Header("Box & Key")]
    public const int ADOPENMAXCOUNT = 10;

    public const int NUMBEROFBOXKEYKINDS = 7;
    public const int BoxOpenCoolTime_STANDARD = 300;

    public int boxOpenCount_ad;     //
    public int boxOpenCoolTime_ad;      //

    public static List<int> boxCountList = new List<int>();

    public static List<int> keyCountList = new List<int>();

    [Header("Shop")]
    public const int MonthlyProduct_PERIOD = 25;

    public const int MonthlyProduct_DAILYREWARD = 300;
    public static ObscuredInt rebirthCoupon;

    public static int anyOpenKey;
    public static ObscuredInt lpPotion;

    public static int rebirthCoupon_DailyCount;
    public static int lpPotion_DailyCount;
    public string monthlyJewelProductBuyDate;
    public bool monthlyJewelPayments;
    public static bool isHotTimePush_lunch;
    public static bool isHotTimePush_afternoon;
    public int hotTimeJewelCount = 0;

    public List<bool> isBuffAdRemoved = new List<bool>();
    public static bool cheaterDetected = false;

    #region

    public delegate void StartSetting();

    public event StartSetting startSetting;

    #endregion

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => LoadManager.instance.isDataLoaded);
        startSetting();
        boss_day = int.Parse(DateTime.Now.ToString("dd"));
        ObscuredCheatingDetector.StartDetection(OnCheaterDetected);
    }

    private void OnCheaterDetected()
    {
        cheaterDetected = true;
    }

    public bool IsBossMonster()
    {
        if (regionCount.Equals(MaxRegion))
            return true;
        else
            return false;
    }

    public int ReturnEquipLevel(int whakKind, int equipIndex)
    {
        var result = 0;
        switch (whakKind)
        {
            case 0:
                result = weapon_Level[equipIndex];
                break;

            case 1:
                result = head_Level[equipIndex];
                break;

            case 2:
                result = chest_Level[equipIndex];
                break;

            case 3:
                result = gloves_Level[equipIndex];
                break;

            case 4:
                result = pants_Level[equipIndex];
                break;

            case 5:
                result = shoes_Level[equipIndex];
                break;

            case 6:
                result = back_Level[equipIndex];
                break;

            case 7:
                result = face_Level[equipIndex];
                break;

            default:
                break;
        }
        return result;
    }

    public Sprite ReturnEquipSprite(int whatkind, int equipIndex)
    {
        Sprite result = weapon_Sprite[0];
        switch (whatkind)
        {
            case 0:
                result = weapon_Sprite[equipIndex];
                break;

            case 1:
                result = head_Sprite[equipIndex];
                break;

            case 2:
                result = chest_Sprite[equipIndex];
                break;

            case 3:
                result = gloves_Sprite[equipIndex];
                break;

            case 4:
                result = pants_Sprite[equipIndex];
                break;

            case 5:
                result = shoes_Sprite[equipIndex];
                break;

            case 6:
                result = back_Sprite[equipIndex];
                break;

            case 7:
                result = face_Sprite[equipIndex];
                break;

            default:
                break;
        }
        return result;
    }

    public Sprite ReturnEquipSprite_equipMennu(int whatkind, int equipIndex)
    {
        Sprite result = weapon_Sprite_equip[0];
        switch (whatkind)
        {
            case 0:
                result = weapon_Sprite_equip[equipIndex];
                break;

            case 1:
                result = head_Sprite_equip[equipIndex];
                break;

            case 2:
                result = chest_Sprite_equip[equipIndex];
                break;

            case 3:
                result = gloves_Sprite_equip[equipIndex];
                break;

            case 4:
                result = pants_Sprite_equip[equipIndex];
                break;

            case 5:
                result = shoes_Sprite_equip[equipIndex];
                break;

            case 6:
                result = back_Sprite_equip[equipIndex];
                break;

            case 7:
                result = face_Sprite_equip[equipIndex];
                break;

            default:
                break;
        }
        return result;
    }
}