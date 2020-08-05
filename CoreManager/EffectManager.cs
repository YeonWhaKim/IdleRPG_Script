using System.Collections;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;
    public GameObject levelUpEffect;

    // Start is called before the first frame update
    private void Start()
    {
        instance = this;
    }

    public void HitEffectPlay()
    {
        var hitEffect = ObjectPoolManager.instance.PopFromPool_HitEffect();
        hitEffect.GetComponent<Transform>().localPosition
            = FormulaCollection.PosNearByMonster(MonsterManager.instance.progressBarPos);
    }

    public void HitEffectPlay_Cri()
    {
        var hitEffect = ObjectPoolManager.instance.PopFromPool_HitEffect_Cri();
        hitEffect.GetComponent<Transform>().localPosition
            = FormulaCollection.PosNearByMonster(MonsterManager.instance.progressBarPos);
    }

    public void LevelUpEffectPlay()
    {
        StartCoroutine(LevelUpEffectCO());
        SoundManager.instance.UISoundPlay(1);
    }

    private IEnumerator LevelUpEffectCO()
    {
        levelUpEffect.SetActive(true);
        yield return GameManager.YieldInstructionCache.WaitForSeconds(1f);
        levelUpEffect.SetActive(false);
    }
}