using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager instance;
    public SkeletonAnimation monsterSkeletonAnimation;
    private Skeleton monsterSkeleton;
    public CharacterState characterState;
    public Vector2 progressBarPos;
    public GameObject bossAppearEffectGO;
    public GameObject monsterGO;
    private decimal monsterHP;

    [Header("UI")]
    public GameObject progressBar;

    private Vector2[] monsterHPbarPos ={
        new Vector2(-10,-65), new Vector2(0,-75), new Vector2(-4,-20),
        new Vector2(-4,-50), new Vector2(-11,-20),new Vector2(-15,-30),
        new Vector2(-10,-10), new Vector2(0,-25), new Vector2(0,-40),
        new Vector2(-10,-10), new Vector2(0,-40), new Vector2(0,30),
        new Vector2(0,-4.5f), new Vector2(-10,0), new Vector2(-10,8),
        new Vector2(1,-25), new Vector2(-10,35), new Vector2(0,25),
        new Vector2(-11,-45), new Vector2(-5,6), new Vector2(-20,-8)
    };

    private Vector2[] bossMonserHPbarPos = {
        new Vector2(-10,40),
        new Vector2(-10,30),
        new Vector2(-15,20),
        new Vector2(-10,65),
        new Vector2(0,40),
        new Vector2(-10,50),
        new Vector2(-35,25),
        new Vector2(-10,45),
        new Vector2(-35,50),
        new Vector2(-10,65),
        new Vector2(-60,50),
        new Vector2(0,85),
        new Vector2(-16,46.5f),
        new Vector2(0,65)
    };

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        monsterSkeleton = monsterSkeletonAnimation.skeleton;
    }

    public decimal MonsterSetting(DungeonManager.DUNGEON_STATE dungeonState)
    {
        decimal result = 0;
        switch (dungeonState)
        {
            case DungeonManager.DUNGEON_STATE.NORMAL:
                result = NormalStageMonster();
                break;

            case DungeonManager.DUNGEON_STATE.BOSS:
                result = BossMonsterSetting();
                break;

            default:
                break;
        }
        return result;
    }

    private decimal NormalStageMonster()
    {
        var ranStandardValue = RealStageCount_Loop() * 3; //여기서 loop로 변경
        var random = UnityEngine.Random.Range(ranStandardValue - 2, ranStandardValue + 1);
        string skinName = "";
        progressBarPos = Vector2.zero;
        decimal monsterHP = 0;

        if (CentralInfoManager.instance.IsBossMonster())
        {
            int bossIndex = (int)CentralInfoManager.stageCount % StageManager.CLASSIFIED_BG_COUNT;
            if (bossIndex <= 0)
                bossIndex = StageManager.CLASSIFIED_BG_COUNT;
            int ran = Random.Range(0, 2);
            if (ran.Equals(0))
            {
                skinName = string.Format("boss/boss{0}", string.Format("{0:D3}", bossIndex));
            }
            else
            {
                skinName = string.Format("boss/boss{0}_2", string.Format("{0:D3}", bossIndex));
            }

            progressBarPos = bossMonserHPbarPos[((bossIndex - 1) * 2) + ran];
            monsterHP = TableData.Stage_Boss_Monster_Hp[(int)CentralInfoManager.stageCount - 1]; //여기 나중에 수치 받아와야함.- 보스몬스터
            StartCoroutine(BossAppearSoundPlay());
        }
        else
        {
            skinName = string.Format("monster/monster{0}", string.Format("{0:D3}", (int)random));
            progressBarPos = monsterHPbarPos[(int)random - 1];
            monsterHP = TableData.Stage_Normal_Monster_Hp[(int)CentralInfoManager.stageCount - 1] +
                (TableData.Stage_Normal_Clear_Hp_Plus[(int)CentralInfoManager.stageCount - 1] * (CentralInfoManager.regionCount - 1)); ;    //여기 나중에 수치 받아와야함. - 일반몬스터
        }

        monsterSkeletonAnimation.Skeleton.SetSkin(skinName);
        progressBar.GetComponent<RectTransform>().localPosition = progressBarPos;
        return monsterHP;
    }

    private decimal BossMonsterSetting()
    {
        var random = CentralInfoManager.bossStageCount % (StageManager.CLASSIFIED_BG_COUNT * 2);
        if (random <= 0)
            random = StageManager.CLASSIFIED_BG_COUNT * 2;
        string skinName = "";
        int bossIndex = 0;
        if (random % 2 == 1)
        {
            bossIndex = (random / 2) + 1;
            skinName = string.Format("boss/boss{0}", string.Format("{0:D3}", bossIndex));
        }
        else
        {
            bossIndex = random / 2;
            skinName = string.Format("boss/boss{0}_2", string.Format("{0:D3}", bossIndex));
        }

        monsterSkeletonAnimation.Skeleton.SetSkin(skinName);
        progressBarPos = bossMonserHPbarPos[random - 1];
        progressBar.GetComponent<RectTransform>().localPosition = progressBarPos;

        monsterHP = (decimal)TableData.Dungeon_Boss_HP[CentralInfoManager.bossStageCount - 1];
        BossDungeonManager.instance.bossHpProgressBar.fillAmount = 1;
        BossDungeonManager.instance.monsterHp_standard = monsterHP;
        BossDungeonManager.instance.monsterHp = monsterHP;

        return monsterHP;
    }

    private IEnumerator BossAppearSoundPlay()
    {
        yield return GameManager.YieldInstructionCache.WaitForSeconds(1f);
        bossAppearEffectGO.SetActive(true);
    }

    public void MonsterDie()
    {
        switch (DungeonManager.instance.dungeon_state)
        {
            case DungeonManager.DUNGEON_STATE.NORMAL:
                // 치키 경험치 업
                CharacterStatus.instance.MonsterDieReward();
                // 골드보상
                CurrencyManager.instance.AddGold(FormulaCollection.StageGoldReward(CentralInfoManager.stageCount));
                // 보스일경우 상자 리워드
                if (CentralInfoManager.regionCount == 10)
                {
                    BoxManager.instance.BoxReward();
                    StateManager.instance.GoBossDieMoveState();
                }
                else
                    StateManager.instance.GoMoveState();

                break;

            case DungeonManager.DUNGEON_STATE.ENCHANT:
                break;

            case DungeonManager.DUNGEON_STATE.BOSS:
                StateManager.instance.GoMoveState();
                BossDungeonManager.instance.Monsterkilled();
                BoxManager.instance.KeyReward();
                break;

            default:
                break;
        }
    }

    public int RealStageCount_Loop()
    {
        var monsterKindsCount = monsterHPbarPos.Length / 3;       // 이건 어쩔 수 없이 손으로 써줘야함.. 각 스테이지별로 3마리씩 몇스테이지가 가능한가?
        var result = CentralInfoManager.stageCount % monsterKindsCount;
        if (result.Equals(0))
            result = monsterKindsCount;

        return (int)result;
    }

    public void AppearAndMove()
    {
        StartCoroutine(MonsterState.instance.AppearAndMoveCo());
    }
}