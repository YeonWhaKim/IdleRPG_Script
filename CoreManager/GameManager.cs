using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    internal static class YieldInstructionCache
    {
        private class FloatComparer : IEqualityComparer<float>
        {
            bool IEqualityComparer<float>.Equals(float x, float y)
            {
                return x == y;
            }

            int IEqualityComparer<float>.GetHashCode(float obj)
            {
                return obj.GetHashCode();
            }
        }

        public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
        public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();

        private static readonly Dictionary<float, WaitForSeconds> _timeInterval = new Dictionary<float, WaitForSeconds>(new FloatComparer());

        public static WaitForSeconds WaitForSeconds(float seconds)
        {
            WaitForSeconds wfs;
            if (!_timeInterval.TryGetValue(seconds, out wfs))
                _timeInterval.Add(seconds, wfs = new WaitForSeconds(seconds));
            return wfs;
        }
    }

    public GameObject exitPopUp;

    private void Awake()
    {
        Screen.SetResolution(1080, 1920, true);
        Application.targetFrameRate = 30;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        // 초기화
        // [.net4][il2cpp] 사용 시 필수 사용
        Backend.Initialize(() =>
        {
            // 초기화 성공한 경우 실행
            if (Backend.IsInitialized)
            {
                // example
                // 버전체크 -> 업데이트
            }
            // 초기화 실패한 경우 실행
            else
            {
                Debug.Log("실패");
            }
        });
    }

    private void Start()
    {
        //Time.timeScale = 3;     ///////////////////////////////////////////////////
        StartCoroutine(UpdateCO());
    }

    private IEnumerator UpdateCO()
    {
        while (true)
        {
            yield return YieldInstructionCache.WaitForFixedUpdate;
#if UNITY_ANDROID
            if (Input.GetKeyDown(KeyCode.Escape))
                exitPopUp.SetActive(true);
#endif
        }
    }

    //public static string NumberNotation(double value)
    //{
    //    return CurrencyNotationManager.ToCurrencyString(value);
    //    //return string.Format("{0:#,##0.####}", value);
    //}

    //public static string NumberNotation(decimal value)
    //{
    //    return CurrencyNotationManager.ToCurrencyString(value);
    //    //return string.Format("{0:#,##0.####}", value);
    //}
    public static void SaveLogToServer(string param_Key, string param_Value, string type)
    {
        Param param = new Param();
        param.Add(param_Key, param_Value);
        BackendAsyncClass.BackendAsync(Backend.GameInfo.InsertLog, type, param,
            (callback) =>
            { //Debug.Log(string.Format("'{0}'로그 저장", type));
            });
    }

    public static string NumberNotation_comma(double value)
    {
        //return CurrencyNotationManager.ToCurrencyString(value);
        return string.Format("{0:#,##0}", value);
    }

    public static string NumberNotation_comma(decimal value)
    {
        //return CurrencyNotationManager.ToCurrencyString(value);
        return string.Format("{0:#,##0}", value);
    }

    public static string NumberNotation_Korean(decimal value, bool isCurrency)
    {
        string s = "";
        var origin = value;

        if (value >= 1.0E20m)    //해
        {
            s += string.Format("{0}해 ", (int)((value % 1.0E24m) / 1.0E20m));
            value = value % 1.0E20m;
        }
        if (value >= 1.0E16m)  //경
        {
            s += string.Format("{0}경 ", (int)((value % 1.0E20m) / 1.0E16m));
            value %= 1.0e16m;
        }
        if (value >= 1.0E12m)  //조
        {
            s += string.Format("{0}조 ", (int)((value % 1.0E16m) / 1.0E12m));
            value %= 1.0e12m;
        }
        if (value >= 1.0E8m)    //억
        {
            s += string.Format("{0}억 ", (int)((value % 1.0E12m) / 1.0E8m));
            value %= 1.0e8m;
        }
        if (value >= 1.0E4m)    //만
        {
            s += string.Format("{0}만 ", (int)((value % 1.0E8m) / 1.0E4m));
            value %= 1.0e4m;
        }
        if (value >= 1)
        {
            string rr = ((int)((value % 1.0E4m))).ToString();
            rr = int.Parse(rr) > 0 ? rr : "0";
            s += string.Format("{0}", rr);
        }
        if (value <= 0)
        {
            if (origin <= 0)
                s += "0";
        }
        return s;
    }

    public static string MM__SS(float second)
    {
        int intSecond = (int)second;
        if (intSecond % 60 == 0)
            return string.Format("{0}:00", (intSecond / 60));
        else
        {
            if (intSecond % 60 < 10)
                return string.Format("{0}:0{1}", (intSecond / 60), (intSecond % 60));
            else
                return string.Format("{0}:{1}", (intSecond / 60), (intSecond % 60));
        }
    }

    public static Color HexToColor(string hex)
    {
        hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }

    public static void GameSpeedSet()
    {
        switch (DungeonManager.instance.dungeon_state)
        {
            case DungeonManager.DUNGEON_STATE.NORMAL:
                if (SpeedManager.instance.speedDuration > 0)
                    Time.timeScale = 2;
                else
                    Time.timeScale = 1;
                break;

            case DungeonManager.DUNGEON_STATE.ENCHANT:
                break;

            case DungeonManager.DUNGEON_STATE.BOSS:
                Time.timeScale = 2;
                break;

            default:
                break;
        }
    }

    public static void GameSpeedSet_SpeedM(float speed)
    {
        Time.timeScale = speed;
    }

    public void SpeedDetecter()
    {
        SaveLogToServer("속도 Cheater", Time.timeScale.ToString(), "SpeedDetector");
    }

    public void OnClickExit()
    {
        try
        {
            SaveManager.instance.SaveData_WhenExit();
            StartCoroutine(SaveAndQuitCO());
        }
        catch (System.Exception)
        {
            OnClickExit_aa();
        }
    }

    public void OnClickExit_aa()
    {
        Application.Quit();
    }

    private IEnumerator SaveAndQuitCO()
    {
        yield return new WaitUntil(() => SaveManager.instance.isSaved);
        Application.Quit();
    }

    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        SaveManager.instance.SaveData_WhenExit();
        //PlayerPrefs.DeleteAll();
#endif
    }

    private void OnApplicationPause(bool pause)
    {
        try
        {
            if (pause.Equals(true))
                SaveManager.instance.SaveData();
            else
            {
                if (LoadManager.instance.isDataLoaded.Equals(true))
                    HotTimePushManager.instance.Push();
            }
        }
        catch (System.Exception)
        {
            Application.Quit();
        }
    }
}