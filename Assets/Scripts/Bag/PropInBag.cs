using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class PropInBag : MonoBehaviour
{
    public Prop ownedPropData;
    public Image icon;
    public Text num;
    public static PropPos propPos;
    public static PropPos toPos;

    private void Start()
    {
        EventCenter.AddListener(Wzb.EventType.UpdatePropInfo, UpdateInfo);
        UpdateInfo();
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(Wzb.EventType.UpdatePropInfo, UpdateInfo);
    }
    public async void UpdateInfo()
    {
        if(ItemClassification.instance == null)
        {
            ownedPropData = DataBase.GetOwnedPropByIndex(transform.GetSiblingIndex(), propPos);
        }
        else
        {
            ownedPropData = DataBase.GetOwnedPropByIndex(transform.GetSiblingIndex(),
                            ItemClassification.instance.classTags, propPos);
        }

        if (ownedPropData != null)
        {                       
            num.transform.parent.gameObject.SetActive(true);
            num.text = ownedPropData.num.ToString();

            icon.gameObject.SetActive(true);
            icon.sprite = await Addressables.LoadAssetAsync<Sprite>
            (DataBase.GetPropInfoById(ownedPropData.id)["iconFileName"].str).Task;
        }
        else
        {
            icon.gameObject.SetActive(false);
            icon.sprite = null;
            num.transform.parent.gameObject.SetActive(false);
            num.text = "";
        }
    }
}
