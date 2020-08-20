using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Wzb;

public class PropInMap : MonoBehaviour {
  public int propId;
  public int propNum;
  public float reappearTime;
  public Sprite operateIcon;
  public GameObject propGameObejct;
  public Text propName;
  public GameObject nameGameObejct;
  [Header("是否开启角色采集/拾取动画")]
  public bool isOpenAnim;

  private Animator playerAnim;
  private JSONObject propInfo;
  private Sprite propIcon;

  private async void Start() {
    propInfo = Database.GetPropInfoById(propId);
    propIcon = await Addressables.LoadAssetAsync<Sprite>(propInfo[StringMap.ICON_FILE_NAME].str).Task;
    playerAnim = isOpenAnim ? GameObject.FindGameObjectWithTag(StringMap.PLAYER).GetComponent<Animator>() : null;
    nameGameObejct.GetComponent<Canvas>().worldCamera = Camera.main;
  }
  public void GetProp() {
    PropUIInMap.Instance.propMenu.SetActive(false);
    nameGameObejct.SetActive(false);

    if (playerAnim) {
      playerAnim.SetTrigger(StringMap.PICK);
    }

    MessageCenter.Instance.AddMessage(new Message(MessageStyle.ColorFade,
    string.Format(StringMap.GET_PROP, propInfo[StringMap.NAME].str, propNum), propIcon, 0.4f, 0.8f));
    Database.AddPropById(propId, propNum);

    if (reappearTime > 0) {
      propGameObejct.SetActive(false);
      Invoke("Reappear", reappearTime);
    } else {
      Destroy(propGameObejct);
    }
  }
  private void Reappear() {
    propGameObejct.SetActive(true);
  }
  private void OnTriggerEnter(Collider other) {
    if (other.tag.Contains(StringMap.PLAYER)) {
      PropUIInMap.Instance.operateImg.sprite = operateIcon;
      PropUIInMap.Instance.operateButton.onClick.RemoveAllListeners();
      PropUIInMap.Instance.operateButton.onClick.AddListener(GetProp);
      PropUIInMap.Instance.propMenu.SetActive(true);

      propName.text = propInfo[StringMap.NAME].str;
      nameGameObejct.SetActive(true);
    }
  }
  private void OnTriggerExit(Collider other) {
    if (other.tag.Contains(StringMap.PLAYER)) {
      PropUIInMap.Instance.operateButton.transform.localScale = Vector3.one;
      PropUIInMap.Instance.propMenu.SetActive(false);
      nameGameObejct.SetActive(false);
    }
  }
}