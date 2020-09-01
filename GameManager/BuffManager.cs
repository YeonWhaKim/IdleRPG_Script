using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;
    public const int BUFF_KINDS = 2;
    public const int BUFFDURATION = 300;    //5분
    public const int BUFFCOOLTIME = 600;    //10분
    public const int BUFFCOOLTIMERESET_VALUE = 50;
    public GameObject[] buffPopUp;
    public GameObject[] buffExpPopUp;
    public GameObject coolTimeResetPopUp;
    public Button coolTimeResetButton;
    public Image coolTimeResetIcon;
    public Text coolTimeResetValueText;
    public GameObject[] buffProgressBarGO;
    public Text[] coolTimeText;
    public List<int> buffCoolTime = new List<int>();
    public List<int> buffCoolTimeStandard = new List<int>();
    public List<bool> isBuffing = new List<bool>();

    [Header("BuffValue")]
    public int[] buffValue = { 1, 1 };

    private int selectedIndex = 0;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        instance = this;
        yield return new WaitUntil(() => LoadManager.instance.isDataLoaded);

        BuffSetting();
    }

    // 일단은 대충 하나만 할거로 생각하고 빠르게 짜고 나중에 바꾸기
    // 버프는 계속 추가됨.
    public void BuffStart(int index)
    {
#if UNITY_EDITOR
        if (index.Equals(0))
            BuffStart_Real(0);
        else if (index.Equals(1))
            BuffStart_Real(1);

#elif UNITY_ANDROID
        if (index.Equals(0))
            AdmobManager.instance.ShowRewardBasedVided(AdmobManager.CallObject.BUFF_DOUBLEGOLD);
        else if (index.Equals(1))
            AdmobManager.instance.ShowRewardBasedVided(AdmobManager.CallObject.BUFF_DOUBLEATK);
#endif
    }

    public void BuffStart_Real(int index)
    {
        buffCoolTimeStandard[index] = BUFFDURATION;
        buffCoolTime[index] = buffCoolTimeStandard[index];
        isBuffing[index] = true;
        StartCoroutine(BuffingCO(index));

        buffPopUp[index].GetComponent<Animator>().SetTrigger("False");
    }

    private IEnumerator BuffingCO(int index)
    {
        buffValue[index] = 2;
        buffProgressBarGO[index].SetActive(false);
        coolTimeText[index].enabled = true;
        coolTimeText[index].text = buffCoolTime[index].ToString();
        while (buffCoolTime[index] > 0)
        {
            yield return new WaitForSecondsRealtime(1f);
            buffCoolTime[index]--;
            coolTimeText[index].text = buffCoolTime[index].ToString();
        }

        buffCoolTime[index] = 0;

        StartCoroutine(BuffCoolTimeCO(index, BUFFCOOLTIME));
    }

    private IEnumerator BuffCoolTimeCO(int index, int coolTime)
    {
        buffCoolTime[index] = coolTime;
        buffValue[index] = 1;
        isBuffing[index] = false;
        buffProgressBarGO[index].SetActive(true);
        coolTimeText[index].enabled = true;
        coolTimeText[index].text = buffCoolTime[index].ToString();
        while (buffCoolTime[index] > 0)
        {
            yield return new WaitForSecondsRealtime(1f);
            buffCoolTime[index]--;
            coolTimeText[index].text = buffCoolTime[index].ToString();
        }
        buffCoolTime[index] = 0;
        coolTimeText[index].enabled = false;    //쿨타임 끝나면
        buffProgressBarGO[index].SetActive(false);
    }

    private void BuffSetting()
    {
        for (int i = 0; i < BUFF_KINDS; i++)
        {
            if (isBuffing[i].Equals(true) && buffCoolTime[i] > 0)
                StartCoroutine(BuffingCO(i));
            else if (isBuffing[i].Equals(false) && buffCoolTime[i] > 0)
                StartCoroutine(BuffCoolTimeCO(i, buffCoolTime[i]));
        }
    }

    public void OnClickBuff(int index)
    {
        selectedIndex = index;
        if (index.Equals(0))
            TutorialManager.instance.isGoldBuffTutorial = false;
        else
            TutorialManager.instance.isAtkBuffTutorial = false;
        buffExpPopUp[index].SetActive(false);
        if (isBuffing[index].Equals(true))
            return;

        // 쿨타임일때 아닐때 나눠서 팝업 띄우기
        if (isBuffing[index].Equals(false) && buffCoolTime[index] > 0)
        {
            //쿨타임 일때
            if (coolTimeResetPopUp.activeSelf)
                coolTimeResetPopUp.GetComponent<Animator>().SetTrigger("False");
            else
            {
                if (CurrencyManager.instance.Token < BUFFCOOLTIMERESET_VALUE)
                {
                    coolTimeResetButton.interactable = false;
                    coolTimeResetIcon.color = new Vector4(1, 1, 1, 0.6f);
                    coolTimeResetValueText.color = new Vector4(0.4f, 0.8588236f, 0.5921569f, 0.6f);
                }
                else
                {
                    coolTimeResetButton.interactable = true;
                    coolTimeResetIcon.color = new Vector4(1, 1, 1, 1);
                    coolTimeResetValueText.color = new Vector4(0.4f, 0.8588236f, 0.5921569f, 1f);
                }
                for (int i = 0; i < buffPopUp.Length; i++)
                {
                    buffPopUp[i].SetActive(false);
                }
                coolTimeResetPopUp.SetActive(true);
            }
        }
        //쿨타임 아닐때
        else if (isBuffing[index].Equals(false) && buffCoolTime[index] <= 0)
        {
            if (CentralInfoManager.instance.isBuffAdRemoved[index].Equals(true))
                BuffStart_Real(index);
            else
            {
                for (int i = 0; i < buffPopUp.Length; i++)
                {
                    if (i == index)
                    {
                        if (buffPopUp[i].activeSelf)
                            buffPopUp[i].GetComponent<Animator>().SetTrigger("False");
                        else
                        {
                            coolTimeResetPopUp.SetActive(false);
                            buffPopUp[i].SetActive(true);
                        }
                    }
                    else
                    {
                        if (buffPopUp[i].activeSelf)
                            buffPopUp[i].GetComponent<Animator>().SetTrigger("False");
                    }
                }
            }
        }
    }

    public void BuffCoolTimeGO()
    {
        CurrencyManager.instance.Token -= BUFFCOOLTIMERESET_VALUE;
        BuffCoolTimeReset(selectedIndex);
    }

    private void BuffCoolTimeReset(int index)
    {
        buffCoolTime[index] = 0;
        coolTimeText[index].enabled = false;    //쿨타임 끝나면
        isBuffing[index] = false;
        buffProgressBarGO[index].SetActive(false);
    }
}