using System;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    public event Action<Collider> OnGoalEntered;

    [SerializeField] private ParticleSystem goalParticles;

    private void Awake()
    {
        if (goalParticles == null)
        {
            goalParticles = GetComponentInChildren<ParticleSystem>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (goalParticles != null)
        {
            goalParticles.Play();
        }

        OnGoalEntered?.Invoke(other);
    }
}