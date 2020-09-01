using UnityEngine;
using UnityEngine.UI;

public class RankMenu : MonoBehaviour
{
    public static RankMenu instance;
    public string[] rankName = new string[4] { "전투력", "누적 층수", "최고 층수", "전승 횟수" };

    [Header("Rank MenuTab % Panel")]
    public Image[] menutab;

    public Text[] menutabTexts;
    public Text renewingExpText;

    [Header("My Rank UI")]
    public Text myRankText;

    public Text myInfoText;

    [Header("All User Rank UI")]
    public GameObject[] rankUIGO;

    public Text[] orderTexts;
    public Text[] nickNameTexts;
    public Text[] infoTexts;

    // Start is called before the first frame update
    private void Start()
    {
        instance = this;
    }

    public void OnClickMenuTab(int index)
    {
        RankSetting(index);
        for (int i = 0; i < menutab.Length; i++)
        {
            if (i.Equals(index))
            {
                menutab[i].sprite = EquipMenu.instance.ingSprite;
                menutabTexts[i].color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 1);
            }
            else
            {
                menutab[i].sprite = EquipMenu.instance.availableSprite;
                menutabTexts[i].color = new Vector4(0.9411765f, 0.8705883f, 0.7215686f, 1);
            }
        }
    }

    private void RankSetting(int index)
    {
        UIClear();

        renewingExpText.text = "* 내 점수 갱신은 '전승'시 이루어집니다.";
        //내 정보 세팅
        RankManager.instance.GetMyRTRank(index);
        //다른 유저 정보 세팅
        RankManager.instance.GetRtRank(index, 50);
    }

    private void UIClear()
    {
        for (int i = 0; i < rankUIGO.Length; i++)
        {
            rankUIGO[i].SetActive(false);
        }
    }

    public void MyInfoSetting(int index, LitJson.JsonData data)
    {
        myRankText.text = string.Format("{0}위", data[0]["rank"]["N"].ToString());
        myInfoText.text = string.Format("<color=#A2A2A2>{0}</color>     {1}", rankName[index], GameManager.NumberNotation_comma(double.Parse(data[0]["score"]["N"].ToString())));
    }

    public void MyInfoSetting_null(int index)
    {
        myRankText.text = string.Format("-- 위");
        myInfoText.text = string.Format("<color=#A2A2A2>{0}</color>     ---", rankName[index]);
    }

    public void OthersUserInfoSetting(int index, LitJson.JsonData data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            orderTexts[i].text = data[i]["rank"]["N"].ToString();
            nickNameTexts[i].text = data[i]["nickname"].ToString();
            infoTexts[i].text = string.Format("<color=#A2A2A2><size=16>{0}</size></color>\n{1}", rankName[index],
                GameManager.NumberNotation_comma(double.Parse(data[i]["score"]["N"].ToString())));
            rankUIGO[i].SetActive(true);
        }
    }
}