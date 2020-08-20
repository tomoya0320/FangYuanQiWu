using System.Collections;
using UnityEngine;

public class FriedChickenTakeout : MonoBehaviour {
  public Property playerProperty;
  public float coolTime;
  private bool isCooled;
  public float threshold;
  public float hpRecovery;

  private void Start() {
    isCooled = true;
  }
  private void Update() {
    if (playerProperty.GetCurrentHP() / playerProperty.GetMaxHP() < threshold && isCooled) {
      isCooled = false;
      StartCoroutine(CoolSkill());
      playerProperty.SetCurrentHP(playerProperty.GetCurrentHP() + playerProperty.GetMaxHP() * hpRecovery);
    }
  }
  private IEnumerator CoolSkill() {
    yield return new WaitForSeconds(coolTime);
    isCooled = true;
  }
}