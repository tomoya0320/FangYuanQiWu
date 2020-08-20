using Wzb;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MessageShow : MonoBehaviour {
  private Message message;
  private RectTransform textRect;
  public Text description;
  public Image img;
  public void Init(Message message) {
    this.message = message;
    description.text = message.description;
    textRect = description.GetComponent<RectTransform>();
    Vector2 size = textRect.rect.size;
    // height保持不变
    textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
    // 动态设置width
    textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
       Mathf.Clamp(description.preferredWidth, 300, 1500));

    if (message.isShowImg) {
      if (message.img != null) {
        img.sprite = message.img;
      }
    } else {
      img.enabled = false;
    }
    switch (message.messageStyle) {
      case MessageStyle.Slide: {
          RectTransform rectTransform = GetComponent<RectTransform>();
          rectTransform.DOAnchorPosY(-textRect.rect.height, message.duration).SetRelative().OnComplete(() =>
              rectTransform.DOAnchorPosY(textRect.rect.height, message.duration).
              SetDelay(message.delay).SetRelative().OnComplete(ShowNextMessage)
          );
          break;
        }
      case MessageStyle.ColorFade: {
          CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
          canvasGroup.DOFade(1.0f, message.duration).OnComplete(() =>
              canvasGroup.DOFade(0, message.duration).SetDelay(message.delay).OnComplete(ShowNextMessage)
          );
          break;
        }
    }
  }
  public void ShowNextMessage() {
    MessageCenter.Instance.RemoveMessage(message);
    MessageCenter.Instance.ShowMessage();
    Destroy(gameObject);
  }
}