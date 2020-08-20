using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class UserDataShowInMM : MonoBehaviour {
  public Text ap;
  public Text crystal;
  public Text gold;

  public Image playerIcon;
  public Text lv;
  public Text nickname;
  public Text title;
  public Image titleBg;
  public Text expText;
  public Image expImg;

  // Start is called before the first frame update
  private void Start() {
    EventCenter.AddListener(Wzb.EventType.UpdateUserInfo, UpdateUserData);
    UpdateUserData();
  }
  private void OnDestroy() {
    EventCenter.RemoveListener(Wzb.EventType.UpdateUserInfo, UpdateUserData);
  }
  public async void UpdateUserData() {
    ap.text = Database.APInfo;
    gold.text = Database.GetPropNumById(-1).ToString();
    crystal.text = Database.GetPropNumById(-2).ToString();

    lv.text = Database.Level.ToString();
    nickname.text = Database.Nickname;
    title.text = Database.CurrentTitleName;
    titleBg.color = Database.CurrentTitleColor;
    float needEXP = Utils.CalNeededExp(Database.Level);
    expText.text = Database.UserExp + "/" + needEXP;
    expImg.fillAmount = Database.UserExp / needEXP;

    playerIcon.sprite = await Addressables.LoadAssetAsync<Sprite>
    (Database.GetPersonalityInfoByIndex(Database.CurrentPersonalityId)[StringMap.HEAD_ICON_FILE_NAME].str).Task;
  }
}