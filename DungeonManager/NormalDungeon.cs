using UnityEngine;

public class NormalDungeon : MonoBehaviour
{
    public static NormalDungeon n_instance;

    // Start is called before the first frame update
    private void Start()
    {
        n_instance = this;
    }

    public void DungeonSetting()
    {
        DungeonManager.instance.n_StageTextGO.SetActive(true);
        for (int i = 0; i < DungeonManager.instance.n_buffGO.Length; i++)
        {
            DungeonManager.instance.n_buffGO[i].SetActive(true);
        }
        //if (SpeedManager.instance.speedDuration <= 0)
        //    DungeonManager.instance.n_buffGO[DungeonManager.instance.n_buffGO.Length - 1].SetActive(false);
        //else
        //    DungeonManager.instance.n_buffGO[DungeonManager.instance.n_buffGO.Length - 1].SetActive(true);

        DungeonManager.instance.b_BossHPGO.SetActive(false);
        DungeonManager.instance.b_FloorCountGO.SetActive(false);
        DungeonManager.instance.b_TimerGO.SetActive(false);
        DungeonManager.instance.b_ExitGO.SetActive(false);
        DungeonManager.instance.b_speedGO.SetActive(false);

        StageManager.instance.StgeSetting(DungeonManager.DUNGEON_STATE.NORMAL);
        DungeonManager.instance.animationNotActive = false;
        StateManager.instance.RestartStageCoroutine();
    }
}