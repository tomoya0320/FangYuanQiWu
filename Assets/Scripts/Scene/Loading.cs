using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using DG.Tweening;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using System.Threading.Tasks;

public class Globe {
  public static string nextSceneName;
  public static string loadingTip;

  public static void LodingScene(string nextSceneName, string loadingTip) {
    Globe.nextSceneName = nextSceneName;
    Globe.loadingTip = loadingTip;
    SceneManager.LoadScene(StringMap.LOADING);
  }
}

public class Loading : MonoBehaviour {
  public Text loadingTip;
  public Text sceneNameTip;
  public Image progressBar;
  public CanvasGroup maskOut;

  private void Start() {
    loadingTip.text = Globe.loadingTip;
  }
  public async void StartLoadSceneCoroutine() {
    if (Globe.nextSceneName.Contains(StringMap.MAP)) {
      JSONObject playerInfo = Database.GetPersonalityInfoByIndex(Database.CurrentPersonalityId);
      PlayerInit.playerPrefab = await Addressables.LoadAssetAsync<GameObject>(playerInfo[StringMap.PL_PREF_NAME].str).Task;
      int index = int.Parse(Globe.nextSceneName.Split('_')[1]);
      sceneNameTip.text = Database.GetMapInfoByIndex(index)[StringMap.NAME].str;
    }
    if (Globe.nextSceneName == StringMap.START) {
      SceneManager.LoadScene(Globe.nextSceneName);
    } else {
      LoadScene();
    }
  }
  private async void LoadScene() {
    AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(Globe.nextSceneName, LoadSceneMode.Single, false);

    while (!handle.IsDone) {
      progressBar.fillAmount = handle.PercentComplete / 0.9f;
      await Task.Delay((int)(Time.deltaTime * 1000.0f));
    }

    maskOut.alpha = 0;
    maskOut.gameObject.SetActive(true);
    maskOut.DOFade(1.0f, 0.5f).OnComplete(() => {
      BgmManager.Instance.PlayBgm();
      handle.Result.ActivateAsync();
    });
  }
}