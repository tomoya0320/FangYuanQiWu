using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Wzb;

public class SideTaskTrigger : MonoBehaviour {
  public GameObject secondMenu;
  public Button operateButton;
  public Text nameInSecondMenu;
  public Text operateName;
  public Material highlight;
  public UnityEvent clickEvent;
  private Task task;

  private void Start() {
    highlight.SetFloat(StringMap.OUTLINE, 0);
    task = Database.GetTask(TaskType.MainStoryTask, 3);
  }
  private void OnTriggerEnter(Collider other) {
    if (other.tag.Contains(StringMap.PLAYER) && task.state != TaskState.Inactive) {
      operateName.text = StringMap.LOOK;
      nameInSecondMenu.text = StringMap.INDICATOR;
      operateButton.onClick.RemoveAllListeners();
      operateButton.onClick.AddListener(() => clickEvent.Invoke());
      highlight.SetFloat(StringMap.OUTLINE, 0.01f);
      secondMenu.SetActive(true);
    }
  }
  private void OnTriggerExit(Collider other) {
    if (other.tag.Contains(StringMap.PLAYER) && task.state != TaskState.Inactive) {
      highlight.SetFloat(StringMap.OUTLINE, 0);
      secondMenu.SetActive(false);
    }
  }
}