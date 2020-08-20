using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailInList : MonoBehaviour {
  public Sprite readIcon;
  public Sprite unreadIcon;
  public Image mailIcon;
  public Image attachmentIcon;
  public Text title;
  public Text dateDelta;
  public Mail mail;
  private MailShow context;

  public void Init(Mail mail, MailShow context) {
    this.mail = mail;
    this.context = context;
    mailIcon.sprite = mail.isRead ? readIcon : unreadIcon;
    attachmentIcon.enabled = mail.attachments.Count > 0;
    title.text = mail.title;
    dateDelta.text = Utils.CalDateDeltaFromNow(mail.dateTime);
  }
  public void Click() {
    context.Switch(this);
    Read();
  }
  public void Read() {
    if (!mail.isRead) {
      Database.ReadMail(mail);
      mailIcon.sprite = readIcon;
      context.CheckReadAllButton();
    }
  }
  public List<Prop> Get() {
    List<Prop> acquiredPropList = new List<Prop>();
    if (!mail.isReceived) {
      acquiredPropList = Database.GetMailAttachments(mail);
      context.CheckGetAllButton();
    }
    return acquiredPropList;
  }
}