using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class MapHandbook : MonoBehaviour
{
    private int index;
    private bool unlocked;
    private Image icon;

    private void Start()
    {
        EventCenter.AddListener(Wzb.EventType.UpdateMainMapHandbook, UpdateHandbook);
        icon = GetComponent<Image>();
        index = transform.GetSiblingIndex();
        UpdateHandbook();
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(Wzb.EventType.UpdateMainMapHandbook, UpdateHandbook);
    }
    public async void UpdateHandbook()
    {
        unlocked = DataBase.IsUnlockedMainMapById(index);
        if (unlocked)
        {
            icon.sprite = await Addressables.LoadAssetAsync<Sprite>
            (DataBase.GetMapInfoByIndex(index)["iconFileName"].str).Task;
        }
    }
}
