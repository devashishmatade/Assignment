using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityManager : MonoBehaviour
{
    public static GravityManager Instance;

    [Header("Gravity Settings")]
    public float gravityStrength = 9.81f;
    private Vector3 gravityDirection = Vector3.down;

    public delegate void OnGravityChanged(Vector3 newDirection);
    public event OnGravityChanged GravityChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SetGravityDirection(gravityDirection); // Apply default on start
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetGravityDirection(Vector3 direction)
    {
        gravityDirection = direction.normalized;
        Physics.gravity = gravityDirection * gravityStrength;

        GravityChanged?.Invoke(gravityDirection);
    }

    public Vector3 GetGravityDirection()
    {
        return gravityDirection;
    }

    public Vector3 GetGravityVector()
    {
        return gravityDirection * gravityStrength;
    }

    /// <summary>
    /// Changes gravity to pull toward the surface the player collided with.
    /// </summary>
    public void SetGravityFromSurfaceNormal(Vector3 surfaceNormal)
    {
        SetGravityDirection(-surfaceNormal.normalized); // Pull into the surface
    }
}