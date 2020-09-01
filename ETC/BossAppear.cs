using System.Collections;
using UnityEngine;

public class BossAppear : MonoBehaviour
{
    public void BossAppearSound()
    {
        SoundManager.instance.BossAppearPlay();
    }

    public void End()
    {
        gameObject.SetActive(false);
    }

    public void ObjectFalse()
    {
        // currencyExp
        StartCoroutine(ObjectFalseCo());
    }

    private IEnumerator ObjectFalseCo()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        GetComponent<Animator>().SetTrigger("Hide");
    }

    public void ObjectFalse_false()
    {
        // currencyExp
        StartCoroutine(ObjectFalseCo_false());
    }

    private IEnumerator ObjectFalseCo_false()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        GetComponent<Animator>().SetTrigger("False");
    }
}