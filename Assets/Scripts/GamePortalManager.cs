using LearnXR.Core;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

public class GamePortalManager : Singleton<GamePortalManager>
{
    [SerializeField]
    private GameObject gate;
    
    [SerializeField]
    private GameObject gameArea;

    private ScaleObjectOverTime gateScale;
    private MoveObjectOverTime gameAreaMovement;
    
    private Vector3 gateOpenedValues = new Vector3(1, 1, 0.1f);
    private Vector3 gateClosedValues = new Vector3(1, 1, 1.785f);

    private Vector3 gameAreaInsidePortalValues = new Vector3(0, -1.091f, 0);
    private Vector3 gameAreaOutsidePortalValues = new Vector3(0, 0.4f, 0);
    
    private bool isGateClosed = true;
    
    public void Setup()
    {
        gate = GameObject.Find("Gate");
        gameArea = GameObject.Find("GameArea");
        
        gateScale = gate.GetComponent<ScaleObjectOverTime>();
        if (!gateScale)
            gateScale = gate.AddComponent<ScaleObjectOverTime>();
        
        gameAreaMovement = gameArea.GetComponent<MoveObjectOverTime>();
        if (!gameAreaMovement)
            gameAreaMovement = gameArea.AddComponent<MoveObjectOverTime>();
        
        // This is needed for initial room placement boundary calculation. Keep the portal closed to avoid 
        // generating additional gaps during TableSetup placement executed from [BuildingBlock] Find Spawn Positions
        gameAreaMovement.Move(gameAreaOutsidePortalValues, gameAreaInsidePortalValues, 0, 0);
        
        gameAreaMovement.onTransitionStarted.AddListener(BowlingPinManager.Instance.LockPins);
        gameAreaMovement.onTransitionEnded.AddListener(BowlingPinManager.Instance.UnlockPins);
    }

    public bool ToggleGameArea()
    {
        // only allow a toggle if we don't have transitions
        if (gameAreaMovement.IsTransitionInProgress || gateScale.IsTransitionInProgress)
            return isGateClosed;
            
        isGateClosed = !isGateClosed;    
            
        if (isGateClosed)
        {
            gameAreaMovement.Move(gameAreaOutsidePortalValues, gameAreaInsidePortalValues, 0, 1.5f);
            gateScale.Scale(gateOpenedValues, gateClosedValues, 1.5f, 1.5f);
        }
        else
        {
            gateScale.Scale(gateClosedValues, gateOpenedValues, 0, 1.5f);
            gameAreaMovement.Move(gameAreaInsidePortalValues, gameAreaOutsidePortalValues, 1.5f, 1.5f);
        }

        return isGateClosed;
    }
}
