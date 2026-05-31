using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextReveal : MonoBehaviour
{
    public TextStanza stanza;

    private Renderer textRenderer;
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
        textRenderer = GetComponent<Renderer>();
        textRenderer.enabled = false;

        textCollider = GetComponent<BoxCollider>();
    }

    void AppearText()
    {
        stanza.SetText(GetComponent<TextMeshPro>(), textCollider);

        textRenderer.enabled = true;
        hasAppeared = true;

        StartCoroutine(TextAttributes.Instance.FadeIn(textRenderer));
    }


}
