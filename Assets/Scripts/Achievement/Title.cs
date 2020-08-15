using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public Image icon;
    public int index;
    private bool isUnlocked;
    private Button button;

    private void Start()
    {
        index = transform.GetSiblingIndex();
        button = GetComponent<Button>();
        EventCenter.AddListener(Wzb.EventType.UpdateTitleInfo, UpdateInfo);
        UpdateInfo();
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(Wzb.EventType.UpdateTitleInfo, UpdateInfo);
    }
    public void UpdateInfo()
    {
        isUnlocked = DataBase.IsUnlockedTitleById(index);
        button.interactable = isUnlocked;
        icon.color = Color.white * (isUnlocked ? 1.0f : 0.5f);
    }
}
