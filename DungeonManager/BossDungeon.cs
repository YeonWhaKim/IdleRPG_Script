using System.Collections;
using UnityEngine;

public class BossDungeon : MonoBehaviour
{
    public static BossDungeon b_instance;

    // Start is called before the first frame update
    private void Start()
    {
        b_instance = this;
    }

    public void DungeonSetting()
    {
        StartCoroutine(DungeonSettingCO());
    }

    private IEnumerator DungeonSettingCO()
    {
        StateManager.instance.GoIdleState();
        BoxManager.instance.KeyListReset();
        DungeonManager.instance.b_BossHPGO.SetActive(true);
        DungeonManager.instance.b_FloorCountGO.SetActive(true);
        DungeonManager.instance.b_TimerGO.SetActive(true);
        DungeonManager.instance.b_ExitGO.SetActive(true);
        DungeonManager.instance.b_speedGO.SetActive(true);

        DungeonManager.instance.n_StageTextGO.SetActive(false);
        for (int i = 0; i < DungeonManager.instance.n_buffGO.Length; i++)
        {
            DungeonManager.instance.n_buffGO[i].SetActive(false);
        }

        BossDungeonManager.instance.bossHpProgressBar.fillAmount = 1;

        StageManager.instance.StgeSetting(DungeonManager.DUNGEON_STATE.BOSS);
        yield return new WaitForSecondsRealtime(0.5f);
        BossDungeonMenu.instance.entranceButton_ticket.interactable = false;     //입장버튼 비활성화
        BossDungeonMenu.instance.entranceButton_ad.interactable = false;
        BossDungeonMenu.instance.entranceText_ticket.text = "던전 진행중...";
        BossDungeonMenu.instance.entranceText_ad.text = "던전 진행중...";
        DungeonManager.instance.startGO.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        DungeonManager.instance.animationNotActive = false;
        StateManager.instance.RestartStageCoroutine();
        BossDungeonManager.instance.BossDungeonTimerGo();
    }
}