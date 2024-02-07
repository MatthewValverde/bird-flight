
using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    public Transform target;
    public float distanceFromTarget = 5.0f;

    void LateUpdate()
    {
        if (target != null)
        {
            //transform.position = target.position - transform.forward * distanceFromTarget;
            transform.LookAt(target);
        }
    }
}

