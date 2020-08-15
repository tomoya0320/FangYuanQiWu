using MyBehaviorTree;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Wzb;

[Serializable]
public class FallenProp
{
    public float probability;
    public AssetReference propPrefab;
}
public class MonsterController : MonoBehaviour
{
    [Header("攻击和追踪范围的缩放比")]
    public float attackRangeRate = 0.5f;
    public float followRangeRate = 0.5f;
    [Header("动画相关")]
    public AnimatorOverrideController[] aoc_atks;
    [Header("攻击判定位置")]
    public GameObject[] atkColliderObjects;
    [Header("一些组件")]
    public CharacterController cc;
    public Animator anim;
    public MonsterProperty monsterProperty;
    [Header("掉落的道具")]
    public FallenProp[] fallenProps;
    [Header("怪物死亡后的刷新时间")]
    public float reappearTime;

    private Vector3 oPos;//初始的位置
    private Vector3 oAngle;//初始的位置
    private AnimState animState;
    private Array animStateArray;

    private SelectorNode btAI;
    private SequenceNode sequence_1;
    private SequenceNode sequence_2;
    private SequenceNode sequence_3;
    private SelectorNode selector_1;

    private void Start()
    {
        oPos = transform.position;
        oAngle = transform.eulerAngles;
        animStateArray = Enum.GetValues(typeof(AnimState));

        btAI = BT.SelectorNode();
        sequence_1 = BT.SequenceNode();
        sequence_2 = BT.SequenceNode();
        sequence_3 = BT.SequenceNode();
        selector_1 = BT.SelectorNode();

        sequence_1.AddNode(BT.ConditionNode(CanAttack), BT.ActionNode(RotateDir), BT.ActionNode(Attack));

        sequence_2.AddNode(BT.ConditionNode(CanMove), BT.ActionNode(Run));

        selector_1.AddNode(sequence_1, sequence_2, BT.ActionNode(Idle));

        sequence_3.AddNode(BT.ConditionNode(IsLived), selector_1);

        btAI.AddNode(sequence_3);
    }
    private void FixedUpdate()
    {
        animState = GetPlayerState(anim);
        monsterProperty.SetAnimState(animState);
        if (monsterProperty.GetCurrentHP() <= 0)
        {
            if (animState != AnimState.Dead)
            {
                anim.speed = 1.0f;
                anim.SetTrigger("dead");
            }
            return;
        }
        if (PlayerController.GetComp<PlayerProperty>().GetCurrentHP() <= 0)
        {
            anim.SetBool("run", false);
            return;
        }
        btAI.Execute();
    }
    private AnimState GetPlayerState(Animator animator)
    {
        foreach (AnimState state in animStateArray)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(state.ToString()))
            {
                return state;
            }
        }
        return AnimState.Buffer;
    }
    private bool CanAttack()
    {
        return IsInAttackRange() && monsterProperty.GetAttackable();
    }
    private bool IsInAttackRange()
    {
        Vector3 targetPos = PlayerController.GetTransform().position;
        targetPos.y = transform.position.y;
        float distance = Vector3.Distance(targetPos, transform.position + transform.forward.normalized * monsterProperty.GetAttackDist() * attackRangeRate);
        return distance < monsterProperty.GetAttackDist();
    }
    private bool IsInFollowRange()
    {
        Vector3 targetPos = PlayerController.GetTransform().position;
        targetPos.y = transform.position.y;
        float distance = Vector3.Distance(targetPos, transform.position + transform.forward.normalized * monsterProperty.GetFollowDist() * followRangeRate);
        return distance < monsterProperty.GetFollowDist() && Vector3.Distance(targetPos, oPos) < monsterProperty.GetMaxMoveDist();
    }
    private bool CanMove()
    {
        return (animState == AnimState.Idle || animState == AnimState.Run) && IsInFollowRange() && !IsInAttackRange();
    }
    private bool IsLived()
    {
        return monsterProperty.GetCurrentHP() > 0;
    }
    private void Run()
    {
        RotateDir();
        anim.SetBool("run", true);
        cc.SimpleMove(transform.forward * monsterProperty.GetMoveSpeed());
    }
    private void RotateDir()
    {
        Vector3 target = PlayerController.GetTransform().position;
        target.y = transform.position.y;
        transform.LookAt(Vector3.Slerp(transform.forward, target - transform.position,
        monsterProperty.GetSteeringSpeed() * Time.fixedDeltaTime) + transform.position);
    }
    private void Idle()
    {
        anim.SetBool("run", false);
    }
    private IEnumerator<BTNodeResult> Attack()
    {
        monsterProperty.SetAttackable(false);
        Invoke("EnableAttack", monsterProperty.GetAttackDelta());
        monsterProperty.SetAttackMode(UnityEngine.Random.Range(0, aoc_atks.Length));
        anim.runtimeAnimatorController = aoc_atks[monsterProperty.GetAttackMode()];
        anim.SetTrigger("attack");
        while (animState == AnimState.Attack)
        {
            yield return BTNodeResult.Running;
        }
        if (animState == AnimState.Damage)
        {
            yield return BTNodeResult.Failure;
        }
        else
        {
            yield return BTNodeResult.Success;
        }
    }
    private void EnableAttack()
    {
        monsterProperty.SetAttackable(true);
    }
    public void SetSkillEnable(int mode)
    {
        monsterProperty.SetAttackMode(mode);
        SetSkillColliderState(mode, true);
    }
    public void SetSkillDisable()
    {
        if (monsterProperty.GetAttackMode() >= 0)
        {
            SetSkillColliderState(monsterProperty.GetAttackMode(), false);
            monsterProperty.SetAttackMode(-1);
        }
    }
    private void SetSkillColliderState(int mode, bool state)
    {
        Transform[] allChild = atkColliderObjects[mode].GetComponentsInChildren<Transform>();
        foreach (Transform child in allChild)
        {
            foreach (Collider collider in child.GetComponents<Collider>())
            {
                collider.enabled = state;
            }
        }
    }
    public void BreakSkill()
    {
        SetSkillDisable();
    }
    public async void DestroySelf()
    {
        TaskCenter.UpdateTaskState(RequirementType.MonsterKill.ToString(), monsterProperty.index, 1);
        if (monsterProperty.GetKillExp() > 0)
        {
            DataBase.AddExp(monsterProperty.GetKillExp());
        }
        foreach (FallenProp fallenProp in fallenProps)
        {
            if (UnityEngine.Random.Range(0, 1.0f) < fallenProp.probability)
            {
                GameObject temp = await fallenProp.propPrefab.InstantiateAsync().Task;
                temp.transform.position = transform.position;
                temp.transform.eulerAngles = transform.eulerAngles;
            }
        }
        if(reappearTime > 0)
        {
            gameObject.SetActive(false);
            Invoke("Reappear", reappearTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Reappear()
    {
        transform.position = oPos;
        transform.eulerAngles = oAngle;
        gameObject.SetActive(true);
        monsterProperty.InitProperty();
    }
}
