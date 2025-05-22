using UnityEngine;

public class CameraFollowAndShake : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target;           // Player Transform
    public float smoothSpeed = 0.125f;
    public Vector3 offset;



    void LateUpdate()
    {
        // Camera follow logic
        if (target != null)
        {
            Vector3 desiredPos = target.position + offset;
            Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
            transform.position = smoothedPos;
        }

      
    }

    // Call this method to trigger a shake
    
}
