using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Wzb;

public class SideTaskTrigger : MonoBehaviour
{
    public GameObject secondMenu;
    public Button operateButton;
    public Text nameInSecondMenu;
    public Text operateName;
    public Material highlight;
    public UnityEvent clickEvent;
    private Task task;

    private void Start()
    {
        highlight.SetFloat("_Outline", 0);
        task = DataBase.GetTask(TaskType.MainStoryTask, 3);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Player") && task.state != TaskState.Inactive)
        {
            operateName.text = "查看";
            nameInSecondMenu.text = "路牌";
            operateButton.onClick.RemoveAllListeners();
            operateButton.onClick.AddListener(() => clickEvent.Invoke());
            highlight.SetFloat("_Outline", 0.01f);
            secondMenu.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains("Player") && task.state != TaskState.Inactive)
        {
            highlight.SetFloat("_Outline", 0);
            secondMenu.SetActive(false);
        }
    }
}
