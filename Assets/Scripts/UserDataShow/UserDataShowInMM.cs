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
        ap.text = DataBase.APInfo;
        gold.text = DataBase.GetPropNumById(-1).ToString();
        crystal.text = DataBase.GetPropNumById(-2).ToString();

        lv.text = DataBase.Level.ToString();
        nickname.text = DataBase.Nickname;
        title.text = DataBase.CurrentTitleName;
        titleBg.color = DataBase.CurrentTitleColor;
        float needEXP = Utils.CalNeededExp(DataBase.Level);
        expText.text = DataBase.UserExp + "/" + needEXP;
        expImg.fillAmount = DataBase.UserExp / needEXP;

        playerIcon.sprite = await Addressables.LoadAssetAsync<Sprite>
        (DataBase.GetPersonalityInfoByIndex(DataBase.CurrentPersonalityId)["headIconFileName"].str).Task;
    }
}
