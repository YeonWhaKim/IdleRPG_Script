using System.Collections;
using UnityEngine;

public class OffensePowerText : MonoBehaviour
{
    public RectTransform rectTransform;
    private Coroutine textUp;

    // Start is called before the first frame update

    public void ReturnGameobject()
    {
        if (textUp != null)
            StopCoroutine(textUp);
        ObjectPoolManager.instance.ReturnToPool_Offense(gameObject);
    }

    public void StartTextUp()
    {
        if (gameObject.activeSelf == true)
            textUp = StartCoroutine(TextUp());
    }

    private IEnumerator TextUp()
    {
        while (true)
        {
            yield return GameManager.YieldInstructionCache.WaitForEndOfFrame;
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + 1.5f * Time.timeScale);
        }
    }
}