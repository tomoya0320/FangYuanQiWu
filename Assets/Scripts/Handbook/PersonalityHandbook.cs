using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class PersonalityHandbook : MonoBehaviour {
  private int index;
  private bool unlocked;
  private Image icon;
  public GameObject newIcon;

  private void Start() {
    EventCenter.AddListener(Wzb.EventType.UpdatePersonalityHandbook, UpdateHandbook);
    icon = GetComponent<Image>();
    index = transform.GetSiblingIndex();
    UpdateHandbook();
  }
  public async void UpdateHandbook() {
    unlocked = Database.IsUnlockedPersonalityById(index);
    if (unlocked) {
      icon.sprite = await Addressables.LoadAssetAsync<Sprite>
      (Database.GetPersonalityInfoByIndex(index)[StringMap.HB_ICON_FILE_NAME].str).Task;
      newIcon.SetActive(PlayerPrefs.GetInt(StringMap.PERSONALITY + index) == 0);
      PlayerPrefs.SetInt(StringMap.PERSONALITY + index, 1);
    }
  }
  private void OnDestroy() {
    EventCenter.RemoveListener(Wzb.EventType.UpdatePersonalityHandbook, UpdateHandbook);
  }
  private void OnDisable() {
    newIcon.SetActive(unlocked && PlayerPrefs.GetInt(StringMap.PERSONALITY + index) == 0);
  }
}