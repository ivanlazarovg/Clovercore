using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextStanza : MonoBehaviour
{
    [SerializeField, TextArea()] protected string[] poemTexts;
    public AudioClip[] voiceOverClips;

    public int textIndex = 0;

    public virtual void SetText(TextMeshPro poemTextMesh, BoxCollider collider)
    {
        poemTextMesh.text = poemTexts[textIndex];
        poemTextMesh.ForceMeshUpdate();

        AudioManager.Instance.TriggerVoiceOver(voiceOverClips[textIndex]);
        textIndex++;

        collider.size = new Vector3(poemTextMesh.textBounds.size.x * 1.2f, poemTextMesh.textBounds.size.y * 1.5f, poemTextMesh.textBounds.size.z);
    }
}
