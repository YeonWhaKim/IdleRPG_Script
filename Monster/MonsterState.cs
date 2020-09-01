using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MonsterState : MonoBehaviour
{
    public static MonsterState instance;
    public MeshRenderer mr;
    public SkeletonAnimation skeletonAnim;
    public GameObject progressBarGO;
    public Image progressBarImage;
    private decimal monsterHp_Standard;
    private decimal monsterHp;

    // Start is called before the first frame update
    private void Start()
    {
        instance = this;
        StateManager.instance.idle += Idle;
        StateManager.instance.move += AppearAndMove;
    }

    public void Idle()
    {
        gameObject.GetComponent<Animator>().Rebind();
        gameObject.SetActive(false);
    }

    public void AppearAndMove()
    {
        MonsterManager.instance.AppearAndMove();
    }

    public IEnumerator AppearAndMoveCo()
    {
        yield return GameManager.YieldInstructionCache.WaitForSeconds(0.5f);
        gameObject.GetComponent<Animator>().Rebind();
        gameObject.SetActive(true);
        MonaterUISetting(MonsterManager.instance.MonsterSetting(DungeonManager.instance.dungeon_state));
        if (DungeonManager.instance.animationNotActive.Equals(false))
            gameObject.GetComponent<Animator>().SetTrigger("Move");
        skeletonAnim.AnimationState.SetAnimation(0, "idle", true);

        switch (DungeonManager.instance.dungeon_state)
        {
            case DungeonManager.DUNGEON_STATE.NORMAL:
                progressBarGO.SetActive(true);
                break;

            case DungeonManager.DUNGEON_STATE.ENCHANT:
                progressBarGO.SetActive(true);
                break;

            case DungeonManager.DUNGEON_STATE.BOSS:
                progressBarGO.SetActive(false);
                break;

            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Weapon"))
        {
            MonsterHit();
        }
    }

    private void MonsterHit()
    {
        var realOffence = AttackMeasurementManager.instance.RealOffense();
        monsterHp -= realOffence;
        progressBarImage.fillAmount = (float)(monsterHp / monsterHp_Standard);

        if (DungeonManager.instance.dungeon_state.Equals(DungeonManager.DUNGEON_STATE.BOSS))
            BossDungeonManager.instance.BossMonsterHPProgressBarSetting(realOffence);
        if (monsterHp <= 0)
        {
            progressBarGO.SetActive(false);
            StartCoroutine(MonsterDie());
            if (DungeonManager.instance.animationNotActive.Equals(false))
                MonsterManager.instance.MonsterDie();
            else
                Idle();
        }
        else
            skeletonAnim.AnimationState.SetAnimation(0, "hit", false);
    }

    private IEnumerator MonsterDie()
    {
        skeletonAnim.AnimationState.SetAnimation(0, "die", false);
        yield return GameManager.YieldInstructionCache.WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    private void MonaterUISetting(decimal monsterHP)
    {
        progressBarImage.fillAmount = 1;
        monsterHp_Standard = monsterHP;
        monsterHp = monsterHp_Standard;
    }
}