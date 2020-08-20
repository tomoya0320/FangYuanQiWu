using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class PropHandbook : MonoBehaviour {
  private int index;
  private bool unlocked;
  public Image icon;

  private void Start() {
    EventCenter.AddListener(Wzb.EventType.UpdatePropHandbook, UpdateHandbook);
    index = transform.GetSiblingIndex();
    UpdateHandbook();
  }
  private void OnDestroy() {
    EventCenter.RemoveListener(Wzb.EventType.UpdatePropHandbook, UpdateHandbook);
  }
  public async void UpdateHandbook() {
    unlocked = Database.IsUnlockedPropById(index);
    if (unlocked) {
      icon.sprite = await Addressables.LoadAssetAsync<Sprite>
      (Database.GetPropInfoById(index)[StringMap.ICON_FILE_NAME].str).Task;

      GetComponent<Image>().sprite = await Addressables.LoadAssetAsync<Sprite>
      (StringMap.HB_HUB_BOTTOM).Task;
    }
  }
}