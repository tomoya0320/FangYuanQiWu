using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class PropInSettlement : MonoBehaviour
{
    public Image icon;
    public Text num;
    public async void Init(Prop prop)
    {
        num.text = prop.num.ToString();

        icon.sprite = await Addressables.LoadAssetAsync<Sprite>
        (DataBase.GetPropInfoById(prop.id)["iconFileName"].str).Task;
    }   
    public async void Init(int propId,int propNum)
    {
        num.text = propNum.ToString();

        icon.sprite = await Addressables.LoadAssetAsync<Sprite>
        (DataBase.GetPropInfoById(propId)["iconFileName"].str).Task;
    }
}
