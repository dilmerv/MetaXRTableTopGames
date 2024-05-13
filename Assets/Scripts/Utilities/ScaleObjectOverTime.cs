using System.Collections;
using UnityEngine;

namespace Utilities
{
    public class ScaleObjectOverTime : BaseTransitionObject
    {
        [SerializeField] private Vector3 fromScale = new (1f, 1f, 1f);
        [SerializeField] private Vector3 toScale = new (2f, 2f, 2f);
        
        public void Scale(Vector3 fromScale, Vector3 toScale, float delay, float duration)
        {
            this.fromScale = fromScale;
            this.toScale = toScale;
            this.delay = delay;
            this.duration = duration;

            StartCoroutine(ScaleCoroutine());
        }

        private IEnumerator ScaleCoroutine()
        {
            StartTransition();
            yield return new WaitForSeconds(delay);

            float timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                float progress = timer / duration;
                transform.localScale = Vector3.Lerp(fromScale, toScale, progress);
                yield return null;
            }

            transform.localScale = toScale;
            EndTransition();
        }
    }
}