using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterShow : MonoBehaviour {
  public Text text;
  public Text signature;
  public Text date;
  private Letter currentLetter;
  private List<Letter> unreadMainStoryLetterList;

  private void Start() {
    unreadMainStoryLetterList = Database.GetUnreadMainStoryLetterList();
    currentLetter = unreadMainStoryLetterList[0];
    if (unreadMainStoryLetterList.Count > 0) {
      text.text = string.Format(currentLetter.text, Database.Nickname);
      signature.text = currentLetter.signature;
      date.text = Convert.ToDateTime(DateTime.Now.Date).ToShortDateString();
    }
  }
  public void Read() {
    unreadMainStoryLetterList.RemoveAt(0);
    Database.AddMail(MailType.MainStory, currentLetter.ToMail());
  }
}