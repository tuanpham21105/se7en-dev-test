using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private JammoCharacterController jammoCharacterController;

    private void Awake()
    {
        if (jammoCharacterController == null)
        {
            jammoCharacterController = FindFirstObjectByType<JammoCharacterController>();
        }
    }

    private void Update()
    {
        if (jammoCharacterController == null)
        {
            return;
        }

        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        jammoCharacterController.Move(Vector3.ClampMagnitude(direction, 1f));
    }
}