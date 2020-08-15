using UnityEngine;
using UnityEngine.UI;

public class UserDataShowInBag : MonoBehaviour
{
    public Text crystal;
    public Text gold;
    private void Start()
    {
        EventCenter.AddListener(Wzb.EventType.UpdateUserInfo, UpdateUserData);
        UpdateUserData();
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(Wzb.EventType.UpdateUserInfo, UpdateUserData);
    }
    public void UpdateUserData()
    {
        crystal.text = DataBase.GetPropNumById(-2).ToString();
        gold.text = DataBase.GetPropNumById(-1).ToString();
    }
}
