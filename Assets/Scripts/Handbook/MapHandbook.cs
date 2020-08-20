using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class MapHandbook : MonoBehaviour {
  private int index;
  private bool unlocked;
  private Image icon;

  private void Start() {
    EventCenter.AddListener(Wzb.EventType.UpdateMainMapHandbook, UpdateHandbook);
    icon = GetComponent<Image>();
    index = transform.GetSiblingIndex();
    UpdateHandbook();
  }
  private void OnDestroy() {
    EventCenter.RemoveListener(Wzb.EventType.UpdateMainMapHandbook, UpdateHandbook);
  }
  public async void UpdateHandbook() {
    unlocked = Database.IsUnlockedMainMapById(index);
    if (unlocked) {
      icon.sprite = await Addressables.LoadAssetAsync<Sprite>
      (Database.GetMapInfoByIndex(index)[StringMap.ICON_FILE_NAME].str).Task;
    }
  }
}