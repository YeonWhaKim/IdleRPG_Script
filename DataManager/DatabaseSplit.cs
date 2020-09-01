using System.Collections.Generic;
using UnityEngine;

public class DatabaseSplit : MonoBehaviour
{
    public static DatabaseSplit instance;
    private char[] enter = new char[] { '\n' };
    private static List<AvatarInfoFrame> avatar_Weapon = new List<AvatarInfoFrame>();
    private static List<AvatarInfoFrame> avatar_Head = new List<AvatarInfoFrame>();
    private static List<AvatarInfoFrame> avatar_Chest = new List<AvatarInfoFrame>();
    private static List<AvatarInfoFrame> avatar_Gloves = new List<AvatarInfoFrame>();
    private static List<AvatarInfoFrame> avatar_Pants = new List<AvatarInfoFrame>();
    private static List<AvatarInfoFrame> avatar_Shoes = new List<AvatarInfoFrame>();
    private static List<AvatarInfoFrame> avatar_Back = new List<AvatarInfoFrame>();

    public static List<AvatarInfoFrame> tmpAvatarInfo = new List<AvatarInfoFrame>();

    // Start is called before the first frame update
    private void Start()
    {
        instance = this;

        AvatarInfoDataSplit_Weapon();
        AvatarInfoDataSplit_Head();
        AvatarInfoDataSplit_Chest();
        AvatarInfoDataSplit_Gloves();
        AvatarInfoDataSplit_Pants();
        AvatarInfoDataSplit_Shoes();
        AvatarInfoDataSplit_Back();
    }

    private void AvatarInfoDataSplit_Weapon()
    {
        var str = Resources.Load<TextAsset>("AvatarInfo - Weapon");
        var removeR = str.text.Replace("\r", string.Empty);
        var strSplit = removeR.Split(enter, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (var item in strSplit)
        {
            var lastStr = item.Split(',');
            var newData = new AvatarInfoFrame(lastStr[0], lastStr[1], lastStr[2], lastStr[3], lastStr[4], int.Parse(lastStr[5]));
            avatar_Weapon.Add(newData);
        }
    }

    private void AvatarInfoDataSplit_Head()
    {
        var str = Resources.Load<TextAsset>("AvatarInfo - Head");
        var removeR = str.text.Replace("\r", string.Empty);
        var strSplit = removeR.Split(enter, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (var item in strSplit)
        {
            var lastStr = item.Split(',');
            var newData = new AvatarInfoFrame(lastStr[0], lastStr[1], lastStr[2], lastStr[3], lastStr[4], int.Parse(lastStr[5]));
            avatar_Head.Add(newData);
        }
    }

    private void AvatarInfoDataSplit_Chest()
    {
        var str = Resources.Load<TextAsset>("AvatarInfo - Chest");
        var removeR = str.text.Replace("\r", string.Empty);
        var strSplit = removeR.Split(enter, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (var item in strSplit)
        {
            var lastStr = item.Split(',');
            var newData = new AvatarInfoFrame(lastStr[0], lastStr[1], lastStr[2], lastStr[3], lastStr[4], int.Parse(lastStr[5]));
            avatar_Chest.Add(newData);
        }
    }

    private void AvatarInfoDataSplit_Gloves()
    {
        var str = Resources.Load<TextAsset>("AvatarInfo - Gloves");
        var removeR = str.text.Replace("\r", string.Empty);
        var strSplit = removeR.Split(enter, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (var item in strSplit)
        {
            var lastStr = item.Split(',');
            var newData = new AvatarInfoFrame(lastStr[0], lastStr[1], lastStr[2], lastStr[3], lastStr[4], int.Parse(lastStr[5]));
            avatar_Gloves.Add(newData);
        }
    }

    private void AvatarInfoDataSplit_Pants()
    {
        var str = Resources.Load<TextAsset>("AvatarInfo - Pants");
        var removeR = str.text.Replace("\r", string.Empty);
        var strSplit = removeR.Split(enter, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (var item in strSplit)
        {
            var lastStr = item.Split(',');
            var newData = new AvatarInfoFrame(lastStr[0], lastStr[1], lastStr[2], lastStr[3], lastStr[4], int.Parse(lastStr[5]));
            avatar_Pants.Add(newData);
        }
    }

    private void AvatarInfoDataSplit_Shoes()
    {
        var str = Resources.Load<TextAsset>("AvatarInfo - Shoes");
        var removeR = str.text.Replace("\r", string.Empty);
        var strSplit = removeR.Split(enter, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (var item in strSplit)
        {
            var lastStr = item.Split(',');
            var newData = new AvatarInfoFrame(lastStr[0], lastStr[1], lastStr[2], lastStr[3], lastStr[4], int.Parse(lastStr[5]));
            avatar_Shoes.Add(newData);
        }
    }

    private void AvatarInfoDataSplit_Back()
    {
        var str = Resources.Load<TextAsset>("AvatarInfo - Back");
        var removeR = str.text.Replace("\r", string.Empty);
        var strSplit = removeR.Split(enter, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (var item in strSplit)
        {
            var lastStr = item.Split(',');
            var newData = new AvatarInfoFrame(lastStr[0], lastStr[1], lastStr[2], lastStr[3], lastStr[4], int.Parse(lastStr[5]));
            avatar_Back.Add(newData);
        }
    }

    public List<AvatarInfoFrame> ReturnAvatarInfoList(int index)
    {
        switch (index)
        {
            case 0:
                tmpAvatarInfo = avatar_Weapon;
                break;

            case 1:
                tmpAvatarInfo = avatar_Head;
                break;

            case 2:
                tmpAvatarInfo = avatar_Chest;
                break;

            case 3:
                tmpAvatarInfo = avatar_Gloves;
                break;

            case 4:
                tmpAvatarInfo = avatar_Pants;
                break;

            case 5:
                tmpAvatarInfo = avatar_Shoes;
                break;

            case 6:
                tmpAvatarInfo = avatar_Back;
                break;
        }
        return tmpAvatarInfo;
    }
}