using Wzb;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class ShortTaskInfo {
  public TaskType type;
  public int id;

  public ShortTaskInfo(TaskType type, int id) {
    this.type = type;
    this.id = id;
  }
}
public class DialogControl : MonoBehaviour {

  #region 变量
  [Range(0.01f, 1.0f)]
  public float deltTimeShowWord;  //显示文字的间隔时间
  private float count;            //计时器
  private bool showed;            //文字是否已完全显示
  private Tween textTween;        //对话内容动画
  private Tween nextColorTween;   //浮标透明度动画
  private Tween nextRotaTween;    //浮标旋转动画

  //对白
  private DialogNPCTrigger dialogNPC;//当前对话的NPC数据
  private bool isNPCInTaskState;//当前对话的NPC是否处于有任务的状态（可接受或可交付）
  public GameObject dialog;   //对话窗口
  public Image roleL, roleR;  //角色图片（左/右）
  public Text nameL, nameR;   //角色名字（左/右）
  public Text text;           //对白内容
  public Image nextR, nextL;  //下一句浮标图片

  //任务
  public GameObject task;     //任务窗
  public Text content;        //任务接受按钮内容

  public ShortTaskInfo taskInfo; //接受的任务信息

  private DialogContent data;             //对话数据
  private bool isChatting = false;  //是否正在对话

  public static DialogControl instance;
  #endregion

  private void Awake() {
    instance = this;
  }

  private void Update() {
    if (isChatting) {
      if (count < data.text.Length * deltTimeShowWord) {
        count += Time.deltaTime;
        if (count >= data.text.Length * deltTimeShowWord) {
          showed = true;
          nextColorTween.Kill();
          nextRotaTween.Kill();
          if (data.role == StringMap.ME) {
            nextL.color = new Color(1, 1, 1, 0);
            nextColorTween = nextL.DOColor(new Color(1, 1, 1, 1), 0.5f);
            nextL.transform.localEulerAngles = new Vector3(0, 180, 0);
            nextRotaTween = nextL.transform.DORotate(new Vector3(0, 0, 0), 0.5f);
            nextL.gameObject.SetActive(true);
          } else {
            nextR.color = new Color(1, 1, 1, 0);
            nextColorTween = nextR.DOColor(new Color(1, 1, 1, 1), 0.5f);
            nextR.transform.localEulerAngles = new Vector3(0, 180, 0);
            nextRotaTween = nextR.transform.DORotate(new Vector3(0, 0, 0), 0.5f);
            nextR.gameObject.SetActive(true);
          }
        }
      }
    }
  }

  //触发对话
  public async void TriggerDialog(DialogNPCTrigger dialogNPC) {
    dialog.GetComponent<CanvasGroup>().alpha = 0;
    dialog.SetActive(true);
    this.dialogNPC = dialogNPC;
    isNPCInTaskState = dialogNPC.IsInTaskState();
    TextAsset json = await Addressables.LoadAssetAsync<TextAsset>(dialogNPC.GetFileName()).Task;
    DialogJsonRead.LoadJson(json);
    PlayerController.GetComp<Animator>().SetTrigger(StringMap.CHAT_TRIGGER);
    PlayerController.GetComp<Animator>().SetBool(StringMap.CHAT, true);
    isChatting = true;
    Next();
  }

  //点击拒绝按钮
  public void OnClickRefuse() {
    DialogJsonRead.AcceptTask(false);
    task.SetActive(false);
    Next();
  }

  //点击接受按钮
  public void OnClickAccept() {
    taskInfo = new ShortTaskInfo(data.taskType, data.taskId);
    DialogJsonRead.AcceptTask(true);
    task.SetActive(false);
    Next();
  }

  //下一条
  public void Next() {
    if (isChatting) {
      task.SetActive(false);
      data = GetData();
      if (data == null) {
        //结束对话
        isChatting = false;
        PlayerController.GetComp<Animator>().SetBool(StringMap.CHAT, false);
        task.GetComponent<CanvasGroup>().DOFade(0f, 0.5f).OnComplete(() => task.SetActive(false));
        dialog.GetComponent<CanvasGroup>().DOFade(0, 0.5f).OnComplete(() => dialog.SetActive(false));

        if (isNPCInTaskState) {
          if (taskInfo != null) {
            MessageCenter.Instance.AddMessage(new Message(MessageStyle.Slide, string.Format
            (StringMap.ACCEPT_TASK, Database.GetTask(taskInfo.type, taskInfo.id).title)));
            Database.ActiveTask(taskInfo.type, taskInfo.id);
            dialogNPC.dialogueIndex += dialogNPC.IsTransfer() ? 1 : 0;
            taskInfo = null;
          } else {
            TaskState state = dialogNPC.GetCurrentDialogueTaskState();
            switch (state) {
              case TaskState.InProcess: {
                  dialogNPC.dialogueIndex += dialogNPC.IsTransfer() ? 1 : 0;
                  break;
                }
              case TaskState.Completed: {
                  Database.CompleteTask(dialogNPC.GetCurrentTaskType(), dialogNPC.GetCurrentTaskId());
                  dialogNPC.dialogueIndex++;
                  break;
                }
            }
          }
          Database.SetNPCDialogueIndexById(dialogNPC.npcId, dialogNPC.dialogueIndex);
          dialogNPC = null;
        }
      } else if (data.category == StringMap.DIALOG) {
        UpdateDialog();
      } else if (data.category == StringMap.TASK) {
        UpdateTask();
      }
      count = 0;
      showed = false;
    }
  }

  //点击下一条
  public void OnClickNext() {
    if (showed) {   //已显示完，下一条
      Next();
    } else {  //未显示完，直接全部显示
      if (isChatting) {
        showed = true;
        textTween.Kill();
        text.text = data.text;
        nextColorTween.Kill();
        nextRotaTween.Kill();
        if (data.role == StringMap.ME) {
          nextL.color = new Color(1, 1, 1, 0);
          nextColorTween = nextL.DOColor(new Color(1, 1, 1, 1), 0.5f);
          nextL.transform.localEulerAngles = new Vector3(0, 180, 0);
          nextRotaTween = nextL.transform.DORotate(new Vector3(0, 0, 0), 0.5f);
          nextL.gameObject.SetActive(true);
        } else {
          nextR.color = new Color(1, 1, 1, 0);
          nextColorTween = nextR.DOColor(new Color(1, 1, 1, 1), 0.5f);
          nextR.transform.localEulerAngles = new Vector3(0, 180, 0);
          nextRotaTween = nextR.transform.DORotate(new Vector3(0, 0, 0), 0.5f);
          nextR.gameObject.SetActive(true);
        }
      }
    }
  }

  //获取当前对话数据
  private DialogContent GetData() {
    DialogContent data_ = new DialogContent();
    JSONObject jsonObject = DialogJsonRead.ReadJson();
    if (jsonObject == null) return null;
    data_.category = jsonObject[StringMap.CATEGORY].str;
    if (jsonObject.HasField(StringMap.ILL_INDEX)) {
      data_.illustrationIndex = (int)jsonObject[StringMap.ILL_INDEX].n;
    }
    if (data_.category == StringMap.DIALOG) {
      data_.role = jsonObject[StringMap.ROLE].str;
      if (data_.role != StringMap.ME) {
        data_.roleId = (int)jsonObject[StringMap.ROLE_ID].n;
      }
      data_.text = string.Format(jsonObject[StringMap.TEXT].str, Database.Nickname);
    } else if (data_.category == StringMap.TASK) {
      data_.taskId = (int)jsonObject[StringMap.TASK_ID].n;
      data_.taskType = (TaskType)jsonObject[StringMap.TASK_TYPE].n;
      data_.taskContent = Database.GetTask(data_.taskType, data_.taskId).title;
      data_.text = string.Format(StringMap.ACCEPT_TASK, data_.taskContent);
    }
    return data_;
  }

  //更新对话内容
  private async void UpdateDialog() {
    if (data.role == StringMap.ME) {    //玩家说话
      int id = Database.CurrentPersonalityId;
      JSONObject JsonObject = Database.GetPersonalityInfoByIndex(id);
      string fileName = JsonObject[StringMap.ILL_FILE_NAMES].list[data.illustrationIndex].str;
      Sprite sprite = await Addressables.LoadAssetAsync<Sprite>(fileName).Task;
      roleR.sprite = sprite;
      nameL.text = StringMap.EMPTY;
      nameR.text = GetUserName();
      roleL.gameObject.SetActive(false);
      roleR.gameObject.SetActive(true);
    } else {  //npc说话
      JSONObject JsonObject;
      JsonObject = Database.GetNPCInfoByIndex(dialogNPC.npcId);
      string fileName = JsonObject[StringMap.ILL_FILE_NAMES].list[data.illustrationIndex].str;
      Sprite sprite = await Addressables.LoadAssetAsync<Sprite>(fileName).Task;
      roleL.sprite = sprite;
      nameL.text = data.role;
      nameR.text = StringMap.EMPTY;
      roleL.gameObject.SetActive(true);
      roleR.gameObject.SetActive(false);
    }
    dialog.GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f);
    text.text = StringMap.EMPTY;
    textTween = text.DOText(data.text, deltTimeShowWord * data.text.Length).SetEase(Ease.Linear);
    nextL.gameObject.SetActive(false);
    nextR.gameObject.SetActive(false);
  }

  //更新任务内容
  private void UpdateTask() {
    task.GetComponent<CanvasGroup>().alpha = 0;
    task.GetComponent<CanvasGroup>().DOFade(1.0f, 0.5f);
    task.SetActive(true);
    content.text = data.text;
  }

  //获取用户昵称
  private string GetUserName() {
    try {
      if (Database.Nickname == null) return StringMap.ME;
      else return Database.Nickname;
    } catch {
      return StringMap.ME;
    }
  }
}