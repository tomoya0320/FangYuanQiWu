using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class UserDataShowInMM : MonoBehaviour
{
    public Text ap;
    public Text crystal;
    public Text gold;

    public Image playerIcon;
    public Text lv;
    public Text nickname;
    public Text title;
    public Image titleBg;
    public Text expText;
    public Image expImg;

    // Start is called before the first frame update
    private void Start()
    {
        EventCenter.AddListener(Wzb.EventType.UpdateUserInfo, UpdateUserData);
        UpdateUserData();
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(Wzb.EventType.UpdateUserInfo, UpdateUserData);
    }
    public async void UpdateUserData()
    {
        ap.text = DataBase.GetAPInfo();
        gold.text = DataBase.GetPropNumById(-1).ToString();
        crystal.text = DataBase.GetPropNumById(-2).ToString();

        lv.text = DataBase.GetLevel().ToString();
        nickname.text = DataBase.GetNickname();
        title.text = DataBase.GetCurrentTitleName();
        titleBg.color = DataBase.GetCurrentTitleColor();
        float needEXP = Utils.CalNeededExp(DataBase.GetLevel());
        expText.text = DataBase.GetUserExp() + "/" + needEXP;
        expImg.fillAmount = DataBase.GetUserExp() / needEXP;

        playerIcon.sprite = await Addressables.LoadAssetAsync<Sprite>
        (DataBase.GetPersonalityInfoByIndex(DataBase.GetCurrentPersonalityId())["headIconFileName"].str).Task;
    }
}
