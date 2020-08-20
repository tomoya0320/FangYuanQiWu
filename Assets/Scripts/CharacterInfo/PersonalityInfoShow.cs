using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Wzb;

public class PersonalityInfoShow : MonoBehaviour {
  public GameObject leftButton;
  public GameObject rightButton;
  public Button switchButton;
  public Text characteristic;
  public Image[] starImgs;
  public Sprite[] starSprites;
  public RectTransform upstarPropContent;
  public Text[] propertyTexts;
  public Image[] propertyImgs;
  public Image illustration;

  private int currentIndex;
  private JSONObject personalityInfo;
  private List<Personality> ownedPersonalities;
  private List<GameObject> upstarProps;
  private GameObject upstarPropPrefab;

  private async void Start() {
    upstarPropPrefab = await Addressables.LoadAssetAsync<GameObject>(StringMap.UP_STAR_PROP).Task;
    EventCenter.AddListener(Wzb.EventType.UpdatePersonalityInfo, UpdatePersonalityInfo);
    ownedPersonalities = Database.GetOwnedPersonalities();
    currentIndex = ownedPersonalities.FindIndex(personality => personality.id == Database.CurrentPersonalityId);
    upstarProps = new List<GameObject>();
    UpdatePersonalityInfo();
  }
  private void OnDestroy() {
    EventCenter.RemoveListener(Wzb.EventType.UpdatePersonalityInfo, UpdatePersonalityInfo);
  }
  public async void UpdatePersonalityInfo() {
    leftButton.SetActive(currentIndex != 0);
    rightButton.SetActive(currentIndex != ownedPersonalities.Count - 1);
    switchButton.interactable = Database.CurrentPersonality != ownedPersonalities[currentIndex];

    personalityInfo = Database.GetPersonalityInfoByIndex(ownedPersonalities[currentIndex].id);
    for (int i = 0; i < starImgs.Length; i++) {
      starImgs[i].sprite = starSprites[i < ownedPersonalities[currentIndex].rarity ? 1 : 0];
    }
    foreach (GameObject tempGo in upstarProps) {
      Destroy(tempGo);
    }
    upstarProps.Clear();
    if (ownedPersonalities[currentIndex].rarity < 5) {
      foreach (JSONObject json in personalityInfo[StringMap.UP_STAR_PROPS].list) {
        GameObject temp = Instantiate(upstarPropPrefab, upstarPropContent);
        int propNum = Database.GetPropNumById((int)json[StringMap.ID].n);
        temp.GetComponent<UpStarProp>().Init((int)json[StringMap.ID].n, propNum, (int)json[StringMap.NUM].n);
        upstarProps.Add(temp);
      }
    }
    characteristic.text = personalityInfo[StringMap.TRAIT].str;

    SetProperty(propertyTexts[0], propertyImgs[0], (int)ownedPersonalities[currentIndex].maxHP);
    SetProperty(propertyTexts[1], propertyImgs[1], (int)ownedPersonalities[currentIndex].defence);
    SetProperty(propertyTexts[2], propertyImgs[2], (int)ownedPersonalities[currentIndex].moveSpeed);
    SetProperty(propertyTexts[3], propertyImgs[3], (int)ownedPersonalities[currentIndex].attack);

    illustration.sprite = await Addressables.LoadAssetAsync<Sprite>
    (personalityInfo[StringMap.ILL2_FILE_NAMES].list[ownedPersonalities[currentIndex].illustration2Index].str).Task;
  }
  private void SetProperty(Text text, Image img, float value) {
    text.text = value.ToString();
    img.fillAmount = value / 1000.0f;
  }
  public void SwitchPersonality(int delta) {
    currentIndex = Mathf.Clamp(currentIndex + delta, 0, ownedPersonalities.Count - 1);
    UpdatePersonalityInfo();
  }
  public void SetCurrentPersonality() {
    switchButton.interactable = false;
    Database.SetCurrentPersonalityById(ownedPersonalities[currentIndex].id);
  }
  public void UpStar() {
    if (ownedPersonalities[currentIndex].rarity == 5) {
      MessageCenter.Instance.AddMessage(new Message(MessageStyle.ColorFade, StringMap.BEEN_TOP_STAR));
    } else {
      foreach (GameObject temp in upstarProps) {
        if (temp.GetComponent<UpStarProp>().mask.activeSelf) {
          MessageCenter.Instance.AddMessage(new Message(MessageStyle.ColorFade, StringMap.PROP_NOT_ENOUGH));
          return;
        }
      }
      foreach (JSONObject json in personalityInfo[StringMap.UP_STAR_PROPS].list) {
        Database.ConsumePropById(-1, (int)json[StringMap.ID].n, (int)json[StringMap.NUM].n);
      }
      Database.UpPersonalityStar(ownedPersonalities[currentIndex]);
      UpdatePersonalityInfo();
      MessageCenter.Instance.AddMessage(new Message(MessageStyle.ColorFade, StringMap.UP_STAR_SUCCESS));
    }
  }
}