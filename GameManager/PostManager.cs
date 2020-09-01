using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PostManager : MonoBehaviour
{
    public const int POSTTITLELENGTH = 10;
    public List<string> itemNameList = new List<string>();
    public List<string> itemCountList = new List<string>();
    public List<string> indateList = new List<string>();
    public GameObject postPopUp;
    public GameObject chkPoint;
    public Sprite[] rewardIamgeSprites;

    public Transform listContent;
    private BackendReturnObject bro;
    private bool isPostSetting = false;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => LoadManager.instance.isDataLoaded);
        yield return new WaitUntil(() => BackendAsyncClass.backendAsyncQueueState == BackendAsyncClass.BackendAsyncQueueState.Running);
        isPostSetting = false;
        StartCoroutine(ReceiveAdminPostAll());
    }

    public IEnumerator ReceiveAdminPostAll()
    {
        BackendAsyncClass.BackendAsyncEnqueue(Backend.Social.Post.GetPostListV2, callback =>
        {
            bro = callback;
            isPostSetting = true;
        });
        yield return new WaitUntil(() => isPostSetting);
        //bro = Backend.Social.Post.GetPostListV2();

        for (int i = 0; i < listContent.childCount; i++)
        {
            try
            {
                var json = bro.GetReturnValuetoJSON()["fromAdmin"][i];
                var title = json["title"][0].ToString();
                var itemName = json["item"][0]["name"][0].ToString();
                var itemCount = json["itemCount"][0].ToString();
                var expirationDate = json["expirationDate"][0].ToString();
                var inDate = json["inDate"][0].ToString();

                // UI 세팅하면서
                //(이미지, 제목, 날짜, 버튼 Listener)
                // 아이콘 설정
                var child = listContent.GetChild(i);
                child.GetChild(0).GetChild(0).GetComponent<Image>().sprite = IconSetting(itemName);
                child.GetChild(0).GetChild(0).GetComponent<Image>().SetNativeSize();
                child.GetChild(0).GetChild(1).GetComponent<Text>().text = itemCount;

                // 제목 설정
                if (title.Length > POSTTITLELENGTH)
                {
                    title = title.Substring(0, POSTTITLELENGTH);
                    child.GetChild(1).GetComponent<Text>().text = string.Format("{0} ...", title);
                }
                else
                    child.GetChild(1).GetComponent<Text>().text = string.Format(title);

                // 만기 날짜 설정
                var str = expirationDate.Split('T');
                child.GetChild(2).GetComponent<Text>().text = string.Format("삭제 예정일 : <color=#67D3BB>{0}</color>", str[0]);

                child.gameObject.SetActive(true);

                // 딕셔너리에 넣기
                itemNameList.Add(itemName);
                itemCountList.Add(itemCount);
                indateList.Add(inDate);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
                break;
            }
        }
        ChkPostCount();
    }

    private Sprite IconSetting(string itemName)
    {
        Sprite returnsp = rewardIamgeSprites[0];
        switch (itemName)
        {
            case "token":
                returnsp = rewardIamgeSprites[2];
                break;

            case "jewel":
                returnsp = rewardIamgeSprites[1];
                break;

            default:
                break;
        }
        return returnsp;
    }

    private void ItemUp(string itmeName, long upValue)
    {
        switch (itmeName)
        {
            case "token":
                CurrencyManager.instance.Token += upValue;
                break;

            case "jewel":
                CurrencyManager.instance.Jewel += upValue;
                break;
        }
    }

    private void ChkPostCount()
    {
        for (int i = 0; i < listContent.childCount; i++)
        {
            if (listContent.GetChild(i).gameObject.activeSelf)
            {
                chkPoint.SetActive(true);
                break;
            }
            else
                chkPoint.SetActive(false);
        }
    }

    public void OnClickPostButton()
    {
        postPopUp.SetActive(true);
    }

    public void OnClickGetReward(int index)
    {
        //아이템 UP
        ItemUp(itemNameList[index], long.Parse(itemCountList[index]));
        //수령할 때 여기서 쏴주기(관리자 보상 수령)
        BackendAsyncClass.BackendAsyncEnqueue(Backend.Social.Post.ReceiveAdminPostItemV2, indateList[index], callback => { Debug.Log(callback); });

        //Backend.Social.Post.ReceiveAdminPostItemV2(indateList[index]);
        //해당 오브젝트 false
        listContent.GetChild(index).gameObject.SetActive(false);
        ChkPostCount();
    }
}