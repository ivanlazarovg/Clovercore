using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowReveal : MonoBehaviour
{
    public TextReveal textReveal;

    private Renderer renderer;
    private bool isRolling = false;
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (textReveal.hasAppeared && !isRolling)
        {
            StartCoroutine(RollUp());
            isRolling = true;
        }
    }

    IEnumerator RollUp()
    {
        float t = 0;
        Vector3 startscale = transform.localScale;
        while ( t< 1)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startscale, new Vector3(0,startscale.z, startscale.z), t);
            yield return null;
        }
        Destroy(this);
    }
}
