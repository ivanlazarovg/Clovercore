using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunflowerTrigger : MonoBehaviour
{
    public TextReveal finalReveal;
    public float revealSpeed;
    public float finalHeight;
    public float startHeight;

    public Material[] materials;
    private bool textRevealed;
    void OnEnable()
    {
        StartCoroutine(RevealSunflowers(0, startHeight));
    }

    // Update is called once per frame
    void Update()
    {
        if (finalReveal.hasAppeared && !textRevealed)
        {
            StartCoroutine(RevealSunflowers(startHeight, finalHeight));
            textRevealed = true;
        }
    }

    IEnumerator RevealSunflowers(float startHeight, float finalHeight)
    {
        foreach (var mat in materials)
        {
            mat.SetVector("_CenterPosition", transform.position);
        }

        float t = 0;

        while(t < 1)
        {
            t += Time.deltaTime * revealSpeed;
            foreach(var mat in materials)
            {
                mat.SetFloat("_Height", Mathf.SmoothStep(startHeight, finalHeight, t));
            }
            yield return null;
        }
    }
}
