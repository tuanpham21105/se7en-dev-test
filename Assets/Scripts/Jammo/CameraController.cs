using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField, Min(0f)] private float moveSpeed = 10f;
    [SerializeField] private Transform target;

    private Vector3 horizontalOffset;
    private float height;

    private void Awake()
    {
        height = transform.position.y;

        if (target == null)
        {
            JammoCharacterController jammoCharacterController = FindFirstObjectByType<JammoCharacterController>();

            if (jammoCharacterController != null)
            {
                target = jammoCharacterController.transform;
            }
        }

        if (target != null)
        {
            horizontalOffset = transform.position - target.position;
            horizontalOffset.y = 0f;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }

        Move(target.position);
    }

    public void Move(Vector3 targetPosition)
    {
        Vector3 followPosition = targetPosition + horizontalOffset;
        Vector3 desiredPosition = new Vector3(followPosition.x, height, followPosition.z);
        Vector3 nextPosition = Vector3.MoveTowards(
            transform.position,
            desiredPosition,
            moveSpeed * Time.fixedDeltaTime);

        transform.position = new Vector3(nextPosition.x, height, nextPosition.z);
    }
}