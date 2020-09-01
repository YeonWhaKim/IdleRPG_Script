using System.Collections;
using UnityEngine;

public class LevelUpButtonMore : MonoBehaviour
{
    private Coroutine co;

    // Start is called before the first frame update

    public void OnClick()
    {
        if (co != null)
        {
            StopCoroutine(co);
            co = null;
        }
        co = StartCoroutine(ActiveCoroutine());
    }

    private IEnumerator ActiveCoroutine()
    {
        yield return GameManager.YieldInstructionCache.WaitForSeconds(3f);
        if (gameObject.activeSelf)
            gameObject.SetActive(false);
    }
}