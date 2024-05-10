using System.Collections;
using UnityEngine;

namespace Utilities
{
    public class ScaleObjectOverTime : MonoBehaviour
    {
        [SerializeField] private Vector3 fromScale = new Vector3(1f, 1f, 1f);
        [SerializeField] private Vector3 toScale = new Vector3(2f, 2f, 2f);
        [SerializeField] private float duration = 2f;
        [SerializeField] private float delay = 1f;
        
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
        }
    }
}