using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    public bool isGoldBuffTutorial;
    public bool isAtkBuffTutorial;
    public GameObject goldBuffTutorialGO;
    public GameObject atkBuffTutorialGO;

    [Header("Rebirth Tutorial")]
    public GameObject[] rebirthTutorialGO;

    public int rebirthTutorialIndex;
    public bool isRebirthTutorial;

    [Header("Equip Upgrade Tutorial")]
    public GameObject[] equipUpTutorialGO;

    public int equipUpTutorialIndex;
    public bool isEquipTutorial;

    [Header("BossDungeon Tutorial")]
    public GameObject[] bossDungeonTutorialGO;

    public int bossDungeonTutorialIndex;
    public bool isBossTutorial;

    private void Awake()
    {
        isRebirthTutorial = false;
        instance = this;
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => LoadManager.instance.isDataLoaded);
        if (isAtkBuffTutorial)
            atkBuffTutorialGO.SetActive(true);
        else
            atkBuffTutorialGO.SetActive(false);
        if (isGoldBuffTutorial)
            goldBuffTutorialGO.SetActive(true);
        else
            goldBuffTutorialGO.SetActive(false);
    }

    // 전승 튜토리얼
    public void RebirthTutorial()
    {
        isRebirthTutorial = true;
        Time.timeScale = 0;
        rebirthTutorialIndex = 0;
        rebirthTutorialGO[0].SetActive(true);
        MenuManager.instance.OnClickMenuButton(0);
    }

    public void OnClickTutorialPanel_Rebirth()
    {
        rebirthTutorialIndex++;
        var tmpIndex = rebirthTutorialIndex;
        tmpIndex--;

        if (rebirthTutorialIndex >= rebirthTutorialGO.Length)
        {
            rebirthTutorialGO[rebirthTutorialGO.Length - 2].SetActive(false);
            isRebirthTutorial = false;
            GameManager.GameSpeedSet();
            StartCoroutine(StateManager.instance.RestartStageCoroutineCo());
            return;
        }
        else
            rebirthTutorialGO[rebirthTutorialIndex].SetActive(true);

        if (rebirthTutorialIndex.Equals(3))
            rebirthTutorialGO[0].SetActive(false);
        else if (rebirthTutorialIndex >= 4 && rebirthTutorialIndex <= 9)
        {
            if (rebirthTutorialIndex.Equals(4) || rebirthTutorialIndex.Equals(5) || rebirthTutorialIndex.Equals(6))
            {
                MenuManager.instance.OnClickMenuButton(0);
                InfoMenu.instance.OnClickEquip(8);
            }
            rebirthTutorialGO[tmpIndex].SetActive(false);
        }
    }

    //장비 튜토리얼
    public void EquipTutorial()
    {
        isEquipTutorial = true;
        Time.timeScale = 0;
        equipUpTutorialIndex = 0;
        equipUpTutorialGO[0].SetActive(true);
    }

    public void OnClickTutorialPanel_Equip()
    {
        equipUpTutorialIndex++;
        var tmpIndex = equipUpTutorialIndex;
        tmpIndex--;
        CentralInfoManager.instance.isEquipUpgraded = true;
        if (equipUpTutorialIndex >= equipUpTutorialGO.Length)
        {
            equipUpTutorialGO[equipUpTutorialGO.Length - 2].SetActive(false);
            isEquipTutorial = false;
            GameManager.GameSpeedSet();
            return;
        }
        else
            equipUpTutorialGO[equipUpTutorialIndex].SetActive(true);

        if (equipUpTutorialIndex.Equals(2))
            equipUpTutorialGO[0].SetActive(false);
        else if (equipUpTutorialIndex >= 3 && equipUpTutorialIndex <= 4)
            equipUpTutorialGO[tmpIndex].SetActive(false);
    }

    //보스전 튜토리얼
    public void BossTutorial()
    {
        isBossTutorial = true;
        CentralInfoManager.instance.isBossDungeonEnter = true;
        Time.timeScale = 0;
        bossDungeonTutorialIndex = 0;
        bossDungeonTutorialGO[0].SetActive(true);
    }

    public void OnClickTutorialPanel_Boss()
    {
        bossDungeonTutorialIndex++;
        var tmpindex = bossDungeonTutorialIndex;
        tmpindex--;

        if (bossDungeonTutorialIndex >= bossDungeonTutorialGO.Length)
        {
            bossDungeonTutorialGO[bossDungeonTutorialGO.Length - 3].SetActive(false);
            isBossTutorial = false;
            GameManager.GameSpeedSet();
            return;
        }
        else
            bossDungeonTutorialGO[bossDungeonTutorialIndex].SetActive(true);

        if (bossDungeonTutorialIndex.Equals(1))
            bossDungeonTutorialGO[0].SetActive(false);
        else if (bossDungeonTutorialIndex.Equals(3))
            bossDungeonTutorialGO[1].SetActive(false);
        else if (bossDungeonTutorialIndex >= 4 && bossDungeonTutorialIndex <= 5)
            bossDungeonTutorialGO[tmpindex].SetActive(false);
    }
}