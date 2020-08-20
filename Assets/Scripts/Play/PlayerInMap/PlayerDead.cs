using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class PlayerDead : MonoBehaviour {
  public Text deadTip;
  public RectTransform content;
  public CanvasGroup canvasGroup;
  public GrayScale grayScale;
  private List<GameObject> propInSettlementList;
  private List<Prop> ownedPropList;
  private GameObject propInSettlement;
  private HashSet<int> indexSet;

  public async void Init() {
    propInSettlement = await Addressables.LoadAssetAsync<GameObject>(StringMap.PROP_IN_SETTLEMENT).Task;
    propInSettlementList = new List<GameObject>();
    ownedPropList = new List<Prop>();
  }
  public void Hide() {
    gameObject.SetActive(false);
    grayScale.enabled = false;
  }
  public void Show() {
    ownedPropList.Clear();
    ownedPropList.AddRange(Database.GetOwnedPropListByClassTag(0));
    ownedPropList.AddRange(Database.GetOwnedPropListByClassTag(2));

    foreach (GameObject temp in propInSettlementList) {
      Destroy(temp);
    }
    propInSettlementList.Clear();

    gameObject.SetActive(true);
    grayScale.enabled = true;
    canvasGroup.alpha = 0;
    canvasGroup.DOFade(1.0f, 0.5f);

    if (ownedPropList.Count == 0) {
      deadTip.text = StringMap.YOU_ARE_STUNNED;
    } else {
      deadTip.text = StringMap.YOU_ARE_STUNNED_AND_LOSE_PROPS;
      indexSet = GetRandomIndex(Random.Range(1, 4));
      foreach (int index in indexSet) {
        int consumeNum = Random.Range(1, 3);
        if (consumeNum > ownedPropList[index].num) {
          consumeNum = ownedPropList[index].num;
        }
        Database.ConsumePropById(0, ownedPropList[index].id, consumeNum);
        GameObject temp = Instantiate(propInSettlement, content);
        temp.GetComponent<PropInSettlement>().Init(ownedPropList[index].id, consumeNum);
        propInSettlementList.Add(temp);
      }
    }
  }
  private HashSet<int> GetRandomIndex(int randomNum) {
    int index = 0;
    HashSet<int> indexSet = new HashSet<int>();
    randomNum = randomNum > ownedPropList.Count ? ownedPropList.Count : randomNum;
    while (randomNum-- > 0) {
      while (indexSet.Contains(index)) {
        index = Random.Range(0, ownedPropList.Count);
      }
      indexSet.Add(index);
    }
    return indexSet;
  }
}