using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
  private static GameObject player;
  [Header("动画相关")]
  [Tooltip("闪避动画")]
  public AnimatorOverrideController[] aoc_dashs;
  [Tooltip("攻击动画")]
  public AnimatorOverrideController[] aoc_atks;

  [Header("闪避冷却")]
  public float dashCoolTime = 2.0f;
  private bool dashCooled = true;
  private int dashDir = -1;
  public Image dashCoolImg { set; get; }

  [Header("攻击判定位置")]
  public GameObject[] atkColliderObjects;

  [Header("武器")]
  public GameObject[] weaponObjects;

  [Header("一些组件")]
  public Animator anim;
  public CharacterController cc;
  public Property playerProperty;
  public AudioSource audioSource;
  public PlayerDead playerDead { set; get; }

  [Header("相机跟随/朝向位置")]
  public Transform lookAtAndFollow;

  //一些移动操作用到的变量
  private Vector3 moveVector;
  private Vector3 moveDir;
  private float hRaw = 0;
  private float vRaw = 0;
  private float h = 0;
  private float v = 0;

  private void Awake() {
    player = gameObject;
  }
  private void FixedUpdate() {
    //获取方向键按下情况
    hRaw = InputManager.GetAxisRaw(Direction.Horizontal);
    vRaw = InputManager.GetAxisRaw(Direction.Vertical);
    h = InputManager.GetAxis(Direction.Horizontal);
    v = InputManager.GetAxis(Direction.Vertical);

    //角色移动向量计算和角色朝向的转变   
    if ((Mathf.Abs(hRaw) > 0 || Mathf.Abs(vRaw) > 0) && playerProperty.GetMovable()) {
      if (cc.isGrounded) {
        anim.SetBool(StringMap.RUN, true);
      }
      moveDir = new Vector3(h, 0, v);
      if (CameraController.operateCameraFlag == 2) {
        moveDir = Utils.RotateVector3(moveDir, LookAround.oCameraAngle.y);
      } else {
        moveDir = Utils.RotateVector3(moveDir, Camera.main.transform.eulerAngles.y);
      }
      transform.LookAt(Vector3.Slerp(transform.forward, moveDir,
          playerProperty.GetSteeringSpeed() * Time.fixedDeltaTime) + transform.position);

      moveVector = transform.forward * playerProperty.GetMoveSpeed();
    } else {
      moveVector = Vector3.zero;
      anim.SetBool(StringMap.RUN, false);
    }
    #region 一些技能
    //角色跳跃
    if (InputManager.GetButton(ButtonType.Jump) && playerProperty.GetJumpable()) {
      anim.SetTrigger(StringMap.JUMP);
      playerProperty.SetCurrentJumpSpeed(playerProperty.GetJumpSpeed());
    } else
    //角色攻击
    if (InputManager.GetButton(ButtonType.Attack) && playerProperty.GetAttackable()) {
      DoSkill(0);
    } else
    //角色闪避
    if (InputManager.GetButton(ButtonType.Dash) && dashCooled) {
      HideWeapon();
      dashDir = hRaw != 0 || vRaw != 0 ? 1 : -1;
      anim.runtimeAnimatorController = aoc_dashs[(dashDir + 1) / 2];
      anim.SetTrigger(StringMap.DASH);
      DashCool();
    }
    #endregion
    //应用闪避
    if (playerProperty.GetAnimState() == AnimState.Dash) {
      gameObject.tag = StringMap.INV_PL;
      moveVector = dashDir * transform.forward * playerProperty.GetMoveSpeed() * playerProperty.GetDashSpeedRate();
    } else {
      gameObject.tag = StringMap.PLAYER;
    }
    //应用重力和移动
    cc.Move(transform.up * playerProperty.GetCurrentJumpSpeed() + moveVector);
  }
  private async void DashCool() {
    dashCooled = false;
    float startTime = Time.time;
    while (Time.time - startTime < dashCoolTime) {
      if (dashCoolImg == null) return;
      dashCoolImg.fillAmount = 1.0f - (Time.time - startTime) / dashCoolTime;
      await Task.Delay((int)(Time.deltaTime * 1000.0f));
    }
    if (dashCoolImg == null) return;
    dashCoolImg.fillAmount = 0;
    dashCooled = true;
  }
  private void DoSkill(int id) {
    if (id >= 0 && id < aoc_atks.Length) {
      anim.runtimeAnimatorController = aoc_atks[id];
      anim.SetTrigger(StringMap.ATTACK);
    }
  }
  public void SetSkillEnable(int mode) {
    playerProperty.SetAttackMode(mode);
    SetSkillColliderState(mode, true);
  }
  public void SetSkillDisable() {
    if (playerProperty.GetAttackMode() >= 0) {
      SetSkillColliderState(playerProperty.GetAttackMode(), false);
      playerProperty.SetAttackMode(-1);
    }
  }
  public void ShowWeapon(int id) {
    if (id >= 0 && id < weaponObjects.Length) {
      weaponObjects[id].SetActive(true);
    }
  }
  public void HideWeapon() {
    foreach (GameObject temp in weaponObjects) {
      temp.SetActive(false);
    }
  }
  private void SetSkillColliderState(int mode, bool state) {
    Collider[] colliders = atkColliderObjects[mode].GetComponentsInChildren<Collider>();
    foreach (Collider collider in colliders) {
      collider.enabled = state;
    }
  }
  public void BreakSkill() {
    HideWeapon();
    SetSkillDisable();
  }
  public void PlayDamageSound() {

  }
  public void PlayDeadSound() {

  }
  private void EventOnDead() {
    playerDead.Show();
  }
  public static T GetComp<T>() where T : Object {
    if (player == null) {
      return null;
    }
    return player.GetComponent<T>();
  }
  public static Transform GetTransform() {
    if (player == null) {
      return null;
    }
    return player.transform;
  }
}