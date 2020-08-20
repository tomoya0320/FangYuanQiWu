using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchEffectManager : SingletonMono<TouchEffectManager> {
  public GameObject effect;

  private void Update() {
    if (Input.GetMouseButtonDown(0) && !SceneManager.GetActiveScene().name.Contains(StringMap.MAP)) {
      GameObject go = Instantiate(effect, transform);
      go.transform.position = Input.mousePosition;
    }
  }
}