using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class PropInKit : MonoBehaviour {
  public Prop ownedPropData;
  public Image propIcon;
  public Image coolIcon;
  public Text propNum;
  public Button button;
  private JSONObject propInfo;

  private void Start() {
    EventCenter.AddListener(Wzb.EventType.UpdatePropInfo, UpdatePropInfo);
  }
  private void OnDestroy() {
    EventCenter.RemoveListener(Wzb.EventType.UpdatePropInfo, UpdatePropInfo);
  }
  public void UpdatePropInfo() {
    if (ownedPropData.num > 0) {
      propNum.text = ownedPropData.num.ToString();
    } else {
      EventCenter.Broadcast(Wzb.EventType.RemovePropFromKit, gameObject);
    }
  }
  public async void Init(Prop ownedPropData) {
    this.ownedPropData = ownedPropData;
    propInfo = Database.GetPropInfoById(ownedPropData.id);
    propIcon.sprite = await Addressables.LoadAssetAsync<Sprite>(propInfo[StringMap.ICON_FILE_NAME].str).Task;
    UpdatePropInfo();
  }
  public void UseProp() {
    if (Database.ConsumePropById(-1, ownedPropData.id, 1)) {
      BuffManager buffManager = PlayerController.GetComp<BuffManager>();
      PlayerProperty playerProperty = PlayerController.GetComp<PlayerProperty>();
      //产生效果
      if (propInfo.HasField(StringMap.HP_RECOVERY)) {
        playerProperty.SetCurrentHP(playerProperty.GetCurrentHP() + (int)propInfo[StringMap.HP_RECOVERY].n);
      }
      if (propInfo.HasField(StringMap.ENERGY_RECOVERY)) {
        Database.SetEnergyVariation((int)propInfo[StringMap.ENERGY_RECOVERY].n);
      }
      if (propInfo.HasField(StringMap.BUFFS)) {
        foreach (JSONObject json in propInfo[StringMap.BUFFS].list) {
          buffManager.AddBuff(Database.GetBuffByIndex((int)json[StringMap.ID].n));
        }
      }
      if (ownedPropData.num > 0 && propInfo.HasField(StringMap.COOL_TIME)) {
        CoolProp(propInfo[StringMap.COOL_TIME].n);
      }
    }
  }
  private async void CoolProp(float coolTime) {
    button.enabled = false;
    float startTime = Time.time;
    while (Time.time - startTime < coolTime) {
      if (coolIcon == null) return;
      coolIcon.fillAmount = 1.0f - (Time.time - startTime) / coolTime;
      await Task.Delay((int)(Time.deltaTime * 1000.0f));
    }
    if (coolIcon == null) return;
    coolIcon.fillAmount = 0;
    button.enabled = true;
  }
}