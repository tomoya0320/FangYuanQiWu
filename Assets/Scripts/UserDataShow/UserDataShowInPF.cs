using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class UserDataShowInPF : MonoBehaviour {
  public Image playerIcon;
  public Text lv;
  public Text expText;
  public Image expImg;
  public InputField nickname;
  public InputField signature;
  public Text title;
  public Text account;
  public Text unlockedDressNum;
  public Text unlockedAchievementNum;
  public Text unlockedMapNum;
  public Text unlockedPersonalityNum;
  private void Start() {
    EventCenter.AddListener(Wzb.EventType.UpdateUserInfo, UpdateUserData);
    UpdateUserData();
  }
  private void OnDestroy() {
    EventCenter.RemoveListener(Wzb.EventType.UpdateUserInfo, UpdateUserData);
  }
  public async void UpdateUserData() {
    lv.text = Database.Level.ToString();
    nickname.text = Database.Nickname;
    signature.text = Database.Signature;
    title.text = Database.CurrentTitleName;
    float needEXP = Utils.CalNeededExp(Database.Level);
    expText.text = Database.UserExp + "/" + needEXP;
    expImg.fillAmount = Database.UserExp / needEXP;

    account.text = StringMap.ACCOUNT + "：" + Database.Account;
    unlockedDressNum.text = StringMap.DRESS_NUM + "：" + 0;
    unlockedAchievementNum.text = StringMap.ACHIVEMENT_NUM + "：" + Database.UnlockedTitleNum;
    unlockedMapNum.text = StringMap.MAP_NUM + "：" + Database.UnlockedMainMapNum;
    unlockedPersonalityNum.text = StringMap.PERSONALITY_NUM + "：" + Database.UnlockedPersonalityNum;

    playerIcon.sprite = await Addressables.LoadAssetAsync<Sprite>
    (Database.GetPersonalityInfoByIndex(Database.CurrentPersonalityId)[StringMap.HEAD_ICON_FILE_NAME].str).Task;
  }
  public void ChangeNickname() {
    if (nickname.text == StringMap.EMPTY) {
      nickname.text = Database.Nickname;
    } else {
      Database.SetNickname(nickname.text);
    }
  }
  public void ChangeSignature() {
    if (signature.text == StringMap.EMPTY) {
      signature.text = Database.Signature;
    } else {
      Database.SetSignature(signature.text);
    }
  }
}