using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Chat;
using ExitGames.Client.Photon;
using Photon.Pun;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    public ChatClient chatClient;
    string chatName;
    [SerializeField] InputField userID;

    [SerializeField] Text chatText;
    public InputField textMessage;

    private void Start()
    {
        if (userID == null)
        {
            GameObject obj = new GameObject();
            InputField some = Instantiate(obj, Vector3.one, Quaternion.identity).AddComponent<InputField>();
            some.text= PlayerPrefs.GetString("NickName");
            userID = some;
            CheckNickName();
        }
    }

    public void DebugReturn(DebugLevel level, string message)
    {

        Debug.Log($"{level}, {message}");
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log(state);
    }


    public void OnConnected()
    {
        chatText.text += "\n Вы вошли в лобби!";
        chatClient.Subscribe(chatName);
    }

    public void OnDisconnected()
    {
        chatText.text += "\n Вы вышли из лобби!";
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        //if (channelName != chatName)
        //{
        //    Debug.Log(channelName);
        //    Debug.Log(chatName);
        //    return;
        //}

        for (int i = 0; i < senders.Length; i++)
        {
            chatText.text += $"\n {senders[i]}: {messages[i]}";
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        for (int i = 0; i < channels.Length; i++)
            chatText.text = $"Вы подключились к каналу {channels[i]}";
    }

    public void OnUnsubscribed(string[] channels)
    {
        for (int i = 0; i < channels.Length; i++)
        {
            chatText.text = $"отключился от канала {channels[i]}";
            Debug.Log("Вы отписались от канала: " + channels[i]);
        }
    }

    public void OnUserSubscribed(string channel, string user)
    {
        chatText.text += $"{user} подключился к каналу {channel}";
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        chatText.text += $"{user} вышел из канала {channel}";
    }

    public void EnterServerChat(InputField name)
    {
        if (name != null)
        {
            chatName = name.text;
            PlayerPrefs.SetString("ConnectedChatName", chatName);
        }
        else
            chatName = PlayerPrefs.GetString("ConnectedChatName");
        
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(MainMenu.instance.GetNickName()));

        Debug.Log("you entered " + chatName);


    }

    public void ClearChat()
    {
        string[] channels = { chatName };
        chatClient.Unsubscribe(channels);
        chatText.text = null;
        Debug.Log("CLearChat");
    }

    void CheckNickName()
    {
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(MainMenu.instance.GetNickName()));
    }

    void Update()
    {
        if (chatName != null)
        chatClient.Service();
    }

    public void SendButton()
    {
        if (chatClient.AuthValues.UserId != MainMenu.instance.GetNickName())
            CheckNickName();

        chatClient.PublishMessage(chatName, textMessage.text);
        textMessage.text = "";

    }
}
