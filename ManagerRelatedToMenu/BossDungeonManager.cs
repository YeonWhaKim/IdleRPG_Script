using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossDungeonManager : MonoBehaviour
{
    public static BossDungeonManager instance;
    private const int bossDungeonTime = 60;
    public float tmpTime;
    public decimal monsterHp;
    public decimal monsterHp_standard;
    public int killedBossCount;
    private Coroutine timerCO;

    [Header("Up UI")]
    public Text bossDungeonFloorText;

    public Image bossHpProgressBar;
    public Text timerText;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    public void BossDungeonSetting()
    {
        DungeonManager.instance.DungeonChange(DungeonManager.DUNGEON_STATE.BOSS);
        killedBossCount = 0;
        bossDungeonFloorText.text = string.Format("{0}층", CentralInfoManager.bossStageCount);
    }

    public void BossDungeonTimerGo()
    {
        timerCO = StartCoroutine(TimerStartCo());
    }

    public void NormalDungeonSetting()
    {
        DungeonManager.instance.DungeonChange(DungeonManager.DUNGEON_STATE.NORMAL);
    }

    private IEnumerator TimerStartCo()
    {
        tmpTime = bossDungeonTime;
        while (tmpTime > 0)
        {
            timerText.text = GameManager.MM__SS(tmpTime);
            yield return new WaitForSeconds(1f);
            tmpTime--;
        }
        DungeonEnd(false);
    }

    public void BossMonsterHPProgressBarSetting(decimal offense)
    {
        monsterHp -= offense;
        bossHpProgressBar.fillAmount = (float)(monsterHp / monsterHp_standard);
    }

    public void Monsterkilled()
    {
        tmpTime += TableData.Dungeon_Clear_Plus_Time[CentralInfoManager.bossStageCount - 1];
        killedBossCount++;
        CentralInfoManager.bossStageCount++;
        if (CentralInfoManager.bossStageCount > TableData.Dungeon_Boss_HP.Count)
        {
            CentralInfoManager.bossStageCount = TableData.Dungeon_Boss_HP.Count;
            if (CentralInfoManager.isDungeonConquer.Equals(false))
            {
                DungeonEnd(true);
                CentralInfoManager.isDungeonConquer = true;
            }
            else
            {
                bossDungeonFloorText.text = string.Format("{0}층", CentralInfoManager.bossStageCount);
                BossDungeonMenu.instance.currFloorTitleText.text = string.Format("<color=#FA6E6D>{0}</color>층 도전중", CentralInfoManager.bossStageCount);
            }
        }
        else
        {
            bossDungeonFloorText.text = string.Format("{0}층", CentralInfoManager.bossStageCount);
            BossDungeonMenu.instance.currFloorTitleText.text = string.Format("<color=#FA6E6D>{0}</color>층 도전중", CentralInfoManager.bossStageCount);
        }
        BossDungeonMenu.instance.BossInfoSetting();
    }

    public string RankSetting()
    {
        return "";
    }

    public void DungeonEnd(bool isDungeonConquer)
    {
        StateManager.instance.GoIdleState();

        BossDungeonMenu.instance.DungeonEnd(isDungeonConquer);
    }

    public void DungeonEnd_Exit()
    {
        if (timerCO != null)
            StopCoroutine(timerCO);
    }
}