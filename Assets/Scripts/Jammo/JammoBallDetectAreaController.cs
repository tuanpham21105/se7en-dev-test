using System.Collections.Generic;
using UnityEngine;

public class JammoBallDetectAreaController : MonoBehaviour
{
    private readonly List<Collider> detectedObjects = new List<Collider>();

    [SerializeField] private JammoCharacterController characterController;

    [SerializeField] private GameObject kickButton;

    private void Awake()
    {
        if (characterController == null)
        {
            characterController = GetComponent<JammoCharacterController>();
        }

        if (characterController == null)
        {
            characterController = GetComponentInParent<JammoCharacterController>();
        }

        if (characterController == null)
        {
            characterController = FindFirstObjectByType<JammoCharacterController>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!detectedObjects.Contains(other))
        {
            detectedObjects.Add(other);
        }

        UpdateIsNearBall();
    }

    private void OnTriggerExit(Collider other)
    {
        detectedObjects.Remove(other);
        detectedObjects.RemoveAll(col => col == null);

        UpdateIsNearBall();
    }

    private void UpdateIsNearBall()
    {
        if (characterController == null)
        {
            return;
        }

        characterController.States.IsNearBall.Set(detectedObjects.Count > 0);

        if (kickButton != null)
        {
            kickButton.SetActive(detectedObjects.Count > 0);
        }
    }
}