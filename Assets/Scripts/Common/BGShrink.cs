using UnityEngine;

public class BGShrink : MonoBehaviour
{
    private RectTransform rectTransform;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        float rate = (float)Screen.width / (float)Screen.height;
        if(rate < 2.0f)
        {
            rectTransform.sizeDelta *= 2.0f / rate;
        }
    }
}
