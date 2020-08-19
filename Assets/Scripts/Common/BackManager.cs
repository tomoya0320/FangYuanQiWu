using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public interface IBack
{
    void Close();
}
public class BackManager : SingletonMono<BackManager> {
  public CanvasGroup canvasGroup;
  public GameObject quitGameConfirmation;
  private List<IBack> backStack;
  private IBack tempIBack;
  private Tween tween;

  protected override void Awake() {
    base.Awake();
    backStack = new List<IBack>();
  }
  private void Update() {
    if (Input.GetKeyDown(KeyCode.Escape)) {
      if (backStack.Count == 0) {
        if (quitGameConfirmation.activeSelf) {
          HideWindow();
        } else {
          if (tween != null) {
            tween.Complete();
          }
          canvasGroup.alpha = 0;
          quitGameConfirmation.SetActive(true);
          tween = canvasGroup.DOFade(1.0f, 0.15f).OnComplete(() => {
            tween = null;
          });
        }
      } else {
        tempIBack = backStack[backStack.Count - 1];
        backStack.RemoveAt(backStack.Count - 1);
        tempIBack.Close();
      }
    }
  }
  public void QuitGame() {
    Application.Quit();
  }
  public void HideWindow() {
    if (tween != null) {
      tween.Complete();
    }
    tween = canvasGroup.DOFade(0, 0.15f).OnComplete(() => {
      quitGameConfirmation.SetActive(false);
      tween = null;
    });
  }
  public void AddToBackStack(IBack back) {
    Instance.backStack.Add(back);
  }
  public void RemoveFromBackStack(IBack back) {
    Instance.backStack.Remove(back);
  }
  public void ClearBackStack() {
    Instance.backStack.Clear();
  }
}