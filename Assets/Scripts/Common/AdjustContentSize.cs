using UnityEngine;
using UnityEngine.UI;

public class AdjustContentSize : MonoBehaviour {
  [SerializeField]
  private RectTransform _rectTransform;
  [SerializeField]
  private GridLayoutGroup _glg;

  private void OnEnable() {
    int activeChildCount = 0;
    for (int i = 0; i < transform.childCount; i++) {
      if (transform.GetChild(i).gameObject.activeSelf) {
        activeChildCount++;
      }
    }
    float height = _glg.cellSize.y + _glg.spacing.y;
    _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, height * activeChildCount);
  }
}