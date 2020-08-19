using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchEffectManager : SingletonMono<TouchEffectManager> {
  public GameObject effect;

  private void Update() {
    if (Input.GetMouseButtonDown(0) && !SceneManager.GetActiveScene().name.Contains("Map_")) {
      GameObject go = Instantiate(effect, transform);
      go.transform.position = Input.mousePosition;
    }
  }
}