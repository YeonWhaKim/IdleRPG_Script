using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

public class PetManager : MonoBehaviour
{
    public static PetManager instance;
    public ShopMenu shopMenu;
    public const string PETMAXLEVEL_SIGN = "-";
    public const int PETKINDS = 3;
    public GameObject petGO;
    public SkeletonAnimation skeletonAnimation;
    public List<int> petLevel;
    public List<bool> isPetBuy;
    public Sprite[] petSprite;

    //public string[] petName = { "미니핀", "푸들", "나비" };
    public string[] petStatName = { "치명타 확률", "치명타 데미지", "공격력" };

    public List<float> petStat;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    public void StatSetting()
    {
        CharacterStatus.instance.CriticalPer += petStat[0];
        CharacterStatus.instance.CriticalDamage += petStat[1];
        CharacterStatus.instance.AttackPower += petStat[2];
    }

    public void PetBuy(int index)
    {
        // 보석 재화 적용하기
        petLevel[index]++;
        isPetBuy[index] = true;
        petStat[index] = TableData.Pet_Normal_Status[index];
        // 각각 스텟 업하기+++++
        switch (index)
        {
            case 0:
                CharacterStatus.instance.CriticalPer += TableData.Pet_Normal_Status[index];
                break;

            case 1:
                CharacterStatus.instance.CriticalDamage += TableData.Pet_Normal_Status[index];
                break;

            case 2:
                CharacterStatus.instance.AttackPower += TableData.Pet_Normal_Status[index];
                break;

            default:
                break;
        }
    }

    public void PetUpgrade(int index)
    {
        // 보석 재화 적용하기
        petLevel[index]++;
        petStat[index] += (TableData.Pet_Status_Upgrade[index] + ((petLevel[index] / 10) * TableData.Pet_Status_Upgrade10[index]));
        // 각각 스텟 업하기++++++++
        switch (index)
        {
            case 0:
                CharacterStatus.instance.CriticalPer += (TableData.Pet_Status_Upgrade[index] + ((petLevel[index] / 10) * TableData.Pet_Status_Upgrade10[index]));
                break;

            case 1:
                CharacterStatus.instance.CriticalDamage += (TableData.Pet_Status_Upgrade[index] + ((petLevel[index] / 10) * TableData.Pet_Status_Upgrade10[index]));
                break;

            case 2:
                CharacterStatus.instance.AttackPower += (TableData.Pet_Status_Upgrade[index] + ((petLevel[index] / 10) * TableData.Pet_Status_Upgrade10[index]));
                break;

            default:
                break;
        }
    }

    public void PetApply(int index)
    {
        skeletonAnimation.Skeleton.SetSkin(
            string.Format("pet{0}", string.Format("{0:D3}", index + 1)));
        if (PetMenu.instance.mountingPetIndex < 0)
            petGO.SetActive(false);
        else
            petGO.SetActive(true);
    }

    public string ReturnPetName(int index)
    {
        if (isPetBuy[index].Equals(false))
            return TableData.Pet_Type[index];
        else
        {
            if (petLevel[index].Equals(TableData.Pet_Max_Level[index]))
                return string.Format("<color=#67D3BB><b>Lv.Max({0})</b></color> {1}",
                petLevel[index], TableData.Pet_Type[index]);
            else
                return string.Format("<color=#67D3BB><b>Lv.{0}</b></color> {1}",
                    petLevel[index], TableData.Pet_Type[index]);
        }
    }

    public string ReturnPetStat(int index)
    {
        var result = "";
        var upValue = TableData.Pet_Status_Upgrade[index] + ((petLevel[index] / 10) * TableData.Pet_Status_Upgrade10[index]);
        if (index != 2)
            upValue *= 100;

        if (isPetBuy[index].Equals(false))
        {
            if (index == 2)
                result = string.Format("{0} : <color=#EFC54D><b>+{1}{2}</b></color>",
                    petStatName[index], GameManager.NumberNotation_comma((double)TableData.Pet_Normal_Status[index]), ReturnStatUnit(index));
            else
                result = string.Format("{0} : <color=#EFC54D><b>+{1}{2}</b></color>",
    petStatName[index], GameManager.NumberNotation_comma((double)TableData.Pet_Normal_Status[index] * 100), ReturnStatUnit(index));
        }
        else
        {
            if (index == 2)
                result = string.Format("{0} : <color=#EFC54D><b>+{1}{2} → +{3}{4}</b></color>",
    petStatName[index], GameManager.NumberNotation_comma((double)petStat[index]), ReturnStatUnit(index),
    GameManager.NumberNotation_comma((double)petStat[index] + upValue), ReturnStatUnit(index));
            else
                result = string.Format("{0} : <color=#EFC54D><b>+{1}{2} → +{3}{4}</b></color>",
    petStatName[index], GameManager.NumberNotation_comma((double)petStat[index] * 100d), ReturnStatUnit(index),
    GameManager.NumberNotation_comma((double)petStat[index] * 100d + upValue), ReturnStatUnit(index));
        }

        if (petLevel[index].Equals(TableData.Pet_Max_Level[index]))
        {
            if (index == 2)
                result = string.Format("{0} : <color=#EFC54D><b>+{1}{2}</b></color>",
    petStatName[index], GameManager.NumberNotation_comma((double)petStat[index]), ReturnStatUnit(index));
            else
                result = string.Format("{0} : <color=#EFC54D><b>+{1}{2}</b></color>",
    petStatName[index], GameManager.NumberNotation_comma((double)petStat[index] * 100d), ReturnStatUnit(index));
        }
        return result;
    }

    public string ReturnUpgradePrice(int index)
    {
        var result = "";
        if (isPetBuy[index].Equals(false))
            result = shopMenu.ReturnShopPrice(string.Format("SH40{0}", index)).ToString();
        else
        {
            switch (index)
            {
                case 0:
                    result = TableData.Minipin_Jewel[petLevel[index]].ToString();
                    break;

                case 1:
                    result = TableData.Poodle_Jewel[petLevel[index]].ToString();
                    break;

                case 2:
                    result = TableData.Cat_Jewel[petLevel[index]].ToString();
                    break;

                default:
                    break;
            }
        }

        if (petLevel[index].Equals(TableData.Pet_Max_Level[index]))
            result = PETMAXLEVEL_SIGN;
        return result;
    }

    private string ReturnStatUnit(int index)
    {
        var result = "";
        switch (index)
        {
            case 0:
            case 1:
                result = "%";
                break;

            default:
                break;
        }
        return result;
    }
}