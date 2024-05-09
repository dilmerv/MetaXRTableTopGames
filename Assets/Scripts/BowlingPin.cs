using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BowlingPin : MonoBehaviour
{
    [SerializeField] private bool isKnockedDown;
    [SerializeField] private float minKnockedDownThreshold = 0.3f;
    [SerializeField] private float minKnockedDownYPosition = -0.06f;
    [SerializeField] private float currentKnockedDownThresholdValue;
    [SerializeField] private UnityEvent onPinKnockedDown = new();

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Rigidbody physics;
    
    public bool IsKnockedDown => isKnockedDown;

    private void Awake()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
        physics = GetComponent<Rigidbody>();
    }

    public void SetPhysics(bool isEnabled)
    {
        physics.isKinematic = !isEnabled;
    }

    public void RestoreDefaultSettings()
    {
        physics.isKinematic = true;
        transform.SetLocalPositionAndRotation(initialPosition, initialRotation);
        StartCoroutine(RestorePhysicsSettings());
    }

    private IEnumerator RestorePhysicsSettings()
    {
        yield return new WaitUntil(() => transform.localPosition == initialPosition);
        physics.isKinematic = false;
        physics.velocity = Vector3.zero;
        physics.angularVelocity = Vector3.zero;
    }

    void Update()
    {
        Quaternion currentRotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        currentKnockedDownThresholdValue = Quaternion.Angle(currentRotation, Quaternion.identity);
        if (currentKnockedDownThresholdValue >= minKnockedDownThreshold || transform.localPosition.y <= minKnockedDownYPosition)
        {
            isKnockedDown = true;
            onPinKnockedDown?.Invoke();
        }
        else
        {
            isKnockedDown = false;
        }
    }
}
