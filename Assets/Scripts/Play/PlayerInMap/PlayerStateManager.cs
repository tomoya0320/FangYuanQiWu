using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wzb;

public enum AnimState {
  Idle, Run,
  JumpA, JumpB,
  JumpC, Attack,
  Dash, Dead,
  Damage, Buffer,
}

public class PlayerStateManager : MonoBehaviour {
  public CharacterController cc;
  public Property playerProperty;
  public Animator anim;
  private AnimState animState;
  private Array animStateArray;
  private int requirementId;
  private float startTime;
  private string sceneName;
  private float currentJumpSpeed;

  public static int currentLiveTime;

  private void Start() {
    sceneName = SceneManager.GetActiveScene().name;
    if (sceneName.Contains(StringMap.MAP)) {
      startTime = Time.fixedTime;
      requirementId = int.Parse(sceneName.Split('_')[1]);
      TaskCenter.UpdateTaskState(RequirementType.EnterMap.ToString(), requirementId, 1);
      Database.SaveUserData();
    }

    animStateArray = Enum.GetValues(typeof(AnimState));
  }
  private void OnDestroy() {
    currentLiveTime = 0;
    TaskCenter.UpdateTaskState(RequirementType.SurvivalTimeInMap.ToString(), requirementId, 0);
  }
  private void FixedUpdate() {
    if (sceneName.Contains(StringMap.MAP)) {
      if (Time.fixedTime - startTime >= 60.0f) {
        currentLiveTime++;
        TaskCenter.UpdateTaskState(RequirementType.SurvivalTimeInMap.ToString(), requirementId, 1);
        startTime = Time.fixedTime;
        Database.SaveUserData();
      }
    }

    animState = GetPlayerState(anim);
    playerProperty.SetAnimState(animState);

    //落地、能否跳跃的判断和重力计算
    currentJumpSpeed = playerProperty.GetCurrentJumpSpeed();
    if (!cc.isGrounded) {
      currentJumpSpeed -= playerProperty.GetGravity();
    } else if (currentJumpSpeed < -2.0f * playerProperty.GetGravity() ||
               currentJumpSpeed > -1.0f * playerProperty.GetGravity()) {
      currentJumpSpeed = 0;
    }

    anim.speed = playerProperty.GetAnimSpeed();
    anim.SetBool(StringMap.IS_GROUNDED, cc.isGrounded);
    anim.SetFloat(StringMap.DROP_SPEED, currentJumpSpeed);

    playerProperty.SetCurrentJumpSpeed(currentJumpSpeed);
    playerProperty.SetJumpable(CanJump(animState));

    //能否移动的判断
    playerProperty.SetMovable(CanMove(animState));
    //能否攻击的判断
    playerProperty.SetAttackable(CanAttack(animState));
  }
  private AnimState GetPlayerState(Animator animator) {
    foreach (AnimState state in animStateArray) {
      if (animator.GetCurrentAnimatorStateInfo(0).IsName(StringMap.AnimStateToString(state))) {
        return state;
      }
    }
    return AnimState.Buffer;
  }
  private bool CanMove(AnimState playerState) {
    return (playerState == AnimState.Run || playerState == AnimState.Idle ||
            playerState == AnimState.JumpA || playerState == AnimState.JumpB)
            && !anim.IsInTransition(0);

  }//except for jumpC and attack
  private bool CanJump(AnimState playerState) {
    return (playerState == AnimState.Run || playerState == AnimState.Idle)
            && !anim.IsInTransition(0);

  }//include Run Walk and Idle
  private bool CanAttack(AnimState playerState) {
    return (playerState == AnimState.Run || playerState == AnimState.Idle)
            && !anim.IsInTransition(0);
  }
}