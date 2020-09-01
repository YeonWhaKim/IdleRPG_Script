using UnityEngine;
using UnityEngine.UI;

public class DungeonMenu : MonoBehaviour
{
    public static DungeonMenu instance;

    [Header("Dungeon Menu Tab & Panel")]
    public Image[] menutab;

    public GameObject[] menupanel;

    // Start is called before the first frame update
    private void Start()
    {
        instance = this;
    }

    public void OnClickMenuTab(int index)
    {
        for (int i = 0; i < menutab.Length; i++)
        {
            if (i.Equals(index))
            {
                menutab[i].color = Color.white;
                menupanel[i].SetActive(true);
            }
            else
            {
                menutab[i].color = new Vector4(0.509f, 0.509f, 0.509f, 1);
                menupanel[i].SetActive(false);
            }
        }
    }
}