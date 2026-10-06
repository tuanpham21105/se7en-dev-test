using UnityEngine;

public class JammoCharacterAnimationControllerEventListeners : MonoBehaviour
{
    [SerializeField] private JammoCharacterController jammoCharacterController;
    [SerializeField] private JammoCharacterAnimationController animationController;

    private void Awake()
    {
        if (jammoCharacterController == null)
        {
            jammoCharacterController = GetComponent<JammoCharacterController>();
        }

        if (jammoCharacterController == null)
        {
            jammoCharacterController = FindFirstObjectByType<JammoCharacterController>();
        }

        if (animationController == null)
        {
            animationController = GetComponent<JammoCharacterAnimationController>();
        }

        if (animationController == null)
        {
            animationController = GetComponentInChildren<JammoCharacterAnimationController>();
        }

        if (animationController == null)
        {
            animationController = FindFirstObjectByType<JammoCharacterAnimationController>();
        }
    }

    private void OnEnable()
    {
        if (jammoCharacterController == null)
        {
            return;
        }

        jammoCharacterController.States.OnMoveDirectionChanged += HandleMoveDirectionChanged;
        HandleMoveDirectionChanged(jammoCharacterController.States.MoveDirection.Value, default);
    }

    private void OnDisable()
    {
        if (jammoCharacterController == null)
        {
            return;
        }

        jammoCharacterController.States.OnMoveDirectionChanged -= HandleMoveDirectionChanged;
    }

    private void HandleMoveDirectionChanged(Vector3 newDirection, Vector3 oldDirection)
    {
        if (animationController == null)
        {
            return;
        }

        if (newDirection != Vector3.zero)
        {
            animationController.PlayRun();
        }
        else
        {
            animationController.PlayIdle();
        }
    }
}