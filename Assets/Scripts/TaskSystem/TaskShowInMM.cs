using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Wzb;

public class TaskShowInMM : MonoBehaviour {
  public TaskType taskType;
  public RectTransform taskContent;
  private List<Task> tasks;
  private GameObject taskInList;
  private float length;
  private async void Start() {
    taskInList = await Addressables.LoadAssetAsync<GameObject>(StringMap.TASK_IN_LIST).Task;
    tasks = Database.GetTaskListByType(taskType);
    GridLayoutGroup grid = taskContent.GetComponent<GridLayoutGroup>();
    length = grid.cellSize.y + grid.spacing.y;
    int taskShowNum = 0;
    List<TaskInList> tempList = new List<TaskInList>();
    foreach (Task task in tasks) {
      if (task.state != TaskState.Inactive && !task.isIncludeDialogue) {
        taskShowNum++;
        GameObject temp = Instantiate(taskInList, taskContent);
        temp.GetComponent<TaskInList>().Init(task);
        tempList.Add(temp.GetComponent<TaskInList>());
      }
    }
    foreach (TaskInList taskInList in tempList) {
      if (taskInList.task.state == TaskState.Final) {
        taskInList.transform.SetAsLastSibling();
      }
    }
    taskContent.sizeDelta = new Vector2(taskContent.sizeDelta.x, length * taskShowNum);
  }
}