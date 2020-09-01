using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static MessageManager;

public class MessageMenu : MonoBehaviour
{
    public static MessageMenu instance;

    [Header("채팅창")]
    public Button[] channelBtns;

    public Text[] channelTexts;
    public GameObject chatList;
    public GameObject baseChatFormat;
    public InputField inputField;
    public Text placeHolder;
    public Button sendBtn;
    private int[] userCounts = { 0, 0, 0, 0, 0 };

    [Header("유저목록")]
    public GameObject userListUI;

    public GameObject userList;
    public GameObject baseNameFormat;
    public Button[] userbtns;

    [Header("Etc")]
    public GameObject cmdUI;

    public GameObject blockedCmdUI;

    private const string CHAT_NICK = "[{0}]";
    private string clickNickName = "";
    private char[] comma = { ',' };

    private Color32 infoTextColor = new Color32(246, 94, 94, 255);
    private Color32 whisperColor = new Color32(144, 151, 255, 255);
    private Color32 warningColor = new Color32(255, 0, 0, 255);
    private Color32 notiColor = new Color32(110, 227, 200, 255);
    private const int LIMIT_NUM_CHAT = 100;

    // Start is called before the first frame update
    private void Start()
    {
        instance = this;
        InputSetting(false);
    }

    private void OnApplicationQuit()
    {
        // 게임 종료 시 객체 삭제
        ClearChat();
        ClearUserList();
    }

    #region 채팅관련 UI

    /*
     * 채팅 UI 열기/닫기
     * InputField에 있는 텍스트를 MessageManager로 전달
     * 클라이언트에 저장된 모든 채팅기록 삭제
     * cmd 패널 열기/닫기
     * 유저 리스트 UI 열기/닫기
     * 유저 신고
     */

    //채팅 UI 표시
    public void ShowChatUI()
    {
        //ShowUI(chatUI);
        // 채팅창을 켤 때 위치를 최하단으로 초기화
        //채팅 UI에 포함된 하위 UI 모두 닫기
        //InputSetting(false);
        CloseUserList();
        CloseCmdPanal();
        CloseBlockedCmdPanal();
    }

    // 채팅 UI 닫기
    public void CloseChatUI()
    {
        // 채팅 UI에 포함된 하위 UI 포함 모든 채팅 UI 닫기
        CloseUserList();
        CloseCmdPanal();
        CloseBlockedCmdPanal();

        //CloseUI(chatUI);
    }

    // 채널 버튼 세팅
    public void ChannelBtnSetting(int index, int userCount)
    {
        channelBtns[index].gameObject.SetActive(true);
        channelTexts[index].text = string.Format("채널{0}\n<size=15>({1}/100)</size>", index + 1, userCount);
        userCounts[index] = userCount;

        if (userCounts[index] + 1 < 100)
            channelBtns[index].interactable = true;
        else
            channelBtns[index].interactable = false;
    }

    // InputField 세팅
    private void InputSetting(bool setActive)
    {
        inputField.interactable = setActive;
        sendBtn.interactable = setActive;
        if (setActive == false)
            placeHolder.text = "채널을 선택해주세요.";
        else
            placeHolder.text = "채팅 입력";
    }

    // 채널 입장
    public void OnClickChannelBtn(int index)
    {
        //MessageManager.instance.JoinChannel(index, MessageManager.instance.channelList[index]);
        //channelTexts[index].text = string.Format("채널{0}\n<size=15>({1}/100)</size>", index + 1, userCounts[index] + 1);
        InputSetting(true);
    }

    // 서버에서 수신된 메시지를 UI에 추가
    public void AddChat(ChatItem chatItem, bool isError = false, bool isNotification = false)
    {
        // 총 메시지 갯수 체크
        CheckNumOfChat();

        GameObject newMsg = Instantiate(baseChatFormat, chatList.transform);

        Text text = newMsg.GetComponent<Text>();
        text.text = string.Format(CHAT_NICK + " {1}", chatItem.Nickname, chatItem.Contents);
        //texts[1].text = chatItem.Contents;

        // 다른 유저에게 온 메시지 인 경우
        if (chatItem.session != BackEnd.Tcp.SessionInfo.None)
        {
            Button nickButton = newMsg.GetComponentInChildren<Button>();
            nickButton.onClick.AddListener(delegate
            {
                // 닉네임에 버튼이벤트 동적으로 연결
                // 닉네임을 누르면 cmd 패널 오픈
                //OpenCmdPanal();
            });
        }
        // 시스템 메시지인 경우
        else if (!chatItem.isWhisper)
            text.color = infoTextColor;

        // 귓속말인 경우
        if (chatItem.isWhisper)
            text.color = whisperColor;

        // 에러일 경우
        if (isError)
            text.color = warningColor;

        if (isNotification)
            text.color = notiColor;
    }

    // 전송 버튼을 누르면 MessageManager의 ChatToChannel 호출해 inputField 내에 있는 메시지를 보냄
    public void SendChatMsgToChannel()
    {
        if (inputField.text == "")
        {
            return;
        }

        MessageManager.instance.ChatToChannel(inputField.text);
        inputField.text = "";
    }

    // 모든 채팅기록 초기화
    public void ClearChat()
    {
        int numOfChat = chatList.transform.childCount;
        for (int i = 0; i < numOfChat; ++i)
        {
            Destroy(chatList.transform.GetChild(i).gameObject);
        }
        chatList.transform.DetachChildren();
    }

    // 채팅 리스트 내 메시지 갯수 체크
    public void CheckNumOfChat()
    {
        // LIMIT_NUM_CHAT 이상의 메시지가 쌓이면 위에서부터 삭제
        int numOfChat = chatList.transform.childCount;
        if (numOfChat >= LIMIT_NUM_CHAT)
        {
            GameObject deleteMsg = chatList.transform.GetChild(0).gameObject;
            deleteMsg.transform.SetParent(null);
            // 제일 위 객체를 삭제
            Destroy(deleteMsg);
        }
    }

    // CMD 패널 UI 열기
    public void OpenCmdPanal()
    {
        // 닉네임 설정 (괄호를 지우기)
        GameObject nowObject = EventSystem.current.currentSelectedGameObject;
        string message = nowObject.GetComponentsInChildren<Text>()[0].text;
        var nickname = message.Split(']');
        var realNickName = "";
        realNickName = nickname[0].Replace("[", "");
        clickNickName = realNickName;

        if (SaveManager.instance.nickName == clickNickName)
            return;

        cmdUI.SetActive(true);
        Text userName = cmdUI.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        userName.text = realNickName;
    }

    // BlockedCMD 패널 UI 열기
    public void OpenBlockedCmdPanal()
    {
        // 닉네임 설정 (괄호를 지우기)
        GameObject nowObject = EventSystem.current.currentSelectedGameObject;
        string message = nowObject.GetComponentsInChildren<Text>()[0].text;
        var nickname = message.Split(']');
        var realNickName = "";
        realNickName = nickname[0].Replace("[", "");
        clickNickName = realNickName;

        if (SaveManager.instance.nickName == clickNickName)
            return;

        blockedCmdUI.SetActive(true);
    }

    // CMD UI 닫기
    public void CloseCmdPanal()
    {
        cmdUI.SetActive(false);
    }

    // CMD UI 닫기
    public void CloseBlockedCmdPanal()
    {
        blockedCmdUI.SetActive(false);
    }

    // 귓속말 커맨드 입력 (/w nickname)
    public void WhisperButtonClick()
    {
        inputField.text = "/w " + clickNickName + " ";
        CloseCmdPanal();
        CloseBlockedCmdPanal();
        CloseUserList();
    }

    // 차단 커맨드 사용 (/b nickname)
    public void BlockButtonClick()
    {
        inputField.text = "/b " + clickNickName;
        CloseCmdPanal();
        CloseBlockedCmdPanal();
        CloseUserList();
        SendChatMsgToChannel();
    }

    // 차단 해제 커맨드 사용 (/ub nickName)
    public void UnBlockButtonClick()
    {
        inputField.text = "/ub " + clickNickName;
        CloseCmdPanal();
        CloseBlockedCmdPanal();

        CloseUserList();
        SendChatMsgToChannel();
    }

    // 모든 유저목록 초기화
    public void ClearUserList()
    {
        int numOfUser = userList.transform.childCount;
        for (int i = 0; i < numOfUser; ++i)
        {
            Destroy(userList.transform.GetChild(i).gameObject);
        }
        userList.transform.DetachChildren();
    }

    // 유저목록 띄우기
    public void ShowUserList(List<string> list)
    {
        CloseCmdPanal();
        CloseBlockedCmdPanal();

        userListUI.SetActive(true);
        ClearUserList();

        foreach (string user in list)
        {
            GameObject newUser = Instantiate(baseNameFormat, userList.transform);
            Text text = newUser.GetComponent<Text>();
            text.text = user;

            Button nickButton = newUser.GetComponentInChildren<Button>();
            nickButton.onClick.AddListener(delegate
            {
                // 닉네임에 버튼이벤트 동적으로 연결
                // cmd 패널 오픈
                OpenCmdPanal();
            });
        }
    }

    // 차단목록 띄우기
    public void ShowBlockedUserList()
    {
        string blockedUserList = PlayerPrefs.GetString("BlockedUserList");
        var strSplit = blockedUserList.Split(comma, System.StringSplitOptions.RemoveEmptyEntries);

        CloseCmdPanal();
        CloseBlockedCmdPanal();

        userListUI.SetActive(true);
        ClearUserList();

        foreach (string user in strSplit)
        {
            GameObject newUser = Instantiate(baseNameFormat, userList.transform);
            Text text = newUser.GetComponent<Text>();
            text.text = user;

            Button nickButton = newUser.GetComponentInChildren<Button>();
            nickButton.onClick.AddListener(delegate
            {
                // 닉네임에 버튼이벤트 동적으로 연결
                // cmd 패널 오픈
                OpenBlockedCmdPanal();
            });
        }
    }

    // 유저목록 UI 닫기
    public void CloseUserList()
    {
        userListUI.SetActive(false);
        cmdUI.SetActive(false);
    }

    // 유저목록 UI가 표시되어 있는니 체크
    public bool IsOpenUserList()
    {
        return userListUI.activeSelf;
    }

    public void OnClickUserList()
    {
        MessageManager.instance.ShowAllUserListInChannel();
    }

    public void OnClickBlockedUserList()
    {
        userbtns[0].interactable = true;
        userbtns[1].interactable = false;
        ShowBlockedUserList();
    }

    #endregion 채팅관련 UI
}