using UnityEngine;
using UnityEngine.UI;

public class PropUIInMap : SingletonMono<PropUIInMap> {
  [Header("UI显示")]
  public Image operateImg;
  public Button operateButton;
  public GameObject propMenu;
}