using UnityEngine;
using UnityEngine.UI;

public class ImageAlphaTest : MonoBehaviour {
  public float threshold = 0.5f;

  private void Start() {
    Image image = GetComponent<Image>();
    if (image) {
      image.alphaHitTestMinimumThreshold = threshold;
    }
  }
}