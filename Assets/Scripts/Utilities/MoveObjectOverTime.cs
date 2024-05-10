using System.Collections;
using UnityEngine;

public class MoveObjectOverTime : MonoBehaviour
{
    [SerializeField]
    private Vector3 fromPosition = new Vector3(0f, 0f, 0f);
    
    [SerializeField]
    private Vector3 toPosition = new Vector3(0f, 1f, 0f);
    
    public float duration = 2f; 
    public float delay = 1f;

    public void Move(Vector3 fromPosition, Vector3 toPosition, float delay, float duration)
    {
        this.fromPosition = fromPosition;
        this.toPosition = toPosition;
        this.delay = delay;
        this.duration = duration;

        StartCoroutine(MoveCoroutine());
    }
    
    private IEnumerator MoveCoroutine()
    {
        yield return new WaitForSeconds(delay);

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;
            transform.localPosition = Vector3.Lerp(fromPosition, toPosition, progress);
            yield return null;
        }
        transform.localPosition = toPosition;
    }
}