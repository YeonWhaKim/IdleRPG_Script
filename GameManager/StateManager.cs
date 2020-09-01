using System.Collections;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager instance;

    public enum STATE { IDLE, MOVE, ACTION, BOSSDIEMOVE };

    public STATE state;

    // Start is called before the first frame update
    public delegate void Idle();

    public delegate void Move();

    public delegate void Action();

    public delegate void BossDieMove();

    public event Idle idle;

    public event Move move;

    public event Action action;

    public event BossDieMove bossDieMove;

    public GameObject fadeGO_Rebirth;
    private Coroutine stateCoroutine;

    private void Awake()
    {
        instance = this;
        state = STATE.MOVE;
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => LoadManager.instance.isDataLoaded);
        yield return new WaitWhile(() => LoadManager.instance.fade.activeSelf);
        yield return null;
        if (stateCoroutine == null)
            stateCoroutine = StartCoroutine(StateMachine());
        else
        {
            stateCoroutine = null;
            StopCoroutine(stateCoroutine);
            stateCoroutine = StartCoroutine(StateMachine());
        }
        SoundManager.instance.AllSoundPlayExceptBG();
    }

    private IEnumerator StateMachine()
    {
        STATE chkstate = STATE.MOVE;
        move();
        StartCoroutine(GoActionState());
        while (true)
        {
            yield return GameManager.YieldInstructionCache.WaitForEndOfFrame;
            if (chkstate != state)
            {
                chkstate = state;
                StateChange(state);
            }
        }
    }

    private void StateChange(STATE state)
    {
        switch (state)
        {
            case STATE.IDLE:
                break;

            case STATE.MOVE:
                //if (DungeonManager.instance.dungeon_state.Equals(DungeonManager.DUNGEON_STATE.NORMAL))
                if (CentralInfoManager.regionCount != 10)
                    StageManager.instance.StgeSetting(DungeonManager.instance.dungeon_state);
                //move();
                //StartCoroutine(GoActionState());
                GoMoveState();
                break;

            case STATE.ACTION:
                action();
                break;

            case STATE.BOSSDIEMOVE:
                bossDieMove();
                break;

            default:
                break;
        }
    }

    private IEnumerator GoActionState()
    {
        yield return GameManager.YieldInstructionCache.WaitForSeconds(2f);
        if (TutorialManager.instance.isRebirthTutorial.Equals(false)
            && DungeonManager.instance.animationNotActive.Equals(false))
            state = STATE.ACTION;
    }

    public void GoMoveState()
    {
        state = STATE.MOVE;
        move();

        StartCoroutine(GoActionState());
    }

    public void GoIdleState()
    {
        if (state.Equals(STATE.IDLE))
            return;
        state = STATE.IDLE;
        idle();
        //if (stateCoroutine != null)
        //{
        //    StopCoroutine(stateCoroutine);
        //    stateCoroutine = null;
        //}
    }

    public void GoBossDieMoveState()
    {
        state = STATE.BOSSDIEMOVE;
    }

    public void RestartStageCoroutine()
    {
        StartCoroutine(RestartStageCoroutineCo());
    }

    public IEnumerator RebirthCoroutine()
    {
        DungeonManager.instance.animationNotActive = true;
        GameManager.GameSpeedSet();
        GoIdleState();

        yield return GameManager.YieldInstructionCache.WaitForSeconds(1f);

        CharacterState.instance.CharacterRebirthEffect();

        yield return GameManager.YieldInstructionCache.WaitForSeconds(2.5f);

        StageManager.instance.RebirthStageSetting();
        if (!fadeGO_Rebirth.activeSelf)
            fadeGO_Rebirth.SetActive(true);
        yield return GameManager.YieldInstructionCache.WaitForSeconds(1.5f);
        yield return new WaitUntil(() => RebirthManager.instance.availableInfoSetting);
        RankManager.instance.isUpdate = false;
        InfoMenu.instance.InfoMenuSetting();
        fadeGO_Rebirth.GetComponent<Animator>().SetTrigger("Fade");
        yield return GameManager.YieldInstructionCache.WaitForSeconds(0.5f);
        if (TutorialManager.instance.isRebirthTutorial)
        {
            MenuManager.instance.OnClickMenuButton(0);
            InfoMenu.instance.OnClickEquip(8);
            TutorialManager.instance.OnClickTutorialPanel_Rebirth();
            Time.timeScale = 0;
        }
        else
            StartCoroutine(RestartStageCoroutineCo());
        DungeonManager.instance.animationNotActive = false;
        RebirthManager.instance.rebirthEffect.SetActive(false);
        RebirthManager.instance.isRebirth = false;
    }

    public IEnumerator RestartStageCoroutineCo()
    {
        yield return new WaitWhile(() => TutorialManager.instance.isRebirthTutorial);
        state = STATE.MOVE;
        if (stateCoroutine != null)
        {
            StopCoroutine(stateCoroutine);
            //StopAllCoroutines();
            stateCoroutine = null;
        }
        yield return null;
        stateCoroutine = StartCoroutine(StateMachine());
    }
}