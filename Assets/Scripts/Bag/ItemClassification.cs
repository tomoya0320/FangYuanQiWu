using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemClassification : MonoBehaviour {
  public static ItemClassification instance;
  public List<Toggle> toggles;
  public List<int> classTags;

  private void Awake() {
    instance = this;
    classTags = new List<int>();
    for (int i = 0; i < toggles.Count; i++) {
      if (toggles[i].isOn) {
        classTags.Add(i);
      }
    }
  }
  public void UpdateItems(Toggle toggle) {
    int classTag = toggles.IndexOf(toggle);
    if (classTag >= 0) {
      if (toggle.isOn) {
        classTags.Add(classTag);
      } else {
        classTags.Remove(classTag);
      }
    }
    EventCenter.Broadcast(Wzb.EventType.UpdatePropInfo);
  }
}