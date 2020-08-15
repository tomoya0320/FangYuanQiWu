using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public interface IBack
{
    void Close();
}
public class BackManager : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public GameObject quitGameConfirmation;
    private static BackManager instance;
    private List<IBack> backStack;
    private IBack tempIBack;
    private Tween tween;

    private void Awake()
    {
        instance = this;
        backStack = new List<IBack>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (backStack.Count == 0)
            {
                if (quitGameConfirmation.activeSelf)
                {
                    HideWindow();
                }
                else
                {
                    if (tween != null)
                    {
                        tween.Complete();
                    }
                    canvasGroup.alpha = 0;
                    quitGameConfirmation.SetActive(true);
                    tween = canvasGroup.DOFade(1.0f, 0.15f).OnComplete(() =>
                    {
                        tween = null;
                    });
                }
            }
            else
            {
                tempIBack = backStack[backStack.Count - 1];
                backStack.RemoveAt(backStack.Count - 1);
                tempIBack.Close();
            }
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void HideWindow()
    {
        if (tween != null)
        {
            tween.Complete();
        }
        tween = canvasGroup.DOFade(0, 0.15f).OnComplete(() =>
        {
            quitGameConfirmation.SetActive(false);
            tween = null;
        });
    }
    public static void AddToBackStack(IBack back)
    {
        instance.backStack.Add(back);
    }
    public static void RemoveFromBackStack(IBack back)
    {
        instance.backStack.Remove(back);
    }
    public static void ClearBackStack()
    {
        instance.backStack.Clear();
    }
}
