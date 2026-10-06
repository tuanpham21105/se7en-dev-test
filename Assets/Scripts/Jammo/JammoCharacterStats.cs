using System;
using UnityEngine;

[CreateAssetMenu(fileName = "JammoCharacterStats", menuName = "Jammo/Character Stats")]
public class JammoCharacterStats : ScriptableObject
{
    [SerializeField, Min(0f)] private float moveSpeed = 5f;
    [SerializeField, Min(0f)] private float rotateSpeed = 720f;

    private ChangeableVariable<float> _moveSpeed;
    private ChangeableVariable<float> _rotateSpeed;

    public event Action<float, float> OnMoveSpeedChanged
    {
        add => MoveSpeed.OnValueChanged += value;
        remove => MoveSpeed.OnValueChanged -= value;
    }

    public event Action<float, float> OnRotateSpeedChanged
    {
        add => RotateSpeed.OnValueChanged += value;
        remove => RotateSpeed.OnValueChanged -= value;
    }

    public ChangeableVariable<float> MoveSpeed => _moveSpeed ??= new ChangeableVariable<float>();

    public ChangeableVariable<float> RotateSpeed => _rotateSpeed ??= new ChangeableVariable<float>();

    public void SetMoveSpeed(float value)
    {
        moveSpeed = Mathf.Max(0f, value);
        MoveSpeed.Set(moveSpeed);
    }

    public void SetRotateSpeed(float value)
    {
        rotateSpeed = Mathf.Max(0f, value);
        RotateSpeed.Set(rotateSpeed);
    }

    private void OnEnable()
    {
        MoveSpeed.Set(moveSpeed);
        RotateSpeed.Set(rotateSpeed);
    }

    private void OnValidate()
    {
        MoveSpeed.Set(moveSpeed);
        RotateSpeed.Set(rotateSpeed);
    }
}