using UnityEngine;

public class DealMonsterAtk : DealAtk
{
    [Header("这是攻击到才产生的特效")]
    public GameObject effect;
    [Header("怪物的属性")]
    public MonsterProperty monsterProperty;
    private PlayerProperty playerProperty;
    private PlayerController playerController;

    protected override bool GetData()
    {
        if (otherCollider.CompareTag("Player"))
        {
            playerController = otherCollider.GetComponent<PlayerController>();
            playerProperty = playerController.playerProperty;
            otherHp = playerProperty.GetCurrentHP();
            otherDefence = playerProperty.GetDefence();
            otherInterruptable = playerProperty.GetInterruptable();
            ownedAtk = monsterProperty.GetAttack();
            otherAnim = playerController.anim;        
            return true;
        }
        return false;
    }
    protected override void UpdateData()
    {
        if (otherInterruptable || otherHp <= 0)
        {
            playerController.BreakSkill();
        }
        playerProperty.SetCurrentHP(otherHp);
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
        otherAnim.speed = playerProperty.GetAnimSpeed();
        playerController.PlayDamageSound();
    }
}