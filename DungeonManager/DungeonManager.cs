using System.Collections;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance;

    public enum DUNGEON_STATE { NORMAL, ENCHANT, BOSS };

    public DUNGEON_STATE dungeon_state;

    public GameObject fadeGO;
    public GameObject[] exitPopUpGo;

    [Header("Normal Dungeon Object")]
    public GameObject n_StageTextGO;

    public GameObject[] n_buffGO;

    [Header("Enchant Dungeon Object")]
    public GameObject startGO;

    [Header("Boss Dungeon Object")]
    public GameObject b_FloorCountGO;

    public GameObject b_BossHPGO;
    public GameObject b_TimerGO;
    public GameObject b_ExitGO;
    public GameObject b_speedGO;

    public bool animationNotActive = false;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        dungeon_state = DUNGEON_STATE.NORMAL;

        n_StageTextGO.SetActive(true);
        for (int i = 0; i < n_buffGO.Length; i++)
        {
            n_buffGO[i].SetActive(true);
        }
        b_FloorCountGO.SetActive(false);
        b_BossHPGO.SetActive(false);
        b_TimerGO.SetActive(false);
        b_ExitGO.SetActive(false);
        b_speedGO.SetActive(false);
    }

    public void DungeonChange(DUNGEON_STATE state)
    {
        StartCoroutine(DungeonChangeCO(state));
    }

    private IEnumerator DungeonChangeCO(DUNGEON_STATE state)
    {
        dungeon_state = state;
        yield return GameManager.YieldInstructionCache.WaitForEndOfFrame;
        fadeGO.SetActive(true);
        //fadeGO.GetComponent<Animator>().Rebind();'
        if (state.Equals(DUNGEON_STATE.BOSS))
            animationNotActive = true;

        yield return new WaitForSecondsRealtime(1f);
        GameManager.GameSpeedSet();
        switch (state)
        {
            case DUNGEON_STATE.NORMAL:
                NormalDungeon.n_instance.DungeonSetting();
                SoundManager.instance.BGMChange(1);
                SaveManager.instance.SaveData();
                break;

            case DUNGEON_STATE.ENCHANT:
                EnchantDungeon.e_instance.DungeonSetting();
                break;

            case DUNGEON_STATE.BOSS:

                BossDungeon.b_instance.DungeonSetting();
                SoundManager.instance.BGMChange(0);
                break;

            default:
                break;
        }
    }

    public void OnClickExit(int index)
    {
        exitPopUpGo[index].SetActive(true);
    }
}