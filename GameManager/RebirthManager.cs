using Firebase.Analytics;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RebirthManager : MonoBehaviour
{
    public static RebirthManager instance;
    public GameObject rebirthEffect;
    public GameObject rebirthPopUp;
    public GameObject rebirthINGPopUp_Button;

    //public GameObject rebirthINGPopUp;
    public Text rebirhResultText;

    public bool isRebirthCoupon;
    public bool isRebirth;
    public bool availableInfoSetting;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    public void RebirhPopUp(bool isRebirthCoupon_)
    {
        if (isRebirth)
            rebirthINGPopUp_Button.SetActive(true);
        else
        {
            var stage = CentralInfoManager.stageCount / 100;
            stage *= 100;
            if (stage < 1)
                stage = 1;

            isRebirthCoupon = isRebirthCoupon_;
            rebirhResultText.text = string.Format("전승 가능 레벨 : {0} → <color=#EFC54D>{1}</color>\n최대 레벨 : {2} → <color=#EFC54D>{3}</color>\n시작 층 : {4}→ <color=#EFC54D>{5}</color>",
                ReturnAvailableRebrithLevel(CharacterStatus.instance.Rebirth),
                 ReturnAvailableRebrithLevel(CharacterStatus.instance.Rebirth + 1),
                 ReturnMaxLevel_DependingOnRebirthCount(CharacterStatus.instance.Rebirth),
                 ReturnMaxLevel_DependingOnRebirthCount(CharacterStatus.instance.Rebirth + 1),
                 CentralInfoManager.rebirthStartStage,
                 stage);

            CentralInfoManager.rebirthStartStage = stage;
            rebirthPopUp.SetActive(true);
        }
    }

    public void Rebirth()
    {
        // 장비 잠금은 여기서 리셋할라면 해도 됨.
        isRebirth = true;
        StartCoroutine(RebirthCo());
    }

    private IEnumerator RebirthCo()
    {
        RankManager.instance.isUpdate = false;
        availableInfoSetting = false;
        if (TutorialManager.instance.rebirthTutorialIndex < TutorialManager.instance.rebirthTutorialGO.Length)
            TutorialManager.instance.rebirthTutorialGO[TutorialManager.instance.rebirthTutorialIndex].SetActive(false);
        StartCoroutine(StateManager.instance.RebirthCoroutine());
        StartCoroutine(RankManager.instance.UpdateRank(2, CharacterStatus.instance.Rebirth + 1));
        yield return new WaitUntil(() => RankManager.instance.isUpdate);
        yield return new WaitForSecondsRealtime(2f);

        CharacterStatus.instance.isMaxLevel = false;
        CharacterStatus.instance.Level = 1;
        CharacterStatus.instance.Rebirth += 1;
        GameManager.SaveLogToServer("전승 횟수", CharacterStatus.instance.Rebirth.ToString(), "전승");

        CharacterStatus.instance.MaxExpForLevelUp = TableData.Level_Exp[CharacterStatus.instance.Level - 1];
        CharacterStatus.instance.CurrExp = 0;

        CharacterStatus.instance.Strength_Auto = 1;
        CharacterStatus.instance.Agility_Auto = 1;
        CharacterStatus.instance.Dex_Auto = 1;
        CharacterStatus.instance.Lucky_Auto = 1;

        availableInfoSetting = true;

        CharacterStatus.instance.StatSetting();

        if (isRebirthCoupon)
        {
            CentralInfoManager.rebirthCoupon--;
            GameManager.SaveLogToServer("전승권 사용 후 갯수", CentralInfoManager.rebirthCoupon.ToString(), "유료 상품 사용");
        }
        InfoMenu.instance.ui.ReplayExpSetting(CharacterStatus.instance.Rebirth, false);

        FirebaseAnalytics.LogEvent("Rebirth");
    }

    public int ReturnAvailableRebrithLevel(int rebirthCount)
    {
        if (rebirthCount >= TableData.Replay_On_Level.Count)
            rebirthCount = TableData.Replay_On_Level.Count - 1;
        return TableData.Replay_On_Level[rebirthCount];
    }

    public int ReturnMaxLevel_DependingOnRebirthCount(int rebirthCount)
    {
        if (rebirthCount >= TableData.Replay_On_Level.Count)
            rebirthCount = TableData.Replay_On_Level.Count - 1;
        return TableData.Max_Level_Depending_On_RebirthCount[rebirthCount];
    }
}