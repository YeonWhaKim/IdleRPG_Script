using CodeStage.AntiCheat.Detectors;
using CodeStage.AntiCheat.ObscuredTypes;
using System;
using System.Collections;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    public static CharacterStatus instance;
    public EquipManager equipManager;
    public FormulaCollection formulaCollection;
    [SerializeField] private ObscuredInt level;   //*
    [SerializeField] private ObscuredInt rebirth;
    [SerializeField] private ObscuredLong currExp; //*
    [SerializeField] private ObscuredLong maxExpForLevelUp;

    [SerializeField] private ObscuredInt lP;          // *
    [SerializeField] private ObscuredLong strength_auto;   // * 힘 - 공격력
    [SerializeField] private ObscuredLong agility_auto;    // * 민첩 - 치명타 확률
    [SerializeField] private ObscuredLong dex_auto;        // * 솜씨 - 치명타 피해
    [SerializeField] private ObscuredLong lucky_auto;      // * 행운 - 처치골드추가획득

    [SerializeField] private ObscuredLong strength_lp;   // * 힘 - 공격력
    [SerializeField] private ObscuredLong agility_lp;    // * 민첩 - 치명타 확률
    [SerializeField] private ObscuredLong dex_lp;        // * 솜씨 - 치명타 피해
    [SerializeField] private ObscuredLong lucky_lp;      // * 행운 - 처치골드추가획득

    [SerializeField] private ObscuredDouble attackPower;
    [SerializeField] private ObscuredDouble criticalPercentage;
    [SerializeField] private ObscuredDouble criticalDamage;
    [SerializeField] private ObscuredDouble moreGoldGiven;
    public bool isMaxLevel = false;
    private bool cheaterDetected = false;

    #region 프로퍼티

    public int Level
    {
        get { return level; }
        set
        {
            level = value;
            if (Level == 3 && Rebirth == 0 && LoadManager.instance.isDataLoaded)
                TutorialManager.instance.RebirthTutorial();
            //여기서 info에 뿌려주기
            if (level >= RebirthManager.instance.ReturnMaxLevel_DependingOnRebirthCount(Rebirth))
            {
                level = RebirthManager.instance.ReturnMaxLevel_DependingOnRebirthCount(Rebirth);
                isMaxLevel = true;
            }
            InfoMenu.instance.ui.LevelSetting(isMaxLevel, level, cheaterDetected);
        }
    }

    public int Rebirth
    {
        get { return rebirth; }
        set
        {
            rebirth = value;
            if (rebirth > TableData.Replay_On_Level.Count)
                rebirth = TableData.Replay_On_Level.Count;

            InfoMenu.instance.ui.ReplayExpSetting(rebirth, cheaterDetected);
            equipManager.JudgeEquipRock(EquipManager.EOT_REPLAY, Rebirth);
        }
    }

    public long CurrExp
    {
        get { return currExp; }
        set
        {
            currExp = value;
            if (currExp >= maxExpForLevelUp && isMaxLevel == false)
                LevelUp(currExp - maxExpForLevelUp);
            //여기서 info에 뿌려주기 - 경험치 숫자랑, 경험치 바 같이
            InfoMenu.instance.ui.ExpSetting(isMaxLevel, CurrExp, MaxExpForLevelUp);
        }
    }

    public long MaxExpForLevelUp
    {
        get { return maxExpForLevelUp; }
        set
        {
            maxExpForLevelUp = value;
        }
    }

    public int LP
    {
        get { return lP; }
        set
        {
            lP = value;
            //여기서 info에 뿌려주기
            InfoMenu.instance.ui.LPSetting(lP, cheaterDetected);
        }
    }

    public long Strength_Auto
    {
        get { return strength_auto; }
        set
        {
            strength_auto = value;
            AttackPower += TableData.Status_Up[0];
            equipManager.JudgeEquipRock(EquipManager.EOT_STR, Strength_Auto + Strength_LP);
            //여기서 info에 뿌려주기(스테이터스랑, 능력치 둘다)
            InfoMenu.instance.ui.StatusSetting_Auto(0, Strength_Auto, cheaterDetected);
        }
    }

    public long Agility_Auto
    {
        get { return agility_auto; }
        set
        {
            agility_auto = value;
            CriticalPer += TableData.Status_Up[1];
            equipManager.JudgeEquipRock(EquipManager.EOT_AGI, Agility_Auto + Agility_LP);

            //여기서 info에 뿌려주기(스테이터스랑, 능력치 둘다)
            InfoMenu.instance.ui.StatusSetting_Auto(1, Agility_Auto, cheaterDetected);
        }
    }

    public long Dex_Auto
    {
        get { return dex_auto; }
        set
        {
            dex_auto = value;
            CriticalDamage += TableData.Status_Up[2];
            equipManager.JudgeEquipRock(EquipManager.EOT_DEX, Dex_Auto + Dex_LP);

            //여기서 info에 뿌려주기(스테이터스랑, 능력치 둘다)
            InfoMenu.instance.ui.StatusSetting_Auto(2, Dex_Auto, cheaterDetected);
        }
    }

    public long Lucky_Auto
    {
        get { return lucky_auto; }
        set
        {
            lucky_auto = value;
            MGG += TableData.Status_Up[3];
            equipManager.JudgeEquipRock(EquipManager.EOT_LUK, Lucky_Auto + Lucky_LP);
            //여기서 info에 뿌려주기(스테이터스랑, 능력치 둘다)
            InfoMenu.instance.ui.StatusSetting_Auto(3, Lucky_Auto, cheaterDetected);
        }
    }

    public long Strength_LP
    {
        get { return strength_lp; }
        set
        {
            strength_lp = value;
            OverflowManager.ChkStatusOverFlow(strength_lp);
            AttackPower += TableData.Status_Up[0];
            equipManager.JudgeEquipRock(EquipManager.EOT_STR, Strength_Auto + Strength_LP);
            //여기서 info에 뿌려주기(스테이터스랑, 능력치 둘다)
            InfoMenu.instance.ui.StatusSetting_LP(0, Strength_LP, cheaterDetected);
        }
    }

    public long Agility_LP
    {
        get { return agility_lp; }
        set
        {
            agility_lp = value;
            OverflowManager.ChkStatusOverFlow(agility_lp);

            CriticalPer += TableData.Status_Up[1];
            equipManager.JudgeEquipRock(EquipManager.EOT_AGI, Agility_Auto + Agility_LP);
            //여기서 info에 뿌려주기(스테이터스랑, 능력치 둘다)
            InfoMenu.instance.ui.StatusSetting_LP(1, Agility_LP, cheaterDetected);
        }
    }

    public long Dex_LP
    {
        get { return dex_lp; }
        set
        {
            dex_lp = value;
            OverflowManager.ChkStatusOverFlow(dex_lp);

            CriticalDamage += TableData.Status_Up[2];
            equipManager.JudgeEquipRock(EquipManager.EOT_DEX, Dex_Auto + Dex_LP);
            //여기서 info에 뿌려주기(스테이터스랑, 능력치 둘다)
            InfoMenu.instance.ui.StatusSetting_LP(2, Dex_LP, cheaterDetected);
        }
    }

    public long Lucky_LP
    {
        get { return lucky_lp; }
        set
        {
            lucky_lp = value;
            OverflowManager.ChkStatusOverFlow(lucky_lp);

            MGG += TableData.Status_Up[3];
            equipManager.JudgeEquipRock(EquipManager.EOT_LUK, Lucky_Auto + Lucky_LP);
            //여기서 info에 뿌려주기(스테이터스랑, 능력치 둘다)
            InfoMenu.instance.ui.StatusSetting_LP(3, Lucky_LP, cheaterDetected);
        }
    }

    public double AttackPower
    {
        get { return attackPower; }
        set
        {
            attackPower = Math.Truncate(value);
            StartCoroutine(RankManager.instance.UpdateRank(3, formulaCollection.ReturnTotalStats()));
            //여기서 info에 뿌려주기
            InfoMenu.instance.ui.SumOfSetting(0, attackPower);
        }
    }

    public double CriticalPer
    {
        get { return criticalPercentage; }
        set
        {
            criticalPercentage = Math.Round(value, 3);

            StartCoroutine(RankManager.instance.UpdateRank(3, formulaCollection.ReturnTotalStats()));

            //여기서 info에 뿌려주기
            InfoMenu.instance.ui.SumOfSetting(1, criticalPercentage);
        }
    }

    public double CriticalDamage
    {
        get { return criticalDamage; }
        set
        {
            criticalDamage = Math.Round(value, 2);
            StartCoroutine(RankManager.instance.UpdateRank(3, formulaCollection.ReturnTotalStats()));

            //여기서 info에 뿌려주기
            InfoMenu.instance.ui.SumOfSetting(2, criticalDamage);
        }
    }

    public double MGG
    {
        get { return moreGoldGiven; }
        set
        {
            moreGoldGiven = Math.Round(value, 3);
            StartCoroutine(RankManager.instance.UpdateRank(3, formulaCollection.ReturnTotalStats()));

            //여기서 info에 뿌려주기
            InfoMenu.instance.ui.SumOfSetting(3, moreGoldGiven);
        }
    }

    #endregion 프로퍼티

    private void Awake()
    {
        instance = this;
    }

    private IEnumerator Start()
    {
        //각 능력치들도 위에 스테이터스가 지정해준거 무시하고 다시 세팅하기.
        yield return new WaitUntil(() => LoadManager.instance.isDataLoaded);

        StatSetting();

        ObscuredCheatingDetector.StartDetection(OnCheaterDetected);
    }

    private void OnCheaterDetected()
    {
        cheaterDetected = true;
    }

    public void StatSetting()
    {
        AttackPower = (Strength_Auto + Strength_LP) * TableData.Status_Up[0];
        CriticalPer = (Agility_Auto + Agility_LP) * TableData.Status_Up[1];
        CriticalDamage = 1 + ((Dex_Auto + Dex_LP) * TableData.Status_Up[2]);
        MGG = 1 + ((Lucky_Auto + Lucky_LP) * TableData.Status_Up[3]);

        PetManager.instance.StatSetting();

        equipManager.EquipFirstStatSetting();
    }

    public void MonsterDieReward()
    {
        long upExp = 0;
        if (CentralInfoManager.instance.IsBossMonster())
            upExp = TableData.Stage_Boss_Clear_Exp[(int)CentralInfoManager.stageCount - 1];
        else
            upExp = TableData.Stage_Normal_Clear_Exp[(int)CentralInfoManager.stageCount - 1];
        CurrExp += upExp;
    }

    public void LevelUp(long remainingExp)
    {
        var maxLevel = RebirthManager.instance.ReturnMaxLevel_DependingOnRebirthCount(Rebirth);
        if (Level < maxLevel)
        {
            LP++;
            EffectManager.instance.LevelUpEffectPlay();
        }

        Level++;

        if (Level >= maxLevel)
        {
            MaxExpForLevelUp = TableData.Level_Exp[Level - 1];
            CurrExp = MaxExpForLevelUp;
        }
        else
        {
            MaxExpForLevelUp = TableData.Level_Exp[Level - 1];
            if (MaxExpForLevelUp <= remainingExp)
                remainingExp = 0;
            CurrExp = remainingExp;

            var ran = UnityEngine.Random.Range(0, TableData.Status_Up_Type.Count);
            StatusUp(true, ran, TableData.Level_Random_Status[Level - 1]);
        }
    }

    public void IncreasedStatusWithLp(int index)
    {
        // lp 소모시 각 스테이터스 수치에 맞게 감소시켜야함.
        LP -= FormulaCollection.RequiredLpPoint(ReturnStatusLP(index));
        //lp로 스테이터스 올릴때
        StatusUp(false, index, 1);
    }

    private void StatusUp(bool isAuto, int index, int upValue)
    {
        switch (index)
        {
            case 0:
                if (isAuto)
                {
                    for (int i = 0; i < upValue; i++)
                    {
                        Strength_Auto += 1;
                    }
                }
                else
                    Strength_LP += upValue;

                break;

            case 1:
                if (isAuto)
                {
                    for (int i = 0; i < upValue; i++)
                    {
                        Agility_Auto += 1;
                    }
                }
                else
                    Agility_LP += upValue;

                break;

            case 2:
                if (isAuto)
                {
                    for (int i = 0; i < upValue; i++)
                    {
                        Dex_Auto += 1;
                    }
                }
                else
                    Dex_LP += upValue;

                break;

            case 3:
                if (isAuto)
                {
                    for (int i = 0; i < upValue; i++)
                    {
                        Lucky_Auto += 1;
                    }
                }
                else
                    Lucky_LP += upValue;

                break;

            default:
                break;
        }
    }

    public long ReturnStatusLP(int index)
    {
        long result = 0;
        switch (index)
        {
            case 0:
                result = Strength_LP;
                break;

            case 1:
                result = Agility_LP;
                break;

            case 2:
                result = Dex_LP;
                break;

            case 3:
                result = Lucky_LP;
                break;
        }
        return result;
    }
}