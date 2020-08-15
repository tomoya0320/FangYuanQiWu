using UnityEngine;
using DG.Tweening;

public abstract class DealAtk : MonoBehaviour
{
    protected float otherHp;
    protected float otherDefence;
    protected float ownedAtk;
    protected float ownedDamage;
    protected bool otherInterruptable;
    protected Animator otherAnim;
    protected Collider otherCollider;

    private void OnTriggerEnter(Collider other)
    {
        otherCollider = other;
        if (GetData())
        {
            if (otherHp > 0)
            {
                ownedDamage = ownedAtk - otherDefence;
                otherHp -= ownedDamage > 1 ? ownedDamage : 1;
                if (otherInterruptable && otherHp > 0)
                {
                    otherAnim.SetTrigger("damage");
                    PlayDamageSound();
                }
                UpdateData();
                ShakeCamera();
                FinallyDeal();
            }
        }
    }
    protected void ShakeCamera()
    {
        //这里到时候改用Cinemachine来做抖动
    }
    protected abstract void PlayDamageSound();
    protected abstract bool GetData();
    protected abstract void UpdateData();
    protected abstract void FinallyDeal();
}