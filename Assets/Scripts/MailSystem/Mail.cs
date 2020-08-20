using System.Collections.Generic;
using System;

public enum MailType {
  MainStory,
  System,
}

[Serializable]
public class Mail : Letter {
  public bool isRead;
  public bool isReceived;
  public List<Prop> attachments;

  public Mail(string title, string text, string signature, params Prop[] attachments) {
    this.text = text;
    this.title = title;
    this.signature = signature;
    this.attachments = attachments == null ? new List<Prop>() : new List<Prop>(attachments);
    isReceived = this.attachments.Count == 0;
  }
}
[Serializable]
public class Letter {
  public string title;
  public string text;
  public string signature;
  public DateTime dateTime;

  public Letter() {
    dateTime = DateTime.Now;
  }
  public Letter(string title, string text, string signature) {
    this.title = title;
    this.text = text;
    this.signature = signature;
    dateTime = DateTime.Now;
  }
  public Mail ToMail() {
    Mail mail = new Mail(title, text, signature);
    mail.isRead = true;
    mail.isReceived = true;
    return mail;
  }
}