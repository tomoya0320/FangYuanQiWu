using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler {
  public RectTransform centerPoint;
  private float maxDist;
  private Vector2 oPos;
  private Vector2 oAnchoredPos;
  private Image image;
  private static float[] axises;

  private void Awake() {
    axises = new float[Enum.GetValues(typeof(Direction)).Length];
  }
  private void Start() {
    oPos = centerPoint.position;
    oAnchoredPos = centerPoint.anchoredPosition;
    image = centerPoint.GetComponent<Image>();
    maxDist = GetComponent<RectTransform>().sizeDelta.x / 2.0f;
  }
  private void MoveCenterPoint(PointerEventData eventData) {
    centerPoint.anchoredPosition = oAnchoredPos +
    Vector2.ClampMagnitude(eventData.position - oPos, maxDist);
    Vector2 direction = (eventData.position - oPos).normalized;
    axises[Mathf.Clamp((int)Direction.Horizontal, 0, 1)] = direction.x;
    axises[Mathf.Clamp((int)Direction.Vertical, 0, 1)] = direction.y;
  }
  public void OnDrag(PointerEventData eventData) {
    MoveCenterPoint(eventData);
  }

  public void OnPointerDown(PointerEventData eventData) {
    image.color = Color.white;
    MoveCenterPoint(eventData);
  }

  public void OnPointerUp(PointerEventData eventData) {
    image.color = Color.black * 0.8f;
    centerPoint.anchoredPosition = oAnchoredPos;
    axises[Mathf.Clamp((int)Direction.Horizontal, 0, 1)] = 0;
    axises[Mathf.Clamp((int)Direction.Vertical, 0, 1)] = 0;
  }
  public static float GetAxis(int direction) {
    return axises[direction];
  }
}