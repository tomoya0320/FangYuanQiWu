using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
[RequireComponent(typeof(Image))]
public class WVPlayer : MonoBehaviour {
  public Dropdown videoList;

  private Image mask;
  private VideoPlayer videoPlayer;
  private List<JSONObject> videos;

  private async void Start() {
    mask = GetComponent<Image>();
    videoPlayer = GetComponent<VideoPlayer>();

    TextAsset textAsset = await Addressables.LoadAssetAsync<TextAsset>(StringMap.VIDEOS).Task;
    JSONObject json = new JSONObject(textAsset.text);
    videos = new List<JSONObject>(json[StringMap.VIDEOS].list);
    List<string> videoNames = new List<string>();
    foreach (var video in videos) {
      videoNames.Add(video[StringMap.NAME].str);
    }
    videoList.AddOptions(videoNames);
  }
  private void Update() {
    if (Input.GetKeyDown(KeyCode.Escape)) {
      videoPlayer.Stop();
      mask.enabled = false;
    }
  }
  public void EventOnPlay() {
    mask.enabled = true;
    videoPlayer.url = videos[videoList.value][StringMap.KEY].str;
    videoPlayer.Play();
  }
}