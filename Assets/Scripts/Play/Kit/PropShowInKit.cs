using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class PropShowInKit : MonoBehaviour
{
    public RectTransform propContent;

    private GameObject propInKit;
    private float length;

    private async void Start()
    {
        propInKit = await Addressables.LoadAssetAsync<GameObject>("propinkit").Task;
     
        EventCenter.AddListener<Prop>(Wzb.EventType.AddPropToKit, AddPropToKit);
        EventCenter.AddListener<GameObject>(Wzb.EventType.RemovePropFromKit, RemovePropFromKit);
        foreach (Prop ownedProp in Database.GetOwnedPropListByClassTag(0))
        {
            GameObject temp = Instantiate(propInKit, propContent);
            temp.GetComponent<PropInKit>().Init(ownedProp);
        }
        GridLayoutGroup grid = propContent.GetComponent<GridLayoutGroup>();
        length = grid.cellSize.y + grid.spacing.y;
        UpdateContentLength();
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener<Prop>(Wzb.EventType.AddPropToKit, AddPropToKit);
        EventCenter.RemoveListener<GameObject>(Wzb.EventType.RemovePropFromKit, RemovePropFromKit);
    }
    public void AddPropToKit(Prop prop)
    {
        GameObject temp = Instantiate(propInKit, propContent);
        temp.GetComponent<PropInKit>().Init(prop);
        UpdateContentLength();
    } 
    public void RemovePropFromKit(GameObject prop)
    {
        Destroy(prop);
        UpdateContentLength();
    }
    public void UpdateContentLength()
    {
        List<Prop> ownedProps = Database.GetOwnedPropListByClassTag(0);
        int row = (ownedProps.Count / 2) + (ownedProps.Count % 2);
        propContent.sizeDelta = new Vector2(propContent.sizeDelta.x, length * row);
    }
}
