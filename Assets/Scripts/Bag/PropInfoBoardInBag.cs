using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class PropInfoBoardInBag : PropOperate {
  public Image propIcon;
  public Text propNum;
  public Text propName;
  public Text price;
  public Text description;

  public override async void UpdateInfo() {
    if (selectedProp.ownedPropData == null) {
      gameObject.SetActive(false);
      return;
    }
    gameObject.SetActive(true);

    selectedPropInfo = DataBase.GetPropInfoById(selectedProp.ownedPropData.id);
    propNum.text = "*" + selectedProp.ownedPropData.num;
    propName.text = selectedPropInfo[StringMap.NAME].str;
    price.text = selectedPropInfo[StringMap.PRICE].n.ToString();
    useButton.interactable = CanUse();
    description.text = selectedPropInfo[StringMap.DESCRIPTION].str.Replace("\\n", "\n");

    propIcon.sprite = await Addressables.LoadAssetAsync<Sprite>(selectedPropInfo[StringMap.ICON_FILE_NAME].str).Task;
  }
  protected override bool CanUse() {
    return selectedPropInfo[StringMap.CLASS].n == 1;
  }
  public void SellProp() {
    if (selectedProp && selectedProp.ownedPropData != null) {
      confirmationBox.Open(CBType.SELL, selectedProp.ownedPropData.num, selectedPropInfo,
      selectedNum => { ConsumeProp(selectedPropInfo, selectedNum, 1); });
    }
  }
  public void TransferProp() {
    if (selectedProp && selectedProp.ownedPropData != null) {
      confirmationBox.Open(CBType.OK, selectedProp.ownedPropData.num, selectedPropInfo,
      selectedNum => { Transfer(selectedNum); });
    }
  }
  private void Transfer(int num) {
    if (num > 0) {
      DataBase.TransferProp(selectedProp.ownedPropData.id, num, PropInBag.propPos, PropInBag.toPos);
    }
  }
}