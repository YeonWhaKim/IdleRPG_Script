using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public static BoxManager instance;

    public enum OPENTYPE { KEY, ANYOPENKEY, JEWEL };

    public int NUMBEROFBOXREWARD = 0;
    public BossBox bossBox;
    public List<string> boxRewardType = new List<string>();
    public List<int> boxRewardValue = new List<int>();

    public List<int> keyValue = new List<int>();

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < CentralInfoManager.NUMBEROFBOXKEYKINDS; i++)
        {
            boxRewardType.Add("0");
            boxRewardValue.Add(0);
            keyValue.Add(0);
        }
    }

    public void BoxReward()
    {
        // 박스 획득
        var acquireBoxIndex = AcquireBoxRandom();
        var upCount = 0;
        switch (acquireBoxIndex)
        {
            case 0:
                upCount = TableData.Boss_Drop_Box_01[(int)CentralInfoManager.stageCount - 1];
                break;

            case 1:
                upCount = TableData.Boss_Drop_Box_02[(int)CentralInfoManager.stageCount - 1];
                break;

            case 2:
                upCount = TableData.Boss_Drop_Box_03[(int)CentralInfoManager.stageCount - 1];
                break;

            case 3:
                upCount = TableData.Boss_Drop_Box_04[(int)CentralInfoManager.stageCount - 1];
                break;

            case 4:
                upCount = TableData.Boss_Drop_Box_05[(int)CentralInfoManager.stageCount - 1];
                break;

            case 5:
                upCount = TableData.Boss_Drop_Box_06[(int)CentralInfoManager.stageCount - 1];
                break;

            case 6:
                upCount = TableData.Boss_Drop_Box_07[(int)CentralInfoManager.stageCount - 1];
                break;
        }

        if (upCount > 0 && CentralInfoManager.stageCount > 1)
        {
            if (CentralInfoManager.instance.isBossDungeonEnter.Equals(false) && CentralInfoManager.boxCountList[0] <= 0)
                TutorialManager.instance.BossTutorial();

            CentralInfoManager.boxCountList[acquireBoxIndex] += upCount;
            var maxCount = RebirthManager.instance.ReturnMaxLevel_DependingOnRebirthCount(CharacterStatus.instance.Rebirth);
            if (CentralInfoManager.boxCountList[acquireBoxIndex] > maxCount)
                CentralInfoManager.boxCountList[acquireBoxIndex] = maxCount;
            BoxMenu.instance.BoxMenuSetting();
            bossBox.BossBoxOn(acquireBoxIndex);
        }

        // 획득했다는 이펙트 추가할 때 여기다가
    }

    public void KeyReward()
    {
        // 키 획득

        var acquireKeyIndex = AcquireKeyRandom();
        var upCount = 0;
        switch (acquireKeyIndex)
        {
            case 0:
                upCount = TableData.Boss_Drop_Key_01[(int)CentralInfoManager.bossStageCount - 1];
                break;

            case 1:
                upCount = TableData.Boss_Drop_Key_02[(int)CentralInfoManager.bossStageCount - 1];
                break;

            case 2:
                upCount = TableData.Boss_Drop_Key_03[(int)CentralInfoManager.bossStageCount - 1];
                break;

            case 3:
                upCount = TableData.Boss_Drop_Key_04[(int)CentralInfoManager.bossStageCount - 1];
                break;

            case 4:
                upCount = TableData.Boss_Drop_Key_05[(int)CentralInfoManager.bossStageCount - 1];
                break;

            case 5:
                upCount = TableData.Boss_Drop_Key_06[(int)CentralInfoManager.bossStageCount - 1];
                break;

            case 6:
                upCount = TableData.Boss_Drop_Key_07[(int)CentralInfoManager.bossStageCount - 1];
                break;
        }
        //if (upCount > 0)
        //{
        //    CentralInfoManager.keyCountList[acquireKeyIndex] += upCount;
        //    BoxMenu.instance.BoxMenuSetting();
        //}
        // 획득했다는 이펙트 추가할 때 여기다가
    }

    public void BoxOpenReward(OPENTYPE type, int index, bool isAD)
    {
        CentralInfoManager.boxCountList[index]--;
        if (isAD.Equals(false))
        {
            switch (type)
            {
                case OPENTYPE.KEY:
                    CentralInfoManager.keyCountList[index]--;
                    break;

                case OPENTYPE.ANYOPENKEY:
                    CentralInfoManager.anyOpenKey--;
                    break;

                case OPENTYPE.JEWEL:
                    CurrencyManager.instance.Jewel -= TableData.Box_Open_Gem[index];
                    break;

                default:
                    break;
            }
        }

        var random = Random.Range(1, 101);
        var std_index = index * NUMBEROFBOXREWARD;

        var standard = new List<int>();
        standard.Add(TableData.Gatcha_Gift_Per[std_index + 0]);
        for (int i = 0; i < NUMBEROFBOXREWARD - 1; i++)
        {
            standard.Add(standard[i] + TableData.Gatcha_Gift_Per[std_index + i + 1]);
        }
        //var standard_01 = ;
        //var standard_02 = standard_01 + TableData.Gatcha_Gift_Per[std_index + 1];
        //var standard_03 = standard_02 + TableData.Gatcha_Gift_Per[std_index + 2];
        //var standard_04 = standard_03 + TableData.Gatcha_Gift_Per[std_index + 3];
        //var standard_05 = standard_04 + TableData.Gatcha_Gift_Per[std_index + 4];
        //var standard_06 = standard_05 + TableData.Gatcha_Gift_Per[std_index + 5];

        for (int i = 0; i < NUMBEROFBOXREWARD; i++)
        {
            if (random <= standard[i])
            {
                RewardResult(std_index, i);
                break;
            }
            else
                continue;
        }
        //if (random <= standard_01)
        //    RewardResult(std_index, 0);
        //else if (random <= standard_02)
        //    RewardResult(std_index, 1);
        //else if (random <= standard_03)
        //    RewardResult(std_index, 2);
        //else
        //    RewardResult(std_index, 3);
        //else if (random <= standard_05)
        //    RewardResult(std_index, 4);
        //else
        //    RewardResult(std_index, 5);
    }

    public void ListReset(List<string> typeList, List<int> valueList)
    {
        for (int i = 0; i < CentralInfoManager.NUMBEROFBOXKEYKINDS; i++)
        {
            typeList[i] = "0";
            valueList[i] = 0;
        }
    }

    public void KeyListReset()
    {
        for (int i = 0; i < CentralInfoManager.NUMBEROFBOXKEYKINDS; i++)
        {
            keyValue[i] = 0;
        }
    }

    private void RewardResult(int std_index, int index)
    {
        boxRewardType[index] = TableData.Gatcha_Gift_Type[std_index + index];

        var random = Random.Range((int)TableData.Gatcha_Gift_Min[std_index + index], (int)TableData.Gatcha_Gift_Max[std_index + index]);
        boxRewardValue[index] += random;
    }

    private int AcquireBoxRandom()
    {
        var result = 0;
        int bossIndex = (int)CentralInfoManager.stageCount - 1;

        var standard_01 = TableData.Boss_Drop_Box_01_Per[bossIndex];
        var standard_02 = standard_01 + TableData.Boss_Drop_Box_02_Per[bossIndex];
        var standard_03 = standard_02 + TableData.Boss_Drop_Box_03_Per[bossIndex];
        var standard_04 = standard_03 + TableData.Boss_Drop_Box_04_Per[bossIndex];
        var standard_05 = standard_04 + TableData.Boss_Drop_Box_05_Per[bossIndex];
        var standard_06 = standard_05 + TableData.Boss_Drop_Box_06_Per[bossIndex];
        var stnadard_07 = standard_06 + TableData.Boss_Drop_Box_07_Per[bossIndex];
        var ran = Random.Range(0.0f, 1.0f);

        if (ran <= standard_01)
            result = 0;
        else if (ran <= standard_02)
            result = 1;
        else if (ran <= standard_03)
            result = 2;
        else if (ran <= standard_04)
            result = 3;
        else if (ran <= standard_05)
            result = 4;
        else if (ran <= standard_06)
            result = 5;
        else
            result = 6;

        return result;
    }

    private int AcquireKeyRandom()
    {
        var result = 0;
        int bossIndex = (int)CentralInfoManager.bossStageCount - 1;

        var standard_01 = TableData.Boss_Drop_Key_01_Per[bossIndex];
        var standard_02 = standard_01 + TableData.Boss_Drop_Key_02_Per[bossIndex];
        var standard_03 = standard_02 + TableData.Boss_Drop_Key_03_Per[bossIndex];
        var standard_04 = standard_03 + TableData.Boss_Drop_Key_04_Per[bossIndex];
        var standard_05 = standard_04 + TableData.Boss_Drop_Key_05_Per[bossIndex];
        var standard_06 = standard_05 + TableData.Boss_Drop_Key_06_Per[bossIndex];
        var stnadard_07 = standard_06 + TableData.Boss_Drop_Key_07_Per[bossIndex];
        var ran = Random.Range(0.1f, 1.0f);

        if (ran <= standard_01)
            result = 0;
        else if (ran <= standard_02)
            result = 1;
        else if (ran <= standard_03)
            result = 2;
        else if (ran <= standard_04)
            result = 3;
        else if (ran <= standard_05)
            result = 4;
        else if (ran <= standard_06)
            result = 5;
        else
            result = 6;

        keyValue[result]++;

        return result;
    }
}