using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpeedManager : MonoBehaviour
{
    public static SpeedManager instance;
    public const int speedPrice_Jewel = 100;
    public const int speedDurationStandard = 1800;
    public int speedDuration = 0;
    public float speed = 0;
    public GameObject speedGO;
    public Text durationText;

    public GameObject selectSpeedPopUP;
    public GameObject payJewelPopUP;
    public Button payJewelButton;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        instance = this;
        // 저장기능
        yield return new WaitUntil(() => LoadManager.instance.isDataLoaded);
        if (speedDuration > 0)
            StartCoroutine(SpeedUp(speed, speedDuration));
        else
            speedGO.SetActive(false);
    }

    public void OnClickSpeedIcon()
    {
        MenuManager.instance.OnClickMenuButton(6);
        ShopMenu.instance.speedBuffOutlineGO.SetActive(true);
        //if (selectSpeedPopUP.activeSelf.Equals(true))
        //    selectSpeedPopUP.GetComponent<Animator>().SetTrigger("False");
        //else
        //{
        //    if (Time.timeScale.Equals(1))
        //        selectSpeedPopUP.SetActive(true);
        //}
    }

    public void OnClickYes_Ad()
    {
        //광고 재생
        AdmobManager.instance.ShowRewardBasedVided(AdmobManager.CallObject.SPEEDUP);
    }

    public void OnClickYes_Jewel()
    {
        selectSpeedPopUP.GetComponent<Animator>().SetTrigger("False");
        if (CurrencyManager.instance.Jewel < speedPrice_Jewel)
            payJewelButton.interactable = false;
        else
            payJewelButton.interactable = true;
        payJewelPopUP.SetActive(true);
    }

    public void OnClickPayJewel()
    {
        //CurrencyManager.instance.Jewel -= speedPrice_Jewel;
        //배속 시작 - 2.0
        StartCoroutine(SpeedUp(2.0f, speedDurationStandard));
        //payJewelPopUP.SetActive(false);
        ShopMenu.instance.OnClickMenuTab(1);
    }

    public IEnumerator SpeedUp(float tmpSpeed, int duration)
    {
        //이미지 바꾸고
        speedGO.SetActive(true);
        //밑에 텍스트 설정
        speed = tmpSpeed;
        //타임스케일 변경하고
        GameManager.GameSpeedSet_SpeedM(tmpSpeed);

        //지속시간 시작
        speedDuration = duration;
        durationText.gameObject.SetActive(true);
        while (speedDuration > 0)
        {
            durationText.text = GameManager.MM__SS(speedDuration);
            yield return new WaitForSecondsRealtime(1f);
            yield return new WaitUntil(() => DungeonManager.instance.dungeon_state.Equals(DungeonManager.DUNGEON_STATE.NORMAL));
            speedDuration--;
        }
        speedDuration = 0;
        //밑에 텍스트 설정
        durationText.gameObject.SetActive(false);
        speedGO.SetActive(false);
        //타임스케일 변경하고
        GameManager.GameSpeedSet_SpeedM(1);
    }
}