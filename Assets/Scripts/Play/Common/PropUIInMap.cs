using UnityEngine;
using UnityEngine.UI;

public class PropUIInMap : MonoBehaviour
{
    [Header("UI显示")]
    public Image operateImg;
    public Button operateButton;
    public GameObject propMenu;
    public static PropUIInMap instance;

    private void Awake()
    {
        instance = this;
    }
}
