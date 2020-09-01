using System.Collections;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnEnable()
    {
        StartCoroutine(Disable());
    }

    private IEnumerator Disable()
    {
        yield return GameManager.YieldInstructionCache.WaitForSeconds(0.5f);
        ObjectPoolManager.instance.ReturnToPool_HitEffect(gameObject);
    }
}