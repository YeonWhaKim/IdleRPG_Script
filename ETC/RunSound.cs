using System.Collections;
using UnityEngine;

public class RunSound : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnEnable()
    {
        StartCoroutine(Disable());
    }

    private IEnumerator Disable()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        ObjectPoolManager.instance.ReturnToPool_RunSound(gameObject);
    }
}