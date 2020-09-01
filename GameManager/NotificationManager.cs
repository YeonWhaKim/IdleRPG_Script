using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager instance;
    public readonly int NOTTITLESTRINGLIMITLENGTH = 15;
    public GameObject notificationPopUp;
    public GameObject notNotificationPopUp;

    [Header("최근 공지사항")]
    public GameObject contentsScrollViewGO;

    public RectTransform contentRT;
    public Text latestTitleText;
    public Text latestDateText;
    public Text latestContentsText;
    public GameObject goToWebPageGO;
    private string webPageUrl;

    [Header("공지사항 목록")]
    public GameObject listScrollViewGO;

    public Transform listContent;
    public GameObject listObjectPrefab;

    public GameObject beforeListButtonGO;
    public GameObject latestButtonGO;
    public Toggle dontOpenDuringTheDayToggle;
    public bool isTheDayClose;
    public bool isLoadDone;
    private bool isStarttmp;
    private List<BackendReturnObject> loadNot = new List<BackendReturnObject>();

    private void Awake()
    {
        isLoadDone = false;
        instance = this;
    }

    public void LoadNotification(bool isStart)
    {
        if (loadNot.Count > 0)
            loadNot.Clear();

        var i = 0;
        string offset = "";

        loadNot.Add(Backend.Notice.NoticeList());
        if (loadNot[i].IsSuccess())
        {
            try
            {
                offset = loadNot[i].LastEvaluatedKeyString();
                if (!string.IsNullOrEmpty(offset))
                    SendQueue.Enqueue(Backend.Notice.NoticeList, 10, offset, callback =>
                      {
                          loadNot.Add(callback);
                      });

                OnClickLatestButton();
                SettingBeforeList();
                isStarttmp = isStart;
                if (isStart)
                    dontOpenDuringTheDayToggle.gameObject.SetActive(true);
                else
                    dontOpenDuringTheDayToggle.gameObject.SetActive(false);
                notificationPopUp.SetActive(true);
            }
            catch (System.Exception)
            {
                if (!isStart)
                    notNotificationPopUp.SetActive(true);
            }

            //catch (System.Exception)
            //{
            //
            //}

            isLoadDone = true;
        }
    }

    private void SettingContents(int listIndex, int jsonDataIndex)
    {
        var latestContents = loadNot[listIndex].GetReturnValuetoJSON()["rows"][jsonDataIndex];

        latestTitleText.text = latestContents["title"][0].ToString();
        var str = latestContents["postingDate"][0].ToString().Split('T');
        latestDateText.text = str[0];
        latestContentsText.text = latestContents["content"][0].ToString();

        contentRT.sizeDelta = new Vector2(0, latestContentsText.preferredHeight + latestTitleText.preferredHeight + 100);

        var dic = latestContents as IDictionary;

        if (!dic.Contains("linkUrl"))
            goToWebPageGO.SetActive(false);
        else
        {
            webPageUrl = latestContents["linkUrl"][0].ToString();
            contentRT.sizeDelta = new Vector2(0, contentRT.rect.height + 150);
            goToWebPageGO.SetActive(true);
        }

        contentRT.anchoredPosition = Vector2.zero;

        contentsScrollViewGO.SetActive(true);
        listScrollViewGO.SetActive(false);
        beforeListButtonGO.SetActive(true);
        latestButtonGO.SetActive(false);
    }

    public void SettingContents_pick(int index)
    {
        SettingContents(index / 10, index % 10);
    }

    public void SettingBeforeList()
    {
        for (int i = 0; i < loadNot.Count; i++)
        {
            for (int k = 0; k < 10; k++)
            {
                var child = listContent.GetChild(i * 10 + k);
                try
                {
                    var cont = loadNot[i].GetReturnValuetoJSON()["rows"][k];
                    var titleText = cont["title"][0].ToString();
                    if (titleText.Length > NOTTITLESTRINGLIMITLENGTH)
                    {
                        titleText = titleText.Substring(0, NOTTITLESTRINGLIMITLENGTH);
                        child.GetChild(0).GetComponent<Text>().text = string.Format("{0} ...", titleText);
                    }
                    else
                        child.GetChild(0).GetComponent<Text>().text = string.Format(titleText);
                    var str = cont["postingDate"][0].ToString().Split('T');
                    child.GetChild(1).GetComponent<Text>().text = str[0];
                    child.gameObject.SetActive(true);
                }
                catch (System.Exception)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
        if (loadNot.Count <= 1)
        {
            for (int i = 0; i < 10; i++)
            {
                listContent.GetChild(1 * 10 + i).gameObject.SetActive(false);
            }
        }
    }

    public void OnClickGoToWebPage()
    {
        Application.OpenURL(webPageUrl);
    }

    public void OnClickBeforeListButton()
    {
        contentsScrollViewGO.SetActive(false);
        listScrollViewGO.SetActive(true);
        beforeListButtonGO.SetActive(false);
        latestButtonGO.SetActive(true);
    }

    public void OnClickLatestButton()
    {
        SettingContents_pick(0);
    }

    public void OnClickConfirm()
    {
        if (isStarttmp)
            isTheDayClose = dontOpenDuringTheDayToggle.isOn;
        notificationPopUp.SetActive(false);
    }
}