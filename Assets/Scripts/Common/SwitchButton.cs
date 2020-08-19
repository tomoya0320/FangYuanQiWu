using UnityEngine;
using UnityEngine.UI;

public class SwitchButton : MonoBehaviour {
  public bool isChangeSiblingIndex;
  public Sprite[] buttonImgs;
  public Transform lastSelectedButton;
  public GameObject lastSelectedContent;

  public void Switch(Transform button) {
    ChangeImg(button.GetComponent<Image>());
    if (isChangeSiblingIndex && lastSelectedButton) {
      int temp = lastSelectedButton.GetSiblingIndex();
      lastSelectedButton.SetSiblingIndex(button.GetSiblingIndex());
      button.SetSiblingIndex(temp);
    }
    lastSelectedButton = button;
  }
  public void Switch(GameObject content) {
    if (lastSelectedContent) {
      lastSelectedContent.SetActive(false);
    }
    content.SetActive(true);
    lastSelectedContent = content;
  }
  public void ChangeImg(Image button) {
    if (lastSelectedButton) {
      lastSelectedButton.GetComponent<Image>().sprite = buttonImgs[0];
    }
    button.sprite = buttonImgs[1];
  }
}