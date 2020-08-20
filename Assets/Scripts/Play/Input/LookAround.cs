using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LookAround : CameraController {
  public RectTransform centerPoint;
  public static Vector3 oCameraAngle;

  private Image img;
  private float maxDist;
  private Vector2 oPos;
  private Vector2 oAnchoredPos;
  private Vector2 oAxisValue;

  protected override void Start() {
    cameraFlag = 2;
    img = GetComponent<Image>();
    oPos = centerPoint.position;
    oAnchoredPos = centerPoint.anchoredPosition;
    maxDist = GetComponent<RectTransform>().sizeDelta.x / 2.0f;
  }
  protected override void OtherDragDeal(PointerEventData eventData) {
    centerPoint.anchoredPosition = oAnchoredPos +
    Vector2.ClampMagnitude(eventData.position - oPos, maxDist);
  }
  protected override void OtherPointerDownDeal(PointerEventData eventData) {
    img.enabled = true;
    centerPoint.GetComponent<Image>().color = Color.white;
    oAxisValue = new Vector2(freeLook.m_XAxis.Value, freeLook.m_YAxis.Value);
    oCameraAngle = Camera.main.transform.eulerAngles;
  }
  protected override void OtherPointerUpDeal(PointerEventData eventData) {
    centerPoint.GetComponent<Image>().color = Color.white * 0.8f;
    freeLook.m_XAxis.Value = oAxisValue.x;
    freeLook.m_YAxis.Value = oAxisValue.y;
    centerPoint.anchoredPosition = oAnchoredPos;
    img.enabled = false;
  }
}