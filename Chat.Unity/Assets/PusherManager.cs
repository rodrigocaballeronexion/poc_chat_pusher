using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;
using Chat.ClientAPI;
using Chat.ClientAPI.Base;
using Chat.ClientAPI.Entity;
using Newtonsoft.Json;
using PusherClient;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

public class PusherManager : MonoBehaviour
{
    public static PusherManager instance = null;
    private Pusher _pusher;
    private Channel _channel;
    //private const string APP_KEY = "APP_KEY";
    private const string APP_KEY = "43208d9cba5eef4aba10";
    //private const string APP_CLUSTER = "APP_CLUSTER";
    private const string APP_CLUSTER = "mt1";

    private ChatApiAsync _api;
    private string _fixedChannel = "private-live";
    private string _fixedUser = "Susan";

    [SerializeField] private GameObject pnlChatMessage;
    [SerializeField] private Transform pnlMessages;

    async Task Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        await InitializePusher();
        Console.WriteLine("Starting");

        var service = new RestService();
        _api = new ChatApiAsync(service);

        HttpResponseMessage response = await _api.Healthcheck();
        Debug.Log($"API server connected: { response.IsSuccessStatusCode }");

        await GetOldMessages();
    }

    public void AddNewMessage(InputField message)
    {
        MessageToSend messageToSend = new MessageToSend
        {
            ChannelId = _fixedChannel,
            Message = message.text,
            SocketId = _pusher.SocketID,
            UserId = _fixedUser
        };
        TheDispatcher.RunOnMainThread(async () =>
        {
            var result = await _api.SendMessage(messageToSend);
            if (result.IsSuccessStatusCode)
                Debug.Log("Message sent to server successfully");
            else
                Debug.Log($"Error: { result.StatusCode } - { await result.Content.ReadAsStringAsync() } ");
        });

        message.text = string.Empty;
    }

    private void AddMessage(string message, string who)
    {
        // Set message
        GameObject newMessage = (GameObject)Instantiate(pnlChatMessage);
        newMessage.transform.SetParent(pnlMessages);
        newMessage.transform.SetSiblingIndex(pnlMessages.childCount - 2);
        
        var texts = newMessage.GetComponentsInChildren<Text>();
        if (texts != null
            && texts.Length > 1)
        {
            texts[0].text = who;
            texts[1].text = message;
        }
    }

    private async Task InitializePusher()
    {
        if (_pusher == null && (APP_KEY != "APP_KEY") && (APP_CLUSTER != "APP_CLUSTER"))
        {
            _pusher = new Pusher(APP_KEY, new PusherOptions()
            {
                Cluster = APP_CLUSTER,
                Encrypted = true,
                Authorizer = new HttpAuthorizer("http://localhost:5000/pusher/auth")
            });

            _pusher.Error += OnPusherOnError;
            _pusher.ConnectionStateChanged += PusherOnConnectionStateChanged;
            _pusher.Connected += PusherOnConnected;
            _channel = await _pusher.SubscribeAsync(_fixedChannel); 
            _channel.Subscribed += OnChannelOnSubscribed;
            await _pusher.ConnectAsync();
        }
        else
        {
            Debug.LogError("APP_KEY and APP_CLUSTER must be correctly set. Find how to set it at https://dashboard.pusher.com");
        }
    }

    private void PusherOnConnected(object sender)
    {
        Debug.Log("Connected");

        _channel.Bind("new_message", (object data) =>
        {
            try
            {
                var theData = data.ToString()
                    .Replace("\\\"", "\"")
                    .Replace("\"{", "{")
                    .Replace("}\"", "}");
                var received = JsonConvert.DeserializeObject<ChatMessage>(theData);
                if (received != null)
                {
                    TheDispatcher.RunOnMainThread(() => AddMessage(received.Data.Message, received.Data.Name));
                }
            }
            catch(Exception ex)
            { }
        });
    }

    private async Task GetOldMessages()
    {
        var messages = await _api.GetChatMessages(_fixedChannel);
        if (messages != null)
        {
            foreach (var messageSent in messages)
            {
                AddMessage(messageSent.Message, messageSent.Name);
            }
        }
    }

    private void PusherOnConnectionStateChanged(object sender, ConnectionState state)
    {
        Debug.Log($"Connection state changed: { state }");
    }

    private void OnPusherOnError(object s, PusherException e)
    {
        Debug.Log("Errored");
    }

    private void OnChannelOnSubscribed(object s)
    {
        Debug.Log("Subscribed");
    }

    async Task OnApplicationQuit()
    {
        if (_pusher != null)
        {
            await _pusher.DisconnectAsync();
        }
    } 
}

public class ChatMessage
{
    public Data Data { get; set; }
}
public class Data
{
    public string Id { get; set; }
    public string Message { get; set; }
    public string UserId { get; set; }
    public string Name { get; set; }
    public DateTime When { get; set; }
    public string Channel { get; set; }
}
