using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public enum SettingType {
  Bgm,
  Sound,
}

public class SettingSwitch : MonoBehaviour {
  public SettingType settingType;
  public Slider valueSlider;
  public Text closeText;
  public Text openText;
  public RectTransform switchSlider;
  private float switchSliderMoveDist;
  private Tween tween;

  private void Start() {
    switchSliderMoveDist = GetComponent<RectTransform>().sizeDelta.x * 0.5f;
    if (PlayerPrefs.GetInt(settingType.ToString() + StringMap.SWITCH) <= 0) {
      Color temp = closeText.color;
      closeText.color = openText.color;
      openText.color = temp;
      switchSlider.anchoredPosition -= new Vector2(switchSliderMoveDist, 0);
      switchSliderMoveDist *= -1.0f;
    }
    if (valueSlider) {
      valueSlider.value = PlayerPrefs.GetInt(settingType.ToString() + StringMap.VALUE);
    }
  }
  public void ChangeSwitch() {
    PlayerPrefs.SetInt(settingType.ToString() + StringMap.SWITCH, 
    (PlayerPrefs.GetInt(settingType.ToString() + StringMap.SWITCH) + 1) % 2);
    Color temp = closeText.color;
    closeText.color = openText.color;
    openText.color = temp;
    switchSliderMoveDist *= -1.0f;
    if (tween != null) {
      tween.Complete();
    }
    tween = switchSlider.DOAnchorPosX(switchSliderMoveDist, 0.15f).SetRelative().OnComplete(() => {
      tween = null;
    });
    UpdateState();
  }
  public void ChangeValue() {
    PlayerPrefs.SetInt(settingType.ToString() + StringMap.VALUE, (int)valueSlider.value);
    UpdateState();
  }
  private void UpdateState() {
    switch (settingType) {
      case SettingType.Bgm: {
          BgmManager.Instance.UpdateBgm();
          break;
        }
      case SettingType.Sound: {
          SoundManager.Instance.UpdateSound();
          break;
        }
    }
  }
}