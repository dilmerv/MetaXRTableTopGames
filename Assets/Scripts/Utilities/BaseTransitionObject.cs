using System;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities
{
    public class BaseTransitionObject : MonoBehaviour
    {
        protected float duration = 2f;
        protected float delay = 1f;

        private bool isTransitionInProgress;
        
        public bool IsTransitionInProgress
        {
            get => isTransitionInProgress;
            private set => isTransitionInProgress = value;
        }
        
        public UnityEvent onTransitionStarted = new();
        public UnityEvent onTransitionEnded = new();

        protected void StartTransition()
        {
            isTransitionInProgress = true;
            onTransitionStarted.Invoke();
        }

        protected void EndTransition()
        {
            isTransitionInProgress = false;
            onTransitionEnded.Invoke();
        }

        private void OnDestroy()
        {
            onTransitionStarted.RemoveAllListeners();
            onTransitionEnded.RemoveAllListeners();
        }
    }
}