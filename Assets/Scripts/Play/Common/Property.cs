using UnityEngine;

public class Property : MonoBehaviour {
  public int index;
  public SideType sideType;
  public BuffManager buffManager;
  private PropertyValue propertyValue;
  private int buffForHP;
  private float timer;

  private void Awake() {
    InitProperty();
  }
  public void InitProperty() {
    switch (sideType) {
      case SideType.Player: {
          propertyValue = (PropertyValue)Database.CurrentPersonality.Clone();
          break;
        }
      case SideType.Monster: {
          propertyValue = Database.GetDefaultMonsterByIndex(index);
          break;
        }
    }
  }
  private void Update() {
    if (buffManager != null) {
      if (Time.time - timer > 1.0f) {
        buffForHP = buffManager.GetBuffValueForHP();
        if (buffForHP != 0) {
          SetCurrentHP(GetCurrentHP() + buffForHP);
        }
        timer = Time.time;
      }
    }
  }
  public float GetMoveSpeed() {
    if (buffManager != null) {
      return propertyValue.moveSpeed * buffManager.GetBuffValueByClassTag(1) * Time.fixedDeltaTime;
    } else {
      return propertyValue.moveSpeed * Time.fixedDeltaTime;
    }
  }
  public bool GetMovable() {
    return propertyValue.movable;
  }
  public void SetMovable(bool movable) {
    propertyValue.movable = movable;
  }
  public float GetSteeringSpeed() {
    return propertyValue.steeringSpeed;
  }
  public float GetCurrentHP() {
    return (int)propertyValue.currentHP;
  }
  public void SetCurrentHP(float currentHP) {
    if (propertyValue.animState != AnimState.Dead) {
      propertyValue.currentHP = Mathf.Clamp(currentHP, 0, propertyValue.maxHP);
    }
  }
  public float GetMaxHP() {
    return (int)propertyValue.maxHP;
  }
  public bool GetJumpable() {
    return propertyValue.jumpable;
  }
  public void SetJumpable(bool jumpable) {
    propertyValue.jumpable = jumpable;
  }
  public float GetJumpSpeed() {
    return propertyValue.jumpSpeed;
  }
  public float GetCurrentJumpSpeed() {
    return propertyValue.currentJumpSpeed;
  }
  public void SetCurrentJumpSpeed(float currentJumpSpeed) {
    propertyValue.currentJumpSpeed = currentJumpSpeed;
  }
  public bool GetAttackable() {
    return propertyValue.attackable;
  }
  public float GetAttack() {
    return propertyValue.attack;
  }
  public void SetAttackable(bool attackable) {
    propertyValue.attackable = attackable;
  }
  public float GetDashSpeedRate() {
    return propertyValue.dashSpeedRate;
  }
  public void SetAttackMode(int mode) {
    propertyValue.attackMode = mode;
  }
  public int GetAttackMode() {
    return propertyValue.attackMode;
  }
  public float GetAnimSpeed() {
    float rate = 1.0f;
    if (propertyValue.animState == AnimState.Run && buffManager != null) {
      rate *= buffManager.GetBuffValueByClassTag(1);
    }
    return propertyValue.animSpeed * rate;
  }
  public float GetGravity() {
    return propertyValue.gravity * Time.fixedDeltaTime;
  }
  public float GetDefence() {
    return propertyValue.defence;
  }
  public bool GetInterruptable() {
    return propertyValue.interruptable;
  }
  public AnimState GetAnimState() {
    return propertyValue.animState;
  }
  public void SetAnimState(AnimState animState) {
    propertyValue.animState = animState;
  }

  public void SetMoveSpeed(float moveSpeed) {
    propertyValue.moveSpeed = moveSpeed;
  }

  public void SetAttack(float attack) {
    propertyValue.attack = attack;
  }

  public void SetDefence(float defence) {
    propertyValue.defence = defence;
  }
  public float GetAttackDelta() {
    return propertyValue.attackDelta;
  }
  public float GetMaxMoveDist() {
    return propertyValue.maxMoveDist;
  }
  public int GetKillExp() {
    return propertyValue.killExp;
  }
  public float GetAttackDist() {
    return propertyValue.attackDist;
  }
  public float GetFollowDist() {
    return propertyValue.followDist;
  }
}