using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public enum CBType {
  SELL,
  USE,
  DROP,
  OK,
}
public class PropConfirmation : MonoBehaviour {
  public Text ok;
  public Text tip;
  public Text maxNum;
  public Slider slider;
  public Text currentNum;
  public Image propIcon;
  private CBType cbType;
  private Action<int> callback;
  private JSONObject propInfo;
  // Start is called before the first frame update
  public async void Open(CBType cbType, int num, JSONObject propInfo, Action<int> callback) {
    this.cbType = cbType;
    this.callback = callback;
    this.propInfo = propInfo;
    switch (cbType) {
      case CBType.SELL: {
          tip.text = StringMap.TOTAL_PRICE + ":" + propInfo[StringMap.PRICE].n;
          break;
        }
      default: {
          tip.text = StringMap.EMPTY;
          break;
        }
    }
    ok.text = StringMap.CBTypeToString(cbType);
    float rate = 0.5f * Screen.width / Screen.height;
    Vector2 newSize = GetComponent<RectTransform>().sizeDelta;
    if (rate > 1.0f) {
      newSize.x *= rate;
    } else {
      newSize.y /= rate;
    }
    GetComponent<RectTransform>().sizeDelta = newSize;
    maxNum.text = num.ToString();
    slider.maxValue = num;
    slider.value = 0;
    gameObject.SetActive(true);

    propIcon.sprite = await Addressables.LoadAssetAsync<Sprite>
    (propInfo[StringMap.ICON_FILE_NAME].str).Task;
  }
  public void Cancel() {
    SetCallback(0);
  }
  public void Confirm() {
    SetCallback((int)slider.value);
  }
  private void SetCallback(int num) {
    callback(num);
    gameObject.SetActive(false);
  }
  public void UpdateCurrentSliderNum() {
    currentNum.text = slider.value.ToString();
    switch (cbType) {
      case CBType.SELL: {
          float totalPrice = slider.value * propInfo[StringMap.PRICE].n;
          tip.text = StringMap.TOTAL_PRICE + ":" + totalPrice;
          break;
        }
    }
  }
}