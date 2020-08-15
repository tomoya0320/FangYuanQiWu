using UnityEngine;
using UnityEngine.UI;
using Wzb;

public class SideTaskInList : TaskInList
{
    public GameObject completed;
    public Button accepteButton;

    public void AcceptTask()
    {
        accepteButton.interactable = false;
        accepteButton.transform.GetChild(0).GetComponent<Text>().text = "已接受";
        TaskCenter.UpdateTaskState(RequirementType.ActiveSideTaskNum.ToString(), 0, 1);
        DataBase.ActiveTask(task.type, task.id);
    }
    public override void UpdateTaskProcess()
    {
        switch (task.state)
        {
            case TaskState.Inactive:
                {
                    accepteButton.interactable = true;
                    accepteButton.gameObject.SetActive(true);
                    accepteButton.transform.GetChild(0).GetComponent<Text>().text = "接    受";
                    break;
                }
            case TaskState.InProcess:
                {
                    accepteButton.interactable = false;
                    accepteButton.gameObject.SetActive(true);
                    accepteButton.transform.GetChild(0).GetComponent<Text>().text = "已接受";
                    break;
                }
            default:
                {
                    accepteButton.gameObject.SetActive(false);
                    break;
                }
        }
        completed.SetActive(task.state == TaskState.Final);
    }
}
