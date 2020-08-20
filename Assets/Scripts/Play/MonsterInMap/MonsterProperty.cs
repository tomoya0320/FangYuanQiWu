using UnityEngine;

public class MonsterProperty : MonoBehaviour,IProperty
{
    public int index;
    public BuffManager buffManager;
    private Monster monsterProperty;
    private int buffForHP;
    private float timer;

    private void Awake()
    {
        InitProperty();
    }
    public void InitProperty()
    {
        monsterProperty = Database.GetDefaultMonsterByIndex(index);
    }
    private void Update()
    {
        if (Time.time - timer > 1.0f)
        {
            buffForHP = buffManager.GetBuffValueForHP();
            if (buffForHP != 0)
            {
                SetCurrentHP(GetCurrentHP() + buffForHP);
            }
            timer = Time.time;
        }
    }
    public float GetMoveSpeed()
    {
        return monsterProperty.moveSpeed * buffManager.GetBuffValueByClassTag(1);
    }
    public bool GetMovable()
    {
        return monsterProperty.movable;
    }
    public void SetMovable(bool movable)
    {
        monsterProperty.movable = movable;
    }
    public float GetSteeringSpeed()
    {
        return monsterProperty.steeringSpeed;
    }
    public float GetCurrentHP()
    {
        return monsterProperty.currentHP;
    }
    public float GetMaxHP()
    {
        return monsterProperty.maxHP;
    }
    public void SetCurrentHP(float currentHP)
    {
        if (monsterProperty.animState != AnimState.Dead)
        {
            monsterProperty.currentHP = Mathf.Clamp(currentHP, 0, monsterProperty.maxHP);
        }
    }
    public bool GetAttackable()
    {
        return monsterProperty.attackable;
    }
    public float GetAttack()
    {
        return monsterProperty.attack;
    }
    public float GetDefence()
    {
        return monsterProperty.defence;
    }
    public void SetAttackable(bool attackable)
    {
        monsterProperty.attackable = attackable;
    }
    public void SetAttackMode(int mode)
    {
        monsterProperty.attackMode = mode;
    }
    public int GetAttackMode()
    {
        return monsterProperty.attackMode;
    }
    public float GetAnimSpeed()
    {
        float rate = 1.0f;
        if (monsterProperty.animState == AnimState.Run)
        {
            rate *= buffManager.GetBuffValueByClassTag(1);
        }
        return monsterProperty.animSpeed * rate;
    }
    public bool GetInterruptable()
    {
        return monsterProperty.interruptable;
    }
    public float GetAttackDist()
    {
        return monsterProperty.attackDist;
    } 
    public float GetFollowDist()
    {
        return monsterProperty.followDist;
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        monsterProperty.moveSpeed = moveSpeed;
    }

    public void SetAttack(float attack)
    {
        monsterProperty.attack = attack;
    }

    public void SetDefence(float defence)
    {
        monsterProperty.defence = defence;
    }
    public void SetAnimState(AnimState animState)
    {
        monsterProperty.animState = animState;
    }
    public float GetAttackDelta()
    {
        return monsterProperty.attackDelta;
    } 
    public float GetMaxMoveDist()
    {
        return monsterProperty.maxMoveDist;
    }  
    public int GetKillExp()
    {
        return monsterProperty.killExp;
    }
}
