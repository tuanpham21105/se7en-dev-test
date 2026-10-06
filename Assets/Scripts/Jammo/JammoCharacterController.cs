using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class JammoCharacterController : MonoBehaviour
{
    private const float DirectionEpsilon = 0.0001f;

    [SerializeField] private Rigidbody body;
    [SerializeField] private GameObject characterObject;
    [SerializeField] private JammoCharacterStats stats;

    private JammoCharacterStates _states;

    public event Action<Vector3, Vector3> OnMoveDirectionChanged
    {
        add => States.OnMoveDirectionChanged += value;
        remove => States.OnMoveDirectionChanged -= value;
    }

    public JammoCharacterStates States => _states ??= new JammoCharacterStates();

    public JammoCharacterStats Stats
    {
        get
        {
            if (stats == null)
            {
                stats = CreateRuntimeStats();
            }

            return stats;
        }
    }

    private void Awake()
    {
        if (body == null)
        {
            body = GetComponent<Rigidbody>();
        }

        if (characterObject == null && transform.childCount > 0)
        {
            characterObject = transform.GetChild(0).gameObject;
        }

        _ = Stats;
    }

    private void FixedUpdate()
    {
        body.velocity = States.MoveDirection.Value * Stats.MoveSpeed.Value;
    }

    private void Update()
    {
        RotateCharacter(States.MoveDirection.Value);
    }

    public void Move(Vector3 direction)
    {
        States.MoveDirection.Set(direction);
    }

    private void RotateCharacter(Vector3 direction)
    {
        if (characterObject == null || direction.sqrMagnitude <= DirectionEpsilon)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        characterObject.transform.rotation = Quaternion.RotateTowards(
            characterObject.transform.rotation,
            targetRotation,
            Stats.RotateSpeed.Value * Time.deltaTime);
    }

    private static JammoCharacterStats CreateRuntimeStats()
    {
        JammoCharacterStats runtimeStats = ScriptableObject.CreateInstance<JammoCharacterStats>();
        runtimeStats.hideFlags = HideFlags.DontSave;
        return runtimeStats;
    }
}