using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivityValueShow : MonoBehaviour
{
    public Image currentActivityValueImg;
    public Text currentActivityValueText;
    public List<ActivityGift> activityGifts;
    private void Start()
    {
        EventCenter.AddListener(Wzb.EventType.UpdateActivityValue, UpdateInfo);
        UpdateInfo();
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(Wzb.EventType.UpdateActivityValue, UpdateInfo);
    }

    public void UpdateInfo()
    {
        currentActivityValueText.text = DataBase.GetActivityValue().ToString();
        currentActivityValueImg.fillAmount = Mathf.Clamp(
            DataBase.GetActivityValue() / 100.0f, 0, 1.0f);
        foreach(ActivityGift activityGift in activityGifts)
        {
            activityGift.UpdateGiftState();
        }
    }
}
