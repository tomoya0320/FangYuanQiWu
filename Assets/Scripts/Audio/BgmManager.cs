using DG.Tweening;
using UnityEngine;

public class BgmManager : SingletonMono<BgmManager> {
  public AudioClip audioClip { private set; get; }

  private Tween tween;
  private float keyValue;
  private float keySwitch;
  private AudioSource audioSource;
  private SettingType settingType;

  protected override void Awake() {
    base.Awake();
    settingType = SettingType.Bgm;
    audioSource = GetComponent<AudioSource>();
    audioClip = audioSource.clip;
    UpdateBgm();
  }
  public void UpdateBgm() {
    keySwitch = PlayerPrefs.GetInt(settingType.ToString() + StringMap.SWITCH);
    keyValue = 0.01f * PlayerPrefs.GetInt(settingType.ToString() + StringMap.VALUE);
    audioSource.volume = keySwitch * keyValue;
  }
  public void StopBgm() {
    if (tween != null) {
      tween.Complete();
    }
    tween = audioSource.DOFade(0, 1.0f).OnComplete(() => {
      audioSource.Stop();
      tween = null;
    });
  }
  public void PlayBgm() {
    audioSource.clip = audioClip;
    if (tween != null) {
      tween.Complete();
    }
    audioSource.Play();
    tween = audioSource.DOFade(keySwitch * keyValue, 1.0f).OnComplete(() => {
      tween = null;
    });
  }
  /// <summary>
  /// 这时候BGM并没有真正切换，还需要再调用一次PlayBgm方法
  /// </summary>
  /// <param name="clip"></param>
  public void ChangeBgm(AudioClip clip) {
    audioClip = clip;
  }
}