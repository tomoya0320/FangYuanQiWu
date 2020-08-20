using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Wzb;

public class ExchangeCode : MonoBehaviour {
  public OpenOrCloseUIWithAnim openOrCloseUIWithAnim;
  private GameObject settlement;

  private async void Start() {
    settlement = await Addressables.LoadAssetAsync<GameObject>(StringMap.SETTLEMENT).Task;
  }
  public void GetGift(InputField inputField) {
    if (string.IsNullOrEmpty(inputField.text)) {
      MessageCenter.Instance.AddMessage(new Message(MessageStyle.ColorFade, StringMap.INPUT_EXCHANGE_CODE));
    } else if (inputField.text != StringMap.EXCHANGE_CODE) {
      MessageCenter.Instance.AddMessage(new Message(MessageStyle.ColorFade, StringMap.EXCHANGE_CODE_ERROR));
    } else {
      List<Prop> porpList = new List<Prop>
      {
                new Prop(2, 1000),
                new Prop(3, 20),
                new Prop(4, 10),
                new Prop(8, 10),
                new Prop(9, 20),
                new Prop(10, 10)
            };
      GameObject temp = Instantiate(settlement, transform.parent);
      temp.GetComponent<Settlement>().Init(porpList);

      Database.AddPropById(2, 1000);
      Database.AddPropById(3, 20);
      Database.AddPropById(4, 10);
      Database.AddPropById(8, 10);
      Database.AddPropById(9, 20);
      Database.AddPropById(10, 10);

      Database.AddPersonalityById(1);
      Database.AddPersonalityById(2);

      openOrCloseUIWithAnim.Close();
    }
  }
}