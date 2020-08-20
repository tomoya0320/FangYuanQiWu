using UnityEngine;
using UnityEngine.AddressableAssets;

public class Live2DManager : MonoBehaviour {
  private GameObject currentLive2D;

  private void Start() {
    EventCenter.AddListener(Wzb.EventType.UpdateLive2D, UpdateLive2D);
    UpdateLive2D();
  }
  private void OnDestroy() {
    EventCenter.RemoveListener(Wzb.EventType.UpdateLive2D, UpdateLive2D);
  }
  public async void UpdateLive2D() {
    JSONObject playerInfo = Database.GetPersonalityInfoByIndex(Database.CurrentPersonalityId);
    if (currentLive2D) {
      Destroy(currentLive2D);
    }
    currentLive2D = await Addressables.InstantiateAsync(playerInfo[StringMap.L2D_PREF_NAME].str, transform).Task;
  }
}