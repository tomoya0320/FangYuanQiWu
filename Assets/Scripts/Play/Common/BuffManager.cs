using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Buff {
  public int id;
  public string name;
  public string iconFileName;
  public string description;
  public int maxStackNum;
  public bool isDebuff;
  public float duration;
  public float startTime;
  public int layerNum;
  public List<Effect> effects;

  public Buff() {
    layerNum = 1;
    startTime = Time.time;
  }
}

[Serializable]
public class Effect {
  public int classTag;
  public float value;
  public string description;
}

public class BuffManager : MonoBehaviour {
  public RectTransform shortBuffInfoContent;
  public RectTransform detailBuffInfoContent;
  private GameObject shortBuffInfo;
  private GameObject detailBuffInfo;
  private List<Buff> buffs;
  private List<BuffInfoShow> buffInfoShows;

  private async void Start() {
    buffs = new List<Buff>();
    buffInfoShows = new List<BuffInfoShow>();
    shortBuffInfo = await Addressables.LoadAssetAsync<GameObject>(StringMap.SHORT_BUFF_INFO).Task;
    detailBuffInfo = await Addressables.LoadAssetAsync<GameObject>(StringMap.DETAIL_BUFF_INFO).Task;
  }
  private void Update() {
    for (int i = buffInfoShows.Count - 1; i >= 0; i--) {
      if (!buffInfoShows[i].OnUpdate()) {
        buffs.Remove(buffInfoShows[i].buff);
        buffInfoShows.RemoveAt(i);
      }
    }
  }
  public void AddBuff(Buff buff) {
    int index = buffs.FindIndex(b => {
      return b.id == buff.id;
    });
    if (index >= 0) {
      if (buff.maxStackNum > 1) {
        buffs[index].layerNum = Mathf.Clamp(buffs[index].layerNum + 1, 1, buff.maxStackNum);
      } else {
        buffs[index].startTime = Time.time;
      }
      return;
    }
    buffs.Add(buff);
    GameObject tempShort = Instantiate(shortBuffInfo, shortBuffInfoContent);
    if (detailBuffInfoContent) {
      GameObject tempDetail = Instantiate(detailBuffInfo, detailBuffInfoContent);
      tempShort.GetComponent<BuffInfoShow>().Init
      (buff, tempDetail.GetComponent<DetailBuffInfo>(), detailBuffInfoContent, AddBuffInfoShow);
    } else {
      tempShort.GetComponent<BuffInfoShow>().Init(buff, AddBuffInfoShow);
    }
  }
  private void AddBuffInfoShow(BuffInfoShow buffInfoShow) {
    buffInfoShows.Add(buffInfoShow);
  }
  public float GetBuffValueByClassTag(int classTag) {
    float value = 1.0f;
    if (classTag > 0) {
      foreach (Buff buff in buffs) {
        float temp = buff.isDebuff ? -1.0f : 1.0f;
        foreach (Effect effect in buff.effects) {
          if (effect.classTag == classTag) {
            value *= 1 + temp * effect.value * buff.layerNum;
          }
        }
      }
    }
    return value;
  }
  public int GetBuffValueForHP() {
    int value = 0;
    foreach (Buff buff in buffs) {
      int temp = buff.isDebuff ? -1 : 1;
      foreach (Effect effect in buff.effects) {
        if (effect.classTag == 0) {
          value += (int)(temp * effect.value);
        }
      }
    }
    return value;
  }
}