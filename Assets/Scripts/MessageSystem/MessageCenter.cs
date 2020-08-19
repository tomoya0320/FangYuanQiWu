using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Wzb {
  public enum MessageStyle {
    Slide,
    ColorFade,
  }
  public class Message {
    public MessageStyle messageStyle;
    public string description;
    public Sprite img;
    public float duration;
    public float delay;
    public bool isShowImg;


    public Message(MessageStyle messageStyle, string description, Sprite img, float duration = 0.5f, float delay = 1.0f) {
      this.messageStyle = messageStyle;
      this.description = description;
      this.duration = duration;
      this.delay = delay;
      this.img = img;
      isShowImg = true;
    }
    public Message(MessageStyle messageStyle, string description, float duration = 0.8f, float delay = 1.6f) {
      this.messageStyle = messageStyle;
      this.description = description;
      this.duration = duration;
      this.delay = delay;
      isShowImg = false;
    }
  }
  public class MessageCenter : SingletonMono<MessageCenter> {
    private GameObject tempGo;
    private bool isMessageShowing;
    private List<Message> messages;

    protected override void Awake() {
      base.Awake();
      messages = new List<Message>();
    }
    public void AddMessage(Message message) {
      int index = messages.FindIndex(m => {
        return m.description == message.description;
      });
      if (index < 0) {
        messages.Add(message);
        if (!isMessageShowing) {
          isMessageShowing = true;
          ShowMessage();
        }
      }
    }
    public bool RemoveMessage(Message message) {
      return messages.Remove(message);
    }
    public async void ShowMessage() {
      if (messages.Count > 0) {
        int index = (int)messages[0].messageStyle;
        tempGo = await Addressables.InstantiateAsync("message" + index, transform).Task;
        MessageShow ms = tempGo.GetComponent<MessageShow>();
        ms.Init(messages[0]);
      } else {
        isMessageShowing = false;
      }
    }
  }
}