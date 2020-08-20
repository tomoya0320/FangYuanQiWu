using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class TouchEffect : MonoBehaviour {
  /*
  public int childNum;
  public float duration;
  public GameObject childEffect;
  private float delta;
  private float maxDelta;
  private float angle;
  private float oLength;

  private void Start()
  {
      oLength = Mathf.Sqrt(2) * 0.5f * (GetComponent<RectTransform>().rect.width
          + childEffect.GetComponent<RectTransform>().rect.width);
      maxDelta = 360.0f / childNum;
      for (int i = 0; i < childNum; i++)
      {
          delta = Random.Range(0.5f * maxDelta, 1.5f * maxDelta);
          angle += delta;
          Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
          GameObject go = Instantiate(childEffect, transform);
          go.GetComponent<RectTransform>().anchoredPosition3D = 
              dir.normalized * oLength * Random.Range(0.5f, 1.35f);
          go.transform.DORotate(Vector3.forward * 180.0f, duration);
          go.GetComponent<Image>().DOColor(Color.white * 0.5f, duration);
      }
      transform.DOScale(1.35f, duration);
      GetComponent<Image>().DOColor(Color.clear, duration).onComplete += DestroySelf;
  }
  */
  private void DestroySelf() {
    Destroy(gameObject);
  }
}