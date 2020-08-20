using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Wzb;

[Serializable]
public class RandomNumProp {
  public int id;
  public int min;
  public int max;
}

public class TreasureBox : MonoBehaviour {
  public Animator anim;
  public Sprite operateIcon;
  public float reappearTime;
  public Transform[] randomPositions;
  public RandomNumProp[] randomNumProps;
  public Prop[] needProps;
  private bool isOpen;

  private void Start() {
    transform.position = randomPositions[UnityEngine.Random.Range(0, randomPositions.Length)].position;
  }
  private void OpenBox() {
    string error = StringMap.EMPTY;
    foreach (Prop prop in needProps) {
      if (Database.GetPropNumById(prop.id) < prop.num) {
        error += string.Format("[{0}*{1}]", Database.GetPropInfoById(prop.id)[StringMap.NAME].str, prop.num);
      }
    }
    if (string.IsNullOrEmpty(error)) {
      isOpen = true;
      PropUIInMap.Instance.propMenu.SetActive(false);
      anim.SetTrigger(StringMap.OPEN);
    } else {
      MessageCenter.Instance.AddMessage(new Message(MessageStyle.ColorFade, StringMap.NEED_PROP + error));
    }
  }
  public async void GetProp() {
    foreach (Prop prop in needProps) {
      Database.ConsumePropById(-1, prop.id, prop.num);
    }

    foreach (RandomNumProp randomNumProp in randomNumProps) {
      Prop prop = new Prop(randomNumProp.id, UnityEngine.Random.Range(randomNumProp.min, randomNumProp.max + 1));
      Database.AddPropById(prop.id, prop.num);

      JSONObject propInfo = Database.GetPropInfoById(prop.id);
      Sprite propIcon = await Addressables.LoadAssetAsync<Sprite>(propInfo[StringMap.ICON_FILE_NAME].str).Task;
      MessageCenter.Instance.AddMessage(new Message(MessageStyle.ColorFade, string.Format
      (StringMap.GET + "{0}*{1}", propInfo[StringMap.NAME].str, prop.num), propIcon));
    }

    TaskCenter.UpdateTaskState(RequirementType.OpenTreasureBoxInMap.ToString(), 0, 1);

    if (reappearTime > 0) {
      gameObject.SetActive(false);
      Invoke("Reappear", reappearTime);
    } else {
      Destroy(gameObject);
    }
  }
  private void Reappear() {
    int index = UnityEngine.Random.Range(0, randomPositions.Length);
    transform.position = randomPositions[index].position;
    transform.eulerAngles = randomPositions[index].eulerAngles;
    gameObject.SetActive(true);
    isOpen = false;
  }
  private void OnTriggerEnter(Collider other) {
    if (other.tag.Contains(StringMap.PLAYER) && !isOpen) {
      PropUIInMap.Instance.operateImg.sprite = operateIcon;
      PropUIInMap.Instance.operateButton.onClick.RemoveAllListeners();
      PropUIInMap.Instance.operateButton.onClick.AddListener(OpenBox);
      PropUIInMap.Instance.propMenu.SetActive(true);
    }
  }
  private void OnTriggerExit(Collider other) {
    if (other.tag.Contains(StringMap.PLAYER) && !isOpen) {
      PropUIInMap.Instance.operateButton.transform.localScale = Vector3.one;
      PropUIInMap.Instance.propMenu.SetActive(false);
    }
  }
}