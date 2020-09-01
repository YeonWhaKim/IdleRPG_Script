// Include Backend
using BackEnd;
using BackEnd.Tcp;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    public static MessageManager instance;

    private bool chatStatus;
    private bool returnChannelList;

    public bool message_allSetting = false;
    public bool onEnterChannel = false;
    //private bool reply_chat;
    //private bool reply_channel;
    //private bool enterChatServer;

    private string myNickName;
    private ChannelType channelType;
    public List<ChannelNodeObject> channelList;
    private List<string> participantList;
    private ChatItem chatItem;
    private bool isFilterOn;
    public Text channelUserCountText;
    private int channelUserCount;

    private const int maxLength = 50;
    private const string notiText = "운영자 공지";
    private const string infoText = "안내";
    private const string CHAT_INACTIVE = "채팅서버에 접속할 수 없습니다. 잠시 후 다시 시도해주세요.";
    private const string CHAT_ENTER = "{0} 채널에 입장하였습니다."; // '채널 별칭' 채널에 입장하였습니다.
    private const string CHAT_MSGLENGTHEXCEED = "입력 제한 글자수를 초과하였습니다.";
    private const string CHAT_UNKNOWNCMD = "잘못된 명령어입니다.";
    private const string CHAT_NOTARGET = "대상을 입력해주세요.";
    private const string CHAT_UNVAILDTARGET = "잘못된 대상입니다.";
    private const string CHAT_BLOCKFAIL = "존재하지 않는 닉네임입니다.";
    private const string CHAT_BLOCKSUCCESS = "{0}님을 차단합니다.";
    private const string CHAT_UNBLOCKSUCCESS = "{0}님을 차단 해제합니다.";
    private const string CHAT_REPORT = "{0}님을 신고하였습니다.";
    private const string CHAT_NOMSG = "귓속말을 입력해주세요.";
    private const string CHAT_WHISPER = "{0}에게 귓속말 : {1}";

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        //ChatHandlers();
    }

    private void Start()
    {
        chatItem = null;
        chatStatus = false;
        returnChannelList = false;
        //allSetting = false;
        //reply_chat = false;
        //reply_channel = false;
        //enterChatServer = false;
        isFilterOn = true;
        channelType = ChannelType.Public;
        channelList = new List<ChannelNodeObject>();
        participantList = new List<string>();
        StartCoroutine(UpdateCO());
        //ChatHandlers();
    }

    private IEnumerator UpdateCO()
    {
        yield return new WaitUntil(() => chatStatus);
        while (true)
        {
            yield return GameManager.YieldInstructionCache.WaitForFixedUpdate;

            Backend.Chat.Poll();
        }
    }

    private void OnApplicationQuit()
    {
        // 게임 종료 시 채팅 채널 접속 해제
        LeaveChatServer();
    }

    #region 채팅창 상호작용

    /*
     * 채팅창 열기
     * 채팅창 닫기
     * 채팅 채널 접속
     * 채팅 채널 퇴장
     */

    //채팅창 열기
    //public IEnumerator OpenChatUI()
    //{
    //    if (enterChatServer)
    //        yield break;

    //    StartCoroutine(EnterChatServer());
    //    yield return new WaitUntil(() => allSetting);
    //    if (!chatStatus)
    //    {
    //        if (!enterChatServer)
    //        {
    //            Debug.Log("Fail to Enter Chat Server");
    //            yield break;
    //        }
    //    }
    //}

    public void OpenChatUI()
    {
        if (BackendAsyncClass.BackendAsyncQueueCount > 0)
            return;
        if (!chatStatus)
        {
            if (!EnterChatServer())
            {
                Debug.Log("Fail To Enter Chat Server");
                return;
            }
            Debug.Log("Open Chat UI");
            message_allSetting = true;
        }
    }

    // 채팅창 닫기
    public void CloseChatUI()
    {
        // 채팅창을 닫을 때 호출
        Debug.Log("Close Chat UI");
        MessageMenu.instance.CloseChatUI();
    }

    // 채팅채널 접속
    //public IEnumerator EnterChatServer()
    //{
    //    StartCoroutine(GetChatStatus());
    //    yield return new WaitUntil(() => reply_chat);

    //    StartCoroutine(GetChannelList());
    //    yield return new WaitUntil(() => reply_channel);

    //    //for (int i = 0; i < channelList.Count; i++)
    //    //{
    //    //    MessageMenu.instance.ChannelBtnSetting(i, channelList[i].joinedUserCount);
    //    //}
    //    allSetting = true;
    //    yield return new WaitUntil(() => allSetting);
    //    enterChatServer = true;

    //    foreach (ChannelNodeObject channel in channelList)
    //    {
    //        if (channel.joinedUserCount >= channel.maxUserCount)
    //            continue;
    //        JoinChannel(channel);
    //        break;
    //    }

    //    MessageMenu.instance.ShowChatUI();
    //}

    public bool EnterChatServer()
    {
        if (!GetChatStatus())
            return false;
        if (!GetChannelList())
            return false;
        foreach (ChannelNodeObject channel in channelList)
        {
            if (channel.joinedUserCount >= channel.maxUserCount)
                continue;
            JoinChannel(channel);
            break;
        }
        return true;
    }

    // 채팅채널 퇴장
    public void LeaveChatServer()
    {
        if (!chatStatus)
            return;
        Debug.Log("LeaveChatServer");
        Backend.Chat.LeaveChannel(channelType);
    }

    #endregion 채팅창 상호작용

    #region 채팅서버 접속

    /*
    * 뒤끝챗 서버 접속
    * 활성화 된 일반채널 접속
    */

    //뒤끝챗 서버 접속
    //private IEnumerator GetChatStatus()
    //{
    //    chatStatus = false;
    //    channelType = ChannelType.Public;
    //    //현재 채팅서버 상태를 호출함
    //    BackendAsyncClass.BackendAsync(Backend.Chat.GetChatStatus, callback =>
    //    {
    //        if (!callback.IsSuccess() || callback.IsServerError())
    //        {
    //            ShowMessage(callback.ToString());
    //            Debug.Log("Fail to Connect Chat Server - " + callback);
    //        }
    //        else
    //        {
    //            string chatServerStatus = callback.GetReturnValuetoJSON()["chatServerStatus"]["chatServer"].ToString();

    //            chatStatus |= chatServerStatus.Equals("y");
    //            Debug.Log("chatStatus - " + chatStatus + " : " + callback);
    //            chatStatus = true;
    //        }
    //        reply_chat = true;
    //    });
    //    yield return new WaitUntil(() => reply_chat);
    //    if (chatStatus == false)
    //    {
    //        ShowMessage("GetChatStatus : " + CHAT_INACTIVE);
    //    }
    //}

    // 활성화된 채널 리스트 받아오기 (public)
    private bool GetChatStatus()
    {
        //현재 채팅서버 상태를 호출함
        BackendReturnObject chatStatusBRO = Backend.Chat.GetChatStatus();
        chatStatus = false;
        channelType = ChannelType.Public;

        if (!chatStatusBRO.IsSuccess())
        {
            ShowMessage(chatStatusBRO.ToString());
            Debug.Log("Fail To Connect Chat Server - " + chatStatusBRO);
            return false;
        }

        string chatServerStatus = chatStatusBRO.GetReturnValuetoJSON()["chatServerStatus"]["chatServer"].ToString();

        chatStatus |= chatServerStatus.Equals("y");
        Debug.Log("chatStatus - " + chatStatus);
        //chatStatus = true;
        if (!chatStatus)
        {
            ShowMessage(CHAT_INACTIVE);
            return false;
        }

        return true;
    }

    //private IEnumerator GetChannelList()
    //{
    //    returnChannelList = false;
    //    //public 채널 리스트 받아오기
    //    if (!chatStatus)
    //        yield break;

    //    channelList.Clear();
    //    BackendAsyncClass.BackendAsync(Backend.Chat.GetChannelList, callback =>
    //    {
    //        if (!callback.IsSuccess() || callback.IsServerError())
    //        {
    //            Debug.Log("Fail To Get Chat Channel - " + callback);
    //            ShowMessage(CHAT_INACTIVE);

    //            returnChannelList = false;
    //        }
    //        else
    //        {
    //            JsonData rows = callback.GetReturnValuetoJSON()["rows"];
    //            Debug.Log(rows.ToString());
    //            ChannelNodeObject node;

    //            foreach (JsonData data in rows)
    //            {
    //                node = new ChannelNodeObject(channelType, data["uuid"].ToString(), (int)data["joinedUserCount"], (int)data["maxUserCount"],
    //                                             data["serverHostName"].ToString(), data["serverPort"].ToString(), data["alias"].ToString());
    //                channelList.Add(node);
    //            }
    //            returnChannelList = true;
    //        }
    //        reply_channel = true;
    //    });
    //    yield return new WaitUntil(() => reply_channel);
    //    if (returnChannelList == false)
    //        ShowMessage("GetChannelList : " + CHAT_INACTIVE);
    //}

    private bool GetChannelList()
    {
        // Public 채널 리스트 받아오기
        if (!chatStatus)
        {
            return false;
        }

        channelList.Clear();
        BackendReturnObject chatChannelStatusBRO = Backend.Chat.GetChannelList();

        if (!chatChannelStatusBRO.IsSuccess() || chatChannelStatusBRO.IsServerError())
        {
            ShowMessage(CHAT_INACTIVE);
            chatStatus = false;
            Debug.Log("Fail To Get Chat Channel - " + chatChannelStatusBRO);
            return false;
        }

        JsonData rows = chatChannelStatusBRO.GetReturnValuetoJSON()["rows"];
        ChannelNodeObject node;

        foreach (JsonData data in rows)
        {
            node = new ChannelNodeObject(channelType, data["uuid"].ToString(), (int)data["joinedUserCount"], (int)data["maxUserCount"],
                                         data["serverHostName"].ToString(), data["serverPort"].ToString(), data["alias"].ToString());
            channelList.Add(node);
        }
        chatStatus = true;
        return true;
    }

    // 활성화된 채널에 참여
    public void JoinChannel(ChannelNodeObject node)
    {
        ErrorInfo info;
        Backend.Chat.JoinChannel(node.type, node.host, node.port, node.channel_uuid, out info);

        // 채널 입장 메시지
        chatItem = new ChatItem(SessionInfo.None, infoText, string.Format(CHAT_ENTER, node.alias), false);
        //chatItem = new ChatItem(SessionInfo.None, infoText, string.Format(CHAT_ENTER, string.Format("'치키의 모험 #{0}'", index + 1)), false);
        channelUserCount = node.joinedUserCount + 1;
        channelUserCountText.text = string.Format("채팅 참여 인원 : {0}/100", channelUserCount);
        MessageMenu.instance.AddChat(chatItem);
        MessageMenu.instance.OnClickChannelBtn(0);
    }

    public void SetUserCount(int value)
    {
        channelUserCount += value;
        channelUserCountText.text = string.Format("채팅 참여 인원 : {0}/100", channelUserCount);
    }

    #endregion 채팅서버 접속

    #region 채널 관련 상호작용

    /* 채팅 핸들러 설정
     * 필터링
     * 도배장지 메시지
     * 자동 접속 종료 메시지
     * 입장
     * 유저목록
     * 퇴장
     * 채팅
     * 귓속말
     * 오류 핸들러
     */

    public void ChatHandlers()
    {
        // 채팅 필터링 설정
        Backend.Chat.SetFilterReplacementChar('*');

        // 도배 방지 메시지 설정
        Backend.Chat.SetRepeatedChatBlockMessage("도배하면 안돼요.");
        // 자동 접속 종료 메시지 설정
        Backend.Chat.SetTimeoutMessage("오랜시간 대화를 입력하지 않아 채널에서 퇴장당했습니다.");

        // 현재 일반 채널에 접속해 있는 모든 게이머 정보 받아오기
        // 플레이어가 일반 채널에 입장할 때마다 호출됨
        Backend.Chat.OnSessionListInChannel = (args) =>
        {
            participantList.Clear();

            // 채널에 참여중인 유저 목록
            foreach (SessionInfo info in args.SessionList)
            {
                participantList.Add(info.NickName);
            }
        };

        // 플레이어 혹은 다른 유저가 채널에 입장하면 호출됨
        Backend.Chat.OnJoinChannel = (args) =>
        {
            Debug.Log("OnEnterChannel " + args.ErrInfo);

            if (args.ErrInfo == ErrorInfo.Success)
            {
                // 다른 유저가 접속한 경우
                if (args.Session.IsRemote)
                {
                    SetUserCount(1);
                    chatItem = new ChatItem(SessionInfo.None, infoText, string.Format("'{0}'이 입장했습니다.", args.Session.NickName), false);
                    MessageMenu.instance.AddChat(chatItem);
                }
                // 내가 접속한 경우
                else
                {
                    myNickName = args.Session.NickName;
                    Backend.Chat.SetFilterUse(isFilterOn);
                }

                // 접속한 세션이 채널 참여자 리스트에 존재하지 않을 경우 리스트에 추가
                if (!participantList.Contains(args.Session.NickName))
                {
                    participantList.Add(args.Session.NickName);
                }
            }
            else
            {
                // 접속 실패한 경우
                ShowMessage(string.Format("OnEnterChannel : {0}", args.ErrInfo.Reason));
            }
        };

        // 플레이어 혹은 다른 유저가 채널에서 퇴장하면 호출됨
        Backend.Chat.OnLeaveChannel = (args) =>
        {
            Debug.Log("OnLeaveChannel " + args.ErrInfo);

            // 플레이어가 채널에서 퇴장한 경우
            if (!args.Session.IsRemote)
            {
                if (args.ErrInfo.Category != ErrorCode.Success)
                {
                    if (args.ErrInfo.Category == ErrorCode.DisconnectFromRemote &&
                       args.ErrInfo.Detail == ErrorCode.ChannelTimeout)
                    {
                        ShowMessage(args.ErrInfo.Reason);
                    }
                    else
                    {
                        ShowMessage(string.Format("OnLeaveChannel : {0}", args.ErrInfo.Reason));
                    }
                    CloseChatUI();
                    chatStatus = false;
                    OpenChatUI();
                }
            }
            // 다른 유저가 채널에서 퇴장한 경우
            else
            {
                SetUserCount(-1);
                chatItem = new ChatItem(SessionInfo.None, infoText, string.Format("'{0}'이 퇴장했습니다.", args.Session.NickName), false);
                MessageMenu.instance.AddChat(chatItem);

                participantList.Remove(args.Session.NickName);
            }
        };

        // 채팅이 왔을 때 호출됨
        Backend.Chat.OnChat = (args) =>
        {
            Debug.Log(string.Format("OnChat {0}, {1}", DateTime.Now.TimeOfDay, args.Message));
            if (args.ErrInfo == ErrorInfo.Success)
            {
                chatItem = new ChatItem(args.From, args.From.NickName, args.Message, args.From.IsRemote);
                MessageMenu.instance.AddChat(chatItem);
            }
            else if (args.ErrInfo.Category == ErrorCode.BrokenStream)
            {
                //도배방지 메시지
                if (args.ErrInfo.Detail == ErrorCode.BannedChat)
                {
                    chatItem = new ChatItem(SessionInfo.None, infoText, args.ErrInfo.Reason, false);
                    MessageMenu.instance.AddChat(chatItem);
                }
            }
        };
        // 공지
        Backend.Chat.OnNotification = (args) =>
        {
            Debug.Log(string.Format("OnNotification : {0} - {1}", args.Subject, args.Message));
            chatItem = new ChatItem(SessionInfo.None, notiText, args.Message, false);
            MessageMenu.instance.AddChat(chatItem, false, true);
        };
        // 귓속말이 왔을 때 호출됨
        Backend.Chat.OnWhisper = (args) =>
        {
            Debug.Log(string.Format("OnWhisper from {0}, to {1} : {2}", args.From.NickName, args.To.NickName, args.Message));
            if (args.ErrInfo == ErrorInfo.Success)
            {
                if (myNickName != args.From.NickName)
                {
                    string tmpMsg = string.Format("의 귓속말 : {0}", args.Message);
                    chatItem = new ChatItem(args.From, args.From.NickName, tmpMsg, true, args.From.IsRemote);
                }
                else
                {
                    chatItem = new ChatItem(SessionInfo.None, args.From.NickName, string.Format(CHAT_WHISPER, args.To.NickName, args.Message), true, args.From.IsRemote);
                }
                MessageMenu.instance.AddChat(chatItem);
            }
            else
            {
                Debug.Log(args.ErrInfo);
            }
        };

        // Exception 발생 시
        Backend.Chat.OnException = (e) =>
        {
            Debug.LogError(e);
        };
        onEnterChannel = true;
    }

    // 유저 목록 보기
    public void ShowAllUserListInChannel()
    {
        MessageMenu.instance.userbtns[0].interactable = false;
        MessageMenu.instance.userbtns[1].interactable = true;
        MessageMenu.instance.ShowUserList(participantList);
    }

    #endregion 채널 관련 상호작용

    #region 메시지

    /*
     * 메시지 체크 (길이 등등)
     * 일반 채널에 메시지 보내기
     * 커맨드 명령어 수행
     */

    // 메시지 길이 체크
    private int GetStringLength(string msg)
    {
        return System.Text.Encoding.Unicode.GetByteCount(msg);
    }

    // 채널에 메시지 전송
    public void ChatToChannel(string msg)
    {
        if (!chatStatus)
        {
            // 채팅 상태가 false이면 곧바로 리턴
            return;
        }

        if (GetStringLength(msg) > maxLength)
        {
            // 글자수가 최대 글자수를 넘어가는 경우 오류메시지를 띄우고 리턴
            ShowMessage(CHAT_MSGLENGTHEXCEED);
            return;
        }

        if (msg.Length <= 0)
        {
            // 글자수가 0 이하인 경우 어떤 동작도 수행하지 않고 바로 리턴
            // 오류메시지는 표시하지 않음
            return;
        }

        if (msg.StartsWith("/", System.StringComparison.CurrentCulture))
        {
            // command 명령어인 경우

            // 띄어쓰기 단위로 단어 분활
            string[] msgSplit = msg.Split(' ');

            if (msgSplit.Length < 2)
            {
                CmdError(CHAT_NOTARGET);
                return;
            }

            string nickName = msgSplit[1];

            // 닉네임이 내 닉네임이거나, 공백이면 에러메시지 표시
            if (nickName.Equals(myNickName) || string.IsNullOrEmpty(nickName))
            {
                CmdError(CHAT_UNVAILDTARGET);
                return;
            }

            if (IsWhisperCmd(msgSplit[0]))
            {
                if (msgSplit.Length < 3)
                {
                    CmdError(CHAT_NOMSG);
                }
                var wMsgLen = msgSplit[0].Length + msgSplit[1].Length + 2;
                if (wMsgLen < msg.Length)
                {
                    string wMsg = msg.Substring(wMsgLen);
                    Backend.Chat.Whisper(nickName, wMsg);
                }
            }
            else if (IsBlockCmd(msgSplit[0]))
            {
                BlockUser(nickName);
            }
            else if (IsUnblockCmd(msgSplit[0]))
            {
                UnBlockUser(nickName);
            }
            else
            {
                CmdError(CHAT_UNKNOWNCMD);
            }
        }
        else
        {
            //플레이어가 채팅창에 입력한 내용을 현재 채널로 전송
            Backend.Chat.ChatToChannel(channelType, msg);
        }
    }

    // 귓속말
    private bool IsWhisperCmd(string message)
    {
        return message.Equals("/ㄱ")
                    || message.Equals("/w")
                    || message.Equals("/귓");
    }

    // 차단
    private bool IsBlockCmd(string message)
    {
        return message.Equals("/b")
                    || message.Equals("/차단");
    }

    // 차단 해제
    private bool IsUnblockCmd(string message)
    {
        return message.Equals("/ub")
                      || message.Equals("/차단해제");
    }

    // 알수없는 커맨드
    private void CmdError(string msg)
    {
        chatItem = new ChatItem(SessionInfo.None, infoText, msg, false);
        MessageMenu.instance.AddChat(chatItem);
    }

    #endregion 메시지

    #region 커맨드 명령어

    /*
     * 유저 차단
     * 유저 차단 해제
     * 유저 신고
     */

    // 유저 차단
    private void BlockUser(string nickName)
    {
        Backend.Chat.BlockUser(nickName, arg =>
        {
            ChatItem tmpMsg;
            // 차단 성공한 경우
            if (arg)
            {
                tmpMsg = new ChatItem(SessionInfo.None, infoText, string.Format(CHAT_BLOCKSUCCESS, nickName), false);
            }
            // 차단 실패한 경우
            else
            {
                tmpMsg = new ChatItem(SessionInfo.None, infoText, CHAT_BLOCKFAIL, false);
            }
            // add chat에서 인스턴스 생성은 메인스레드에서만 가능
            // 그러므로 디스패처에 이벤트 등
            //Dispatcher.Instance.Invoke(() =>
            //{
            //});
            MessageMenu.instance.AddChat(tmpMsg);
        });
    }

    // 유저 차단 해제
    private void UnBlockUser(string nickName)
    {
        ChatItem tmpMsg;
        if (Backend.Chat.UnblockUser(nickName))
        {
            tmpMsg = new ChatItem(SessionInfo.None, infoText, string.Format(CHAT_UNBLOCKSUCCESS, nickName), false);
        }
        else
        {
            tmpMsg = new ChatItem(SessionInfo.None, infoText, CHAT_BLOCKFAIL, false);
        }
        MessageMenu.instance.AddChat(tmpMsg);
    }

    #endregion 커맨드 명령어

    public void ShowMessage(string msg)
    {
        ChatItem tmpMsg;
        tmpMsg = new ChatItem(SessionInfo.None, "알림", msg, false);
        MessageMenu.instance.AddChat(tmpMsg, true);
    }

    // 채널 노드 관련 클래스
    public class ChannelNodeObject
    {
        public ChannelType type;    // 채널 타입
        public string channel_uuid; // 채널 uuid
        public string participants; // 채널 참가자
        public int joinedUserCount; // 참가 인원수
        public int maxUserCount;    // 최대 참가 인원 (100)
        public string host;         // host
        public ushort port;         // port
        public string alias;        // 채널 별명

        public ChannelNodeObject(ChannelType type, string uuid, int joinedUser, int maxUser, string host, string port, string alias)
        {
            this.type = type;
            this.channel_uuid = uuid;
            this.joinedUserCount = joinedUser;
            this.maxUserCount = maxUser;
            this.participants = joinedUser + "/" + maxUser;
            this.host = host;
            this.port = ushort.Parse(port);
            this.alias = alias;
        }
    }

    // 채팅 메시지 관련 클래스
    public class ChatItem
    {
        internal SessionInfo session { get; set; } // 메시지 보낸 세션 정보
        internal bool IsRemote { get; set; }       // true : 다른유저가 보낸 메시지, false : 내가 보낸 메시지
        internal bool isWhisper { get; set; }      // 귓속말 여부
        internal string Nickname;                  // 보낸이 이름
        internal string Contents;                  // 실제 메시지

        internal ChatItem(SessionInfo session, string nick, string cont, bool isWhisper, bool IsRemote)
        {
            this.session = session;
            Nickname = nick;
            Contents = cont;
            this.isWhisper = isWhisper;
            this.IsRemote = IsRemote;
        }

        internal ChatItem(SessionInfo session, string nick, string cont, bool IsRemote)
        {
            this.session = session;
            Nickname = nick;
            Contents = cont;
            isWhisper = false;
            this.IsRemote = IsRemote;
        }
    }
}