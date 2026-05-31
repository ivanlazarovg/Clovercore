using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextReveal : MonoBehaviour
{
    public TextStanza stanza;

    private Renderer textRenderer;
    private BoxCollider textCollider;
    private TextMeshPro textMesh;
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
        textMesh = GetComponent<TextMeshPro>();
    }

    void AppearText()
    {
        stanza.SetText(textMesh, textCollider);

        textRenderer.enabled = true;
        hasAppeared = true;

        StartCoroutine(TextAttributes.Instance.FadeIn(textRenderer));
    }

}
