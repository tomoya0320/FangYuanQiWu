using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class InitGame : MonoBehaviour
{
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

    private void Awake()
    {
        if (instance == null)
        {
            foreach(SettingType settingType in Enum.GetValues(typeof(SettingType)))
            {
                if (!PlayerPrefs.HasKey(settingType.ToString() + "Switch"))
                {
                    PlayerPrefs.SetInt(settingType.ToString() + "Switch", 1);
                }
                if (!PlayerPrefs.HasKey(settingType.ToString() + "Value"))
                {
                    PlayerPrefs.SetInt(settingType.ToString() + "Value", 100);
                }
            }
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = targetFrameRate;
            instance = Instantiate(singleton);
            DontDestroyOnLoad(instance);
            DataBase.LoadGameData();  
        }
    }
    public async void CheckGameAssets()
    {
        await Addressables.InitializeAsync().Task;
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            downloadError.SetActive(true);
        }
        else
        {
            checkHandle = Addressables.GetDownloadSizeAsync("Preload");
            await checkHandle.Task;
            if (checkHandle.Status != AsyncOperationStatus.Succeeded)
            {
                downloadError.SetActive(true);
                return;
            }
            if (checkHandle.Result > 0)
            {
                DownloadAssets(checkHandle.Result / 1024.0f / 1024.0f);
            }
            else
            {
                ShowButton();
            }
        }
    }
    private async void DownloadAssets(float totalSize)
    {
        downloadProcess.transform.parent.gameObject.SetActive(true);
        downloadHandle = Addressables.DownloadDependenciesAsync("Preload");

        while (!downloadHandle.IsDone)
        {
            downloadText.text = string.Format("下载资源中...{0}%(总大小:{1}MB)",
            (downloadHandle.PercentComplete * 100.0f).ToString("0.00"), totalSize.ToString("0.00"));
            downloadProcess.fillAmount = downloadHandle.PercentComplete;
            await Task.Delay((int)(Time.deltaTime * 1000.0f));
        }
        if (downloadHandle.Status != AsyncOperationStatus.Succeeded)
        {
            downloadError.SetActive(true);
            return;
        }
        downloadProcess.transform.parent.gameObject.SetActive(false);
        ShowButton();
    }
    private void ShowButton()
    {
        downloadText.gameObject.SetActive(false);
        if (DataBase.IsUserDataExists())
        {
            startButton.gameObject.SetActive(true);
            continueButton.gameObject.SetActive(true);
        }
        else
        {
            startButton.gameObject.SetActive(true);
            startButton.position = (startButton.position + continueButton.position) * 0.5f;
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
