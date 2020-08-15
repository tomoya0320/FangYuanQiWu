using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SmallMap : MonoBehaviour
{
    public Vector2 terrainSize;
    public Text posText;
    public Text mapName;
    public RectTransform map;
    public RectTransform posCursor;
    private Vector3 lastPlayerPos;
    private Vector3 lastPlayerAngle;
    private Vector2 mapShowSize;
    private Vector2 normalizedPlayerPos;
    private Vector2 mapSize;
    private Vector2 maxMapMoveDist;
    private Vector2 currentMapPos;
    private Vector2 posCursorPosInParent;
    private Vector2 lastPosCursorPos;
    private Vector2 moveDist;

    private async void Start()
    {
        mapSize = map.sizeDelta;
        moveDist = Vector2.zero;
        currentMapPos = Vector2.zero; 
        lastPosCursorPos = Vector2.zero;
        normalizedPlayerPos = Vector2.zero;
        mapShowSize = GetComponent<RectTransform>().sizeDelta;
        maxMapMoveDist = 0.5f * (mapSize - mapShowSize);
        lastPlayerPos = PlayerController.GetTransform().position;
        lastPlayerAngle = PlayerController.GetTransform().eulerAngles;
        int mapId = int.Parse(SceneManager.GetActiveScene().name.Split('_')[1]);
        mapName.text = DataBase.GetMapInfoByIndex(mapId)["name"].str;
        map.GetComponent<Image>().sprite = await Addressables.LoadAssetAsync<Sprite>
            (DataBase.GetMapInfoByIndex(mapId)["smallMapFileName"].str).Task;

        MoveSmallMapAndPosCursor();
    }

    private void Update()
    {
        if (PlayerController.GetTransform())
        {
            if (PlayerController.GetTransform().position != lastPlayerPos || 
                PlayerController.GetTransform().eulerAngles != lastPlayerAngle)
            {
                MoveSmallMapAndPosCursor();
            }
            lastPlayerPos = PlayerController.GetTransform().position;
            lastPlayerAngle = PlayerController.GetTransform().eulerAngles;
        }
    }
    public void MoveSmallMapAndPosCursor()
    {      
        //归一化角色坐标
        normalizedPlayerPos.x = PlayerController.GetTransform().position.x / terrainSize.x;
        normalizedPlayerPos.y = PlayerController.GetTransform().position.z / terrainSize.y;
        //角色位置的光标的移动和旋转
        posCursor.anchoredPosition = new Vector2(normalizedPlayerPos.x * mapSize.x,
            normalizedPlayerPos.y * mapSize.y) - 0.5f * mapSize;
        posCursor.eulerAngles = new Vector3(0, 0, Utils.AngleSigned(PlayerController.GetTransform().forward, Vector3.forward, Vector3.up));
        posText.text = string.Format("({0},{1})", (int)posCursor.anchoredPosition.x, (int)posCursor.anchoredPosition.y);
        //小地图的移动
        posCursorPosInParent = posCursor.anchoredPosition + map.anchoredPosition;
        moveDist = posCursor.anchoredPosition - lastPosCursorPos;
        if ((moveDist.x > 0 && posCursorPosInParent.x > 0.2f * mapShowSize.x)||
            (moveDist.x < 0 && posCursorPosInParent.x < -0.2f * mapShowSize.x))
        {
            currentMapPos.x -= moveDist.x;
            currentMapPos.x = Mathf.Clamp(currentMapPos.x, -maxMapMoveDist.x, maxMapMoveDist.x);
        }
        if ((moveDist.y > 0 && posCursorPosInParent.y > 0.2f * mapShowSize.y) ||
           (moveDist.y < 0 && posCursorPosInParent.y < -0.2f * mapShowSize.y))
        {
            currentMapPos.y -= moveDist.y;
            currentMapPos.y = Mathf.Clamp(currentMapPos.y, -maxMapMoveDist.y, maxMapMoveDist.y);
        }
        map.anchoredPosition = currentMapPos;
        lastPosCursorPos = posCursor.anchoredPosition;
    }
}
