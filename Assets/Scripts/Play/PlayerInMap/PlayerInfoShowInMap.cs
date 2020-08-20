using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class PlayerInfoShowInMap : MonoBehaviour
{
    [Header("等级和经验")]
    public Text lvText;
    public RectTransform expImg;
    public Text expText;
    private float maxExpLength;
    [Header("头像和昵称")]
    public Image headIcon;
    public Text nickname;
    [Header("血条相关")]
    public Text hpText;
    public RectTransform hpImg;
    public RectTransform hpEffectImg;
    private float maxHpLength;
    [Header("精力相关")]
    public Text energyText;
    public RectTransform energyImg; 
    public RectTransform energyEffectImg;

    private float maxEnergyLength;
    private float currentHPRate;
    private float currentShowHPRate;
    private float currentShowHPEffectRate;
    private string[] energyInfo;
    private float currentEnergyRate;
    private float currentShowEnergyRate;
    private float currentShowEnergyEffectRate;

    // Start is called before the first frame update
    private void Start()
    {
        EventCenter.AddListener(Wzb.EventType.UpdateUserInfo, UpdateUserInfo);
        maxExpLength = expImg.rect.width;
        maxHpLength = hpImg.rect.width;
        maxEnergyLength = energyImg.rect.width;
        InitHPAndEnergyImg();
        UpdateUserInfo();
    }
    private void Update()
    {
        //更新血条效果
        currentHPRate = PlayerController.GetComp<PlayerProperty>().GetCurrentHP() / PlayerController.GetComp<PlayerProperty>().GetMaxHP();
        currentShowHPRate = hpImg.sizeDelta.x / maxHpLength;
        currentShowHPEffectRate = hpEffectImg.sizeDelta.x / maxHpLength;
        UpdateEffect(currentHPRate, currentShowHPRate, currentShowHPEffectRate, hpImg, hpEffectImg, maxHpLength,hpText, PlayerController.GetComp<PlayerProperty>().GetMaxHP());
        //更新精力条效果
        energyInfo = Database.EnergyInfo.Split('/');
        currentEnergyRate = float.Parse(energyInfo[0]) / float.Parse(energyInfo[1]);
        currentShowEnergyRate = energyImg.sizeDelta.x / maxEnergyLength;
        currentShowEnergyEffectRate = energyEffectImg.sizeDelta.x / maxEnergyLength;
        UpdateEffect(currentEnergyRate, currentShowEnergyRate, currentShowEnergyEffectRate, energyImg, energyEffectImg, maxEnergyLength,energyText, float.Parse(energyInfo[1]));
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(Wzb.EventType.UpdateUserInfo, UpdateUserInfo);
    }
    public async void UpdateUserInfo()
    {
        lvText.text = Database.Level.ToString();
        float needEXP = Utils.CalNeededExp(Database.Level);
        expText.text = Database.UserExp + "/" + needEXP;
        expImg.sizeDelta = new Vector2(maxExpLength * (Database.UserExp / needEXP), expImg.sizeDelta.y);

        nickname.text = Database.Nickname;
        headIcon.sprite = await Addressables.LoadAssetAsync<Sprite>
        (Database.GetPersonalityInfoByIndex(Database.CurrentPersonalityId)["headIconFileName"].str).Task;
    }
    private void InitHPAndEnergyImg()
    {
        hpText.text = PlayerController.GetComp<PlayerProperty>().GetCurrentHP() + "/" + PlayerController.GetComp<PlayerProperty>().GetMaxHP();
        hpImg.sizeDelta = new Vector2(maxHpLength * (PlayerController.GetComp<PlayerProperty>().GetCurrentHP() / PlayerController.GetComp<PlayerProperty>().GetMaxHP()), hpImg.sizeDelta.y);
        hpEffectImg.sizeDelta = hpImg.sizeDelta;

        string[] energyInfo = Database.EnergyInfo.Split('/'); 
        energyText.text = energyInfo[0] + "/" + energyInfo[1];
        energyImg.sizeDelta = new Vector2(maxEnergyLength * (float.Parse(energyInfo[0]) / float.Parse(energyInfo[1])), energyImg.sizeDelta.y);
        energyEffectImg.sizeDelta = energyImg.sizeDelta;
    }
    private void UpdateEffect(float currentRate, float currentShowRate, float currentShowEffectRate, RectTransform img, RectTransform effectImg, float maxLength,Text text,float maxNum)
    {
        if (currentRate > currentShowRate)
        {
            currentShowRate = Mathf.Clamp(currentShowRate + Time.deltaTime, currentShowRate, currentRate);
            currentShowEffectRate = currentShowRate;
        }
        else
        {
            currentShowRate = currentRate;
            currentShowEffectRate = Mathf.Clamp(currentShowEffectRate - Time.deltaTime * 0.5f, currentShowRate, currentShowEffectRate);
        }
        img.sizeDelta = new Vector2(currentShowRate * maxLength, img.sizeDelta.y);
        effectImg.sizeDelta = new Vector2(currentShowEffectRate * maxLength, effectImg.sizeDelta.y);
        text.text = Mathf.RoundToInt(currentShowRate * maxNum) + "/" + maxNum;
    }
}
