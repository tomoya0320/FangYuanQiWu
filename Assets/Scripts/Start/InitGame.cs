using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class InitGame : MonoBehaviour {
  public int targetFrameRate;
  public GameObject singleton;
  public GameObject downloadError;
  public Transform startButton;
  public Transform continueButton;
  public Text downloadText;
  public Image downloadProcess;
  private AsyncOperationHandle downloadHandle;
  private AsyncOperationHandle<long> checkHandle;
  private static GameObject instance;

  private void Awake() {
    if (instance == null) {
      foreach (SettingType settingType in Enum.GetValues(typeof(SettingType))) {
        if (!PlayerPrefs.HasKey(settingType.ToString() + StringMap.SWITCH)) {
          PlayerPrefs.SetInt(settingType.ToString() + StringMap.SWITCH, 1);
        }
        if (!PlayerPrefs.HasKey(settingType.ToString() + StringMap.VALUE)) {
          PlayerPrefs.SetInt(settingType.ToString() + StringMap.VALUE, 100);
        }
      }
      Screen.sleepTimeout = SleepTimeout.NeverSleep;
      Application.targetFrameRate = targetFrameRate;
      instance = Instantiate(singleton);
      DontDestroyOnLoad(instance);
      Database.LoadGameData();
    }
  }
  public async void CheckGameAssets() {
    await Addressables.InitializeAsync().Task;
    if (Application.internetReachability == NetworkReachability.NotReachable) {
      downloadError.SetActive(true);
    } else {
      checkHandle = Addressables.GetDownloadSizeAsync(StringMap.PRELOAD);
      await checkHandle.Task;
      if (checkHandle.Status != AsyncOperationStatus.Succeeded) {
        downloadError.SetActive(true);
        return;
      }
      if (checkHandle.Result > 0) {
        DownloadAssets(checkHandle.Result / 1024.0f / 1024.0f);
      } else {
        ShowButton();
      }
    }
  }
  private async void DownloadAssets(float totalSize) {
    downloadProcess.transform.parent.gameObject.SetActive(true);
    downloadHandle = Addressables.DownloadDependenciesAsync(StringMap.PRELOAD);

    while (!downloadHandle.IsDone) {
      downloadText.text = string.Format(StringMap.ASSETS_DOWNLOADING,
      (downloadHandle.PercentComplete * 100.0f).ToString("0.00"), totalSize.ToString("0.00"));
      downloadProcess.fillAmount = downloadHandle.PercentComplete;
      await Task.Delay((int)(Time.deltaTime * 1000.0f));
    }
    if (downloadHandle.Status != AsyncOperationStatus.Succeeded) {
      downloadError.SetActive(true);
      return;
    }
    downloadProcess.transform.parent.gameObject.SetActive(false);
    ShowButton();
  }
  private void ShowButton() {
    downloadText.gameObject.SetActive(false);
    if (Database.IsUserDataExists()) {
      startButton.gameObject.SetActive(true);
      continueButton.gameObject.SetActive(true);
    } else {
      startButton.gameObject.SetActive(true);
      startButton.position = (startButton.position + continueButton.position) * 0.5f;
    }
  }
  public void QuitGame() {
    Application.Quit();
  }
}