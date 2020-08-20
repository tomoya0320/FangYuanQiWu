using UnityEngine;
using UnityEngine.UI;
using Wzb;

public class TaskInTrack : MonoBehaviour {
  public Text title;
  public Text description;
  private Task task;

  private void Start() {
    EventCenter.AddListener(Wzb.EventType.UpdateTaskView, UpdateTaskProcess);
  }
  private void OnDestroy() {
    EventCenter.RemoveListener(Wzb.EventType.UpdateTaskView, UpdateTaskProcess);
  }
  public void Init(Task task) {
    this.task = task;
    title.text = task.title;
    description.text = task.description;
    UpdateTaskProcess();
  }
  public void UpdateTaskProcess() {
    description.text = task.description;
    if (task.state == TaskState.Final) {
      description.text += "\n" + StringMap.COMPLETED;
      transform.SetAsLastSibling();
    } else {
      foreach (TaskRequirement taskRequirement in task.taskRequirements) {
        description.text += string.Format("\n{0}（{1}/{2}）", taskRequirement.description,
        taskRequirement.currentNum, taskRequirement.requiredNum);
      }
    }
  }
}