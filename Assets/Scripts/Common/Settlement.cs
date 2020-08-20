using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class Settlement : MonoBehaviour {
  public Transform contents;
  public RectTransform close;
  public RectTransform title;
  public RectTransform bg;
  private GameObject propInSettlement;

  public async void Init(List<Prop> acquiredPropList) {
    #region 适配屏幕比例
    float rate = 0.5f * Screen.width / Screen.height;
    if (rate > 1.0f) {
      Vector2 bgNewSize = bg.sizeDelta;
      bgNewSize.x *= rate;
      bg.sizeDelta = bgNewSize;
      Vector2 titleNewPos = title.anchoredPosition;
      titleNewPos.x = -0.5f * Screen.width;
      title.anchoredPosition = titleNewPos;
      close.sizeDelta = new Vector2(close.sizeDelta.x * rate, close.sizeDelta.y);
    } else {
      close.sizeDelta = new Vector2(close.sizeDelta.x, close.sizeDelta.y / rate);
    }
    #endregion

    propInSettlement = await Addressables.LoadAssetAsync<GameObject>(StringMap.PROP_IN_SETTLEMENT).Task;

    GridLayoutGroup glg = contents.GetComponent<GridLayoutGroup>();
    float length = glg.cellSize.x + glg.spacing.x;
    if (length * acquiredPropList.Count > contents.GetComponent<RectTransform>().sizeDelta.x) {
      Vector2 size = contents.GetComponent<RectTransform>().sizeDelta;
      size.x = length * acquiredPropList.Count;
      contents.GetComponent<RectTransform>().sizeDelta = size;
    }
    foreach (Prop myProp in acquiredPropList) {
      GameObject tempGo = Instantiate(propInSettlement, contents);
      PropInSettlement pis = tempGo.GetComponent<PropInSettlement>();
      pis.Init(myProp);
    }
  }
  public void Close() {
    Destroy(gameObject);
  }
}