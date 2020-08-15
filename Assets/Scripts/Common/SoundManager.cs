using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static float keyValue;
    private static float keySwitch;
    private static AudioSource audioSource;
    private static SettingType settingType;

    private void Start()
    {
        settingType = SettingType.Sound;
        audioSource = GetComponent<AudioSource>();
        UpdateSound();
    }
    public static void UpdateSound()
    {
        keySwitch = PlayerPrefs.GetInt(settingType.ToString() + "Switch");
        keyValue = 0.01f * PlayerPrefs.GetInt(settingType.ToString() + "Value");
        audioSource.volume = keySwitch * keyValue;
    }
    public static void Play(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
