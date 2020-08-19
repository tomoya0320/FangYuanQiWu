using UnityEngine;
using UnityEngine.AddressableAssets;
using Wzb;

public class ActivityGift : MonoBehaviour
{
    public int giftIndex;
    public int neededActivityValue;
    public Sprite openImg;
    public WButton button;
    private JSONObject giftInfo;

    private void Start()
    {
        giftInfo = DataBase.GetActivityGiftInfoByIndex(giftIndex);

        if (DataBase.IsGotActivityGiftByIndex(giftIndex))
        {
            button.image.sprite = openImg;
            button.enabled = false;
        }
    }
    public void UpdateGiftState()
    {
        if (DataBase.GetActivityValue() >= neededActivityValue)
        {
            button.interactable = true;
        }
    }
    public async void GetGift()
    {
        if (giftInfo["exp"].n > 0)
        {
            DataBase.AddExp((int)giftInfo["exp"].n);
        }
        foreach (JSONObject temp in giftInfo["props"].list)
        {
            DataBase.AddPropById((int)temp["id"].n, (int)temp["num"].n);
            JSONObject propInfo = DataBase.GetPropInfoById((int)temp["id"].n);
            Sprite propIcon = await Addressables.LoadAssetAsync<Sprite>(propInfo["iconFileName"].str).Task;
            MessageCenter.Instance.AddMessage(new Message(MessageStyle.ColorFade, string.Format("获得{0}*{1}", propInfo["name"].str, temp["num"].n), propIcon));
        }
        DataBase.AddReceivedActivityGiftByIndex(giftIndex);
        button.image.sprite = openImg;
        button.enabled = false;
        EventCenter.Broadcast(Wzb.EventType.UpdateNewContentTip,
        DataBase.IsExistUnclaimedTask(), NewContentType.Task);
    }
}
