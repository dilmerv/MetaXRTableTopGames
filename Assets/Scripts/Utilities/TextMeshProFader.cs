using System.Collections;
using TMPro;
using UnityEngine;

namespace Utilities
{
    public class TextMeshProFader : MonoBehaviour
    {
        [SerializeField] private float fadeInDuration = 0.5f;
        [SerializeField] private float showDuration = 1.5f;
        [SerializeField] private float fadeOutDuration = 1f;

        private TextMeshPro textToFade;
    
        private void Start()
        {
            textToFade = GetComponent<TextMeshPro>();
        }

        public void FadeInWithText(string text)
        {
            textToFade.text = text;
            StartCoroutine(FadeTextToFullAlpha(fadeInDuration));
            Invoke(nameof(ShowText), fadeInDuration);
        }

        private void ShowText()
        {
            Invoke(nameof(FadeOutText), showDuration);
        }

        private void FadeOutText()
        {
            StartCoroutine(FadeTextToZeroAlpha(fadeOutDuration));
        }

        private IEnumerator FadeTextToFullAlpha(float t)
        {
            textToFade.color = new Color(textToFade.color.r, textToFade.color.g, textToFade.color.b, 0);
            while (textToFade.color.a < 1.0f)
            {
                textToFade.color = new Color(textToFade.color.r, textToFade.color.g, textToFade.color.b, textToFade.color.a + (Time.deltaTime / t));
                yield return null;
            }
        }

        private IEnumerator FadeTextToZeroAlpha(float t)
        {
            textToFade.color = new Color(textToFade.color.r, textToFade.color.g, textToFade.color.b, 1);
            while (textToFade.color.a > 0.0f)
            {
                textToFade.color = new Color(textToFade.color.r, textToFade.color.g, textToFade.color.b, textToFade.color.a - (Time.deltaTime / t));
                yield return null;
            }
        }
    }
}