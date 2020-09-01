using Firebase.Analytics;
using System;
using System.Collections;
using UnityEngine;

public class HotTimePushManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static HotTimePushManager instance;

    public const int LUNCHTIME = 12;

    public const int ANTIME = 18;

    public GameObject popup_weekend;

    public GameObject pupup_weekdays;

    private void Awake()
    {
        instance = this;
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => LoadManager.instance.isDataLoaded);
        //Push();
        StartCoroutine(HotTimePushCo());
    }

    public void Push()
    {
        var time = DateTime.Now.Hour;
        var dayOfWeek = DateTime.Now.DayOfWeek;
        if (time.Equals(LUNCHTIME))
        {
            if (CentralInfoManager.isHotTimePush_lunch.Equals(false))
            {
                JudgeWeekend(dayOfWeek);
                CentralInfoManager.isHotTimePush_lunch = true;
            }
        }
        else if (time.Equals(ANTIME))
        {
            if (CentralInfoManager.isHotTimePush_afternoon.Equals(false))
            {
                JudgeWeekend(dayOfWeek);
                CentralInfoManager.isHotTimePush_afternoon = true;
            }
        }
    }

    private void JudgeWeekend(DayOfWeek day)
    {
        if (day.Equals(DayOfWeek.Saturday) || day.Equals(DayOfWeek.Sunday))
            popup_weekend.SetActive(true);
        else
            pupup_weekdays.SetActive(true);
    }

    private IEnumerator HotTimePushCo()
    {
        var Time = DateTime.Now.Hour;
        var waitMinute = 0;

        //CentralInfoManager.isHotTimePush_afternoon = false;     ////////////////////////////////

        while (Time < ANTIME + 1 && CentralInfoManager.isHotTimePush_afternoon == false)
        {
            if (Time == LUNCHTIME)
            {
                if (CentralInfoManager.isHotTimePush_lunch.Equals(false))
                    Push();
            }
            else if (Time == ANTIME)
            {
                if (CentralInfoManager.isHotTimePush_afternoon.Equals(false))
                    Push();
            }
            else if (Time == LUNCHTIME - 1 || Time == ANTIME - 1)
                waitMinute = 60;
            else
                waitMinute = 1800;

            yield return new WaitForSecondsRealtime(waitMinute);
            Time = DateTime.Now.Hour;
        }
    }

    public void OnClickPushYes_weekDays()
    {
        CurrencyManager.instance.Jewel += 50;
        GameManager.SaveLogToServer("접속 시간", DateTime.Now.Hour.ToString(), "핫 타임 푸시_평일");
        FirebaseAnalytics.LogEvent(string.Format("HotTime_WeekDays_{0}", DateTime.Now.Hour));
    }

    public void OnClickPushYes_weekend()
    {
        CurrencyManager.instance.Jewel += 100;
        GameManager.SaveLogToServer("접속 시간", DateTime.Now.Hour.ToString(), "핫 타임 푸시_주말");
        FirebaseAnalytics.LogEvent(string.Format("HotTime_Weekend_{0}", DateTime.Now.Hour));
    }
}