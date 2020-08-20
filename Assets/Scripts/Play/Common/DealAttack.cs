using UnityEngine;

public class DealAttack : MonoBehaviour {
  [SerializeField, Header("对方受击时的特效")]
  private GameObject _hitEffect;
  [SerializeField, Header("自己的属性")]
  private Property _ownerProperty;
  [SerializeField]
  private SideType _target;

  private Property m_otherProperty;
  private float m_otherHp;
  private float m_otherDefence;
  private float m_ownerAttack;
  private float m_ownerDamage;
  private bool m_otherInterruptable;
  private Animator m_otherAnim;
  private Collider m_otherCollider;

  private void OnTriggerEnter(Collider other) {
    m_otherCollider = other;
    if (GetData()) {
      CalculateDamage();
      ApplyDamage();
      GenerateEffect();
      ShakeCamera();
    }
  }
  private bool GetData() {
    if (m_otherCollider.CompareTag(StringMap.SideTypeToString(_target))) {
      m_otherProperty = m_otherCollider.GetComponent<Property>();
      m_otherAnim = m_otherCollider.GetComponent<Animator>();
      m_otherInterruptable = m_otherProperty.GetInterruptable();
      m_otherDefence = m_otherProperty.GetDefence();
      m_otherHp = m_otherProperty.GetCurrentHP();
      m_ownerAttack = _ownerProperty.GetAttack();

      return m_otherHp > 0;
    }
    return false;
  }
  private void ShakeCamera() {
    //这里到时候改用Cinemachine来做抖动
  }
  private void CalculateDamage() {
    m_ownerDamage = m_ownerAttack - m_otherDefence > 1 ?
                    m_ownerAttack - m_otherDefence : 1;
    m_otherHp -= m_ownerDamage;
  }
  private void ApplyDamage() {
    if(m_otherHp <= 0) {
      m_otherAnim.SetTrigger(StringMap.DEAD);
    }else if (m_otherInterruptable) {
      m_otherAnim.SetTrigger(StringMap.DAMAGE);
    }
    m_otherProperty.SetCurrentHP(m_otherHp);
  }
  private void GenerateEffect() {
    if (_hitEffect != null) {
      Instantiate(_hitEffect, transform.position, transform.rotation);
    }
  }
}