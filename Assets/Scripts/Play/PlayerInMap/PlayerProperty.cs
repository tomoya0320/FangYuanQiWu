using UnityEngine;

public class PlayerProperty : MonoBehaviour, IProperty
{
    public BuffManager buffManager;
    private Personality playerProperty;
    private int buffForHP;
    private float timer;

    private void Awake()
    {
        playerProperty = (Personality)Database.CurrentPersonality.Clone();
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
        return playerProperty.moveSpeed * buffManager.GetBuffValueByClassTag(1) * Time.fixedDeltaTime;
    }
    public bool GetMovable()
    {
        return playerProperty.movable;
    }
    public void SetMovable(bool movable)
    {
        playerProperty.movable = movable;
    }
    public float GetSteeringSpeed()
    {
        return playerProperty.steeringSpeed;
    }
    public float GetCurrentHP()
    {
        return (int)playerProperty.currentHP;
    }
    public void SetCurrentHP(float currentHP)
    {
        if (playerProperty.animState != AnimState.Dead)
        {
            playerProperty.currentHP = Mathf.Clamp(currentHP, 0, playerProperty.maxHP);
        }
    }
    public float GetMaxHP()
    {
        return (int)playerProperty.maxHP;
    }
    public bool GetJumpable()
    {
        return playerProperty.jumpable;
    }
    public void SetJumpable(bool jumpable)
    {
        playerProperty.jumpable = jumpable;
    }
    public float GetJumpSpeed()
    {
        return playerProperty.jumpSpeed;
    }
    public float GetCurrentJumpSpeed()
    {
        return playerProperty.currentJumpSpeed;
    }
    public void SetCurrentJumpSpeed(float currentJumpSpeed)
    {
        playerProperty.currentJumpSpeed = currentJumpSpeed;
    }
    public bool GetAttackable()
    {
        return playerProperty.attackable;
    }
    public float GetAttack()
    {
        return playerProperty.attack;
    }
    public void SetAttackable(bool attackable)
    {
        playerProperty.attackable = attackable;
    }
    public float GetDashSpeedRate()
    {
        return playerProperty.dashSpeedRate;
    }
    public void SetAttackMode(int mode)
    {
        playerProperty.attackMode = mode;
    }
    public int GetAttackMode()
    {
        return playerProperty.attackMode;
    }
    public float GetAnimSpeed()
    {
        float rate = 1.0f;
        if (playerProperty.animState == AnimState.Run)
        {
            rate *= buffManager.GetBuffValueByClassTag(1);
        }
        return playerProperty.animSpeed * rate;
    }
    public float GetGravity()
    {
        return playerProperty.gravity * Time.fixedDeltaTime;
    }
    public float GetDefence()
    {
        return playerProperty.defence;
    }
    public bool GetInterruptable()
    {
        return playerProperty.interruptable;
    }
    public AnimState GetAnimState()
    {
        return playerProperty.animState;
    }
    public void SetAnimState(AnimState animState)
    {
        playerProperty.animState = animState;
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        playerProperty.moveSpeed = moveSpeed;
    }

    public void SetAttack(float attack)
    {
        playerProperty.attack = attack;
    }

    public void SetDefence(float defence)
    {
        playerProperty.defence = defence;
    }
}
