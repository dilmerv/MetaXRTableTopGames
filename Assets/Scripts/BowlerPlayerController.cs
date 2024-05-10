using System.Collections;
using System.Linq;
using LearnXR.Core;
using TMPro;
using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class BowlerPlayerController : Singleton<BowlerPlayerController>
{
    // interactables
    [SerializeField] private GrabInteractable grabInteractable;
    [SerializeField] private HandGrabInteractable handGrabInteractable;

    // physics & ball behaviour
    [SerializeField] private float maxPullDistance = 1.0f;
    [SerializeField] private float rotationSpeed = 500.0f;
    [SerializeField] private ForceMode launchForceMode = ForceMode.Impulse;
    [SerializeField] private float launchForce = 10f;
    [SerializeField] private float restoreBallDelay = 2.0f;

    // random references
    [SerializeField] private Transform visuals;
    [SerializeField] private Transform spawnArea;
    [SerializeField] private TextMeshPro statsText;
    [SerializeField] private Transform aimArrow;
    
    // sfx
    [SerializeField] private AudioSource sfxForBallSelection;
    [SerializeField] private AudioSource sfxForLaunchingBall;
    
    // public events
    public UnityEvent onBallLaunched = new();
    public UnityEvent onBallHitPin = new();
    
    // Controller(s) Or Hand(s) attach point for ball positioning
    private Transform controllerOrHandsAttachPoint;
    
    // private variables
    private LineRenderer lineRenderer;
    private Vector3 pullInitialPosition;
    private Quaternion initialVisualsRotation;
    private bool isGrabbing;
    private bool hasLaunched;
    private Rigidbody physics;
    private Vector3 launchedBallForceDirection;
    private Grabbable grabbable;
    private const string BowlingPinTag = "BowlingPin";
    
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
        physics = GetComponent<Rigidbody>();
        statsText.gameObject.SetActive(false);

        grabbable = GetComponentInChildren<Grabbable>();
        grabbable.WhenPointerEventRaised += GrabbableOnWhenPointerEventRaised;

        // let's place the player it at a random position
        transform.position = GetRandomSurfacePosition();
        initialVisualsRotation = visuals.rotation;
        aimArrow.position = transform.position;
    }
    
    private void GrabbableOnWhenPointerEventRaised(PointerEvent pointerEvent)
    {
        if (pointerEvent.Type == PointerEventType.Select)
        {
            pullInitialPosition = transform.position;
            if(!sfxForBallSelection.isPlaying) sfxForBallSelection.Play();
        }
    }

    private void Update()
    {
        if (grabbable.SelectingPointsCount > 0)
        {
            isGrabbing = true;
            lineRenderer.enabled = true;
            statsText.gameObject.SetActive(true);
            aimArrow.gameObject.SetActive(true);
            
            controllerOrHandsAttachPoint = GetActiveInteractorTransform();
            
            UpdateLineRenderer();
            
            transform.position = controllerOrHandsAttachPoint.position;
            
            UpdateAimArrowRotation();
            
            float distance = Mathf.Clamp(Vector3.Distance(pullInitialPosition, transform.position), 
                0, maxPullDistance);

            statsText.text = $"Launch Power: {distance:F}";
        }
        else if (isGrabbing && !hasLaunched)
        {
            onBallLaunched?.Invoke();
            LaunchBall();
        }
        
        if (hasLaunched)
        {
            visuals.Rotate(transform.right,  physics.velocity.z * rotationSpeed, Space.World);
        }
    }

    private void UpdateLineRenderer()
    {
        lineRenderer.SetPosition(0, pullInitialPosition);
        lineRenderer.SetPosition(1, transform.position);
    }
    
    private void UpdateAimArrowRotation()
    {
        Vector3 direction = (controllerOrHandsAttachPoint.position - pullInitialPosition).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            aimArrow.rotation = lookRotation;
        }
    }

    private void LaunchBall(float debugDistance = 0)
    {
        GameSessionManager.Instance.StartRollSession();
        
        if(!sfxForLaunchingBall.isPlaying) sfxForLaunchingBall.Play();
        
        Vector3 grabPosition = debugDistance == 0 ? controllerOrHandsAttachPoint.position : transform.position;
        Vector3 direction = debugDistance == 0 ? (pullInitialPosition - grabPosition).normalized : transform.forward;
        physics.isKinematic = false;

        float distance = debugDistance == 0
            ? Mathf.Clamp(Vector3.Distance(pullInitialPosition, grabPosition), 0, maxPullDistance)
            : debugDistance;
        
        // Apply force in the launch direction
        launchedBallForceDirection = direction * (launchForce * (launchForce * distance));
        
        physics.AddForce(launchedBallForceDirection, launchForceMode);
        
        aimArrow.gameObject.SetActive(false);
      
        ResetBall();
        hasLaunched = true;
    }

    private void ResetBall()
    {
        isGrabbing = false;
        lineRenderer.enabled = false;
        StartCoroutine(RestoreBallPositionWithDelay());
    }
    
    private Transform GetActiveInteractorTransform()
    {
        Transform currentTransform = null;

        if (grabInteractable.State == InteractableState.Select)
        {
            currentTransform = grabInteractable.Interactors.First()
                .transform;
        }
        else if (handGrabInteractable.State == InteractableState.Select)
        {
            currentTransform = handGrabInteractable.Interactors.First()
                .PinchPoint;
        }
        return currentTransform;
    }
    
    private IEnumerator RestoreBallPositionWithDelay()
    {
        yield return new WaitForSeconds(restoreBallDelay);
        transform.position = GetRandomSurfacePosition();
        aimArrow.position = transform.position;
        transform.rotation = Quaternion.identity;
        
        // clean up physics
        visuals.rotation = initialVisualsRotation;
        physics.velocity = Vector3.zero;
        physics.angularVelocity = Vector3.zero;
        physics.isKinematic = true;
        launchedBallForceDirection = Vector3.zero;
        
        hasLaunched = false;
    }

    private Vector3 GetRandomSurfacePosition()
    {
        Bounds bounds = spawnArea.GetComponent<Collider>().bounds;

        Vector3 randomPoint = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            bounds.max.y + (GetComponent<SphereCollider>().bounds.size.y * 2),
            Random.Range(bounds.min.z, bounds.max.z)
        );

        return randomPoint;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag(BowlingPinTag))
        {
            onBallHitPin.Invoke();
        }
    }
}