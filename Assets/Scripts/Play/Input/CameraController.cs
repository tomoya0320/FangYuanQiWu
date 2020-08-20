using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler {
  public CinemachineFreeLook freeLook;
  public static int operateCameraFlag;//当前谁在操作相机
  protected int cameraFlag; //表明自身是何种类型的相机

  private PointerEventData currentPointer;
  private Vector2 lastPressedPos;

  protected virtual void Start() {
    cameraFlag = 1;
  }
  public void OnDrag(PointerEventData eventData) {
    if (operateCameraFlag == cameraFlag && currentPointer == eventData) {
      Vector2 delta = eventData.position - lastPressedPos;
      delta.y *= -1.0f;
      float deltaX = 360.0f * (delta.x / Screen.width);
      float deltaY = delta.y / Screen.height;
      freeLook.m_XAxis.Value = (deltaX + freeLook.m_XAxis.Value + 360.0f) % 360.0f;
      freeLook.m_YAxis.Value = Mathf.Clamp(freeLook.m_YAxis.Value + deltaY, 0, 1.0f);
      lastPressedPos = eventData.position;
      OtherDragDeal(eventData);
    }
  }
  public void OnPointerDown(PointerEventData eventData) {
    if (operateCameraFlag == 0) {
      currentPointer = eventData;
      lastPressedPos = eventData.position;
      operateCameraFlag = cameraFlag;
      OtherPointerDownDeal(eventData);
    }
  }

  public void OnPointerUp(PointerEventData eventData) {
    if (operateCameraFlag == cameraFlag && currentPointer == eventData) {
      currentPointer = null;
      operateCameraFlag = 0;
      OtherPointerUpDeal(eventData);
    }
  }

  protected virtual void OtherDragDeal(PointerEventData eventData) { }
  protected virtual void OtherPointerDownDeal(PointerEventData eventData) { }
  protected virtual void OtherPointerUpDeal(PointerEventData eventData) { }
}