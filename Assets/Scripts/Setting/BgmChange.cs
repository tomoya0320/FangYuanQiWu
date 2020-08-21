using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class BgmChange : MonoBehaviour {
  public Dropdown bgmList;
  private AudioClip audioClip;
  private List<JSONObject> bgms;

  private async void Start() {
    TextAsset textAsset = await Addressables.LoadAssetAsync<TextAsset>(StringMap.BGMS).Task;
    JSONObject json = new JSONObject(textAsset.text);
    bgms = new List<JSONObject>(json[StringMap.BGMS].list);
    List<string> bgmNames = new List<string>();
    foreach(var bgm in bgms) {
      bgmNames.Add(bgm[StringMap.NAME].str);
    }
    bgmList.AddOptions(bgmNames);
    bgmList.SetValueWithoutNotify(bgms.FindIndex(b => b[StringMap.KEY].str == BgmManager.Instance.audioClip.name));
  }
  public async void EventOnChangeBgm() {
    audioClip = await Addressables.LoadAssetAsync<AudioClip>(bgms[bgmList.value][StringMap.KEY].str).Task;
    BgmManager.Instance.ChangeBgm(audioClip);
    BgmManager.Instance.PlayBgm();
  }
}