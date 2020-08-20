using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Wzb;

public class DialogNPCTrigger : MonoBehaviour
{
    [Header("NPC信息")]
    public int dialogueIndex;   //当前对话的索引号
    public int npcId;
    public Text npcName;
    [Header("任务提示")]
    public GameObject TaskAcceptTip;
    public GameObject TaskCompleteTip;
    [Header("二级菜单")]
    public GameObject secondMenu;
    public Button communicateButton;
    public Text npcNameInSecondMenu;
    public Text operateName;
    [Header("NPC动画")]
    public Animator anim;
    [Header("描边材质")]
    public Material highlight;
    private JSONObject npcInfo;
    private List<JSONObject> npcDialogueInfo;

    private void Start()
    {
        highlight.SetFloat("_Outline", 0);
        npcInfo = Database.GetNPCInfoByIndex(npcId);
        npcDialogueInfo = npcInfo["dialogueList"].list;
        dialogueIndex = Database.GetNPCDialogueIndexById(npcId);
        npcName.text = npcInfo["name"].str;
    }
    private void Update()
    {
        if (dialogueIndex < npcDialogueInfo.Count)
        {
            switch (GetCurrentDialogueTaskState())
            {
                case TaskState.Inactive:
                    {
                        TaskAcceptTip.SetActive(RequiredTaskCheck());
                        TaskCompleteTip.SetActive(false);
                        break;
                    }
                case TaskState.Completed:
                    {
                        TaskCompleteTip.SetActive(GetCurrentDialogueTaskState() == TaskState.Completed);
                        TaskAcceptTip.SetActive(false);
                        break;
                    }
                default:
                    {
                        TaskAcceptTip.SetActive(false);
                        TaskCompleteTip.SetActive(false);
                        break;
                    }
            }
        }
        else
        {
            TaskAcceptTip.SetActive(false);
            TaskCompleteTip.SetActive(false);
        }
    }
    public string GetFileName()
    {
        if (IsInTaskState())
        {
            return string.Format("Dialogue/{0}/dialogue_{1}_{2}.json",
                   npcId, dialogueIndex, (int)GetCurrentDialogueTaskState());
        }
        return string.Format("Dialogue/{0}/dialogue_normal.json", npcId);
    }
    public void Communicate()
    {
        TaskCenter.UpdateTaskState(RequirementType.NPCDialogue.ToString(), npcId, 1);
        DialogControl.instance.TriggerDialog(this);   //触发对话
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Player"))
        {
            operateName.text = "交流";
            npcNameInSecondMenu.text = npcInfo["name"].str;
            communicateButton.onClick.RemoveAllListeners();
            communicateButton.onClick.AddListener(Communicate);
            highlight.SetFloat("_Outline", 0.01f);
            secondMenu.SetActive(true);
            if (anim)
            {
                anim.SetTrigger("gesture");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains("Player"))
        {
            communicateButton.transform.localScale = Vector3.one;
            highlight.SetFloat("_Outline", 0);
            secondMenu.SetActive(false);
        }
    }
    public bool IsInTaskState()
    {
        return dialogueIndex < npcDialogueInfo.Count && RequiredTaskCheck() &&
               (GetCurrentDialogueTaskState() == TaskState.Inactive ||
                GetCurrentDialogueTaskState() == TaskState.Completed);
    }
    private bool RequiredTaskCheck()
    {
        return (npcDialogueInfo[dialogueIndex].HasField("requiredTask") &&
        (int)GetCurrentRequiredTaskState() == npcDialogueInfo[dialogueIndex]["requiredTask"]["stateFlag"].n)
        || !npcDialogueInfo[dialogueIndex].HasField("requiredTask");
    }
    public TaskState GetCurrentDialogueTaskState()
    {
        return Database.GetTask(GetCurrentTaskType(), GetCurrentTaskId()).state;
    }    
    public TaskState GetCurrentRequiredTaskState()
    {
        return Database.GetTask(GetCurrentRequiredTaskType(), GetCurrentRequiredTaskId()).state;
    }
    public TaskType GetCurrentTaskType()
    {
        return (TaskType)npcDialogueInfo[dialogueIndex]["taskType"].n;
    }
    public int GetCurrentTaskId()
    {
        return (int)npcDialogueInfo[dialogueIndex]["taskId"].n;
    }
    public TaskType GetCurrentRequiredTaskType()
    {
        return (TaskType)npcDialogueInfo[dialogueIndex]["requiredTask"]["type"].n;
    }
    public int GetCurrentRequiredTaskId()
    {
        return (int)npcDialogueInfo[dialogueIndex]["requiredTask"]["id"].n;
    }
    public bool IsTransfer()
    {
        return npcDialogueInfo[dialogueIndex].HasField("isTransfer");
    }
}