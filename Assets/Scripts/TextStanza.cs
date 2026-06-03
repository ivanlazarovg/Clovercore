using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextStanza : MonoBehaviour
{
    [SerializeField, TextArea()] private string[] poemTexts;

    public int textIndex = 0;

    public void SetText(TextMeshPro poemTextMesh, BoxCollider collider)
    {
        poemTextMesh.text = poemTexts[textIndex];
        poemTextMesh.ForceMeshUpdate();
        textIndex++;

        collider.size = new Vector3(poemTextMesh.textBounds.size.x * 1.2f, poemTextMesh.textBounds.size.y * 1.5f, poemTextMesh.textBounds.size.z);
    }
}
