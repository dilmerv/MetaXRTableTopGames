using System.Collections.Generic;
using System.Linq;
using LearnXR.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

[RequireComponent(typeof(AudioSource))]
public class BowlingPinManager : Singleton<BowlingPinManager>
{
    [SerializeField] private TextMeshPro bowlingStateText;
    [SerializeField] private TextMeshPro bowlingRoundText;
    
    public UnityEvent onPinHitPlaySound = new();

    public UnityEvent<int> onPinsDownState = new();
    
    private List<BowlingPin> cachedPins = new();

    private TextMeshProFader bowlingStateTextFader;
    
    private enum BowlingPinStates
    {
        Strike,
        Good,
        Spare,
        Lost
    }
    
    void Awake() => cachedPins = FindObjectsByType<BowlingPin>(FindObjectsSortMode.None).ToList();

    private void Start()
    {
        bowlingStateTextFader = bowlingStateText.GetComponent<TextMeshProFader>();
        bowlingRoundText.text = $"<color=orange>ROUND {GameSessionManager.Instance.currentPlayerAttempt}</color>";
    }

    public void LockPins()
    {
        cachedPins.ForEach(p => p.SetPhysics(false));
    }

    public void UnlockPins()
    {
        cachedPins.ForEach(p => p.SetPhysics(true));
    }
    
    private void OnEnable()
    {
        BowlerPlayerController.Instance.onBallLaunched.AddListener(UnlockPins);
        AxisHandler.Instance.onAxisSelected.AddListener(LockPins);
        AxisHandler.Instance.onAxisCancelled.AddListener(UnlockPins);
        
        BowlerPlayerController.Instance.onBallHitPin.AddListener(() => {
            var pinsHitsAudioSource = GetComponent<AudioSource>();
            if(!pinsHitsAudioSource.isPlaying) pinsHitsAudioSource.Play();
        });
        
        GameSessionManager.Instance.rollSessionElapsed.AddListener(() =>
        {
            int remaining = CheckPinsState();
            if (remaining == 0 && GameSessionManager.Instance.currentPlayerAttempt == 1)
            {
                cachedPins.ForEach(p => p.RestoreDefaultSettings());
                bowlingStateTextFader.FadeInWithText($"{BowlingPinStates.Strike}\n{remaining} pin(s) left!");
            }
            else if (remaining > 0 && GameSessionManager.Instance.currentPlayerAttempt == 1)
            {
                bowlingStateTextFader.FadeInWithText($"{BowlingPinStates.Good}\n{remaining} pin(s) left...");
            }
            else if (remaining == 0 && GameSessionManager.Instance.currentPlayerAttempt == Constants.MaxPlayerRollAttempts)
            {
                bowlingStateTextFader.FadeInWithText($"{BowlingPinStates.Spare}\n{remaining} pin(s) left...");
                cachedPins.ForEach(p => p.RestoreDefaultSettings());
            }
            else if (remaining > 0 && GameSessionManager.Instance.currentPlayerAttempt == Constants.MaxPlayerRollAttempts)
            {
                bowlingStateTextFader.FadeInWithText($"You {BowlingPinStates.Lost}\n{remaining} pin(s) left...");
                cachedPins.ForEach(p => p.RestoreDefaultSettings());
            }
            
            onPinsDownState.Invoke(remaining);
            
            bowlingRoundText.text = $"<color=orange>ROUND {GameSessionManager.Instance.currentPlayerAttempt}</color>";
        });
    }

    private int CheckPinsState() => cachedPins.Count(p => !p.IsKnockedDown);

    private void OnDestroy() =>  onPinHitPlaySound.RemoveAllListeners();
}