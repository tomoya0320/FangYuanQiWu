using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class PropInSettlement : MonoBehaviour {
  public Image icon;
  public Text num;

  public async void Init(Prop prop) {
    num.text = prop.num.ToString();

    icon.sprite = await Addressables.LoadAssetAsync<Sprite>
    (Database.GetPropInfoById(prop.id)[StringMap.ICON_FILE_NAME].str).Task;
  }
  public async void Init(int propId, int propNum) {
    num.text = propNum.ToString();

    icon.sprite = await Addressables.LoadAssetAsync<Sprite>
    (Database.GetPropInfoById(propId)[StringMap.ICON_FILE_NAME].str).Task;
  }
}