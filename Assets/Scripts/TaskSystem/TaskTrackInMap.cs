using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Wzb;

public class TaskTrackInMap : MonoBehaviour {
  public RectTransform taskTrackContent;
  private float length;
  private GameObject taskTrack;
  private async void Start() {
    taskTrack = await Addressables.LoadAssetAsync<GameObject>(StringMap.TASKTRACK_IN_MAP).Task;
    GridLayoutGroup grid = taskTrackContent.GetComponent<GridLayoutGroup>();
    length = grid.cellSize.y + grid.spacing.y;
    int taskShowNum = 0;
    foreach (List<Task> tasks in Database.AllTaskList) {
      foreach (Task task in tasks) {
        if ((task.state == TaskState.InProcess || task.state == TaskState.Completed) &&
        (task.type == TaskType.SideTask || (task.type == TaskType.MainStoryTask && task.isIncludeDialogue))) {
          taskShowNum++;
          GameObject temp = Instantiate(taskTrack, taskTrackContent);
          temp.GetComponent<TaskInTrack>().Init(task);
        }
      }
    }
    taskTrackContent.sizeDelta = new Vector2(taskTrackContent.sizeDelta.x, length * taskShowNum);
    EventCenter.AddListener<Task>(Wzb.EventType.AddTaskToTrackInMap, AddTaskToTrack);
  }
  private void OnDestroy() {
    EventCenter.RemoveListener<Task>(Wzb.EventType.AddTaskToTrackInMap, AddTaskToTrack);
  }
  public void AddTaskToTrack(Task task) {
    if (task.type == TaskType.SideTask || (task.type == TaskType.MainStoryTask && task.isIncludeDialogue)) {
      GameObject temp = Instantiate(taskTrack, taskTrackContent);
      temp.GetComponent<TaskInTrack>().Init(task);
      temp.transform.SetAsFirstSibling();
      taskTrackContent.sizeDelta += new Vector2(0, length);
    }
  }
}