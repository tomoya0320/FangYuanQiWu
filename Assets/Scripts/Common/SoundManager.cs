using UnityEngine;

public class SoundManager : SingletonMono<SoundManager> {
  private float keyValue;
  private float keySwitch;
  private AudioSource audioSource;
  private SettingType settingType;

  protected override void Awake() {
    base.Awake();
    settingType = SettingType.Sound;
    audioSource = GetComponent<AudioSource>();
    UpdateSound();
  }
  public void UpdateSound() {
    keySwitch = PlayerPrefs.GetInt(settingType.ToString() + "Switch");
    keyValue = 0.01f * PlayerPrefs.GetInt(settingType.ToString() + "Value");
    audioSource.volume = keySwitch * keyValue;
  }
  public void Play(AudioClip audioClip) {
    audioSource.clip = audioClip;
    audioSource.Play();
  }
}