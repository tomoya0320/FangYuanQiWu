using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StartInterface : MonoBehaviour
{

    public SceneJump sceneJump;
    public GameObject okButton;
    public InputField nameInput;
    public CanvasGroup maskOut;
    public VideoPlayer videoPlayer;

    public void PlayStartAnim()
    {
        BgmManager.StopBgm();
        maskOut.alpha = 0;
        maskOut.gameObject.SetActive(true);
        maskOut.DOFade(1.0f, 0.5f).OnComplete(() =>
        {
            videoPlayer.gameObject.SetActive(true);
            maskOut.gameObject.SetActive(false);
            StartCoroutine(VideoCallBack());
        });
    }
    public void StartNewGame()
    {
        DataBase.CreateUserData();
        DataBase.SetNickname(nameInput.text);
        sceneJump.GoToNextScene();
    }
    public void ContinueGame()
    {
        DataBase.ReadUserData();
        sceneJump.GoToNextScene();
    }
    private IEnumerator VideoCallBack()
    {
        yield return new WaitUntil(() => videoPlayer.isPlaying);
        yield return new WaitWhile(() => videoPlayer.isPlaying);
        nameInput.gameObject.SetActive(true);
    }
    public void InputValueChange()
    {
        okButton.SetActive(!string.IsNullOrEmpty(nameInput.text));
    }
}
