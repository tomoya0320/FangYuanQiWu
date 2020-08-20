using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class MainStoryMailShow : MailShow {
  public Button getButton;
  public Button getAllButton;
  public Button readAllButton;

  protected override void Start() {
    EventCenter.AddListener(Wzb.EventType.AddMainStoryMail, ShowNewMail);
    mailType = MailType.MainStory;
    base.Start();
  }
  private void OnDestroy() {
    EventCenter.RemoveListener(Wzb.EventType.AddMainStoryMail, ShowNewMail);
  }
  public override void CheckReadAllButton() {
    readAllButton.interactable = false;
    foreach (Mail mail in mailList) {
      if (!mail.isRead) {
        readAllButton.interactable = true;
        break;
      }
    }
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
  public void ReadAll() {
    foreach (Transform mail in mailListContent) {
      mail.GetComponent<MailInList>().Read();
    }
    readAllButton.interactable = false;
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
    List<Mail> mainStoryMails = Database.GetMailListByType(MailType.MainStory);
    Mail mail = mainStoryMails[mainStoryMails.Count - 1];
    temp.GetComponent<MailInList>().Init(mail, this);
    readAllButton.interactable = true;
    getAllButton.interactable = getAllButton.interactable || !mail.isReceived;
    AdjustMailListContentSize();
  }
}