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
        lv.text = DataBase.Level.ToString();
        nickname.text = DataBase.Nickname;
        signature.text = DataBase.Signature;
        title.text = DataBase.CurrentTitleName;
        float needEXP = Utils.CalNeededExp(DataBase.Level);
        expText.text = DataBase.UserExp + "/" + needEXP;
        expImg.fillAmount = DataBase.UserExp / needEXP;

        account.text = "账号ID：" + DataBase.Account;
        unlockedDressNum.text = "装扮总数：" + 0;
        unlockedAchievementNum.text = "成就总数：" + DataBase.UnlockedTitleNum;
        unlockedMapNum.text = "解锁地图：" + DataBase.UnlockedMainMapNum;
        unlockedPersonalityNum.text = "人格总数：" + DataBase.UnlockedPersonalityNum;
       
        playerIcon.sprite = await Addressables.LoadAssetAsync<Sprite>
        (DataBase.GetPersonalityInfoByIndex(DataBase.CurrentPersonalityId)["headIconFileName"].str).Task;
    }
    public void ChangeNickname()
    {
        if(nickname.text == "")
        {
            nickname.text = DataBase.Nickname;
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
            signature.text = DataBase.Signature;
        }
        else
        {
            DataBase.SetSignature(signature.text);
        }
    }
}
