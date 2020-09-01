using BackEnd;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RankManager : MonoBehaviour
{
    public static RankManager instance;
    private const string accumulatedStage_UUID = "cd03b630-dc48-11ea-b6c9-87a46f2f5a7c";
    private const string bestStage_UUID = "bed6ddd0-dc48-11ea-99c8-992b40ac3a63";
    private const string myStat_UUID = "e9dbbeb0-dc48-11ea-99c8-992b40ac3a63";
    private const string bossDungeonStage_UUID = "a9263710-afca-11ea-b69a-31bb9942f2b1";
    private const string rebirthCount_UUID = "dddb3c30-dc48-11ea-99c8-992b40ac3a63";

    public static double accumulatedStage;
    public static double bestStage;
    public static double myStat;
    public static double bossDungeonStage;
    public static double rebirthCount;

    public double myStat_saved;

    private bool isRtScoreIndate = false;
    public bool isUpdate = false;

    private string currColumn = "";
    private int rankIndex = 0;
    public bool UpdateRTRankTable_queue = false;
    public bool UpdateRTRankTable_queue_fail = false;

    public bool GetMyRTRank_queue = false;
    public bool GetMyRTRank_queue_fail = false;
    public bool GetMyRTRank_result = false;
    public LitJson.JsonData myData;

    public bool GetRtRank_queue = false;
    public bool GetRtRank_queue_fail = false;
    public bool GetRtRank_result = false;
    public LitJson.JsonData data;

    public GameObject serverProgressGO;
    public GameObject restartPopUp_ranking;

    public Coroutine rankUpdate;
    private int restartIndex_rankUpdate = 0;
    private int restartIndex_getMyRank = 0;
    private int restartIndex_getUserRank = 0;

    public Text retryText;
    private Coroutine retryTextCO;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => LoadManager.instance.isDataLoaded);
        yield return new WaitUntil(() => QueueManager.instance.isQueueSet);
        SendQueue.Enqueue(Backend.RTRank.GetMyRTRank, ReturnUUID(0), 0, callback =>
        {
            if (!callback.IsSuccess())
                myStat_saved = 0;
            else
            {
                LitJson.JsonData myData = callback.GetReturnValuetoJSON()["rows"];
                myStat_saved = double.Parse(myData[0]["score"]["N"].ToString());
            }
        });
    }

    public IEnumerator UpdateRank(int index, double upValue)
    {
        yield return GameManager.YieldInstructionCache.WaitForEndOfFrame;
#if UNITY_EDITOR
        RankManager.instance.isUpdate = true;
#elif UNITY_ANDROID
        switch (index)
        {
            case 0:     //누적 층수
                accumulatedStage += upValue;
                break;

            case 1:     //최고 층수 -> 여기는 좀 더 연산이필요함
                if (CentralInfoManager.stageCount > bestStage)
                    bestStage = CentralInfoManager.stageCount;
                break;

            case 2:     //환생 횟수
                rebirthCount = upValue;
                rankUpdate = StartCoroutine(UpdateAtOnce());

                break;

            case 3:   //전투력
                if (myStat < upValue)
                    myStat = upValue;
                break;

            case 4:  // 보스전
                if (BossDungeonMenu.instance.myBDScore < upValue)
                {
                    bossDungeonStage = upValue;
                    StartCoroutine(UpdateBossDungeon());
                }
                break;

            default:
                break;
        }
#endif
    }

    private IEnumerator UpdateCHKCO()
    {
        Debug.Log("랭킹 업데이트 확인 코루틴 입성");
        yield return new WaitForSecondsRealtime(8f);
        Debug.Log("코루틴 8초 후------------------------------------ / 상태 : " + StateManager.instance.state);

        if (StateManager.instance.state == StateManager.STATE.IDLE)
        {
            if (Time.timeScale != 0)
            {
                Debug.Log("업데이트 체크 코루틴◆◆◆◆◆◆◆◆◆◆◆◆◆");
                StartCoroutine(GameManager.instance.ChkTokenAndRefresh());
                yield return GameManager.YieldInstructionCache.WaitForEndOfFrame;

                RestartRanking();
            }
            else
            {
                if (RebirthManager.instance.isRebirth)
                {
                    Debug.Log("튜토리얼_업데이트 체크 코루틴◆◆◆◆◆◆◆◆◆◆◆◆◆");
                    StartCoroutine(GameManager.instance.ChkTokenAndRefresh());
                    yield return GameManager.YieldInstructionCache.WaitForEndOfFrame;

                    RestartRanking();
                }
            }
        }
    }

    public void RestartRanking()
    {
        if (rankUpdate != null)
        {
            if (restartIndex_rankUpdate < 3)
            {
                restartIndex_rankUpdate++;
                if (retryTextCO == null)
                    retryTextCO = StartCoroutine(RestartTextCO());
                StopCoroutine(rankUpdate);
                rankUpdate = StartCoroutine(UpdateAtOnce());
            }
            else
            {
                restartIndex_rankUpdate = 0;
                restartPopUp_ranking.SetActive(true);
                retryTextCO = null;
            }
        }
    }

    private IEnumerator RestartTextCO()
    {
        retryText.gameObject.SetActive(true);
        while (StateManager.instance.fadeGO_Rebirth.activeSelf)
        {
            yield return new WaitForSeconds(1f);
            retryText.text = string.Format("전승 정보 저장 재시도중.({0})", restartIndex_rankUpdate);
            yield return new WaitForSeconds(1f);
            retryText.text = string.Format("전승 정보 저장 재시도중..({0})", restartIndex_rankUpdate);
            yield return new WaitForSeconds(1f);
            retryText.text = string.Format("전승 정보 저장 재시도중...({0})", restartIndex_rankUpdate);
        }
    }

    private IEnumerator UpdateAtOnce()
    {
        StartCoroutine(UpdateCHKCO());
        yield return GameManager.YieldInstructionCache.WaitForEndOfFrame;
        UpdateRTRankTable("rebirthCount", rebirthCount);
        yield return new WaitUntil(() => isRtScoreIndate);
        if (myStat > myStat_saved)
        {
            UpdateRTRankTable("myStat", myStat);
            yield return new WaitUntil(() => isRtScoreIndate);
        }

        UpdateRTRankTable("accumulatedStage", accumulatedStage);
        yield return new WaitUntil(() => isRtScoreIndate);

        UpdateRTRankTable("bestStage", bestStage);
        yield return new WaitUntil(() => isRtScoreIndate);

        isUpdate = true;
        //StopCoroutine(retryTextCO);
        //retryTextCO = null;

        SendQueue.Enqueue(Backend.RTRank.GetMyRTRank, ReturnUUID(0), 0, callback =>
        {
            if (!callback.IsSuccess())
                myStat_saved = 0;
            else
            {
                LitJson.JsonData myData = callback.GetReturnValuetoJSON()["rows"];
                myStat_saved = double.Parse(myData[0]["score"]["N"].ToString());
            }
        });
    }

    public IEnumerator UpdateBossDungeon()
    {
        UpdateRTRankTable("bossDungeonStage", bossDungeonStage);
        yield return new WaitUntil(() => isRtScoreIndate);
    }

    // 갱신
    private void UpdateRTRankTable(string cloumn, double value)
    {
        isRtScoreIndate = false;
        currColumn = cloumn;
        if (SaveManager.instance.indate_cha.Equals(string.Empty))
        {
            Debug.Log("Indate가 존재하지 않습니다.");
            return;
        }
        try
        {
            SendQueue.Enqueue(Backend.GameInfo.UpdateRTRankTable, "character", cloumn, (long)value, SaveManager.instance.indate_cha, callback =>
            {
                if (!callback.IsSuccess() || callback.IsServerError())
                {
                    UpdateRTRankTable_queue_fail = true;
                    return;
                }
                else if (callback.IsSuccess())
                {
                    //currIndex = index;
                    UpdateRTRankTable_queue = true;

                    isRtScoreIndate = true;
                }
            });
        }
        catch (System.Exception e)
        {
            Debug.Log("랭크업데이트 catch *********************");
            //if (cloumn != "bossDungeonStage")
            //    RestartRanking();
            GameManager.SaveLogToServer(string.Format("{0} 랭킹 업데이트 에러 발생", cloumn), e.ToString(), "에러");
        }
    }

    //불러오기
    private string ReturnUUID(int index)
    {
        var rValue = "";
        switch (index)
        {
            case 0:
                rValue = myStat_UUID;

                break;

            case 1:
                rValue = accumulatedStage_UUID;
                break;

            case 2:
                rValue = bestStage_UUID;
                break;

            case 3:
                rValue = rebirthCount_UUID;
                break;

            case 4:
                rValue = bossDungeonStage_UUID;
                break;

            default:
                break;
        }
        return rValue;
    }

    // 1. 내 링크 불러오기
    public void GetMyRTRank(int index)
    {
        try
        {
            SendQueue.Enqueue(Backend.RTRank.GetMyRTRank, ReturnUUID(index), 0, callback =>
            {
                rankIndex = index;
                GetMyRTRank_result = callback.IsSuccess();
                if (!callback.IsSuccess() || callback.IsServerError())
                {
                    restartIndex_getMyRank = index;
                    GetMyRTRank_queue_fail = true;
                    GetMyRTRank_queue = true;
                }
                else
                {
                    myData = callback.GetReturnValuetoJSON()["rows"];
                    GetMyRTRank_queue = true;
                }
            });
        }
        catch (System.Exception e)
        {
            GameManager.SaveLogToServer("내 랭킹 정보 가져오기 에러 발생", e.ToString(), "에러");
        }
    }

    public void RestartGetMyRTRank()
    {
        GetMyRTRank(restartIndex_getMyRank);
    }

    public void GetMyRTRank_MT()
    {
        if (!GetMyRTRank_result)
        {
            if (rankIndex != 4)
                RankMenu.instance.MyInfoSetting_null(rankIndex);
            else
                BossDungeonMenu.instance.BossRankingSetting_MineNull();
            Debug.Log("GetMyRTRank 결과가 존재하지 않습니다.");
        }
        else
        {
            if (rankIndex != 4)
                RankMenu.instance.MyInfoSetting(rankIndex, myData);
            else
                BossDungeonMenu.instance.BossRankingSetting_Mine(myData);
        }
    }

    // 2. 다른 유저 랭크 불러오기
    public void GetRtRank(int index, int count)
    {
        try
        {
            SendQueue.Enqueue(Backend.RTRank.GetRTRankByUuid, ReturnUUID(index), count, callback =>
            {
                GetRtRank_result = callback.IsSuccess();
                if (!callback.IsSuccess() || callback.IsServerError())
                {
                    restartIndex_getUserRank = index;
                    GetRtRank_queue = true;
                    GetRtRank_queue_fail = true;
                    return;
                }
                else if (callback.IsSuccess())
                {
                    data = callback.GetReturnValuetoJSON()["rows"];
                    GetRtRank_queue = true;
                }
            });
        }
        catch (System.Exception e)
        {
            GameManager.SaveLogToServer("유저 랭킹 정보 가져오기 에러 발생", e.ToString(), "에러");
        }
    }

    public void RestartGetUserRank()
    {
        if (restartIndex_getUserRank == 4)
            GetRtRank(restartIndex_getUserRank, 10);
        else
            GetRtRank(restartIndex_getUserRank, 50);
    }

    public void GetRtRank_MT()
    {
        serverProgressGO.SetActive(true);
        if (!GetRtRank_result)
        {
            Debug.Log("GetRtRank 결과가 존재하지 않습니다.");
            return;
        }
        else
        {
            if (data.Count <= 0)
            {
                Debug.Log("GetRtRank의 결과가 비어있습니다.");
                return;
            }
            if (rankIndex != 4)
                RankMenu.instance.OthersUserInfoSetting(rankIndex, data);
            else
                BossDungeonMenu.instance.BossRankingSetting_User(data);
            serverProgressGO.SetActive(false);
        }
    }
}