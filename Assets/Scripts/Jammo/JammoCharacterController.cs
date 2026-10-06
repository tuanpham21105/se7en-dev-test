using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class JammoCharacterController : MonoBehaviour
{
    private const float DirectionEpsilon = 0.0001f;

    [SerializeField] private Rigidbody body;
    [SerializeField] private GameObject characterObject;
    [SerializeField] private JammoCharacterStats stats; 
    [SerializeField] private List<GameObject> balls = new List<GameObject>();
    [SerializeField] private List<GameObject> goals = new List<GameObject>();

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

    public void Kick()
    {
        ExecuteKick(FindNearest, FindNearest);
    }

    public void AutoKick()
    {
        ExecuteKick(FindFarthest, FindNearest);
    }

    private void ExecuteKick(
        Func<List<GameObject>, Vector3, GameObject> ballFinder,
        Func<List<GameObject>, Vector3, GameObject> goalFinder)
    {
        GameObject ball = ballFinder(balls, transform.position);

        if (ball == null)
        {
            return;
        }

        GameObject goal = goalFinder(goals, ball.transform.position);

        if (goal == null)
        {
            return;
        }

        Rigidbody ballBody = ball.GetComponent<Rigidbody>();

        if (ballBody == null)
        {
            ballBody = ball.GetComponentInParent<Rigidbody>();
        }

        if (ballBody == null)
        {
            return;
        }

        Vector3 kickDirection = goal.transform.position - ball.transform.position;
        kickDirection.y = 0f;

        if (kickDirection.sqrMagnitude <= DirectionEpsilon)
        {
            return;
        }

        ballBody.AddForce(kickDirection.normalized * Stats.KickForce.Value, ForceMode.Impulse);
    }

    private static GameObject FindNearest(List<GameObject> candidates, Vector3 fromPosition)
    {
        GameObject nearest = null;
        float nearestSqrDistance = float.PositiveInfinity;

        for (int i = 0; i < candidates.Count; i++)
        {
            GameObject candidate = candidates[i];

            if (candidate == null)
            {
                continue;
            }

            float sqrDistance = (candidate.transform.position - fromPosition).sqrMagnitude;

            if (sqrDistance < nearestSqrDistance)
            {
                nearestSqrDistance = sqrDistance;
                nearest = candidate;
            }
        }

        return nearest;
    }

    private static GameObject FindFarthest(List<GameObject> candidates, Vector3 fromPosition)
    {
        GameObject farthest = null;
        float farthestSqrDistance = float.NegativeInfinity;

        for (int i = 0; i < candidates.Count; i++)
        {
            GameObject candidate = candidates[i];

            if (candidate == null)
            {
                continue;
            }

            float sqrDistance = (candidate.transform.position - fromPosition).sqrMagnitude;

            if (sqrDistance > farthestSqrDistance)
            {
                farthestSqrDistance = sqrDistance;
                farthest = candidate;
            }
        }

        return farthest;
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