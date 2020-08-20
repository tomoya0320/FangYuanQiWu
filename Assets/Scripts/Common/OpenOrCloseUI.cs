using UnityEngine;

public class OpenOrCloseUI : MonoBehaviour {
  public GameObject target;

  public void OnClick() {
    if (target != null) {
      target.SetActive(!target.activeSelf);
    }
  }
}