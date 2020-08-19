using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Wzb;

public class PropInMap : MonoBehaviour
{
    public int propId;
    public int propNum;
    public float reappearTime;
    public Sprite operateIcon;
    public GameObject propGameObejct;
    public Text propName;
    public GameObject nameGameObejct;
    [Header("是否开启角色采集/拾取动画")]
    public bool isOpenAnim;

    private Animator playerAnim;
    private JSONObject propInfo;
    private Sprite propIcon;

    private async void Start()
    {
        propInfo = DataBase.GetPropInfoById(propId);
        propIcon = await Addressables.LoadAssetAsync<Sprite>(propInfo["iconFileName"].str).Task;
        playerAnim = isOpenAnim ? GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>() : null;
        nameGameObejct.GetComponent<Canvas>().worldCamera = Camera.main;
    }
    public void GetProp()
    {
        PropUIInMap.instance.propMenu.SetActive(false);
        nameGameObejct.SetActive(false);

        if (playerAnim)
        {
            playerAnim.SetTrigger("pick");
        }

        MessageCenter.Instance.AddMessage(new Message(MessageStyle.ColorFade,
        string.Format("获得道具:{0}*{1}", propInfo["name"].str, propNum), propIcon, 0.4f, 0.8f));
        DataBase.AddPropById(propId, propNum);

        if (reappearTime > 0)
        {
            propGameObejct.SetActive(false);
            Invoke("Reappear", reappearTime);
        }
        else
        {
            Destroy(propGameObejct);
        }
    }
    private void Reappear()
    {
        propGameObejct.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Player"))
        {
            PropUIInMap.instance.operateImg.sprite = operateIcon;       
            PropUIInMap.instance.operateButton.onClick.RemoveAllListeners();
            PropUIInMap.instance.operateButton.onClick.AddListener(GetProp);
            PropUIInMap.instance.propMenu.SetActive(true);

            propName.text = propInfo["name"].str;
            nameGameObejct.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains("Player"))
        {
            PropUIInMap.instance.operateButton.transform.localScale = Vector3.one;
            PropUIInMap.instance.propMenu.SetActive(false);
            nameGameObejct.SetActive(false);
        }
    }
}