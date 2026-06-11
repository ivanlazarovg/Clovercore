using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SunflowerStanza : TextStanza
{
    public GameObject sunflowerObject;

    private void Start()
    {
        sunflowerObject.SetActive(false);
    }
    public override void SetText(TextMeshPro poemTextMesh, BoxCollider collider)
    {
        base.SetText(poemTextMesh, collider);

        if(textIndex == poemTexts.Length - 1)
        {
            sunflowerObject.SetActive(true);
        }
    }

}
