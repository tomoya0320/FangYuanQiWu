using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class PersonalityHandbook : MonoBehaviour
{
    private int index;
    private bool unlocked;
    private Image icon;
    public GameObject newIcon;
   
    private void Start()
    {
        EventCenter.AddListener(Wzb.EventType.UpdatePersonalityHandbook, UpdateHandbook);
        icon = GetComponent<Image>();
        index = transform.GetSiblingIndex();
        UpdateHandbook();
    }
    public async void UpdateHandbook()
    {
        unlocked = DataBase.IsUnlockedPersonalityById(index);
        if (unlocked)
        {
            icon.sprite = await Addressables.LoadAssetAsync<Sprite>
            (DataBase.GetPersonalityInfoByIndex(index)["handbookIconFileName"].str).Task;
            newIcon.SetActive(PlayerPrefs.GetInt("personality" + index) == 0);
            PlayerPrefs.SetInt("personality" + index, 1);
        }
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(Wzb.EventType.UpdatePersonalityHandbook, UpdateHandbook);
    }
    private void OnDisable()
    {
        newIcon.SetActive(unlocked && PlayerPrefs.GetInt("personality" + index) == 0);
    }
}
