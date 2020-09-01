using System.Collections;
using UnityEngine;

public class EnchantDungeon : MonoBehaviour
{
    public static EnchantDungeon e_instance;

    // Start is called before the first frame update
    private void Start()
    {
        e_instance = this;
    }

    public void DungeonSetting()
    {
        StartCoroutine(DungeonSettingCO());
    }

    private IEnumerator DungeonSettingCO()
    {
        StateManager.instance.GoIdleState();

        DungeonManager.instance.n_StageTextGO.SetActive(false);
        for (int i = 0; i < DungeonManager.instance.n_buffGO.Length; i++)
        {
            DungeonManager.instance.n_buffGO[i].SetActive(false);
        }

        DungeonManager.instance.b_BossHPGO.SetActive(false);
        DungeonManager.instance.b_FloorCountGO.SetActive(false);
        DungeonManager.instance.b_TimerGO.SetActive(false);
        DungeonManager.instance.b_ExitGO.SetActive(false);
        DungeonManager.instance.b_speedGO.SetActive(false);

        StageManager.instance.StgeSetting(DungeonManager.DUNGEON_STATE.ENCHANT);
        yield return new WaitForSecondsRealtime(1.5f);
        DungeonManager.instance.startGO.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        StateManager.instance.RestartStageCoroutine();
    }
}