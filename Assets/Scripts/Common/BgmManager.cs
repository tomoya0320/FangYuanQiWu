using DG.Tweening;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    private static Tween tween;
    private static AudioClip audioClip;
    private static float keyValue;
    private static float keySwitch;
    private static AudioSource audioSource;
    private static SettingType settingType;

    private void Start()
    {
        settingType = SettingType.Bgm;
        audioSource = GetComponent<AudioSource>();
        audioClip = audioSource.clip;
        UpdateBgm();
    }
    public static void UpdateBgm()
    {
        keySwitch = PlayerPrefs.GetInt(settingType.ToString() + "Switch");
        keyValue = 0.01f * PlayerPrefs.GetInt(settingType.ToString() + "Value");
        audioSource.volume = keySwitch * keyValue;
    }
    public static void StopBgm()
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
    public static void PlayBgm()
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
    public static void ChangeBgm(AudioClip clip)
    {
        audioClip = clip;
    }
}