using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public float followSpeedRate;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * followSpeedRate);
    }
}
