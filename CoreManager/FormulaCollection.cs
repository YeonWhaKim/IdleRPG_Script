using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FormulaCollection : MonoBehaviour
{
    public CharacterStatus characterstatus;

    public static decimal StageGoldReward(long stage)
    {
        decimal reward = 0;
        decimal max = 0;
        decimal min = 0;
        int ranAddedValue = 0;
        if (CentralInfoManager.instance.IsBossMonster())
        {
            max = TableData.Stage_Boss_Clear_Gold_Max[(int)stage - 1];
            min = TableData.Stage_Boss_Clear_Gold_Min[(int)stage - 1];
        }
        else
        {
            max = TableData.Stage_Normal_Clear_Gold_Max[(int)stage - 1];
            min = TableData.Stage_Normal_Clear_Gold_Min[(int)stage - 1];
        }
        ranAddedValue = Random.Range(0, (int)(max - min) + 1);
        reward = min + ranAddedValue;

        if (reward > max)
            reward = max;

        reward *= (((decimal)CharacterStatus.instance.MGG));
        reward = Math.Round(reward);
        return reward;
    }

    public static int RebirthReward()
    {
        return (int)CentralInfoManager.stageCount * (int)(CentralInfoManager.stageCount * 0.01f);
    }

    public static Vector2 PosNearByMonster(Vector2 standardPos)
    {
        int ranX = Random.Range((int)standardPos.x - 40, (int)standardPos.x + 40);
        int ranY = Random.Range((int)standardPos.y - 60, (int)standardPos.y - 30);
        return new Vector2(ranX, ranY);
    }

    public static int RequiredLpPoint(long statusValue)
    {
        if (statusValue <= 0)
            return TableData.Status_Up_Lp[0];
        else
        {
            int index = (int)(statusValue / 100);
            var judge = statusValue % 100;
            if (judge.Equals(0))
                index -= 1;
            return TableData.Status_Up_Lp[index];
        }
    }

    public double ReturnTotalStats()
    {
        var at = characterstatus.AttackPower * TableData.Status_ComatPoint[0];
        var cp = characterstatus.CriticalPer * TableData.Status_ComatPoint[1] * 100;
        var cd = characterstatus.CriticalDamage * TableData.Status_ComatPoint[2] * 100;
        var mgg = characterstatus.MGG * TableData.Status_ComatPoint[3] * 100;

        var sum = at + cp + cd + mgg;
        return Math.Truncate(sum);
    }
}