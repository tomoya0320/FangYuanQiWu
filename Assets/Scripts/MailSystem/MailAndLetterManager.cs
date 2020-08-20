using System.Collections.Generic;
using UnityEngine;

public class MailAndLetterManager : MonoBehaviour {
  public GameObject mail;
  public GameObject letter;
  private GameObject concrete;
  private Animator animator;
  private List<Letter> unreadMainStoryLetterList;

  private void Start() {
    unreadMainStoryLetterList = Database.GetUnreadMainStoryLetterList();
    animator = GetComponent<Animator>();
    CheckLetter();
  }
  public void CheckLetter() {
    transform.eulerAngles = Vector3.zero;
    if (unreadMainStoryLetterList.Count > 0) {
      concrete = letter;
      animator.enabled = true;
    } else {
      concrete = mail;
      animator.enabled = false;
    }
  }
  public void Open() {
    concrete.GetComponent<OpenOrCloseUIWithAnim>().Open();
  }
}