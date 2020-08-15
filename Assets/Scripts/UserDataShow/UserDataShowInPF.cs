using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class UserDataShowInPF : MonoBehaviour
{
    public Image playerIcon;
    public Text lv;
    public Text expText;
    public Image expImg;
    public InputField nickname;
    public InputField signature;
    public Text title;
    public Text account;
    public Text unlockedDressNum;
    public Text unlockedAchievementNum;
    public Text unlockedMapNum;
    public Text unlockedPersonalityNum;
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
        lv.text = DataBase.GetLevel().ToString();
        nickname.text = DataBase.GetNickname();
        signature.text = DataBase.GetSignature();
        title.text = DataBase.GetCurrentTitleName();
        float needEXP = Utils.CalNeededExp(DataBase.GetLevel());
        expText.text = DataBase.GetUserExp() + "/" + needEXP;
        expImg.fillAmount = DataBase.GetUserExp() / needEXP;

        account.text = "账号ID：" + DataBase.GetAccount();
        unlockedDressNum.text = "装扮总数：" + 0;
        unlockedAchievementNum.text = "成就总数：" + DataBase.GetUnlockedTitleNum();
        unlockedMapNum.text = "解锁地图：" + DataBase.GetUnlockedMainMapNum();
        unlockedPersonalityNum.text = "人格总数：" + DataBase.GetUnlockedPersonalityNum();
       
        playerIcon.sprite = await Addressables.LoadAssetAsync<Sprite>
        (DataBase.GetPersonalityInfoByIndex(DataBase.GetCurrentPersonalityId())["headIconFileName"].str).Task;
    }
    public void ChangeNickname()
    {
        if(nickname.text == "")
        {
            nickname.text = DataBase.GetNickname();
        }
        else
        {
            DataBase.SetNickname(nickname.text);        
        }
    }
    public void ChangeSignature()
    {
        if (signature.text == "")
        {
            signature.text = DataBase.GetSignature();
        }
        else
        {
            DataBase.SetSignature(signature.text);
        }
    }
}
