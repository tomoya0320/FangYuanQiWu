using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class PropOperate : MonoBehaviour
{
    public Sprite[] itemBGImgs;
    public Button useButton;
    public Button discardButton;
    public PropConfirmation confirmationBox;

    protected PropInBag selectedProp;
    protected JSONObject selectedPropInfo;
    protected GameObject settlement;

    private async void Start()
    {
        settlement = await Addressables.LoadAssetAsync<GameObject>("settlement").Task;
        EventCenter.AddListener(Wzb.EventType.UpdatePropInfo, UpdateInfo);
        UpdateInfo();
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(Wzb.EventType.UpdatePropInfo, UpdateInfo);
    }
    public virtual void UpdateInfo()
    {
        if (selectedProp && selectedProp.ownedPropData != null)
        {
            selectedPropInfo = DataBase.GetPropInfoById(selectedProp.ownedPropData.id);
            useButton.interactable = CanUse();
            discardButton.interactable = true;
        }
        else
        {
            useButton.interactable = false;
            discardButton.interactable = false;
        }
    }
    public void SetSelectedProp(PropInBag prop)
    {
        if (selectedProp)
        {
            selectedProp.GetComponent<Image>().sprite = itemBGImgs[0];
        }
        prop.GetComponent<Image>().sprite = itemBGImgs[1];
        selectedProp = prop;
        UpdateInfo();
    }
    public void DiscardProp()
    {
        if (selectedProp && selectedProp.ownedPropData != null)
        {
            confirmationBox.Open(CBType.丢弃, selectedProp.ownedPropData.num, selectedPropInfo,
            selectedNum => { ConsumeProp(selectedPropInfo, selectedNum, 0); });
        }
    }
    public void UseProp()
    {
        if (selectedProp && selectedProp.ownedPropData != null)
        {
            if (selectedPropInfo["class"].n == 0)
            {
                ConsumeProp(selectedPropInfo, 1, -1);
            }
            else
            {
                confirmationBox.Open(CBType.使用, selectedProp.ownedPropData.num, selectedPropInfo,
                selectedNum => { ConsumeProp(selectedPropInfo, selectedNum, -1); });
            }
        }
    }
    protected virtual bool CanUse()
    {
        return selectedPropInfo["class"].n == 0 || selectedPropInfo["class"].n == 1; 
    }
    protected void ConsumeProp(JSONObject propInfo, int num, int flag)
    {
        if (num > 0 && DataBase.ConsumePropById(flag, (int)propInfo["id"].n, num))
        {
            if (flag == 1)
            {
                int price = (int)propInfo["price"].n;
                DataBase.AddPropById(-1, price * num);
            }
            else if (flag == -1)
            {
                switch ((int)propInfo["class"].n)
                {
                    case 0:
                        {
                            BuffManager buffManager = PlayerController.GetComp<BuffManager>();
                            PlayerProperty playerProperty = PlayerController.GetComp<PlayerProperty>(); 
                            //产生效果
                            if (propInfo.HasField("hpRecovery"))
                            {
                                playerProperty.SetCurrentHP(playerProperty.GetCurrentHP() + (int)propInfo["hpRecovery"].n * num);
                            }
                            if (propInfo.HasField("energyRecovery"))
                            {
                                DataBase.SetEnergyVariation((int)propInfo["energyRecovery"].n * num);
                            }
                            if (propInfo.HasField("buffs"))
                            {
                                foreach (JSONObject json in propInfo["buffs"].list)
                                {
                                    buffManager.AddBuff(DataBase.GetBuffByIndex((int)json["id"].n));
                                }
                            }
                            break;
                        }
                    case 1:
                        {
                            //宝箱的使用
                            List<Prop> acquiredPropList = new List<Prop>();
                            if (propInfo["fixedContents"].HasField("exp"))
                            {
                                DataBase.AddExp(num * (int)propInfo["fixedContents"]["exp"].n);
                            }
                            if (propInfo["fixedContents"].HasField("props"))
                            {
                                foreach (JSONObject temp in propInfo["fixedContents"]["props"].list)
                                {
                                    acquiredPropList.Add(new Prop((int)temp["id"].n, (int)temp["num"].n * num));
                                    DataBase.AddPropById((int)temp["id"].n, (int)temp["num"].n * num);
                                }
                            }
                            if (propInfo["randomContents"].HasField("props"))
                            {
                                foreach (JSONObject json in propInfo["randomContents"]["props"].list)
                                {
                                    Prop prop = new Prop((int)json["id"].n, 0);
                                    if (json.HasField("probability"))
                                    {
                                        for (int n = 0; n < num; n++)
                                        {
                                            if (Random.Range(0, 1.0f) < json["probability"].n)
                                            {
                                                prop.num += (int)json["num"].n;
                                            }
                                        }
                                    }
                                    else if (json.HasField("range"))
                                    {
                                        for (int n = 0; n < num; n++)
                                        {
                                            prop.num += Random.Range((int)json["range"]["min"].n, (int)json["range"]["max"].n + 1);
                                        }
                                    }
                                    if (prop.num > 0)
                                    {
                                        acquiredPropList.Add(prop);
                                        DataBase.AddPropById(prop.id, prop.num);
                                    }
                                }
                            }
                            if (acquiredPropList.Count > 0)
                            {
                                GameObject tmp = Instantiate(settlement, transform.parent);
                                tmp.GetComponent<Settlement>().Init(acquiredPropList);
                            }
                            break;
                        }
                }
            }
        }
    }
}