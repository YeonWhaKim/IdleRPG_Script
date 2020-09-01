using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
    public static EquipManager instance;
    public CentralInfoManager centralInfoManager;
    public const string EOT_Stage = "Stage";
    public const string EOT_STR = "STR";
    public const string EOT_AGI = "AGI";
    public const string EOT_DEX = "DEX";
    public const string EOT_LUK = "LUK";
    public const string EOT_REPLAY = "REPLAY";
    public List<int> equipState_weapon = new List<int>();
    public List<int> equipState_head = new List<int>();
    public List<int> equipState_chest = new List<int>();
    public List<int> equipState_gloves = new List<int>();
    public List<int> equipState_pants = new List<int>();
    public List<int> equipState_shoes = new List<int>();
    public List<int> equipState_back = new List<int>();
    public List<int> equipState_face = new List<int>();

    public List<decimal> weaponStat = new List<decimal>();
    public List<decimal> headStat = new List<decimal>();
    public List<decimal> chestStat = new List<decimal>();
    public List<decimal> glovesStat = new List<decimal>();
    public List<decimal> pantsStat = new List<decimal>();
    public List<decimal> shoesStat = new List<decimal>();
    public List<decimal> backStat = new List<decimal>();
    public List<decimal> faceStat = new List<decimal>();

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    #region 장비 인터렉션

    public void EquipLevelUp_FirstBuy(bool isFirstBuy, int whatkind, int equipIndex, int upvalue)
    {
        decimal d_upValue = TableData.Item_Status_Upgrade[ReturnRealIndex(whatkind, equipIndex)] * upvalue;
        decimal sumValue = 0;
        // 스텟도 설정해줘야함.

        switch (whatkind)
        {
            case 0:
                if (isFirstBuy)
                {
                    d_upValue = TableData.Item_Status_First[ReturnRealIndex(whatkind, equipIndex)];
                    EquipStateChange(whatkind, equipIndex, 1);
                }
                else
                    centralInfoManager.weapon_Level[equipIndex] += upvalue;

                sumValue = Math.Truncate(d_upValue);
                weaponStat[equipIndex] += sumValue;
                CharacterStatus.instance.AttackPower += (double)sumValue;
                break;

            case 1:
                if (isFirstBuy)
                {
                    d_upValue = TableData.Item_Status_First[ReturnRealIndex(whatkind, equipIndex)];
                    EquipStateChange(whatkind, equipIndex, 1);
                }
                else
                    centralInfoManager.head_Level[equipIndex] += upvalue;

                sumValue = Math.Round(d_upValue, 4);
                headStat[equipIndex] += sumValue;
                CharacterStatus.instance.CriticalPer += (double)(sumValue);
                break;

            case 2:
                if (isFirstBuy)
                {
                    d_upValue = TableData.Item_Status_First[ReturnRealIndex(whatkind, equipIndex)];
                    EquipStateChange(whatkind, equipIndex, 1);
                }
                else
                    centralInfoManager.chest_Level[equipIndex] += upvalue;

                sumValue = Math.Round(d_upValue, 4);
                chestStat[equipIndex] += sumValue;
                CharacterStatus.instance.CriticalDamage += (double)(sumValue);
                break;

            case 3:
                if (isFirstBuy)
                {
                    d_upValue = TableData.Item_Status_First[ReturnRealIndex(whatkind, equipIndex)];
                    EquipStateChange(whatkind, equipIndex, 1);
                }
                else
                    centralInfoManager.gloves_Level[equipIndex] += upvalue;

                sumValue = Math.Round(d_upValue, 4);
                glovesStat[equipIndex] += sumValue;
                CharacterStatus.instance.MGG += (double)(sumValue);
                break;

            case 4:

                if (isFirstBuy)
                {
                    d_upValue = TableData.Item_Status_First[ReturnRealIndex(whatkind, equipIndex)];
                    EquipStateChange(whatkind, equipIndex, 1);
                }
                else
                    centralInfoManager.pants_Level[equipIndex] += upvalue;

                sumValue = Math.Truncate(d_upValue);
                pantsStat[equipIndex] += sumValue;
                CharacterStatus.instance.AttackPower += (double)sumValue;
                break;

            case 5:
                if (isFirstBuy)
                {
                    d_upValue = TableData.Item_Status_First[ReturnRealIndex(whatkind, equipIndex)];
                    EquipStateChange(whatkind, equipIndex, 1);
                }
                else
                    centralInfoManager.shoes_Level[equipIndex] += upvalue;

                sumValue = Math.Round(d_upValue, 4);
                shoesStat[equipIndex] += sumValue;
                CharacterStatus.instance.CriticalPer += (double)(sumValue);
                break;

            case 6:
                if (isFirstBuy)
                {
                    d_upValue = TableData.Item_Status_First[ReturnRealIndex(whatkind, equipIndex)];
                    EquipStateChange(whatkind, equipIndex, 1);
                }
                else
                    centralInfoManager.back_Level[equipIndex] += upvalue;

                sumValue = Math.Round(d_upValue, 4);
                backStat[equipIndex] += sumValue;
                CharacterStatus.instance.CriticalDamage += (double)(sumValue);
                break;

            case 7:
                if (isFirstBuy)
                {
                    d_upValue = TableData.Item_Status_First[ReturnRealIndex(whatkind, equipIndex)];
                    EquipStateChange(whatkind, equipIndex, 1);
                }
                else
                    centralInfoManager.face_Level[equipIndex] += upvalue;

                sumValue = Math.Round(d_upValue, 4);
                faceStat[equipIndex] += sumValue;
                CharacterStatus.instance.MGG += (double)(sumValue);
                break;

            default:
                break;
        }
    }

    #endregion 장비 인터렉션

    #region 장비 정보 return

    public string ReturnEquipStatValue(int whatkind, int equipIndex, int upValue)
    {
        decimal result = 0;
        string res = "";
        switch (whatkind)
        {
            case 0:
                result = weaponStat[equipIndex] + (upValue * TableData.Item_Status_Upgrade[ReturnRealIndex(whatkind, equipIndex)]);
                result = Math.Truncate(result);
                res = result.ToString();
                break;

            case 1:
                result = headStat[equipIndex] + (upValue * TableData.Item_Status_Upgrade[ReturnRealIndex(whatkind, equipIndex)]);
                result = Math.Truncate(result * 100m);
                res = string.Format("{0}", result);
                break;

            case 2:
                result = chestStat[equipIndex] + (upValue * TableData.Item_Status_Upgrade[ReturnRealIndex(whatkind, equipIndex)]);
                result = Math.Truncate(result * 100m);
                res = string.Format("{0}%", result);
                break;

            case 3:
                result = glovesStat[equipIndex] + (upValue * TableData.Item_Status_Upgrade[ReturnRealIndex(whatkind, equipIndex)]);
                //result = Math.Truncate(result);
                res = string.Format("{0}%", Math.Round(result * 100m, 1));
                break;

            case 4:
                result = pantsStat[equipIndex] + (upValue * TableData.Item_Status_Upgrade[ReturnRealIndex(whatkind, equipIndex)]);
                result = Math.Truncate(result);
                res = result.ToString();
                break;

            case 5:
                result = shoesStat[equipIndex] + (upValue * TableData.Item_Status_Upgrade[ReturnRealIndex(whatkind, equipIndex)]);
                result = Math.Truncate(result * 100m);
                res = string.Format("{0}", result);
                break;

            case 6:
                result = backStat[equipIndex] + (upValue * TableData.Item_Status_Upgrade[ReturnRealIndex(whatkind, equipIndex)]);
                result = Math.Truncate(result * 100m);
                res = string.Format("{0}%", result);
                break;

            case 7:
                result = faceStat[equipIndex] + (upValue * TableData.Item_Status_Upgrade[ReturnRealIndex(whatkind, equipIndex)]);
                //result = Math.Truncate(result);
                res = string.Format("{0}%", Math.Round(result * 100m, 1));
                break;

            default:
                break;
        }
        return res;
    }

    public int ReturnEquipState(int whatKind, int index)
    {
        var result = 0;
        switch (whatKind)
        {
            case 0:
                result = equipState_weapon[index];
                break;

            case 1:
                result = equipState_head[index];
                break;

            case 2:
                result = equipState_chest[index];
                break;

            case 3:
                result = equipState_gloves[index];
                break;

            case 4:
                result = equipState_pants[index];
                break;

            case 5:
                result = equipState_shoes[index];
                break;

            case 6:
                result = equipState_back[index];
                break;

            case 7:
                result = equipState_face[index];
                break;
        }
        return result;
    }

    public decimal Return_EquipLevelUpPrice(int whatKind, int equipIndex, int equipLevel)
    {
        var grade = TableData.Item_Grade[ReturnRealIndex(whatKind, equipIndex)];
        decimal result = 0;
        switch (grade)
        {
            case "S":
                result = TableData.Upgrade_S_Gold[equipLevel - 1];
                break;

            case "A":
                result = TableData.Upgrade_A_Gold[equipLevel - 1];
                break;

            case "B":
                result = TableData.Upgrade_B_Gold[equipLevel - 1];
                break;

            case "C":
                result = TableData.Upgrade_C_Gold[equipLevel - 1];
                break;

            case "D":
                result = TableData.Upgrade_D_Gold[equipLevel - 1];
                break;

            case "E":
                result = TableData.Upgrade_E_Gold[equipLevel - 1];
                break;

            case "F":
                result = TableData.Upgrade_F_Gold[equipLevel - 1];
                break;
        }
        return result;
    }

    public string Return_EquipName(int whatKind, int equipIndex)
    {
        return string.Format("<color=#F0DEB8><b>{0}</b></color>", TableData.Item_Name[ReturnRealIndex(whatKind, equipIndex)]);
    }

    public string Return_EquipGrade(int whatKind, int equipIndex)
    {
        var grade = TableData.Item_Grade[ReturnRealIndex(whatKind, equipIndex)];
        var result = "";
        switch (grade)
        {
            case "S":
                result = string.Format("<color=#FCFB86><b>{0}</b></color>", grade);
                break;

            case "A":
                result = string.Format("<color=#D88BFB><b>{0}</b></color>", grade);
                break;

            case "B":
                result = string.Format("<color=#7AD3E3><b>{0}</b></color>", grade);
                break;

            case "C":
                result = string.Format("<color=#B8BE7E><b>{0}</b></color>", grade);
                break;

            case "D":
                result = string.Format("<color=#AFCD8F><b>{0}</b></color>", grade);
                break;

            case "E":
                result = string.Format("<color=#BCA086><b>{0}</b></color>", grade);
                break;

            case "F":
                result = string.Format("<color=#AEAEAE><b>{0}</b></color>", grade);
                break;
        }
        return result;
    }

    public string Return_EquipStatus(int whatKind, int equipIndex)
    {
        var status = TableData.Item_Status[ReturnRealIndex(whatKind, equipIndex)];
        var result = "";
        switch (status)
        {
            case "ATK":
                result = "공격력";
                break;

            case "CTL":
                result = "치명타";
                break;

            case "CTD":
                result = "치명타 데미지";
                break;

            case "MGG":
                result = "추가 처치골드";
                break;

            default:
                break;
        }
        //Debug.Log(whatKind + " / " + equipIndex + " / " + status + " / " + result);
        return result;
    }

    public string Return_EquipOpenCondition(int whatkind, int equipIndex)
    {
        var openType = TableData.Item_Open_Type[ReturnRealIndex(whatkind, equipIndex)];
        var result = "";
        switch (openType)
        {
            case EOT_Stage:
                result = string.Format("활성화 조건 : <b>스테이지 {0}</b> 클리어", TableData.Item_Open_Value[ReturnRealIndex(whatkind, equipIndex)]);
                break;

            case EOT_STR:
                result = string.Format("활성화 조건 : <b>힘 {0}</b> 달성", TableData.Item_Open_Value[ReturnRealIndex(whatkind, equipIndex)]);
                break;

            case EOT_AGI:
                result = string.Format("활성화 조건 : <b>민첩 {0}</b> 달성", TableData.Item_Open_Value[ReturnRealIndex(whatkind, equipIndex)]);
                break;

            case EOT_DEX:
                result = string.Format("활성화 조건 : <b>솜씨 {0}</b> 달성", TableData.Item_Open_Value[ReturnRealIndex(whatkind, equipIndex)]);
                break;

            case EOT_LUK:
                result = string.Format("활성화 조건 : <b>행운 {0}</b> 달성", TableData.Item_Open_Value[ReturnRealIndex(whatkind, equipIndex)]);
                break;

            case EOT_REPLAY:
                result = string.Format("활성화 조건 : <b>전승 {0}</b> 회", TableData.Item_Open_Value[ReturnRealIndex(whatkind, equipIndex)]);
                break;

            default:
                break;
        }
        return result;
    }

    public string Return_EquipBuyInfo(int whatkind, int equipIndex, out int buyValue)
    {
        buyValue = TableData.Item_Buy_Value[ReturnRealIndex(whatkind, equipIndex)];
        return TableData.Item_Buy_Type[ReturnRealIndex(whatkind, equipIndex)];
    }

    public int Return_EquipMaxLevel(int whatkind, int equipIndex)
    {
        return TableData.Item_Max_Level[ReturnRealIndex(whatkind, equipIndex)];
    }

    public string Return_EquipLookNumber(int whakKind, int equipIndex)
    {
        return TableData.Item_Look_Number[ReturnRealIndex(whakKind, equipIndex)];
    }

    public int Return_EquipGemOpenValue(int whatkind, int equipindex)
    {
        return TableData.Gem_Open_Value[ReturnRealIndex(whatkind, equipindex)];
    }

    #endregion 장비 정보 return

    #region 장비관련 기능

    public int ReturnRealIndex(int whatKind, int equipIndex)
    {
        return ((TableData.Item_Name.Count / 8) * whatKind) + equipIndex;
    }

    public void JudgeEquipRock(string condition, double value)
    {
        for (int i = 0; i < TableData.Item_Buy_Type.Count; i++)
        {
            if (TableData.Item_Open_Type[i].Equals(condition))
            {
                if (value >= TableData.Item_Open_Value[i])
                {
                    var quo = i / (TableData.Item_Open_Value.Count / CentralInfoManager.EQUIPKIND);
                    var remainder = i % (TableData.Item_Open_Value.Count / CentralInfoManager.EQUIPKIND);
                    if (ReturnEquipState(quo, remainder) < 0)
                    {
                        EquipStateChange(quo, remainder, 0);
                        EquipMenu.instance.EquipRockSetting(quo, remainder);
                    }
                }
            }
        }
    }

    public void EquipUnLockJewel(int whatKind, int equipindex, int jewel)
    {
        CurrencyManager.instance.Jewel -= jewel;
        EquipStateChange(whatKind, equipindex, 0);
        EquipMenu.instance.EquipRockSetting(whatKind, equipindex);
    }

    public void EquipFirstStatSetting()
    {
        for (int i = 0; i < TableData.Item_Name.Count / CentralInfoManager.EQUIPKIND; i++)
        {
            if (equipState_weapon[i] > 0)
                CharacterStatus.instance.AttackPower += (double)weaponStat[i];
            if (equipState_head[i] > 0)
                CharacterStatus.instance.CriticalPer += (double)(headStat[i]);
            if (equipState_chest[i] > 0)
                CharacterStatus.instance.CriticalDamage += (double)(chestStat[i]);
            if (equipState_gloves[i] > 0)
                CharacterStatus.instance.MGG += (double)(glovesStat[i]);
            if (equipState_pants[i] > 0)
                CharacterStatus.instance.AttackPower += (double)pantsStat[i];
            if (equipState_shoes[i] > 0)
                CharacterStatus.instance.CriticalPer += (double)(shoesStat[i]);
            if (equipState_back[i] > 0)
                CharacterStatus.instance.CriticalDamage += (double)(backStat[i]);
            if (equipState_face[i] > 0)
                CharacterStatus.instance.MGG += (double)(faceStat[i]);
        }
    }

    public void EquipStateChange(int whatKind, int index, int changeValue)
    {
        switch (whatKind)
        {
            case 0:
                equipState_weapon[index] = changeValue;
                break;

            case 1:
                equipState_head[index] = changeValue;
                break;

            case 2:
                equipState_chest[index] = changeValue;
                break;

            case 3:
                equipState_gloves[index] = changeValue;
                break;

            case 4:
                equipState_pants[index] = changeValue;
                break;

            case 5:
                equipState_shoes[index] = changeValue;
                break;

            case 6:
                equipState_back[index] = changeValue;
                break;

            case 7:
                equipState_face[index] = changeValue;
                break;

            default:
                break;
        }
    }

    public List<int> ReturnEquipStateList(int whatkind)
    {
        List<int> returnValue = new List<int>();
        switch (whatkind)
        {
            case 0:
                returnValue = equipState_weapon;
                break;

            case 1:
                returnValue = equipState_head;
                break;

            case 2:
                returnValue = equipState_chest;
                break;

            case 3:
                returnValue = equipState_gloves;
                break;

            case 4:
                returnValue = equipState_pants;
                break;

            case 5:
                returnValue = equipState_shoes;
                break;

            case 6:
                returnValue = equipState_back;
                break;

            case 7:
                returnValue = equipState_face;
                break;

            default:
                break;
        }
        return returnValue;
    }

    #endregion 장비관련 기능
}