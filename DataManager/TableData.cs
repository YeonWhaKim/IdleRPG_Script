using System.Collections.Generic;
using UnityEngine;

public class TableData : MonoBehaviour
{
    [Header("Level_Data")]
    public static List<long> Level_Exp = new List<long>();

    public static List<int> Level_Random_Status = new List<int>();

    [Header("Stage_Normal_Data")]
    public static List<long> Stage_Normal_Monster_Hp = new List<long>();

    public static List<long> Stage_Normal_Clear_Hp_Plus = new List<long>();
    public static List<float> Stage_Normal_Monster_Critical_Def = new List<float>();
    public static List<decimal> Stage_Normal_Clear_Gold_Min = new List<decimal>();
    public static List<decimal> Stage_Normal_Clear_Gold_Max = new List<decimal>();
    public static List<long> Stage_Normal_Clear_Exp = new List<long>();

    [Header("Stage_Boss_Data")]
    public static List<long> Stage_Boss_Monster_Hp = new List<long>();

    public static List<float> Stage_Boss_Monster_Critical_Def = new List<float>();
    public static List<decimal> Stage_Boss_Clear_Gold_Min = new List<decimal>();
    public static List<decimal> Stage_Boss_Clear_Gold_Max = new List<decimal>();
    public static List<long> Stage_Boss_Clear_Exp = new List<long>();
    public static List<int> Boss_Drop_Box_01 = new List<int>();
    public static List<int> Boss_Drop_Box_02 = new List<int>();
    public static List<int> Boss_Drop_Box_03 = new List<int>();
    public static List<int> Boss_Drop_Box_04 = new List<int>();
    public static List<int> Boss_Drop_Box_05 = new List<int>();
    public static List<int> Boss_Drop_Box_06 = new List<int>();
    public static List<int> Boss_Drop_Box_07 = new List<int>();
    public static List<float> Boss_Drop_Box_01_Per = new List<float>();
    public static List<float> Boss_Drop_Box_02_Per = new List<float>();
    public static List<float> Boss_Drop_Box_03_Per = new List<float>();
    public static List<float> Boss_Drop_Box_04_Per = new List<float>();
    public static List<float> Boss_Drop_Box_05_Per = new List<float>();
    public static List<float> Boss_Drop_Box_06_Per = new List<float>();
    public static List<float> Boss_Drop_Box_07_Per = new List<float>();

    [Header("Status_Data")]
    public static List<long> Status_Min = new List<long>();

    public static List<long> Status_Max = new List<long>();
    public static List<int> Status_Up_Lp = new List<int>();

    [Header("Status_Up_Data")]
    public static List<string> Status_Up_Type = new List<string>();

    public static List<string> Status_Up_Value = new List<string>();
    public static List<float> Status_Up = new List<float>();
    public static List<int> Status_ComatPoint = new List<int>();

    [Header("Item_Data")]
    public static List<string> Item_Name = new List<string>();

    public static List<string> Item_Grade = new List<string>();
    public static List<string> Item_Status = new List<string>();
    public static List<decimal> Item_Status_First = new List<decimal>();
    public static List<decimal> Item_Status_Upgrade = new List<decimal>();
    public static List<string> Item_Open_Type = new List<string>();
    public static List<int> Item_Open_Value = new List<int>();
    public static List<int> Gem_Open_Value = new List<int>();
    public static List<string> Item_Buy_Type = new List<string>();
    public static List<int> Item_Buy_Value = new List<int>();
    public static List<int> Item_Max_Level = new List<int>();
    public static List<string> Item_Look_Number = new List<string>();

    [Header("Upgrade_Data")]
    public static List<decimal> Upgrade_F_Gold = new List<decimal>();

    public static List<decimal> Upgrade_E_Gold = new List<decimal>();
    public static List<decimal> Upgrade_D_Gold = new List<decimal>();
    public static List<decimal> Upgrade_C_Gold = new List<decimal>();
    public static List<decimal> Upgrade_B_Gold = new List<decimal>();
    public static List<decimal> Upgrade_A_Gold = new List<decimal>();
    public static List<decimal> Upgrade_S_Gold = new List<decimal>();
    public static List<double> Minipin_Jewel = new List<double>();
    public static List<double> Poodle_Jewel = new List<double>();
    public static List<double> Cat_Jewel = new List<double>();

    [Header("Replay_Data")]
    public static List<int> Replay_On_Level = new List<int>();

    public static List<int> Max_Level_Depending_On_RebirthCount = new List<int>();

    [Header("Dungeon_Data")]
    public static List<double> Dungeon_Boss_HP = new List<double>();

    public static List<int> Dungeon_Clear_Plus_Time = new List<int>();
    public static List<int> Boss_Drop_Key_01 = new List<int>();
    public static List<int> Boss_Drop_Key_02 = new List<int>();
    public static List<int> Boss_Drop_Key_03 = new List<int>();
    public static List<int> Boss_Drop_Key_04 = new List<int>();
    public static List<int> Boss_Drop_Key_05 = new List<int>();
    public static List<int> Boss_Drop_Key_06 = new List<int>();
    public static List<int> Boss_Drop_Key_07 = new List<int>();

    public static List<float> Boss_Drop_Key_01_Per = new List<float>();
    public static List<float> Boss_Drop_Key_02_Per = new List<float>();
    public static List<float> Boss_Drop_Key_03_Per = new List<float>();
    public static List<float> Boss_Drop_Key_04_Per = new List<float>();
    public static List<float> Boss_Drop_Key_05_Per = new List<float>();
    public static List<float> Boss_Drop_Key_06_Per = new List<float>();
    public static List<float> Boss_Drop_Key_07_Per = new List<float>();

    [Header("Gatcha_Data")]
    public static List<string> Gatcha_Type = new List<string>();

    public static List<string> Gatcha_Gift_Type = new List<string>();
    public static List<long> Gatcha_Gift_Min = new List<long>();
    public static List<long> Gatcha_Gift_Max = new List<long>();
    public static List<int> Gatcha_Gift_Per = new List<int>();

    [Header("Pet_Data")]
    public static List<string> Pet_Type = new List<string>();

    public static List<string> Pet_Status_Up_Value = new List<string>();
    public static List<float> Pet_Normal_Status = new List<float>();
    public static List<float> Pet_Status_Upgrade = new List<float>();
    public static List<float> Pet_Status_Upgrade10 = new List<float>();
    public static List<int> Pet_Max_Level = new List<int>();

    //Box_Data
    public static List<string> Box_Name = new List<string>();

    public static List<string> Box_Open_Key = new List<string>();
    public static List<int> Box_Open_Gem = new List<int>();

    //Key_Data
    public static List<string> Key_Index = new List<string>();

    public static List<string> Key_Name = new List<string>();
    public static List<string> Key_Open_Gatcha = new List<string>();

    //Shop_Data
    public static List<string> Shop_Index = new List<string>();

    public static List<string> Shop_Tap = new List<string>();
    public static List<string> Shop_Name = new List<string>();
    public static List<string> Shop_Explain = new List<string>();
    public static List<string> Shop_Type = new List<string>();
    public static List<int> Shop_Gift_Count = new List<int>();
    public static List<string> Shop_Pay_Type = new List<string>();
    public static List<int> Shop_Daily = new List<int>();
    public static List<int> Shop_Price = new List<int>();

    //DailyAttendance_Data
    public static List<string> DailyAttendance_Index = new List<string>();

    public static List<string> DailyAttendance_Name = new List<string>();
    public static List<string> DailyAttendance_Explain = new List<string>();
    public static List<string> DailyAttendance_Give_Type = new List<string>();
    public static List<int> DailyAttendance_Give_Count = new List<int>();

    //Mission_Data
    public static List<string> Mission_Name = new List<string>();

    public static List<string> Mission_Explain = new List<string>();
    public static List<string> Mission_Group = new List<string>();
    public static List<string> Mission_Type01 = new List<string>();
    public static List<string> Mission_Type02 = new List<string>();
    public static List<float> Mission_Count = new List<float>();
    public static List<string> Mission_Give_Type = new List<string>();
    public static List<int> Mission_Give_Count = new List<int>();
}