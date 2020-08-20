using UnityEngine;
using UnityEngine.UI;

public class MonsterInfoShowInMap : MonoBehaviour {
  [Header("血条相关")]
  public Text hpText;
  public RectTransform hpImg;
  public RectTransform hpEffectImg;
  private float maxHpLength;
  [Header("怪物属性")]
  public MonsterProperty monsterProperty;
  private float currentHPRate;
  private float currentShowHPRate;
  private float currentShowHPEffectRate;

  // Start is called before the first frame update
  private void Start() {
    maxHpLength = hpImg.rect.width;
    InitHP();
  }
  private void Update() {
    //更新血条效果
    currentHPRate = monsterProperty.GetCurrentHP() / monsterProperty.GetMaxHP();
    currentShowHPRate = hpImg.sizeDelta.x / maxHpLength;
    currentShowHPEffectRate = hpEffectImg.sizeDelta.x / maxHpLength;
    UpdateEffect(currentHPRate, currentShowHPRate, currentShowHPEffectRate, hpImg, hpEffectImg, maxHpLength, hpText, monsterProperty.GetMaxHP());
  }
  private void InitHP() {
    hpText.text = monsterProperty.GetCurrentHP() + "/" + monsterProperty.GetMaxHP();
    hpImg.sizeDelta = new Vector2(maxHpLength * (monsterProperty.GetCurrentHP() / monsterProperty.GetMaxHP()), hpImg.sizeDelta.y);
    hpEffectImg.sizeDelta = hpImg.sizeDelta;
  }
  private void UpdateEffect(float currentRate, float currentShowRate, float currentShowEffectRate, RectTransform img, RectTransform effectImg, float maxLength, Text text, float maxNum) {
    if (currentRate > currentShowRate) {
      currentShowRate = Mathf.Clamp(currentShowRate + Time.deltaTime, currentShowRate, currentRate);
      currentShowEffectRate = currentShowRate;
    } else {
      currentShowRate = currentRate;
      currentShowEffectRate = Mathf.Clamp(currentShowEffectRate - Time.deltaTime * 0.5f, currentShowRate, currentShowEffectRate);
    }
    img.sizeDelta = new Vector2(currentShowRate * maxLength, img.sizeDelta.y);
    effectImg.sizeDelta = new Vector2(currentShowEffectRate * maxLength, effectImg.sizeDelta.y);
    text.text = Mathf.RoundToInt(currentShowRate * maxNum) + "/" + maxNum;
  }
}