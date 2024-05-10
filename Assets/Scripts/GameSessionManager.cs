using LearnXR.Core;
using UnityEngine;
using UnityEngine.Events;

public class GameSessionManager : Singleton<GameSessionManager>
{
    [SerializeField] private float sessionMaxTime = 2.0f;
    
    public UnityEvent rollSessionStarted = new();
    public UnityEvent rollSessionElapsed = new();
    
    public int currentPlayerAttempt = 1;
    private float currentRollTimeElapsed = 0;
    private bool sessionStarted;

    public void StartRollSession()
    {
        sessionStarted = true;
        rollSessionStarted.Invoke();
    }

    private void OnEnable()
    {
        BowlingPinManager.Instance.onPinsDownState.AddListener((pinsRemaining) =>
        {
            if (currentPlayerAttempt >= Constants.MaxPlayerRollAttempts || pinsRemaining == 0 && currentPlayerAttempt == 1)
                currentPlayerAttempt = 1;
            else
                currentPlayerAttempt++;
        });
    }

    private void Update()
    {
        if (!sessionStarted) return;
        
        if (currentRollTimeElapsed <= sessionMaxTime)
            currentRollTimeElapsed += Time.deltaTime;
        else
        {
            sessionStarted = false;
            currentRollTimeElapsed = 0;
            rollSessionElapsed.Invoke();
        }
    }
}
