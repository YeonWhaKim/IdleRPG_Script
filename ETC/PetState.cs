using Spine.Unity;
using UnityEngine;

public class PetState : MonoBehaviour
{
    public SkeletonAnimation skeletonAnim;

    // Start is called before the first frame update
    private void Start()
    {
        StateManager.instance.idle += Idle;
        StateManager.instance.move += Move;
        StateManager.instance.action += Attack;
        StateManager.instance.bossDieMove += Move;
    }

    private void OnEnable()
    {
        switch (StateManager.instance.state)
        {
            case StateManager.STATE.IDLE:
                Idle();
                break;

            case StateManager.STATE.MOVE:
                Move();
                break;

            case StateManager.STATE.ACTION:
                Attack();
                break;

            case StateManager.STATE.BOSSDIEMOVE:
                Move();
                break;

            default:
                break;
        }
    }

    public void Idle()
    {
        if (skeletonAnim.initialSkinName == "pet03")
            skeletonAnim.AnimationState.SetAnimation(0, "idle_cat", true);
        else
            skeletonAnim.AnimationState.SetAnimation(0, "idle_dog", true);
    }

    public void Move()
    {
        if (skeletonAnim.initialSkinName == "pet03")
            skeletonAnim.AnimationState.SetAnimation(0, "run_cat", true);
        else
            skeletonAnim.AnimationState.SetAnimation(0, "run_dog", true);
    }

    public void Attack()
    {
        if (skeletonAnim.initialSkinName == "pet03")
            skeletonAnim.AnimationState.SetAnimation(0, "idle_cat", true);
        else
            skeletonAnim.AnimationState.SetAnimation(0, "idle_dog", true);
    }
}