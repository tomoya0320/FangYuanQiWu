using UnityEngine;
using UnityEngine.UI;

public class MapDataShowInExplore : SecondMenuManager {
  public Sprite selected;
  public Sprite unselected;
  public Text mapName;
  public Text mapLevel;
  public Text monsterLevel;
  public Text degreeOfExploration;
  public Text description;
  public Text consumeAP;
  public SceneJump sceneJumping;
  private JSONObject mapInfo;
  private MainMap ownedMainMap;
  public static MapDataShowInExplore currentClickedMap;
  protected override void Start() {
    base.Start();
    mapInfo = Database.GetMapInfoByIndex(transform.GetSiblingIndex());
    ownedMainMap = Database.GetOwnedMainMapByIndex(transform.GetSiblingIndex());
    UpdateInfo();
  }
  public override void SetClikced() {
    base.SetClikced();
    currentClickedMap = this;
    transform.SetAsLastSibling();
    button.image.sprite = selected;
    sceneJumping.consumeAP = (int)mapInfo[StringMap.AP].n;
    sceneJumping.nextSceneName = StringMap.MAP + "_" + (int)mapInfo[StringMap.ID].n;
    sceneJumping.GetComponent<Button>().interactable = true;
  }
  public override void LateClickEventDeal() {
    if (!isClicked) {
      if (currentClickedMap == this) {
        sceneJumping.GetComponent<Button>().interactable = false;
      }
      button.image.sprite = unselected;
    }
    base.LateClickEventDeal();
  }
  private void UpdateInfo() {
    if (mapInfo != null && ownedMainMap != null) {
      mapName.text = mapInfo[StringMap.NAME].str;
      mapLevel.text = mapInfo[StringMap.LEVEL].n.ToString();
      consumeAP.text = mapInfo[StringMap.AP].n + StringMap.AP;
      monsterLevel.text = mapInfo[StringMap.MONSTER_LEVEL].str;
      degreeOfExploration.text = string.Format("{0:N1}%", ownedMainMap.degreeOfExploration * 100.0f);
      description.text = mapInfo[StringMap.DESCRIPTION].str;
    } else {
      gameObject.SetActive(false);
    }
  }
}