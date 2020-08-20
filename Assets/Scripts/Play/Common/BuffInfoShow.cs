using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class BuffInfoShow : MonoBehaviour {
  public Image buffIcon;
  public Image leftTime;

  public CanvasGroup canvasGroup;
  private Tween tween;

  private Buff buff;
  private List<Buff> buffs;
  private DetailBuffInfo detailBuffInfo;
  private RectTransform detailBuffContent;

  private float length;
  private int minute;
  private int second;
  private float duration;
  private bool isNoDetail;

  public async void Init(Buff buff, DetailBuffInfo detailBuffInfo, RectTransform detailBuffContent, List<Buff> buffs) {
    this.buff = buff;
    this.buffs = buffs;
    this.detailBuffInfo = detailBuffInfo;
    this.detailBuffContent = detailBuffContent;

    GridLayoutGroup grid = detailBuffContent.GetComponent<GridLayoutGroup>();
    length = grid.cellSize.y + grid.spacing.y;
    detailBuffContent.sizeDelta += new Vector2(0, length);

    Color color = buff.isDebuff ? new Color(1.0f, 0.325f, 0.325f) : new Color(0.263f, 0.914f, 0.616f);
    detailBuffInfo.buffName.color = color;
    detailBuffInfo.leftTime.color = color;
    detailBuffInfo.buffName.text = buff.name;
    detailBuffInfo.description.text = buff.description;
    detailBuffInfo.buffIcon.sprite = await Addressables.LoadAssetAsync<Sprite>(buff.iconFileName).Task;
    buffIcon.sprite = detailBuffInfo.buffIcon.sprite;
  }
  public async void Init(Buff buff, List<Buff> buffs) {
    isNoDetail = true;
    this.buff = buff;
    this.buffs = buffs;
    buffIcon.sprite = await Addressables.LoadAssetAsync<Sprite>(buff.iconFileName).Task;
  }
  private void Update() {
    //表示持续时间无限
    if (buff.duration < 0) {
      return;
    }
    duration = Time.time - buff.startTime;
    if (duration > buff.duration) {
      buffs.Remove(buff);
      if (!isNoDetail) {
        detailBuffContent.sizeDelta -= new Vector2(0, length);
        Destroy(detailBuffInfo.gameObject);
      }
      Destroy(gameObject);
    } else {
      if (buff.duration - duration > 2.0f && buff.duration - duration <= 5.0f) {
        if (tween == null) {
          tween = canvasGroup.DOFade(0.6f, 0.3f).SetLoops(-1, LoopType.Yoyo);
        }
      } else {
        if (tween != null) {
          tween.Kill();
          tween = null;
          canvasGroup.alpha = 1.0f;
        }
      }
      leftTime.fillAmount = duration / buff.duration;
      if (!isNoDetail) {
        detailBuffInfo.layerNum.text = "*" + buff.layerNum;
        minute = (int)((buff.duration - duration) / 60.0f);
        second = (int)((buff.duration - duration) % 60.0f);
        detailBuffInfo.leftTime.text = minute.ToString().PadLeft(2, '0') + ":" + second.ToString().PadLeft(2, '0');
      }
    }
  }
}