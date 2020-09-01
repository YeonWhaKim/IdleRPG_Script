using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    // Start is called before the first frame update
    public List<GameObject> menuPanels;

    public List<Image> menuIcon;
    public GameObject bossTicketCountGO;
    public GameObject enchantTicketCountGO;

    private void Start()
    {
        instance = this;
        menuPanels[0].SetActive(true);
        menuIcon[0].color = Color.white;
        for (int i = 1; i < menuPanels.Count; i++)
        {
            menuPanels[i].SetActive(false);
            menuIcon[i].color = new Vector4(1, 1, 1, 0.6f);
        }
    }

    public void OnClickMenuButton(int index)
    {
        for (int i = 0; i < menuPanels.Count; i++)
        {
            if (i.Equals(index))
            {
                menuPanels[i].SetActive(true);
                menuIcon[i].color = Color.white;
            }
            else
            {
                menuPanels[i].SetActive(false);
                menuIcon[i].color = new Vector4(1, 1, 1, 0.6f);
            }
        }
        switch (index)
        {
            case 0:
                InfoMenu.instance.InfoMenuSetting();
                break;

            case 1:
                EquipMenu.instance.OnClickEquipTab(0);
                if (TutorialManager.instance.isEquipTutorial)
                    TutorialManager.instance.OnClickTutorialPanel_Equip();
                break;

            case 2:
                BossDungeonMenu.instance.BossDungeonMenuSetting();
                if (TutorialManager.instance.isBossTutorial)
                    TutorialManager.instance.OnClickTutorialPanel_Boss();
                break;

            case 3:
                //상자
                BoxMenu.instance.BoxMenuSetting();
                if (TutorialManager.instance.isBossTutorial)
                    TutorialManager.instance.OnClickTutorialPanel_Boss();
                break;

            case 4:     //
                MessageManager.instance.OpenChatUI();
                break;

            case 5:     //랭킹
                RankMenu.instance.OnClickMenuTab(0);
                break;

            case 6:
                ShopMenu.instance.OnClickMenuTab(0);
                break;

            default:
                break;
        }
        //if (index.Equals(3))
        //{
        //    bossTicketCountGO.SetActive(true);
        //    enchantTicketCountGO.SetActive(true);
        //}
        //else
        //{
        //    bossTicketCountGO.SetActive(false);
        //    enchantTicketCountGO.SetActive(false);
        //}

        SoundManager.instance.menuButtonSource.Play();
    }

    public void OnClickInfoMenuSoSetting()
    {
        InfoMenu.instance.InfoMenuSetting();
    }
}