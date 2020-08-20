using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Wzb;

public class SideTaskShowInMap : MonoBehaviour {
  public RectTransform content;
  private List<Task> sideTasks;
  private List<Task> inactiveSideTasks;
  private GameObject sideTaskInList;

  private async void Start() {
    sideTaskInList = await Addressables.LoadAssetAsync<GameObject>(StringMap.SIDETASK_IN_LIST).Task;
    sideTasks = Database.GetTaskListByType(TaskType.SideTask);
    inactiveSideTasks = new List<Task>();
    int sideTaskNum = 0;
    foreach (Task task in sideTasks) {
      if (task.state == TaskState.Inactive) {
        inactiveSideTasks.Add(task);
      } else {
        ShowTask(task);
        sideTaskNum++;
      }
    }
    while (sideTaskNum++ < 4) {
      int index = Random.Range(0, inactiveSideTasks.Count);
      ShowTask(inactiveSideTasks[index]);
      inactiveSideTasks.RemoveAt(index);
    }
  }
  private void ShowTask(Task task) {
    GameObject temp = Instantiate(sideTaskInList, content);
    temp.GetComponent<TaskInList>().Init(task);
  }
}