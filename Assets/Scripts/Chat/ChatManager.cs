using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

namespace Chat
{
    public class ChatManager : SingletonBehaviour<ChatManager>
    {
        private static bool _forceUpdate;
        private const string Url = "ws://localhost:1337/";
        private readonly WebSocket _webSocket = new WebSocket(Url);
        
        [SerializeField] private TextMeshProUGUI chatTextWindow;
        [SerializeField] private TMP_InputField chatInputField;
        [SerializeField] private ContentSizeFitter contentSizeFitter;
        
        private void Update()
        {
            if (_forceUpdate)
            {
                chatTextWindow.ForceMeshUpdate();
                contentSizeFitter.gameObject.SetActive(false);
                contentSizeFitter.gameObject.SetActive(true);
                _forceUpdate = false;
            }
        }
        
        private ChatManager()
        {
            _webSocket.SetCredentials("test", "none", true);

            _webSocket.OnOpen += OnOpen;
            _webSocket.OnClose += OnClose;
            _webSocket.OnMessage += OnMessage;

            _webSocket.ConnectAsync();

        }

        private void Awake()
        {
            chatTextWindow.text = "";
        }

        public void OnEndEdit()
        {
            if (!Input.GetKey(KeyCode.KeypadEnter) && !Input.GetKey(KeyCode.Return))
            {
                return;
            }

            if (chatInputField.text.IsNullOrEmpty())
            {
                return;
            }
            
            SendChatMessage(chatInputField.text);
            
            chatInputField.text = "";
        }
        
        private void SendChatMessage(string text)
        {
            var message = new ChatMessage(text, ChannelId.Global);
            var json = message.ToJson();
            Debug.Log(json);
            
            _webSocket.Send(message.ToJson());
        }

        private void OnMessage(object sender, MessageEventArgs e)
        {
            Debug.Log("Received " + e.Data);
            ProcessChatMessage(ChatMessage.FromJson(e.Data));
        }

        private void ProcessChatMessage(ChatMessage message)
        {
            switch (message.ChannelId)
            {
                case ChannelId.System:
                    chatTextWindow.text += $"\n<color=\"red\">{message.Message}";
                    break;
                case ChannelId.Whisper:
                case ChannelId.Local:
                case ChannelId.Map:
                case ChannelId.Party:
                case ChannelId.Guild:
                case ChannelId.Global:
                    chatTextWindow.text += $"\n<color=\"white\">{message.Sender}: {message.Message}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            chatTextWindow.text += $"\n{message.Sender}: {message.Message}";
            _forceUpdate = true;
        }

        private void OnOpen(object sender, EventArgs eventArgs)
        {
            ProcessChatMessage(new ChatMessage("Connection to chat server established.", ChannelId.System));
        }

        private void OnClose(object sender, CloseEventArgs closeEventArgs)
        {
            ProcessChatMessage(new ChatMessage("Lost Connection to chat server. :(", ChannelId.System));
            _webSocket.ConnectAsync();
        }
    }
}
