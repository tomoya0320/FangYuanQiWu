using UnityEngine;

public class PropBuffInNear : MonoBehaviour {
  public int buffId;
  private Buff buff;

  private void Start() {
    buff = Database.GetBuffByIndex(buffId);
  }
  private void OnTriggerEnter(Collider other) {
    if (other.tag.Contains(StringMap.PLAYER)) {
      buff.duration = -1.0f;
      other.GetComponent<BuffManager>().AddBuff(buff);
    }
  }
  private void OnTriggerExit(Collider other) {
    if (other.tag.Contains(StringMap.PLAYER)) {
      buff.duration = 0;
    }
  }
}