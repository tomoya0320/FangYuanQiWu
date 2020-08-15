using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class PropHandbook : MonoBehaviour
{
    private int index;
    private bool unlocked;
    public Image icon;

    private void Start()
    {
        EventCenter.AddListener(Wzb.EventType.UpdatePropHandbook, UpdateHandbook);
        index = transform.GetSiblingIndex();
        UpdateHandbook();
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(Wzb.EventType.UpdatePropHandbook, UpdateHandbook);
    }
    public async void UpdateHandbook()
    {
        unlocked = DataBase.IsUnlockedPropById(index);
        if (unlocked)
        {
            icon.sprite = await Addressables.LoadAssetAsync<Sprite>
            (DataBase.GetPropInfoById(index)["iconFileName"].str).Task;

            GetComponent<Image>().sprite = await Addressables.LoadAssetAsync<Sprite>
            ("handbook_hud_wupindikuang").Task;      
        }
    }
}
