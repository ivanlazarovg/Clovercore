using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAttributes : MonoBehaviour
{
    [Header("Text Properties")]
    [Space(10)]
    public float appearSpeed = 1;
    public float fadeInMultiplier = 2;
    public float fadeOutMultiploer = 2;
    public float fadeInDilate;
    public AnimationCurve dilateFadeOutCurve;
    [GradientUsage(true)] public Gradient flashGradient;

    private static TextAttributes instance;

    public static TextAttributes Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindAnyObjectByType<TextAttributes>();
            }
            return instance;
        }
    }

    public IEnumerator FadeIn(Renderer textRenderer)
    {
        textRenderer.material.SetColor("_SpecColor", flashGradient.Evaluate(0));
        textRenderer.material.SetFloat("_FaceDilate", -1);

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * appearSpeed * fadeInMultiplier;

            textRenderer.material.SetFloat("_FaceDilate", Mathf.Lerp(-1, fadeInDilate, t));

            yield return null;
        }

        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * appearSpeed * fadeOutMultiploer;

            textRenderer.material.SetFloat("_FaceDilate", dilateFadeOutCurve.Evaluate(t));
            textRenderer.material.SetColor("_SpecColor", flashGradient.Evaluate(t));

            yield return null;
        }
        StopAllCoroutines();
    }
}
