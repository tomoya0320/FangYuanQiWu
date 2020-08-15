using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class UpStarProp : MonoBehaviour
{
    public Image icon;
    public Text num;
    public GameObject mask;
    private int propId;
    private int needNum;

    private void Start()
    {
        EventCenter.AddListener(Wzb.EventType.UpdatePropInfo, UpdateNum);
        EventCenter.AddListener(Wzb.EventType.UpdateUserInfo, UpdateNum);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(Wzb.EventType.UpdatePropInfo, UpdateNum);
        EventCenter.RemoveListener(Wzb.EventType.UpdateUserInfo, UpdateNum);
    }
    public async void Init(int propId, int propNum, int needNum)
    {
        this.propId = propId;
        this.needNum = needNum;
        num.text = propNum + "/" + needNum;
        mask.SetActive(propNum < needNum);
        num.color = propNum < needNum ? Color.red : Color.white;
        icon.sprite = await Addressables.LoadAssetAsync<Sprite>
        (DataBase.GetPropInfoById(propId)["iconFileName"].str).Task;
    }
    public void UpdateNum()
    {
        int propNum = DataBase.GetPropNumById(propId);
        num.text = propNum + "/" + needNum;
        mask.SetActive(propNum < needNum);
        num.color = propNum < needNum ? Color.red : Color.white;
    }
}
