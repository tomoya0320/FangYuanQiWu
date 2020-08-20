using UnityEngine;
using UnityEngine.UI;

public class MailSwitch : MonoBehaviour {
  public Sprite selected;
  public Sprite unselected;
  public Image lastSelectedButton;
  public GameObject lastSelectedContent;

  public void Switch(Image img) {
    lastSelectedButton.sprite = unselected;
    img.sprite = selected;
    lastSelectedButton = img;
  }
  public void Switch(GameObject content) {
    lastSelectedContent.SetActive(false);
    content.SetActive(true);
    lastSelectedContent = content;
  }
}