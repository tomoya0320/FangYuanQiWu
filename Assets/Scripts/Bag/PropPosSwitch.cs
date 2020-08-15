using UnityEngine;
using UnityEngine.UI;

public class PropPosSwitch : MonoBehaviour
{
    public Text transfer;
    public Image[] images;
    public Sprite[] buttonImgs;
    public RectTransform itemContent;

    private void Start()
    {
        InitPropPos();
    }
    private void OnDestroy()
    {
        InitPropPos();
    }
    private void InitPropPos()
    {
        PropInBag.propPos = PropPos.Bag;
        PropInBag.toPos = PropPos.Warehouse;
    }
    public void SwitchPropPos(int propPos)
    {
        if (PropInBag.propPos != (PropPos)propPos)
        {
            itemContent.anchoredPosition = Vector2.zero;

            images[propPos].sprite = buttonImgs[1];
            images[(int)PropInBag.propPos].sprite = buttonImgs[0];
        
            PropInBag.propPos = (PropPos)propPos;

            switch (PropInBag.propPos)
            {
                case PropPos.Bag:
                    {
                        transfer.text = "放入仓库";
                        PropInBag.toPos = PropPos.Warehouse;
                        break;
                    }     
                case PropPos.Warehouse:
                    {
                        transfer.text = "放入背包";
                        PropInBag.toPos = PropPos.Bag;
                        break;
                    }
            }

            EventCenter.Broadcast(Wzb.EventType.UpdatePropInfo);
        }
    }
}
