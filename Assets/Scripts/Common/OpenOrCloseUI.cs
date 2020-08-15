using UnityEngine;

public class OpenOrCloseUI : MonoBehaviour
{
    public GameObject target;

    public void OnClick()
    {
        target.SetActive(!target.activeSelf);
    }
}
