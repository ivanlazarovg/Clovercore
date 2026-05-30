using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextReveal : MonoBehaviour
{
    [Header("Text Properties")]
    [Space(10)]
    [SerializeField] private float appearSpeed = 1;
    [SerializeField] private float fadeInMultiplier = 2;
    [SerializeField] private float fadeOutMultiploer = 2;
    [SerializeField] private float fadeInDilate;
    [SerializeField] private AnimationCurve dilateFadeOutCurve;
    [SerializeField, GradientUsage(true)] private Gradient flashGradient;

    private Renderer textMeshPro;
    private BoxCollider textCollider;
    private bool hasAppeared = false;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Projectile") && !hasAppeared)
        {
            AppearText();
        }
    }

    void Start()
    {
        textMeshPro = GetComponent<Renderer>();
        textMeshPro.enabled = false;

        textCollider = GetComponent<BoxCollider>();
        textCollider.size = new Vector3(textCollider.size.x * 1.2f, textCollider.size.y * 1.4f, textCollider.size.z);
    }

    void AppearText()
    {
        textMeshPro.enabled = true;
        hasAppeared = true;

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        textMeshPro.material.SetColor("_SpecColor", flashGradient.Evaluate(0));
        textMeshPro.material.SetFloat("_FaceDilate", -1);

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * appearSpeed * fadeInMultiplier;

            textMeshPro.material.SetFloat("_FaceDilate", Mathf.Lerp(-1, fadeInDilate, t));

            yield return null;
        }

        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * appearSpeed * fadeOutMultiploer;

            textMeshPro.material.SetFloat("_FaceDilate", dilateFadeOutCurve.Evaluate(t));
            textMeshPro.material.SetColor("_SpecColor", flashGradient.Evaluate(t));

            yield return null;
        }
        StopAllCoroutines();
    }


}
