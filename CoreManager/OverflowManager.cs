using UnityEngine;

public class OverflowManager : MonoBehaviour
{
    public static OverflowManager overflowManager;

    private void Awake()
    {
        overflowManager = this;
    }

    public static void ChkStageOverFlow()
    {
        if (CentralInfoManager.stageCount > TableData.Stage_Normal_Clear_Exp.Count)
            CentralInfoManager.stageCount = TableData.Stage_Normal_Clear_Exp.Count;
    }

    public static void ChkDungeonOverFlow()
    {
    }

    public static void ChkStatusOverFlow(long value)
    {
        if (value > TableData.Status_Max[TableData.Status_Max.Count - 1])
            value = TableData.Status_Max[TableData.Status_Max.Count - 1];
    }

    public static void ChkReplayOverFlow()
    {
    }
}