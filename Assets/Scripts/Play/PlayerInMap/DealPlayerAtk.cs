using UnityEngine;

public class DealPlayerAtk : DealAtk
{
    [Header("这是攻击到才产生的特效")]
    public GameObject effect;
    private MonsterProperty monsterProperty;
    [Header("角色的属性")]
    public PlayerProperty playerProperty;
    private MonsterController monsterController;

    protected override bool GetData()
    {
        if (otherCollider.CompareTag("Monster"))
        {
            monsterController = otherCollider.GetComponent<MonsterController>();
            monsterProperty = monsterController.monsterProperty;
            otherHp = monsterProperty.GetCurrentHP();
            otherDefence = monsterProperty.GetDefence();
            otherInterruptable = monsterProperty.GetInterruptable();
            ownedAtk = playerProperty.GetAttack();     
            otherAnim = otherCollider.GetComponent<Animator>();
            return true;
        }
        return false;
    }
    protected override void UpdateData()
    {
        if (otherInterruptable || otherHp <= 0)
        {
            monsterController.BreakSkill();
        }
        monsterProperty.SetCurrentHP(otherHp);
    }
    protected override void FinallyDeal()
    {
        if (effect != null)
        {
            Instantiate(effect, transform.position, transform.rotation);
        }
    }
    protected override void PlayDamageSound()
    {
        
    }
}