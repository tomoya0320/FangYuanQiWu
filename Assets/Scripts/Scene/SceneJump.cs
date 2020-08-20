using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Wzb;

public class SceneJump : MonoBehaviour {
  public CanvasGroup maskOut;
  public string loadingTip;
  public string nextSceneName;
  public string nextBgm;
  public int consumeAP;
  private AudioClip audioClip;

  public async void GoToNextScene() {
    if (string.IsNullOrEmpty(nextSceneName)) {
      return;
    }
    if (consumeAP > 0) {
      if (Database.CurrentAP < consumeAP) {
        MessageCenter.Instance.AddMessage(new Message(MessageStyle.ColorFade, StringMap.AP_NOT_ENOUGH));
        return;
      }
      Database.SetAPVariation(-consumeAP);
    }
    BackManager.Instance.ClearBackStack();
    BgmManager.Instance.StopBgm();

    if (maskOut) {
      maskOut.alpha = 0;
      maskOut.gameObject.SetActive(true);
      maskOut.DOFade(1.0f, 0.5f).OnComplete(() => {
        Globe.LodingScene(nextSceneName, loadingTip);
      });
    } else {
      Globe.LodingScene(nextSceneName, loadingTip);
    }

    if (!string.IsNullOrEmpty(nextBgm)) {
      audioClip = await Addressables.LoadAssetAsync<AudioClip>(nextBgm).Task;
      BgmManager.Instance.ChangeBgm(audioClip);
    }
  }
}