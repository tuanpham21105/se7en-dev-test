using UnityEngine;

public class JammoCharacterAnimationController : MonoBehaviour
{
    private const string MoveParameter = "move";

    [SerializeField] private Animator animator;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    public void PlayRun()
    {
        SetMove(true);
    }

    public void PlayIdle()
    {
        SetMove(false);
    }

    private void SetMove(bool value)
    {
        if (animator == null)
        {
            return;
        }

        animator.SetBool(MoveParameter, value);
    }
}