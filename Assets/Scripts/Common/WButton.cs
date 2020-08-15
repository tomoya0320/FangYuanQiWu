using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WButton : Button {
  [Header("缩放动画的开关")]
  public bool scaleAnim = true;
  [Header("点击音效的开关")]
  public bool clickSound = true;
  [Header("点击的音效")]
  public AudioClip audioClip;
  private Tween tween;

  public override void OnPointerDown(PointerEventData eventData) {
    base.OnPointerDown(eventData);
    if (interactable) {
      if (tween != null) {
        tween.Complete();
      }
      if (scaleAnim) {
        tween = transform.DOScale(0.92f, 0.1f).OnComplete(() => tween = null);
      }
      if (clickSound && audioClip != null) {
        SoundManager.Play(audioClip);
      }
    }
  }
  public override void OnPointerUp(PointerEventData eventData) {
    base.OnPointerUp(eventData);
    if (tween != null) {
      tween.Complete();
    }
    tween = transform.DOScale(1.0f, 0.1f).OnComplete(() => tween = null);
  }
}