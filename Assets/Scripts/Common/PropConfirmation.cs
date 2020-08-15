using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public enum CBType
{
    出售,
    使用,
    丢弃,
    确认,
}
public class PropConfirmation : MonoBehaviour
{
    public Text ok;
    public Text tip;
    public Text maxNum;
    public Slider slider;
    public Text currentNum;
    public Image propIcon;
    private CBType cbType;
    private Action<int> callback;
    private JSONObject propInfo;
    // Start is called before the first frame update
    public async void Open(CBType cbType, int num,JSONObject propInfo, Action<int> callback)
    {
        this.cbType = cbType;
        this.callback = callback;
        this.propInfo = propInfo;
        switch (cbType)
        {
            case CBType.出售:
                {
                    tip.text = "总价：" + propInfo["price"].n;
                    break;
                }
            default:
                {
                    tip.text = "";
                    break;
                }
        }
        ok.text = cbType.ToString();
        float rate = 0.5f * Screen.width / Screen.height;
        Vector2 newSize = GetComponent<RectTransform>().sizeDelta;
        if (rate > 1.0f)
        {
            newSize.x *= rate;
        }
        else
        {
            newSize.y /= rate;
        }
        GetComponent<RectTransform>().sizeDelta = newSize;     
        maxNum.text = num.ToString();
        slider.maxValue = num;
        slider.value = 0;
        gameObject.SetActive(true);

        propIcon.sprite = await Addressables.LoadAssetAsync<Sprite>
        (propInfo["iconFileName"].str).Task;
    }
    public void Cancel()
    {
        SetCallback(0);
    }
    public void Confirm()
    {
        SetCallback((int)slider.value);
    }
    private void SetCallback(int num)
    {
        callback(num);
        gameObject.SetActive(false);
    }
    public void UpdateCurrentSliderNum()
    {
        currentNum.text = slider.value.ToString();
        switch (cbType)
        {
            case CBType.出售:
                {
                    float totalPrice = slider.value * propInfo["price"].n;
                    tip.text = "总价：" + totalPrice;
                    break;
                }
        }
    }
}
