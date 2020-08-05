using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;
    private ObscuredDecimal gold;
    private ObscuredLong jewel;
    private ObscuredLong token;
    private double ticket_bossDungeon;

    private bool isStart = false;

    [Header("UI")]
    public Text goldText;

    public Text goldEffectText;
    public Text jewelText;
    public Text tokenText;
    public Text ticket_boss;

    public decimal Gold
    {
        get { return gold; }
        set
        {
            gold = value;

            if (gold < 0)
                gold = 0;

            if (CentralInfoManager.instance.isEquipUpgraded.Equals(false) && Gold >= 350 && LoadManager.instance.isDataLoaded && EquipManager.instance.equipState_weapon[2] < 0)
                TutorialManager.instance.EquipTutorial();

            if (gold >= 1.0E24m)
                gold = 1.0E24m - 1;
            goldText.text = GameManager.NumberNotation_Korean(gold, true);
        }
    }

    public long Jewel
    {
        get { return jewel; }
        set
        {
            jewel = value;
            jewelText.text = GameManager.NumberNotation_comma((decimal)jewel);
        }
    }

    public long Token
    {
        get { return token; }
        set
        {
            token = value;
            tokenText.text = GameManager.NumberNotation_comma((decimal)token);
        }
    }

    public double Ticket_BossDungeon
    {
        get { return ticket_bossDungeon; }
        set
        {
            ticket_bossDungeon = value;
            ticket_boss.text = string.Format("{0}장", ticket_bossDungeon);
        }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    public void AddGold(decimal addedValue)
    {
        addedValue *= BuffManager.instance.buffValue[0];
        Gold += addedValue;
        goldEffectText.text = string.Format("+{0}", GameManager.NumberNotation_Korean(addedValue, true));
        goldEffectText.gameObject.SetActive(true);
        SoundManager.instance.GoldGetPlay();
    }

    public void MinusGold(decimal minusedValue)
    {
        Gold -= minusedValue;
    }

    public void JewelChange(long jewel, string whereToUse)
    {
        Jewel += jewel;
        GameManager.SaveLogToServer(whereToUse, jewel.ToString(), "");
    }
}