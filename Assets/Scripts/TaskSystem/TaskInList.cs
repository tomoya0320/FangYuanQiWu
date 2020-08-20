using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Wzb;

public class TaskInList : MonoBehaviour {
  [HideInInspector]
  public Task task;
  public Text title;
  public Text description;
  public Button getRewardButton;
  public RectTransform processContent;
  public RectTransform rewardContent;
  private GameObject rewardPrefab;
  private GameObject processTextPrefab;
  private List<Text> processTexts;

  private void Start() {
    EventCenter.AddListener(Wzb.EventType.UpdateTaskView, UpdateTaskProcess);
  }
  private void OnDestroy() {
    EventCenter.RemoveListener(Wzb.EventType.UpdateTaskView, UpdateTaskProcess);
  }
  public async void Init(Task task) {
    this.task = task;

    rewardPrefab = await Addressables.LoadAssetAsync<GameObject>(StringMap.REWARD_PREF_NAME).Task;
    processTextPrefab = await Addressables.LoadAssetAsync<GameObject>(StringMap.PROCESSTEXT_PREF_NAME).Task;
    processTexts = new List<Text>();

    title.text = task.title;
    description.text = task.description;

    if (task.taskRewards.activityValue > 0) {
      GameObject temp = Instantiate(rewardPrefab, rewardContent);
      temp.GetComponent<Image>().sprite = await Addressables.LoadAssetAsync<Sprite>(StringMap.ACTIVITY_ICON_FILE_NAME).Task;
      temp.transform.GetChild(0).GetComponent<Text>().text = task.taskRewards.activityValue.ToString();
    }
    if (task.taskRewards.exp > 0) {
      GameObject temp = Instantiate(rewardPrefab, rewardContent);
      temp.GetComponent<Image>().sprite = await Addressables.LoadAssetAsync<Sprite>(StringMap.ACTIVITY_ICON_FILE_NAME).Task;
      temp.transform.GetChild(0).GetComponent<Text>().text = task.taskRewards.exp.ToString();
    }
    foreach (Prop prop in task.taskRewards.props) {
      GameObject temp = Instantiate(rewardPrefab, rewardContent);
      temp.GetComponent<Image>().sprite = await Addressables.LoadAssetAsync<Sprite>
      (Database.GetPropInfoById(prop.id)[StringMap.FLAT_ICON_FILE_NAME].str).Task;
      temp.transform.GetChild(0).GetComponent<Text>().text = prop.num.ToString();
    }
    for (int i = 0; i < task.taskRequirements.Count; i++) {
      GameObject temp = Instantiate(processTextPrefab, processContent);
      processTexts.Add(temp.GetComponent<Text>());
    }
    UpdateTaskProcess();
  }
  public virtual void UpdateTaskProcess() {
    if (processContent) {
      for (int i = 0; i < task.taskRequirements.Count; i++) {
        processTexts[i].text = string.Format("{0}:{1}/{2}", task.taskRequirements[i].description,
            task.taskRequirements[i].currentNum, task.taskRequirements[i].requiredNum);
      }
      processContent.gameObject.SetActive(task.state == TaskState.InProcess);
    }
    getRewardButton.gameObject.SetActive(task.state == TaskState.Completed || task.state == TaskState.Final);
    getRewardButton.interactable = task.state == TaskState.Completed;
    getRewardButton.transform.GetChild(0).GetComponent<Text>().text = 
    task.state == TaskState.Final ? StringMap.COMPLETED : StringMap.CLICK_GET;
    if (task.state == TaskState.Completed) {
      transform.SetAsFirstSibling();
    }
  }
  public virtual void GetReward() {
    getRewardButton.transform.GetChild(0).GetComponent<Text>().text = StringMap.COMPLETED;
    getRewardButton.interactable = false;
    Database.CompleteTask(task);
    transform.SetAsLastSibling();
  }
}