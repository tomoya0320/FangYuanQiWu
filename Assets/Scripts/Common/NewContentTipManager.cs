using System;
using System.Collections.Generic;
using UnityEngine;

public enum NewContentType
{
    Mail,
    Task,
}
[Serializable]
public class NewContentTip
{
    public NewContentType type;
    public GameObject icon;
}
public class NewContentTipManager : MonoBehaviour
{
    public List<NewContentTip> tips;

    private void Start()
    {
        EventCenter.AddListener<bool, NewContentType>(Wzb.EventType.UpdateNewContentTip, UpdateInfo);
        UpdateInfo(DataBase.GetUnreadMainStoryLetterList().Count == 0 && DataBase.IsExistUnreceivedMail(), NewContentType.Mail);
        UpdateInfo(DataBase.IsExistUnclaimedTask(), NewContentType.Task);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener<bool, NewContentType>(Wzb.EventType.UpdateNewContentTip, UpdateInfo);
    }
    public void UpdateInfo(bool state,NewContentType type)
    {
        foreach(NewContentTip tip in tips)
        {
            if(tip.type == type)
            {
                tip.icon.SetActive(state);
            }
        }
    }
}
