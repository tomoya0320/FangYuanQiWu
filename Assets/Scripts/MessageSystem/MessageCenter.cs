using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Wzb
{
    public enum MessageStyle
    {
        Slide,
        ColorFade,
    }
    public class Message
    {
        public MessageStyle messageStyle;
        public string description;
        public Sprite img;
        public float duration;
        public float delay;
        public bool isShowImg;


        public Message(MessageStyle messageStyle, string description, Sprite img, float duration = 0.5f, float delay = 1.0f)
        {
            this.messageStyle = messageStyle;
            this.description = description;
            this.duration = duration;
            this.delay = delay;
            this.img = img;
            isShowImg = true;
        }
        public Message(MessageStyle messageStyle, string description, float duration = 0.8f, float delay = 1.6f)
        {
            this.messageStyle = messageStyle;
            this.description = description;
            this.duration = duration;
            this.delay = delay;
            isShowImg = false;
        }
    }
    public class MessageCenter : MonoBehaviour
    {
        private static Transform pos;
        private static GameObject tempGo;
        private static bool isMessageShowing;
        private static List<Message> messages;

        private void Awake()
        {
            pos = transform;
            messages = new List<Message>();
        }
        public static void AddMessage(Message message)
        {
            int index = messages.FindIndex(m =>
            {
                return m.description == message.description;
            });
            if (index < 0)
            {
                messages.Add(message);
                if (!isMessageShowing)
                {
                    isMessageShowing = true;
                    ShowMessage();
                }
            }
        }
        public static bool RemoveMessage(Message message)
        {
            return messages.Remove(message);
        }
        public async static void ShowMessage()
        {
            if (messages.Count > 0)
            {
                int index = (int)messages[0].messageStyle;
                tempGo = await Addressables.InstantiateAsync("message" + index, pos).Task;
                MessageShow ms = tempGo.GetComponent<MessageShow>();
                ms.Init(messages[0]);
            }
            else
            {
                isMessageShowing = false;
            }
        }
    }
}