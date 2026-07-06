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
    public AudioClip[] audioRevealClips;
    public AudioSource swellSFXSource;
    public Credits credits;
    private bool textRevealed;

    AudioSource source;
    void OnEnable()
    {
        StartCoroutine(RevealSunflowers(0, startHeight, revealSpeed * 6));

        source = GetComponent<AudioSource>();
        source.Play(41100);
    }

    public AudioClip GetRandomClip()
    {
        return audioRevealClips[Random.Range(0, audioRevealClips.Length-1)];
    }

    // Update is called once per frame
    void Update()
    {
        if (finalReveal.hasAppeared && !textRevealed)
        {
            StartCoroutine(RevealSunflowers(startHeight, finalHeight, revealSpeed));
            swellSFXSource.Play();
            textRevealed = true;

            StartCoroutine(credits.Trigger());
        }
    }

    IEnumerator RevealSunflowers(float startHeight, float finalHeight, float _revealSpeed)
    {
        foreach (var mat in materials)
        {
            mat.SetVector("_CenterPosition", transform.position);
        }

        float t = 0;

        while(t < 1)
        {
            t += Time.deltaTime * _revealSpeed;
            foreach(var mat in materials)
            {
                mat.SetFloat("_Height", Mathf.SmoothStep(startHeight, finalHeight, t));
            }
            yield return null;
        }
    }
}
