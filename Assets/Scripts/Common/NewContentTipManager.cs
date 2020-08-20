using System;
using System.Collections.Generic;
using UnityEngine;

public enum NewContentType {
  Mail,
  Task,
}
[Serializable]
public class NewContentTip {
  public NewContentType type;
  public GameObject icon;
}
public class NewContentTipManager : MonoBehaviour {
  public List<NewContentTip> tips;

  private void Start() {
    EventCenter.AddListener<bool, NewContentType>(Wzb.EventType.UpdateNewContentTip, UpdateInfo);
    UpdateInfo(Database.GetUnreadMainStoryLetterList().Count == 0 && Database.IsExistUnreceivedMail(), NewContentType.Mail);
    UpdateInfo(Database.IsExistUnclaimedTask(), NewContentType.Task);
  }
  private void OnDestroy() {
    EventCenter.RemoveListener<bool, NewContentType>(Wzb.EventType.UpdateNewContentTip, UpdateInfo);
  }
  public void UpdateInfo(bool state, NewContentType type) {
    foreach (NewContentTip tip in tips) {
      if (tip.type == type) {
        tip.icon.SetActive(state);
      }
    }
  }
}