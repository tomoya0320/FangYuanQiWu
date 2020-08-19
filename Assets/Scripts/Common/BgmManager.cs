using DG.Tweening;
using UnityEngine;

public class BgmManager : SingletonMono<BgmManager>
{
    private Tween tween;
    private AudioClip audioClip;
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
  public void UpdateBgm()
    {
        keySwitch = PlayerPrefs.GetInt(settingType.ToString() + "Switch");
        keyValue = 0.01f * PlayerPrefs.GetInt(settingType.ToString() + "Value");
        audioSource.volume = keySwitch * keyValue;
    }
    public void StopBgm()
    {
        if (tween != null)
        {
            tween.Complete();
        }
        tween = audioSource.DOFade(0, 1.0f).OnComplete(() =>
        {
            audioSource.Stop();
            tween = null;
        });
    }
    public void PlayBgm()
    {
        audioSource.clip = audioClip;
        if (tween != null)
        {
            tween.Complete();
        }
        audioSource.Play();
        tween = audioSource.DOFade(keySwitch * keyValue, 1.0f).OnComplete(() =>
        {
            tween = null;
        });
    }
    public void ChangeBgm(AudioClip clip)
    {
        audioClip = clip;
    }
}