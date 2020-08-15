using UnityEngine;
using UnityEngine.UI;

public class ImageAlphaTest : MonoBehaviour
{
    private void Start()
    {
        Image image = GetComponent<Image>();
        if (image)
        {
            image.alphaHitTestMinimumThreshold = 0.5f;
        }
    }
}
