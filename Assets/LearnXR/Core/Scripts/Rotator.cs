using System;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private bool useWorldSpace = true;
    [SerializeField] private Vector3 rotationVelocity = new Vector3(30f, 45f, 60f);
    [SerializeField] private float maxRotationTime = -1.0f;
    
    private float rotatorTimer;

    private void Awake()
    {
        // set it to the max to prevent starting with rotations
        rotatorTimer = maxRotationTime;
    }

    void Update()
    {
        // indefinite rotation
        if (maxRotationTime < 0)
        {
            transform.Rotate(rotationVelocity * Time.deltaTime, useWorldSpace ? Space.World : Space.Self);
        }
        else
        {
            if (rotatorTimer <= maxRotationTime)
            {
                transform.Rotate(rotationVelocity * Time.deltaTime, useWorldSpace ? Space.World : Space.Self);
                rotatorTimer += Time.deltaTime;
            }
        }
    }

    public void Activate()
    {
        rotatorTimer = 0;
    }
}
