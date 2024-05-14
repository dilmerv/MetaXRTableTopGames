using System.Collections;
using System.Collections.Generic;
using LearnXR.Core;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Events;

public class AxisHandler : Singleton<AxisHandler>
{
    [SerializeField]
    private Grabbable grabbable;

    public UnityEvent onAxisSelected = new();

    public UnityEvent onAxisCancelled = new();
    
    // Start is called before the first frame update
    void Start()
    {
        grabbable.WhenPointerEventRaised += OnWhenPointerEventRaised;
    }

    private void OnWhenPointerEventRaised(PointerEvent pointerEvent)
    {
        if (pointerEvent.Type == PointerEventType.Select)
        {
            onAxisSelected?.Invoke();
        }
        if (pointerEvent.Type == PointerEventType.Unselect)
        {
            onAxisCancelled?.Invoke();
        }
    }

    private void OnDestroy()
    {
        onAxisSelected.RemoveAllListeners();
        onAxisCancelled.RemoveAllListeners();
    }
}
