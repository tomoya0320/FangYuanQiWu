using UnityEngine;

[RequireComponent(typeof(WButton))]
public class SecondMenuManager : MonoBehaviour {
  public bool alphaTest;
  public GameObject secondMenu;
  protected bool isClicked;
  protected WButton button;

  protected virtual void Start() {
    isClicked = false;
    button = GetComponent<WButton>();
    ChangeAlphaThreshold(alphaTest);
  }

  public virtual void SetClikced() {
    isClicked = true;
    button.scaleAnim = false;
    button.clickSound = false;
  }
  public virtual void LateClickEventDeal() {
    button.scaleAnim = !isClicked;
    button.clickSound = !isClicked;
    button.image.raycastTarget = !isClicked;
    ChangeAlphaThreshold(alphaTest);
    secondMenu.SetActive(isClicked);
    isClicked = false;
  }
  private void LateUpdate() {
    if (Input.GetMouseButtonUp(0)) {
      LateClickEventDeal();
    }
  }
  private void ChangeAlphaThreshold(bool alphaTest) {
    if (alphaTest) {
      button.image.alphaHitTestMinimumThreshold = isClicked ? 0 : 0.5f;
    }
  }
}