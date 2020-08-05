using Spine.Unity;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
    public static CharacterState instance;
    public Animator animator;
    public SkeletonAnimation skeletonAnim;
    public ParticleSystem windEffect;
    public ParticleSystem dustEffect;
    public bool isCritical = false;
    public bool isMonsterDef = false;
    public float minusedAttackSpeedValue = 0;
    private const float defaultAttackSpeed = 0.8f;

    private Spine.EventData eventData_hit;
    private Spine.EventData eventData_swing;
    private Spine.EventData eventData_run;
    private Spine.EventData eventData_rebirth;

    // Start is called before the first frame update
    private void Start()
    {
        instance = this;
        skeletonAnim.AnimationState.Event += HandleAnimationStateEvent;
        StateManager.instance.idle += Idle;
        StateManager.instance.move += Move;
        StateManager.instance.action += Attack;
        StateManager.instance.bossDieMove += BossDieMove;
        Idle();
    }

    public void Idle()
    {
        animator.Rebind();
        skeletonAnim.AnimationState.SetAnimation(0, "idle", true);
        windEffect.Stop();
        dustEffect.Stop();
    }

    public void Move()
    {
        animator.Rebind();
        skeletonAnim.AnimationState.SetAnimation(0, "run", true);
        windEffect.Play();
        dustEffect.Play();
    }

    public void Attack()
    {
        animator.Rebind();
        windEffect.Stop();
        dustEffect.Stop();
        AttackNext();
    }

    public void BossDieMove()
    {
        animator.SetTrigger("run");
        skeletonAnim.AnimationState.SetAnimation(0, "run", true);
        windEffect.Play();
        dustEffect.Play();
    }

    public void AttackNext()
    {
        isCritical = AttackMeasurementManager.instance.ReturnIsCritical(out isMonsterDef);

        string attackName = string.Format("action{0:D2}", Random.Range(1, 4));
        if (DungeonManager.instance.animationNotActive.Equals(false))
            skeletonAnim.AnimationState.AddAnimation(0, attackName, false, 0f);
        else
            Idle();
        //skeletonAnim.AnimationState.SetAnimation(0, attackName, false);
    }

    private void HandleAnimationStateEvent(Spine.TrackEntry entry, Spine.Event e)
    {
        eventData_hit = skeletonAnim.Skeleton.Data.FindEvent("hit");
        eventData_swing = skeletonAnim.Skeleton.Data.FindEvent("swing");
        eventData_run = skeletonAnim.Skeleton.Data.FindEvent("run");
        eventData_rebirth = skeletonAnim.Skeleton.Data.FindEvent("rebirth");
        if (e.Data == eventData_hit)
        {
            ObjectPoolManager.instance.PopFromPool_HitSound();
            if (DungeonManager.instance.animationNotActive.Equals(false))
                AttackNext();
            //else
            //    Idle();
        }
        else if (e.Data == eventData_swing)
            ObjectPoolManager.instance.PopFromPool_SwingSound();
        else if (e.Data == eventData_run)
            ObjectPoolManager.instance.PopFromPool_RunSound();
        else if (e.Data == eventData_rebirth)
        {
            RebirthManager.instance.rebirthEffect.SetActive(true);
            SoundManager.instance.UISoundPlay(0);
        }
    }

    public void CharacterRebirthEffect()
    {
        skeletonAnim.AnimationState.SetAnimation(0, "rebirth", false);
    }

    public void GoMoveState()
    {
        StateManager.instance.GoMoveState();
    }

    public void StageChange()
    {
        StageManager.instance.StgeSetting(DungeonManager.instance.dungeon_state);
    }
}