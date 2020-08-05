using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class AttackMeasurementManager : MonoBehaviour
{
    public static AttackMeasurementManager instance;

    // Start is called before the first frame update
    private void Start()
    {
        instance = this;
    }

    public decimal RealOffense()
    {
        decimal offense = 0;
        var textColor = "#FFE400";
        var offenseText = ObjectPoolManager.instance.PopFromPool_Offense();
        offense = (decimal)CharacterStatus.instance.AttackPower;    //여기 나중에 수치 받아와야함.
        offense = Math.Truncate(offense);

        if (CharacterState.instance.isCritical)
        {
            offense *= ((decimal)CharacterStatus.instance.CriticalDamage);    //여기 나중에 수치 받아와야함.-크리티컬
            textColor = "#FF4A00";
            EffectManager.instance.HitEffectPlay_Cri();
        }
        else
            EffectManager.instance.HitEffectPlay();

        offense = (int)Math.Round(offense);
        if (offense < 1)
            offense = 1;
        offense *= BuffManager.instance.buffValue[1];
        //offense *= 10000;////////////////

        if (CharacterState.instance.isMonsterDef)
            offenseText.transform.GetChild(0).gameObject.SetActive(true);
        else
            offenseText.transform.GetChild(0).gameObject.SetActive(false);

        offenseText.GetComponent<TextMeshPro>().text = string.Format("<color={0}>{1}</color>", textColor, GameManager.NumberNotation_comma(offense));

        offenseText.GetComponent<RectTransform>().localPosition = FormulaCollection.PosNearByMonster(MonsterManager.instance.progressBarPos);
        offenseText.GetComponent<OffensePowerText>().StartTextUp();
        return offense;
    }

    public bool ReturnIsCritical(out bool isMonsterDef)
    {
        var result = false;

        double critical = CharacterStatus.instance.CriticalPer;//여기 나중에 수치 받아와야함.-크리티컬
        double beforeCriti = critical;

        //beforeCriti = 120;//////////////////////////////

        if (DungeonManager.instance.dungeon_state == DungeonManager.DUNGEON_STATE.NORMAL)
        {
            if (CentralInfoManager.instance.IsBossMonster())
                critical -= TableData.Stage_Boss_Monster_Critical_Def[(int)CentralInfoManager.stageCount - 1];
            else
                critical -= TableData.Stage_Normal_Monster_Critical_Def[(int)CentralInfoManager.stageCount - 1];
        }

        if (critical < 0)
            critical = 0;

        //critical = 0.5f;/////////////////////////////////

        critical *= 100;
        beforeCriti *= 100;

        int standard = (int)(critical);

        var ran = Random.Range(1, 101);

        if (ran <= standard)
        {
            result = true;
            isMonsterDef = false;
        }
        else
        {
            if (beforeCriti >= 100 && critical < 100)
                isMonsterDef = true;
            else
                isMonsterDef = false;
        }

        return result;
    }
}