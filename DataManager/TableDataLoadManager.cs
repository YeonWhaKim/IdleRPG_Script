using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableDataLoadManager : MonoBehaviour
{
    public static TableDataLoadManager instance;
    public bool isLoadDone = false;
    public const int Table_Start_Index = 2;

    public string Status_Up_Data_FileId;
    public string Status_Data_FileId;
    public string Stage_Boss_Data_FileId;
    public string Stage_Normal_Data_FileId;
    public string Level_Data_FileId;

    public string Upgrade_Data_FileId;
    public string Item_Data_FileId;

    public string Replay_Data_FileId;

    public string Dungeon_Data_FileId;
    public string Gatcha_Data_FileId;
    public string Pet_Data_FileId;

    public string Box_Data_FileId;
    public string Key_Data_FileId;
    public string Shop_Data_FileId;
    public string DailyAttendance_Data_FileId;
    public string Mission_Data_FileId;

    private List<bool> isTableDataLoad = new List<bool>();

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        if (isTableDataLoad.Count > 0)
            isTableDataLoad.Clear();

        for (int i = 0; i < 20; i++)
        {
            isTableDataLoad.Add(false);
        }
    }

    public IEnumerator Load(bool isNewUser)
    {
        if (TableData.Level_Exp.Count > 0)
        {
            isLoadDone = true;
            LoadManager.instance.ProgressTextSet("게임을 재시작 중입니다");
            yield return new WaitForSecondsRealtime(3f);
            if (isNewUser.Equals(false))
            {
                StartCoroutine(LoadManager.instance.LoadData());
            }
            yield break;
        }

        BackendReturnObject getchartlist = Backend.Chart.GetChartList();

        JsonData rows = getchartlist.GetReturnValuetoJSON()["rows"];
        for (int i = 0; i < rows.Count; i++)
        {
            FileIdSetting(rows[i]["chartName"]["S"].ToString(), rows[i]["selectedChartFileId"]["N"].ToString());
        }

        LoadManager.instance.ProgressTextSet("데이터 불러오는 중(1%)");

        Backend.Chart.GetChartContents(Level_Data_FileId, callback =>
       {
           if (callback.IsSuccess())
           {
               JsonData row_level = callback.GetReturnValuetoJSON()["rows"];
               for (int i = Table_Start_Index; i < row_level.Count; i++)
               {
                   TableData.Level_Exp.Add(long.Parse(row_level[i]["Level_Exp"]["S"].ToString()));
                   TableData.Level_Random_Status.Add(int.Parse(row_level[i]["Level_Random_Status"]["S"].ToString()));
                    //Debug.Log(row["Level_Data"]["L"][i]["S"].ToString());
                }
               isTableDataLoad[0] = true;
           }
       });
        yield return new WaitUntil(() => isTableDataLoad[0]);
        LoadManager.instance.ProgressTextSet("데이터 불러오는 중(7%)");

        Backend.Chart.GetChartContents(Stage_Normal_Data_FileId, callback =>
        {
            if (callback.IsSuccess())
            {
                JsonData row_normal = callback.GetReturnValuetoJSON()["rows"];
                for (int i = Table_Start_Index; i < row_normal.Count; i++)
                {
                    TableData.Stage_Normal_Monster_Hp.Add(long.Parse(row_normal[i]["Stage_Normal_Monster_HP"]["S"].ToString()));
                    TableData.Stage_Normal_Clear_Hp_Plus.Add(long.Parse(row_normal[i]["Stage_Normal_Clear_HP_Plus"]["S"].ToString()));
                    TableData.Stage_Normal_Monster_Critical_Def.Add(float.Parse(row_normal[i]["Stage_Normal_Monster_Critical_Def"]["S"].ToString()));
                    TableData.Stage_Normal_Clear_Gold_Min.Add(decimal.Parse(row_normal[i]["Stage_Normal_Clear_Gold_Min"]["S"].ToString()));
                    TableData.Stage_Normal_Clear_Gold_Max.Add(decimal.Parse(row_normal[i]["Stage_Normal_Clear_Gold_Max"]["S"].ToString()));
                    TableData.Stage_Normal_Clear_Exp.Add(long.Parse(row_normal[i]["Stage_Normal_Clear_EXP"]["S"].ToString()));
                }
                isTableDataLoad[1] = true;
            }
        });
        yield return new WaitUntil(() => isTableDataLoad[1]);
        LoadManager.instance.ProgressTextSet("데이터 불러오는 중(11%)");

        Backend.Chart.GetChartContents(Stage_Boss_Data_FileId, callback =>
       {
           if (callback.IsSuccess())
           {
               JsonData row_boss = callback.GetReturnValuetoJSON()["rows"];
               for (int i = Table_Start_Index; i < row_boss.Count; i++)
               {
                   TableData.Stage_Boss_Monster_Hp.Add(long.Parse(row_boss[i]["Stage_Boss_Monster_HP"]["S"].ToString()));
                   TableData.Stage_Boss_Monster_Critical_Def.Add(float.Parse(row_boss[i]["Stage_Boss_Monster_Critical_Def"]["S"].ToString()));
                   TableData.Stage_Boss_Clear_Gold_Min.Add(decimal.Parse(row_boss[i]["Stage_Boss_Clear_Gold_Min"]["S"].ToString()));
                   TableData.Stage_Boss_Clear_Gold_Max.Add(decimal.Parse(row_boss[i]["Stage_Boss_Clear_Gold_Max"]["S"].ToString()));
                   TableData.Stage_Boss_Clear_Exp.Add(long.Parse(row_boss[i]["Stage_Boss_Clear_EXP"]["S"].ToString()));

                   TableData.Boss_Drop_Box_01.Add(int.Parse(row_boss[i]["Boss_Drop_Box_01"]["S"].ToString()));
                   TableData.Boss_Drop_Box_02.Add(int.Parse(row_boss[i]["Boss_Drop_Box_02"]["S"].ToString()));
                   TableData.Boss_Drop_Box_03.Add(int.Parse(row_boss[i]["Boss_Drop_Box_03"]["S"].ToString()));
                   TableData.Boss_Drop_Box_04.Add(int.Parse(row_boss[i]["Boss_Drop_Box_04"]["S"].ToString()));
                   TableData.Boss_Drop_Box_05.Add(int.Parse(row_boss[i]["Boss_Drop_Box_05"]["S"].ToString()));
                   TableData.Boss_Drop_Box_06.Add(int.Parse(row_boss[i]["Boss_Drop_Box_06"]["S"].ToString()));
                   TableData.Boss_Drop_Box_07.Add(int.Parse(row_boss[i]["Boss_Drop_Box_07"]["S"].ToString()));

                   TableData.Boss_Drop_Box_01_Per.Add(float.Parse(row_boss[i]["Boss_Drop_Box_01_Per"]["S"].ToString()));
                   TableData.Boss_Drop_Box_02_Per.Add(float.Parse(row_boss[i]["Boss_Drop_Box_02_Per"]["S"].ToString()));
                   TableData.Boss_Drop_Box_03_Per.Add(float.Parse(row_boss[i]["Boss_Drop_Box_03_Per"]["S"].ToString()));
                   TableData.Boss_Drop_Box_04_Per.Add(float.Parse(row_boss[i]["Boss_Drop_Box_04_Per"]["S"].ToString()));
                   TableData.Boss_Drop_Box_05_Per.Add(float.Parse(row_boss[i]["Boss_Drop_Box_05_Per"]["S"].ToString()));
                   TableData.Boss_Drop_Box_06_Per.Add(float.Parse(row_boss[i]["Boss_Drop_Box_06_Per"]["S"].ToString()));
                   TableData.Boss_Drop_Box_07_Per.Add(float.Parse(row_boss[i]["Boss_Drop_Box_07_Per"]["S"].ToString()));
               }
               isTableDataLoad[2] = true;
           }
       });
        yield return new WaitUntil(() => isTableDataLoad[2]);
        LoadManager.instance.ProgressTextSet("데이터 불러오는 중(18%)");

        Backend.Chart.GetChartContents(Status_Data_FileId, callback =>
       {
           if (callback.IsSuccess())
           {
               JsonData row_status = callback.GetReturnValuetoJSON()["rows"];
               for (int i = Table_Start_Index; i < row_status.Count; i++)
               {
                   TableData.Status_Min.Add(long.Parse(row_status[i]["Status_Min"]["S"].ToString()));
                   TableData.Status_Max.Add(long.Parse(row_status[i]["Status_Max"]["S"].ToString()));
                   TableData.Status_Up_Lp.Add(int.Parse(row_status[i]["Status_Up_Lp"]["S"].ToString()));
               }
               isTableDataLoad[3] = true;
           }
       });
        yield return new WaitUntil(() => isTableDataLoad[3]);
        LoadManager.instance.ProgressTextSet("데이터 불러오는 중(30%)");

        Backend.Chart.GetChartContents(Status_Up_Data_FileId, callback =>
       {
           if (callback.IsSuccess())
           {
               JsonData row_status_up = callback.GetReturnValuetoJSON()["rows"];
               for (int i = Table_Start_Index; i < row_status_up.Count; i++)
               {
                   TableData.Status_Up_Type.Add(row_status_up[i]["Status_Up_Type"]["S"].ToString());
                   TableData.Status_Up_Value.Add(row_status_up[i]["Status_Up_Value"]["S"].ToString());
                   TableData.Status_Up.Add(float.Parse(row_status_up[i]["Status_Up"]["S"].ToString()));
                   TableData.Status_ComatPoint.Add(int.Parse(row_status_up[i]["Status_CombatPoint"]["S"].ToString()));
               }
               isTableDataLoad[4] = true;
           }
       });
        yield return new WaitUntil(() => isTableDataLoad[4]);
        LoadManager.instance.ProgressTextSet("데이터 불러오는 중(36%)");

        Backend.Chart.GetChartContents(Item_Data_FileId, callback =>
        {
            if (callback.IsSuccess())
            {
                JsonData row_item = callback.GetReturnValuetoJSON()["rows"];
                for (int i = Table_Start_Index; i < row_item.Count; i++)
                {
                    TableData.Item_Name.Add(row_item[i]["Item_Name"]["S"].ToString());
                    TableData.Item_Grade.Add(row_item[i]["Item_Grade"]["S"].ToString());
                    TableData.Item_Status.Add(row_item[i]["Item_Status"]["S"].ToString());
                    TableData.Item_Status_First.Add(decimal.Parse(row_item[i]["Item_Status_First"]["S"].ToString()));
                    TableData.Item_Status_Upgrade.Add(decimal.Parse(row_item[i]["Item_Status_Upgrade"]["S"].ToString()));
                    TableData.Item_Open_Type.Add(row_item[i]["Item_Open_Type"]["S"].ToString());
                    TableData.Item_Open_Value.Add(int.Parse(row_item[i]["Item_Open_Value"]["S"].ToString()));
                    TableData.Gem_Open_Value.Add(int.Parse(row_item[i]["Gem_Open_Value"]["S"].ToString()));
                    TableData.Item_Buy_Type.Add(row_item[i]["Item_Buy_Type"]["S"].ToString());
                    TableData.Item_Buy_Value.Add(int.Parse(row_item[i]["Item_Buy_Value"]["S"].ToString()));
                    TableData.Item_Max_Level.Add(int.Parse(row_item[i]["Item_Max_Level"]["S"].ToString()));
                    TableData.Item_Look_Number.Add(row_item[i]["Item_Look_Number"]["S"].ToString());
                }
                isTableDataLoad[5] = true;
            }
        });
        yield return new WaitUntil(() => isTableDataLoad[5]);
        LoadManager.instance.ProgressTextSet("데이터 불러오는 중(42%)");

        Backend.Chart.GetChartContents(Upgrade_Data_FileId, callback =>
       {
           if (callback.IsSuccess())
           {
               JsonData row_upgrade = callback.GetReturnValuetoJSON()["rows"];
               for (int i = Table_Start_Index; i < row_upgrade.Count; i++)
               {
                   TableData.Upgrade_F_Gold.Add(decimal.Parse(row_upgrade[i]["Upgrade_F_Gold"]["S"].ToString()));
                   TableData.Upgrade_E_Gold.Add(decimal.Parse(row_upgrade[i]["Upgrade_E_Gold"]["S"].ToString()));
                   TableData.Upgrade_D_Gold.Add(decimal.Parse(row_upgrade[i]["Upgrade_D_Gold"]["S"].ToString()));
                   TableData.Upgrade_C_Gold.Add(decimal.Parse(row_upgrade[i]["Upgrade_C_Gold"]["S"].ToString()));
                   TableData.Upgrade_B_Gold.Add(decimal.Parse(row_upgrade[i]["Upgrade_B_Gold"]["S"].ToString()));
                   TableData.Upgrade_A_Gold.Add(decimal.Parse(row_upgrade[i]["Upgrade_A_Gold"]["S"].ToString()));
                   TableData.Upgrade_S_Gold.Add(decimal.Parse(row_upgrade[i]["Upgrade_S_Gold"]["S"].ToString()));
                   TableData.Minipin_Jewel.Add(double.Parse(row_upgrade[i]["Minipin_Jewel"]["S"].ToString()));
                   TableData.Poodle_Jewel.Add(double.Parse(row_upgrade[i]["Poodle_Jewel"]["S"].ToString()));
                   TableData.Cat_Jewel.Add(double.Parse(row_upgrade[i]["Cat_Jewel"]["S"].ToString()));
               }
               isTableDataLoad[6] = true;
           }
       });
        yield return new WaitUntil(() => isTableDataLoad[6]);
        LoadManager.instance.ProgressTextSet("데이터 불러오는 중(55%)");

        Backend.Chart.GetChartContents(Replay_Data_FileId, callback =>
       {
           if (callback.IsSuccess())
           {
               JsonData row_replay = callback.GetReturnValuetoJSON()["rows"];
               for (int i = Table_Start_Index; i < row_replay.Count; i++)
               {
                   TableData.Replay_On_Level.Add(int.Parse(row_replay[i]["Replay_On_Level"]["S"].ToString()));
                   TableData.Max_Level_Depending_On_RebirthCount.Add(int.Parse(row_replay[i]["Replay_Level"]["S"].ToString()));
               }
               isTableDataLoad[7] = true;
           }
       });
        yield return new WaitUntil(() => isTableDataLoad[7]);
        LoadManager.instance.ProgressTextSet("데이터 불러오는 중(60%)");

        Backend.Chart.GetChartContents(Dungeon_Data_FileId, callback =>
       {
           if (callback.IsSuccess())
           {
               JsonData row_dungeon = callback.GetReturnValuetoJSON()["rows"];
               for (int i = Table_Start_Index; i < row_dungeon.Count; i++)
               {
                   TableData.Dungeon_Boss_HP.Add(double.Parse(row_dungeon[i]["Dungeon_Boss_HP"]["S"].ToString()));
                   TableData.Dungeon_Clear_Plus_Time.Add(int.Parse(row_dungeon[i]["Dungeon_Clear_Plus_Time"]["S"].ToString()));
                   TableData.Boss_Drop_Key_01.Add(int.Parse(row_dungeon[i]["Boss_Drop_Key_01"]["S"].ToString()));
                   TableData.Boss_Drop_Key_02.Add(int.Parse(row_dungeon[i]["Boss_Drop_Key_02"]["S"].ToString()));
                   TableData.Boss_Drop_Key_03.Add(int.Parse(row_dungeon[i]["Boss_Drop_Key_03"]["S"].ToString()));
                   TableData.Boss_Drop_Key_04.Add(int.Parse(row_dungeon[i]["Boss_Drop_Key_04"]["S"].ToString()));
                   TableData.Boss_Drop_Key_05.Add(int.Parse(row_dungeon[i]["Boss_Drop_Key_05"]["S"].ToString()));
                   TableData.Boss_Drop_Key_06.Add(int.Parse(row_dungeon[i]["Boss_Drop_Key_06"]["S"].ToString()));
                   TableData.Boss_Drop_Key_07.Add(int.Parse(row_dungeon[i]["Boss_Drop_Key_07"]["S"].ToString()));

                   TableData.Boss_Drop_Key_01_Per.Add(float.Parse(row_dungeon[i]["Boss_Drop_Key_01_Per"]["S"].ToString()));
                   TableData.Boss_Drop_Key_02_Per.Add(float.Parse(row_dungeon[i]["Boss_Drop_Key_02_Per"]["S"].ToString()));
                   TableData.Boss_Drop_Key_03_Per.Add(float.Parse(row_dungeon[i]["Boss_Drop_Key_03_Per"]["S"].ToString()));
                   TableData.Boss_Drop_Key_04_Per.Add(float.Parse(row_dungeon[i]["Boss_Drop_Key_04_Per"]["S"].ToString()));
                   TableData.Boss_Drop_Key_05_Per.Add(float.Parse(row_dungeon[i]["Boss_Drop_Key_05_Per"]["S"].ToString()));
                   TableData.Boss_Drop_Key_06_Per.Add(float.Parse(row_dungeon[i]["Boss_Drop_Key_06_Per"]["S"].ToString()));
                   TableData.Boss_Drop_Key_07_Per.Add(float.Parse(row_dungeon[i]["Boss_Drop_Key_07_Per"]["S"].ToString()));
               }
               isTableDataLoad[8] = true;
           }
       });
        yield return new WaitUntil(() => isTableDataLoad[8]);
        LoadManager.instance.ProgressTextSet("데이터 불러오는 중(67%)");

        Backend.Chart.GetChartContents(Gatcha_Data_FileId, callback =>
       {
           if (callback.IsSuccess())
           {
               JsonData row_gatcha = callback.GetReturnValuetoJSON()["rows"];
               for (int i = Table_Start_Index; i < row_gatcha.Count; i++)
               {
                   TableData.Gatcha_Type.Add(row_gatcha[i]["Gatcha_Type"]["S"].ToString());
                   TableData.Gatcha_Gift_Type.Add(row_gatcha[i]["Gatcha_Gift_Type"]["S"].ToString());
                   TableData.Gatcha_Gift_Min.Add(long.Parse(row_gatcha[i]["Gatcha_Gift_Min"]["S"].ToString()));
                   TableData.Gatcha_Gift_Max.Add(long.Parse(row_gatcha[i]["Gatcha_Gift_Max"]["S"].ToString()));
                   TableData.Gatcha_Gift_Per.Add(int.Parse(row_gatcha[i]["Gatcha_Gift_Per"]["S"].ToString()));
               }
               for (int i = 0; i < TableData.Gatcha_Type.Count; i++)
               {
                   if (TableData.Gatcha_Type[i].Equals("G01"))
                       BoxManager.instance.NUMBEROFBOXREWARD++;
                   else
                       break;
               }
               isTableDataLoad[9] = true;
           }
       });
        yield return new WaitUntil(() => isTableDataLoad[9]);
        LoadManager.instance.ProgressTextSet("데이터 불러오는 중(73%)");

        Backend.Chart.GetChartContents(Pet_Data_FileId, callback =>
       {
           if (callback.IsSuccess())
           {
               JsonData row_pet = callback.GetReturnValuetoJSON()["rows"];
               for (int i = Table_Start_Index; i < row_pet.Count; i++)
               {
                   TableData.Pet_Type.Add(row_pet[i]["Pet_Type"]["S"].ToString());
                   TableData.Pet_Status_Up_Value.Add(row_pet[i]["Pet_Status_Up_Value"]["S"].ToString());
                   TableData.Pet_Normal_Status.Add(float.Parse(row_pet[i]["Pet_Normal_Status"]["S"].ToString()));
                   TableData.Pet_Status_Upgrade.Add(float.Parse(row_pet[i]["Pet_Status_Upgrade"]["S"].ToString()));

                   TableData.Pet_Status_Upgrade10.Add(float.Parse(row_pet[i]["Pet_Status_Upgrade10"]["S"].ToString()));
                   TableData.Pet_Max_Level.Add(int.Parse(row_pet[i]["Pet_Max_Level"]["S"].ToString()));
               }
               isTableDataLoad[10] = true;
           }
       });
        yield return new WaitUntil(() => isTableDataLoad[10]);
        LoadManager.instance.ProgressTextSet("데이터 불러오는 중(77%)");

        Backend.Chart.GetChartContents(Box_Data_FileId, callback =>
       {
           if (callback.IsSuccess())
           {
               JsonData row_box = callback.GetReturnValuetoJSON()["rows"];
               for (int i = Table_Start_Index; i < row_box.Count; i++)
               {
                   TableData.Box_Name.Add(row_box[i]["Box_Name"]["S"].ToString());
                   TableData.Box_Open_Key.Add(row_box[i]["Box_Open_Key"]["S"].ToString());
                   TableData.Box_Open_Gem.Add(int.Parse(row_box[i]["Box_Open_Gem"]["S"].ToString()));
               }
               isTableDataLoad[11] = true;
           }
       });
        yield return new WaitUntil(() => isTableDataLoad[11]);
        LoadManager.instance.ProgressTextSet("데이터 불러오는 중(80%)");

        Backend.Chart.GetChartContents(Key_Data_FileId, callback =>
       {
           if (callback.IsSuccess())
           {
               JsonData row_key = callback.GetReturnValuetoJSON()["rows"];
               for (int i = Table_Start_Index; i < row_key.Count; i++)
               {
                   TableData.Key_Index.Add(row_key[i]["Key_Index"]["S"].ToString());
                   TableData.Key_Name.Add(row_key[i]["Key_Name"]["S"].ToString());
                   TableData.Key_Open_Gatcha.Add(row_key[i]["Key_Open_Gatcha"]["S"].ToString());
               }
               isTableDataLoad[12] = true;
           }
       });
        yield return new WaitUntil(() => isTableDataLoad[12]);
        LoadManager.instance.ProgressTextSet("데이터 불러오는 중(83%)");

        Backend.Chart.GetChartContents(Shop_Data_FileId, callback =>
       {
           if (callback.IsSuccess())
           {
               JsonData row_shop = callback.GetReturnValuetoJSON()["rows"];
               for (int i = Table_Start_Index; i < row_shop.Count; i++)
               {
                   TableData.Shop_Index.Add(row_shop[i]["Shop_Index"]["S"].ToString());
                   TableData.Shop_Tap.Add(row_shop[i]["Shop_Tap"]["S"].ToString());
                   TableData.Shop_Name.Add(row_shop[i]["Shop_Name"]["S"].ToString());
                   TableData.Shop_Explain.Add(row_shop[i]["Shop_Explain"]["S"].ToString());
                   TableData.Shop_Type.Add(row_shop[i]["Shop_Type"]["S"].ToString());
                   TableData.Shop_Pay_Type.Add(row_shop[i]["Shop_Pay_Type"]["S"].ToString());
                   TableData.Shop_Gift_Count.Add(int.Parse(row_shop[i]["Shop_Gift_Count"]["S"].ToString()));
                   TableData.Shop_Daily.Add(int.Parse(row_shop[i]["Shop_Daily"]["S"].ToString()));
                   TableData.Shop_Price.Add(int.Parse(row_shop[i]["Shop_Price"]["S"].ToString()));
               }
               isTableDataLoad[13] = true;
           }
       });
        yield return new WaitUntil(() => isTableDataLoad[13]);
        LoadManager.instance.ProgressTextSet("데이터 불러오는 중(89%)");

        Backend.Chart.GetChartContents(DailyAttendance_Data_FileId, callback =>
       {
           if (callback.IsSuccess())
           {
               JsonData row_da = callback.GetReturnValuetoJSON()["rows"];

               for (int i = Table_Start_Index; i < row_da.Count; i++)
               {
                   TableData.DailyAttendance_Index.Add(row_da[i]["DailyAttendance_Index"]["S"].ToString());
                   TableData.DailyAttendance_Name.Add(row_da[i]["DailyAttendance_Name"]["S"].ToString());
                   TableData.DailyAttendance_Explain.Add(row_da[i]["DailyAttendance_Explain"]["S"].ToString());
                   TableData.DailyAttendance_Give_Type.Add(row_da[i]["DailyAttendance_Give_Type"]["S"].ToString());
                   TableData.DailyAttendance_Give_Count.Add(int.Parse(row_da[i]["DailyAttendance_Give_Count"]["S"].ToString()));
               }
               isTableDataLoad[14] = true;
           }
       });
        yield return new WaitUntil(() => isTableDataLoad[14]);
        LoadManager.instance.ProgressTextSet("데이터 불러오는 중(93%)");

        Backend.Chart.GetChartContents(Mission_Data_FileId, callback =>
       {
           if (callback.IsSuccess())
           {
               JsonData row_mission = callback.GetReturnValuetoJSON()["rows"];
               for (int i = Table_Start_Index; i < row_mission.Count; i++)
               {
                   TableData.Mission_Name.Add(row_mission[i]["Mission_Name"]["S"].ToString());
                   TableData.Mission_Explain.Add(row_mission[i]["Mission_Explain"]["S"].ToString());
                   TableData.Mission_Group.Add(row_mission[i]["Mission_Group"]["S"].ToString());
                   TableData.Mission_Type01.Add(row_mission[i]["Mission_Type01"]["S"].ToString());
                   TableData.Mission_Type02.Add(row_mission[i]["Mission_Type02"]["S"].ToString());
                   TableData.Mission_Count.Add(float.Parse(row_mission[i]["Mission_Count"]["S"].ToString()));
                   TableData.Mission_Give_Type.Add(row_mission[i]["Mission_Give_Type"]["S"].ToString());
                   TableData.Mission_Give_Count.Add(int.Parse(row_mission[i]["Mission_Give_Count"]["S"].ToString()));
               }
               isTableDataLoad[15] = true;
           }
       });
        //Debug.Log(res_da.GetReturnValue());
        yield return new WaitUntil(() => isTableDataLoad[15]);
        LoadManager.instance.ProgressTextSet("데이터 불러오는 중(98%)");

        isLoadDone = true;
        if (isNewUser.Equals(false))
        {
            StartCoroutine(LoadManager.instance.LoadData());
        }
    }

    private void FileIdSetting(string chartName, string id)
    {
        switch (chartName)
        {
            case "Status_Up_Data":
                Status_Up_Data_FileId = id;
                break;

            case "Status_Data":
                Status_Data_FileId = id;
                break;

            case "Stage_Boss_Data":
                Stage_Boss_Data_FileId = id;
                break;

            case "Stage_Normal_Data":
                Stage_Normal_Data_FileId = id;
                break;

            case "Level_Data":
                Level_Data_FileId = id;
                break;

            case "Upgrade_Data":
                Upgrade_Data_FileId = id;
                break;

            case "Item_Data":
                Item_Data_FileId = id;
                break;

            case "Replay_Data":
                Replay_Data_FileId = id;
                break;

            case "Dungeon_Data":
                Dungeon_Data_FileId = id;
                break;

            case "Gatcha_Data":
                Gatcha_Data_FileId = id;
                break;

            case "Pet_Data":
                Pet_Data_FileId = id;
                break;

            case "Box_Data":
                Box_Data_FileId = id;
                break;

            case "Key_Data":
                Key_Data_FileId = id;
                break;

            case "Shop_Data":
                Shop_Data_FileId = id;
                break;

            case "DailyAttendance_Data":
                DailyAttendance_Data_FileId = id;
                break;

            case "Mission_Data":
                Mission_Data_FileId = id;
                break;

            default:
                break;
        }
    }
}