using DG.Tweening;
using UnityEngine;

public class OpenOrCloseUIWithAnim : MonoBehaviour, IBack {
  public CanvasGroup canvasGroup;
  public GameObject[] targets;
  private Tween tween;

  public void Open() {
    AddToBackStack();
    if (tween != null) {
      tween.Complete();
    }
    canvasGroup.alpha = 0;
    tween = canvasGroup.DOFade(1.0f, 0.15f).OnComplete(() => {
      tween = null;
    });
    foreach (GameObject temp in targets) {
      temp.SetActive(true);
    }
  }
  public void Close() {
    if (tween != null) {
      tween.Complete();
    }
    canvasGroup.alpha = 1.0f;
    tween = canvasGroup.DOFade(0, 0.15f).OnComplete(() => {
      foreach (GameObject temp in targets) {
        temp.SetActive(false);
      }
      tween = null;
    });
    RemoveFromBackStack();
  }
  private void AddToBackStack() {
    BackManager.Instance.AddToBackStack(this);
  }
  private void RemoveFromBackStack() {
    BackManager.Instance.RemoveFromBackStack(this);
  }
}