using UnityEngine;
using UnityEngine.AddressableAssets;
using Wzb;

public class ActivityGift : MonoBehaviour {
  public int giftIndex;
  public int neededActivityValue;
  public Sprite openImg;
  public WButton button;
  private JSONObject giftInfo;

  private void Start() {
    giftInfo = Database.GetActivityGiftInfoByIndex(giftIndex);

    if (Database.IsGotActivityGiftByIndex(giftIndex)) {
      button.image.sprite = openImg;
      button.enabled = false;
    }
  }
  public void UpdateGiftState() {
    if (Database.ActivityValue >= neededActivityValue) {
      button.interactable = true;
    }
  }
  public async void GetGift() {
    if (giftInfo[StringMap.EXP].n > 0) {
      Database.AddExp((int)giftInfo[StringMap.EXP].n);
    }
    foreach (JSONObject temp in giftInfo[StringMap.PROPS].list) {
      Database.AddPropById((int)temp[StringMap.ID].n, (int)temp[StringMap.NUM].n);
      JSONObject propInfo = Database.GetPropInfoById((int)temp[StringMap.ID].n);
      Sprite propIcon = await Addressables.LoadAssetAsync<Sprite>(propInfo[StringMap.ICON_FILE_NAME].str).Task;
      MessageCenter.Instance.AddMessage(new Message(MessageStyle.ColorFade, 
      string.Format(StringMap.GET + "{0}*{1}", propInfo[StringMap.NAME].str, temp[StringMap.NUM].n), propIcon));
    }
    Database.AddReceivedActivityGiftByIndex(giftIndex);
    button.image.sprite = openImg;
    button.enabled = false;
    EventCenter.Broadcast(Wzb.EventType.UpdateNewContentTip,
    Database.IsExistUnclaimedTask(), NewContentType.Task);
  }
}