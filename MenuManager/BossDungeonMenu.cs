using Firebase.Analytics;
using LitJson;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossDungeonMenu : MonoBehaviour
{
    public static BossDungeonMenu instance;
    public Sprite[] bossSprites;

    [Header("Boss End PopUp")]
    public GameObject bossEndPopUp;

    public Text titleText;
    public Text resultText;
    public Transform bossRewardParent;

    [Header("Boss Dungeon MenuPanel")]
    public Button entranceButton_ticket;

    public Button entranceButton_ad;

    public Image bossImage;
    public Image keyImage;
    public Text keyRewardText;
    public Text entranceText_ticket;
    public Text entranceText_ad;
    public Text entranceCountText;
    public Text currFloorTitleText;
    public GameObject bossRankGOMine;
    public GameObject[] bossRankGOUsers;
    public Sprite[] keySprites;
    private List<float> bossKeyPer = new List<float>();
    public int myBDScore;

    // Start is called before the first frame update
    private void Start()
    {
        instance = this;
        for (int i = 0; i < CentralInfoManager.NUMBEROFBOXKEYKINDS; i++)
        {
            bossKeyPer.Add(0);
        }
    }

    public void BossDungeonMenuSetting()
    {
        BossDungeonManager.instance.RankSetting();
        if (DungeonManager.instance.dungeon_state.Equals(DungeonManager.DUNGEON_STATE.BOSS))
        {
            currFloorTitleText.text = string.Format("<color=#FA6E6D>{0}</color>층 도전중", CentralInfoManager.bossStageCount);
            entranceButton_ticket.interactable = false;
            entranceButton_ad.interactable = false;
        }
        else if (DungeonManager.instance.dungeon_state.Equals(DungeonManager.DUNGEON_STATE.ENCHANT))
        {
            currFloorTitleText.text = string.Format("{0}층", CentralInfoManager.bossStageCount);
            entranceButton_ticket.interactable = false;
            entranceButton_ad.interactable = false;
        }
        else
        {
            currFloorTitleText.text = string.Format("{0}층", CentralInfoManager.bossStageCount);
            entranceButton_ticket.interactable = true;
            entranceButton_ad.interactable = true;
        }

        // 보스 이미지도 여기서 세팅하기
        BossInfoSetting();

        EntranceTicketSetting(CurrencyManager.instance.Ticket_BossDungeon);
        EntranceTicketSetting_Ad();
        BossRankingSetting();
    }

    public void BossInfoSetting()
    {
        bossKeyPer[0] = TableData.Boss_Drop_Key_01_Per[(int)CentralInfoManager.bossStageCount - 1];
        bossKeyPer[1] = TableData.Boss_Drop_Key_02_Per[(int)CentralInfoManager.bossStageCount - 1];
        bossKeyPer[2] = TableData.Boss_Drop_Key_03_Per[(int)CentralInfoManager.bossStageCount - 1];
        bossKeyPer[3] = TableData.Boss_Drop_Key_04_Per[(int)CentralInfoManager.bossStageCount - 1];
        bossKeyPer[4] = TableData.Boss_Drop_Key_05_Per[(int)CentralInfoManager.bossStageCount - 1];
        bossKeyPer[5] = TableData.Boss_Drop_Key_06_Per[(int)CentralInfoManager.bossStageCount - 1];
        bossKeyPer[6] = TableData.Boss_Drop_Key_07_Per[(int)CentralInfoManager.bossStageCount - 1];

        var index = 0;
        for (int i = bossKeyPer.Count - 1; i >= 0; i--)
        {
            if (bossKeyPer[i] > 0)
            {
                index = i;
                break;
            }
        }

        var bossImageIndex = CentralInfoManager.bossStageCount % bossSprites.Length;
        if (bossImageIndex.Equals(0))
            bossImageIndex = bossSprites.Length;
        bossImage.sprite = bossSprites[bossImageIndex - 1];
        keyImage.sprite = keySprites[index];
        keyRewardText.text = string.Format("<color=#6EE3C8>{0}</color>\n획득 가능!!", TableData.Key_Name[index]);
    }

    public void OnClickBossDungeonEntrance(int index)
    {
        BossDungeonManager.instance.BossDungeonSetting();
        entranceButton_ticket.interactable = false;     //입장버튼 비활성화
        entranceButton_ad.interactable = false;
        entranceText_ticket.text = "던전 진행중...";
        entranceText_ad.text = "던전 진행중...";
        if (index.Equals(0)) //입장권
            CurrencyManager.instance.Ticket_BossDungeon--;
        else   // 광고
            CentralInfoManager.boss_adCount++;
        FirebaseAnalytics.LogEvent("EntranceBossDungeon");
        BossDungeonMenuSetting();
    }

    public void OnClickBossDungeonEntrance_ad()
    {
        AdmobManager.instance.ShowRewardBasedVided(AdmobManager.CallObject.DUNGEON_BOSS);
    }

    public void DungeonEnd(bool isDungeonConquer)
    {
        var stage = CentralInfoManager.bossStageCount;
        if (stage < TableData.Dungeon_Boss_HP.Count)
            stage--;

        if (isDungeonConquer)
            titleText.text = "보스전 정복 완료!!";
        else
            titleText.text = "보스전 완료";
        StartCoroutine(RankManager.instance.UpdateRank(4, stage));

        // 여기 팝업 만들고 다시 꾸미기
        resultText.text = string.Format("<color=#FA6E6D>{0}</color>층 성공", stage);
        // 얻은 키 결과
        for (int i = 0; i < BoxManager.instance.keyValue.Count; i++)
        {
            if (BoxManager.instance.keyValue[i] <= 0)
                bossRewardParent.GetChild(i).gameObject.SetActive(false);
            else
            {
                var child = bossRewardParent.GetChild(i);
                child.GetChild(0).GetComponent<Image>().sprite = BoxMenu.instance.keySprites[i];
                child.GetChild(1).GetComponent<Text>().text = string.Format("{0} + {1}", TableData.Key_Name[i], BoxManager.instance.keyValue[i]);
                child.gameObject.SetActive(true);
                CentralInfoManager.keyCountList[i] += BoxManager.instance.keyValue[i];
            }
        }
        // 나중에 여기서 순위 변동 사항도 표시해주기
        BossRankingSetting();
        BoxMenu.instance.BoxMenuSetting();

        DungeonManager.instance.animationNotActive = true;
        bossEndPopUp.SetActive(true);
        BossDungeonManager.instance.RankSetting();
    }

    public void OnClickCompletePopUpYes()
    {
        DungeonManager.instance.DungeonChange(DungeonManager.DUNGEON_STATE.NORMAL);
        entranceButton_ticket.interactable = true;     //입장버튼 비활성화
        entranceButton_ad.interactable = true;
        entranceText_ticket.text = "입장하기";
        string colorCode = "F0DEB8";
        if (CurrencyManager.instance.Ticket_BossDungeon <= 0)
            colorCode = "FA6E6D";
        entranceText_ad.text = string.Format("광고보고 입장하기 (1/<color=#{0}>{1}</color>)", colorCode, CentralInfoManager.boss_adCount);
        BossDungeonMenuSetting();
    }

    private void EntranceTicketSetting(double entranceTicket)
    {
        string colorCode = "F0DEB8";
        if (entranceTicket <= 0)
        {
            colorCode = "FA6E6D";
            entranceButton_ticket.interactable = false;
        }

        entranceCountText.text = string.Format("<b>1/<color=#{0}>{1}</color></b>", colorCode, entranceTicket);
    }

    private void EntranceTicketSetting_Ad()
    {
        string colorCode = "F0DEB8";
        if (CentralInfoManager.boss_adCount >= 10)
        {
            colorCode = "FA6E6D";
            entranceButton_ad.interactable = false;
        }

        entranceText_ad.text = string.Format("광고보고 입장하기 ({0}/<color=#{1}>10</color>)", CentralInfoManager.boss_adCount, colorCode);
    }

    private void BossRankingSetting()
    {
        RankManager.instance.GetMyRTRank(4);
        RankManager.instance.GetRtRank(4, 10);
    }

    public void BossRankingSetting_Mine(JsonData data)
    {
        bossRankGOMine.transform.GetChild(0).GetComponent<Text>().text = data[0]["rank"]["N"].ToString();
        bossRankGOMine.transform.GetChild(1).GetComponent<Text>().text = data[0]["nickname"].ToString();
        bossRankGOMine.transform.GetChild(2).GetComponent<Text>().text = data[0]["score"]["N"].ToString();
        myBDScore = int.Parse(data[0]["score"]["N"].ToString());
    }

    public void BossRankingSetting_MineNull()
    {
        bossRankGOMine.transform.GetChild(0).GetComponent<Text>().text = "---";
        bossRankGOMine.transform.GetChild(1).GetComponent<Text>().text = SaveManager.instance.nickName;
        bossRankGOMine.transform.GetChild(2).GetComponent<Text>().text = "---";
        myBDScore = 0;
    }

    public void BossRankingSetting_User(JsonData data)
    {
        for (int i = 0; i < bossRankGOUsers.Length; i++)
        {
            if (i < data.Count)
            {
                bossRankGOUsers[i].transform.GetChild(0).GetComponent<Text>().text = data[i]["rank"]["N"].ToString();
                bossRankGOUsers[i].transform.GetChild(1).GetComponent<Text>().text = data[i]["nickname"].ToString();
                bossRankGOUsers[i].transform.GetChild(2).GetComponent<Text>().text = data[i]["score"]["N"].ToString();
            }
            else
            {
                bossRankGOUsers[i].transform.GetChild(0).GetComponent<Text>().text = "";
                bossRankGOUsers[i].transform.GetChild(1).GetComponent<Text>().text = "---";
                bossRankGOUsers[i].transform.GetChild(2).GetComponent<Text>().text = "";
            }
        }
    }
}