using System;
using System.Linq;
using ChatServer;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Chat
{
    [Serializable]
    public class ChatMessage
    {
        public ChannelId ChannelId { get; }
        public string Recipient { get; }
        public string Sender { get; }
        public string Message { get; }

        public ChatMessage(string message, ChannelId channelId, string recipient = null, string sender = null)
        { 
            Message = message;
            ChannelId = channelId;
            Recipient = recipient;
            Sender = sender;
        }

        public string ToJson()
        {
            return "{" +
                   $"\"ChannelId\":{(int) ChannelId}," +
                   $"\"Message\":\"{Message}\"," +
                   $"\"Sender\":{Sender ?? "null"}," +
                   $"\"Recipient\":{Recipient ?? "null"}" +
                   "}";
            return JsonUtility.ToJson(this);
        }

        public static ChatMessage FromJson(string json)
        {
            var elements = json.Replace("{", "")
                .Replace("}", "")
                .Split(',')
                .Select(x => x.Split(':'));

            var channelId = ChannelId.Global;
            var message = "";
            var sender = "";

            foreach (var element in elements)
            {
                switch (element[0].ToLower())
                {
                    case "\"channelid\"":
                        Enum.TryParse(element[1], out channelId);
                        break;
                    case "\"message\"":
                        message = string.Join(":", element.Skip(1)).Trim('"');
                        break;
                    case "\"sender\"":
                        sender = element[1].Trim('"');
                        break;
                }
            }
            
            return new ChatMessage(message, channelId, null, sender);
            //return JsonUtility.FromJson<ChatMessage>(json);
        }
    }
}