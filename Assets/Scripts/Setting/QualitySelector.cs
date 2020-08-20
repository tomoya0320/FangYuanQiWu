using UnityEngine;
using UnityEngine.UI;

public class QualitySelector : MonoBehaviour {
  public Toggle[] toggles;
  private int quality;

  private void Start() {
    quality = PlayerPrefs.GetInt(StringMap.QUALITY);
    QualitySettings.SetQualityLevel(quality);
    toggles[quality].isOn = true;
  }
  public void ChangeQuality() {
    for (int i = 0; i < toggles.Length; i++) {
      if (toggles[i].isOn) {
        PlayerPrefs.SetInt(StringMap.QUALITY, i);
        QualitySettings.SetQualityLevel(i);
        break;
      }
    }
  }
}