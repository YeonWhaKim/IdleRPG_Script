using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PressedCurrencies : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private int time;
    private bool coroutineOut;
    public GameObject expGO;

    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(StopWatch());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        coroutineOut = false;
        if (time >= 1)
            expGO.SetActive(true);
    }

    private IEnumerator StopWatch()
    {
        time = 0;
        coroutineOut = true;
        while (coroutineOut == true)
        {
            yield return new WaitForSecondsRealtime(1f);
            time++;
        }
    }
}