using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;

public class BossBox : MonoBehaviour
{
    public SkeletonAnimation skeletonAnim;
    public MeshRenderer mr;
    public AudioSource getSound;
    private Skeleton skeleton;
    private Skin baseSkin;

    private void Awake()
    {
        skeleton = skeletonAnim.Skeleton;
        baseSkin = skeleton.Data.FindSkin("default");
    }

    // Start is called before the first frame update

    public void BossBoxOn(int acquireboxIndex)
    {
        mr.enabled = true;

        skeletonAnim.Skeleton.SetSkin(skeleton.Data.FindSkin(string.Format("box{0}", string.Format("{0:D2}", acquireboxIndex + 1))));
        skeletonAnim.AnimationState.SetAnimation(0, "drop", false);
        skeletonAnim.AnimationState.AddAnimation(0, "idle", true, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Character"))
        {
            if (mr.enabled.Equals(true))
                StartCoroutine(BoxGetCO());
        }
    }

    private IEnumerator BoxGetCO()
    {
        skeletonAnim.AnimationState.SetAnimation(0, "get", false);
        getSound.Play();
        yield return GameManager.YieldInstructionCache.WaitForSeconds(1f);
        mr.enabled = false;
    }
}