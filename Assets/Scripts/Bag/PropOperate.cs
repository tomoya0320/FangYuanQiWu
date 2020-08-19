using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class PropOperate : MonoBehaviour {
  public Sprite[] itemBGImgs;
  public Button useButton;
  public Button discardButton;
  public PropConfirmation confirmationBox;

  protected PropInBag selectedProp;
  protected JSONObject selectedPropInfo;
  protected GameObject settlement;

  private async void Start() {
    settlement = await Addressables.LoadAssetAsync<GameObject>(StringMap.SETTLEMENT).Task;
    EventCenter.AddListener(Wzb.EventType.UpdatePropInfo, UpdateInfo);
    UpdateInfo();
  }
  private void OnDestroy() {
    EventCenter.RemoveListener(Wzb.EventType.UpdatePropInfo, UpdateInfo);
  }
  public virtual void UpdateInfo() {
    if (selectedProp && selectedProp.ownedPropData != null) {
      selectedPropInfo = DataBase.GetPropInfoById(selectedProp.ownedPropData.id);
      useButton.interactable = CanUse();
      discardButton.interactable = true;
    } else {
      useButton.interactable = false;
      discardButton.interactable = false;
    }
  }
  public void SetSelectedProp(PropInBag prop) {
    if (selectedProp) {
      selectedProp.GetComponent<Image>().sprite = itemBGImgs[0];
    }
    prop.GetComponent<Image>().sprite = itemBGImgs[1];
    selectedProp = prop;
    UpdateInfo();
  }
  public void DiscardProp() {
    if (selectedProp && selectedProp.ownedPropData != null) {
      confirmationBox.Open(CBType.DROP, selectedProp.ownedPropData.num, selectedPropInfo,
      selectedNum => { ConsumeProp(selectedPropInfo, selectedNum, 0); });
    }
  }
  public void UseProp() {
    if (selectedProp && selectedProp.ownedPropData != null) {
      if (selectedPropInfo[StringMap.CLASS].n == 0) {
        ConsumeProp(selectedPropInfo, 1, -1);
      } else {
        confirmationBox.Open(CBType.USE, selectedProp.ownedPropData.num, selectedPropInfo,
        selectedNum => { ConsumeProp(selectedPropInfo, selectedNum, -1); });
      }
    }
  }
  protected virtual bool CanUse() {
    return selectedPropInfo[StringMap.CLASS].n == 0 || selectedPropInfo[StringMap.CLASS].n == 1;
  }
  protected void ConsumeProp(JSONObject propInfo, int num, int flag) {
    if (num > 0 && DataBase.ConsumePropById(flag, (int)propInfo[StringMap.ID].n, num)) {
      if (flag == 1) {
        int price = (int)propInfo[StringMap.PRICE].n;
        DataBase.AddPropById(-1, price * num);
      } else if (flag == -1) {
        switch ((int)propInfo[StringMap.CLASS].n) {
          case 0: {
              BuffManager buffManager = PlayerController.GetComp<BuffManager>();
              PlayerProperty playerProperty = PlayerController.GetComp<PlayerProperty>();
              //产生效果
              if (propInfo.HasField(StringMap.HP_RECOVERY)) {
                playerProperty.SetCurrentHP(playerProperty.GetCurrentHP() + (int)propInfo[StringMap.HP_RECOVERY].n * num);
              }
              if (propInfo.HasField(StringMap.ENERGY_RECOVERY)) {
                DataBase.SetEnergyVariation((int)propInfo[StringMap.ENERGY_RECOVERY].n * num);
              }
              if (propInfo.HasField(StringMap.BUFFS)) {
                foreach (JSONObject json in propInfo[StringMap.BUFFS].list) {
                  buffManager.AddBuff(DataBase.GetBuffByIndex((int)json[StringMap.ID].n));
                }
              }
              break;
            }
          case 1: {
              //宝箱的使用
              List<Prop> acquiredPropList = new List<Prop>();
              if (propInfo[StringMap.FIXED_CONTENTS].HasField(StringMap.EXP)) {
                DataBase.AddExp(num * (int)propInfo[StringMap.FIXED_CONTENTS][StringMap.EXP].n);
              }
              if (propInfo[StringMap.FIXED_CONTENTS].HasField(StringMap.PROPS)) {
                foreach (JSONObject temp in propInfo[StringMap.FIXED_CONTENTS][StringMap.PROPS].list) {
                  acquiredPropList.Add(new Prop((int)temp[StringMap.ID].n, (int)temp[StringMap.NUM].n * num));
                  DataBase.AddPropById((int)temp[StringMap.ID].n, (int)temp[StringMap.NUM].n * num);
                }
              }
              if (propInfo[StringMap.RANDOM_CONTENTS].HasField(StringMap.PROPS)) {
                foreach (JSONObject json in propInfo[StringMap.RANDOM_CONTENTS][StringMap.PROPS].list) {
                  Prop prop = new Prop((int)json[StringMap.ID].n, 0);
                  if (json.HasField(StringMap.PROB)) {
                    for (int n = 0; n < num; n++) {
                      if (Random.Range(0, 1.0f) < json[StringMap.PROB].n) {
                        prop.num += (int)json[StringMap.NUM].n;
                      }
                    }
                  } else if (json.HasField(StringMap.RANGE)) {
                    for (int n = 0; n < num; n++) {
                      prop.num += Random.Range((int)json[StringMap.RANGE][StringMap.MIN].n, 
                                  (int)json[StringMap.RANGE][StringMap.MAX].n + 1);
                    }
                  }
                  if (prop.num > 0) {
                    acquiredPropList.Add(prop);
                    DataBase.AddPropById(prop.id, prop.num);
                  }
                }
              }
              if (acquiredPropList.Count > 0) {
                GameObject tmp = Instantiate(settlement, transform.parent);
                tmp.GetComponent<Settlement>().Init(acquiredPropList);
              }
              break;
            }
        }
      }
    }
  }
}