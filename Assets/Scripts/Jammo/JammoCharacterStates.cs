using System;
using UnityEngine;

public class JammoCharacterStates
{
    private readonly ChangeableVariable<Vector3> _moveDirection = new ChangeableVariable<Vector3>();
    private readonly ChangeableVariable<bool> _isNearBall = new ChangeableVariable<bool>();

    public event Action<Vector3, Vector3> OnMoveDirectionChanged
    {
        add => _moveDirection.OnValueChanged += value;
        remove => _moveDirection.OnValueChanged -= value;
    }

    public event Action<bool, bool> OnIsNearBallChanged
    {
        add => _isNearBall.OnValueChanged += value;
        remove => _isNearBall.OnValueChanged -= value;
    }

    public ChangeableVariable<Vector3> MoveDirection => _moveDirection;

    public ChangeableVariable<bool> IsNearBall => _isNearBall;
}