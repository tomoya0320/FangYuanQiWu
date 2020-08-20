using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SystemMailShow : MailShow {
  public Button getButton;
  public Button getAllButton;
  public Button deleteAllButton;

  protected override void Start() {
    EventCenter.AddListener(Wzb.EventType.AddSystemMail, ShowNewMail);
    mailType = MailType.System;
    base.Start();
  }
  private void OnDestroy() {
    EventCenter.RemoveListener(Wzb.EventType.AddSystemMail, ShowNewMail);
  }
  public override void CheckGetAllButton() {
    getAllButton.interactable = false;
    foreach (Mail mail in mailList) {
      if (!mail.isReceived) {
        getAllButton.interactable = true;
        break;
      }
    }
  }
  public override void Switch(MailInList mailInList) {
    base.Switch(mailInList);
    getButton.interactable = !mailInList.mail.isReceived;
  }
  public void DeleteAll() {
    MailInList mailInList;
    foreach (Transform mailTransform in mailListContent) {
      mailInList = mailTransform.GetComponent<MailInList>();
      if (mailInList.mail.isReceived) {
        if (Database.DeleteMail(MailType.System, mailInList.mail)) {
          if (mailInList == lastClickedMail) {
            show.SetActive(false);
          }
          Destroy(mailTransform.gameObject);
        }
      }
    }
    noMail.SetActive(mailList.Count == 0);
    AdjustMailListContentSize();
  }
  public void Get() {
    if (lastClickedMail) {
      List<Prop> acquiredPropList = lastClickedMail.Get();
      foreach (Transform attachment in attachmentListContent) {
        attachment.GetComponent<PropInSettlement>().icon.color = Color.white * 0.8f;
      }
      if (acquiredPropList.Count > 0) {
        GameObject temp = Instantiate(settlement, transform.parent);
        temp.GetComponent<Settlement>().Init(acquiredPropList);
      }
      getButton.interactable = false;
    }
  }
  public void GetAll() {
    List<Prop> acquiredPropList = new List<Prop>();
    foreach (Transform mail in mailListContent) {
      acquiredPropList.AddRange(mail.GetComponent<MailInList>().Get());
      if (mail.GetComponent<MailInList>() == lastClickedMail) {
        getButton.interactable = false;
        foreach (Transform attachment in attachmentListContent) {
          attachment.GetComponent<PropInSettlement>().icon.color = Color.white * 0.8f;
        }
      }
    }
    if (acquiredPropList.Count > 0) {
      GameObject temp = Instantiate(settlement, transform.parent);
      temp.GetComponent<Settlement>().Init(Utils.MergePropList(acquiredPropList));
    }
    getAllButton.interactable = false;
  }
  public void ShowNewMail() {
    noMail.SetActive(false);
    GameObject temp = Instantiate(mailInListPrefab, mailListContent);
    List<Mail> systemMails = Database.GetMailListByType(MailType.System);
    Mail mail = systemMails[systemMails.Count - 1];
    temp.GetComponent<MailInList>().Init(mail, this);
    deleteAllButton.interactable = true;
    getAllButton.interactable = getAllButton.interactable || !mail.isReceived;
    AdjustMailListContentSize();
  }
}