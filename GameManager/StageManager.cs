using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [Serializable]
    public class UI
    {
        public Text stageText;

        public void StageSet(long stage, long region)
        {
            if (region >= 10)
                stageText.text = string.Format("{0}층 - 보스방", stage);
            else
                stageText.text = string.Format("{0}층 - {1}번 방", stage, region);

            if (CentralInfoManager.cheaterDetected) GameManager.SaveLogToServer("Stage or Region Cheater", stage.ToString(), "CheaterDetector");
        }
    }

    public static StageManager instance;
    public const int CLASSIFIED_BG_COUNT = 7;

    [SerializeField] public UI ui;
    public List<SpriteRenderer> bg_0;
    public List<SpriteRenderer> bg_1;
    public List<SpriteRenderer> bg_2;
    public List<SpriteRenderer> bg_3;
    public List<SpriteRenderer> bg_4;
    public List<SpriteRenderer> bg_5;
    public SpriteRenderer bg_6;

    [Header("Normal Dungeon Sprite")]
    public List<Sprite> bg_n;

    [Header("Enchant Dungeon Sprite")]
    public List<Sprite> bg_e;

    [Header("Boss Dungeon Sprite")]
    public List<Sprite> bg_b;

    private void OnEnable()
    {
        CentralInfoManager.instance.startSetting += StartSetting;
        instance = this;
    }

    private void StartSetting()
    {
        var realbgIndex = (RealStageCount_Loop() - 1) * CLASSIFIED_BG_COUNT;  //여기서 loop로 변경
        BGChange(bg_n, realbgIndex);
        ui.StageSet(CentralInfoManager.stageCount, CentralInfoManager.regionCount);
    }

    public void StgeSetting(DungeonManager.DUNGEON_STATE dungeonState)
    {
        switch (dungeonState)
        {
            case DungeonManager.DUNGEON_STATE.NORMAL:
                NormalStageSetting();
                break;

            case DungeonManager.DUNGEON_STATE.ENCHANT:
                EnchantStageSetting();
                break;

            case DungeonManager.DUNGEON_STATE.BOSS:
                BossStageSetting();
                break;

            default:
                break;
        }
    }

    private void NormalStageSetting()
    {//나중에 여기서 스테이지 count 세팅 지금도...?
        if (CentralInfoManager.instance.IsBossMonster())
        {
            CentralInfoManager.regionCount = 11;
            CentralInfoManager.stageCount += 1;

            OverflowManager.ChkStageOverFlow();
            EquipManager.instance.JudgeEquipRock(EquipManager.EOT_Stage, CentralInfoManager.stageCount);
            StartCoroutine(RankManager.instance.UpdateRank(0, 1));
            StartCoroutine(RankManager.instance.UpdateRank(1, 1));

            if (CentralInfoManager.stageCount % 10 == 0
                || CentralInfoManager.rebirthStartStage + 1 == CentralInfoManager.stageCount)         /////////////////////////////////////
                SaveManager.instance.SaveData();
        }
        else
        {
            if (CentralInfoManager.regionCount >= 10)
                CentralInfoManager.regionCount = 1;
            else
                CentralInfoManager.regionCount++;
        }

        var realbgIndex = (RealStageCount_Loop() - 1) * CLASSIFIED_BG_COUNT;  //여기서 loop로 변경
        BGChange(bg_n, realbgIndex);
        ui.StageSet(CentralInfoManager.stageCount, CentralInfoManager.regionCount);
    }

    public void RebirthStageSetting()
    {
        CentralInfoManager.regionCount = 1;

        var stage = CentralInfoManager.stageCount / 100;
        stage *= 100;
        if (stage < 1)
            stage = 1;
        CentralInfoManager.stageCount = stage;

        var realbgIndex = (RealStageCount_Loop() - 1) * CLASSIFIED_BG_COUNT;  //여기서 loop로 변경
        BGChange(bg_n, realbgIndex);
        ui.StageSet(CentralInfoManager.stageCount, CentralInfoManager.regionCount);
    }

    private void EnchantStageSetting()
    {
        BGChange(bg_e, 0);
    }

    private void BossStageSetting()
    {
        BGChange(bg_b, 0);
    }

    private void BGChange(List<Sprite> bgSprite, int index)
    {
        for (int i = 0; i < bg_0.Count; i++)
        {
            bg_0[i].sprite = bgSprite[0 + index];
            bg_1[i].sprite = bgSprite[1 + index];
            bg_2[i].sprite = bgSprite[2 + index];
        }
        for (int i = 0; i < bg_3.Count; i++)
        {
            bg_3[i].sprite = bgSprite[3 + index];
        }
        for (int i = 0; i < bg_4.Count; i++)
        {
            bg_4[i].sprite = bgSprite[4 + index];
        }
        for (int i = 0; i < bg_5.Count; i++)
        {
            bg_5[i].sprite = bgSprite[5 + index];
        }
        bg_6.sprite = bgSprite[6 + index];
    }

    public int RealStageCount_Loop()
    {
        var bgKindsCount = bg_n.Count / CLASSIFIED_BG_COUNT;
        var result = CentralInfoManager.stageCount % bgKindsCount;
        if (result.Equals(0))
            result = bgKindsCount;

        return (int)result;
    }
}