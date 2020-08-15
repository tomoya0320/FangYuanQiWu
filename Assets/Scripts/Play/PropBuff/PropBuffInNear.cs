using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropBuffInNear : MonoBehaviour
{
    public int buffId;
    private Buff buff;

    private void Start()
    {
        buff = DataBase.GetBuffByIndex(buffId);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Player"))
        {
            buff.duration = -1.0f;
            other.GetComponent<BuffManager>().AddBuff(buff);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Contains("Player"))
        {
            buff.duration = 0;
        }
    }
}
