using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloverSpawn : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;

    private Renderer[] cloverChildren;
    
    private void OnEnable()
    {
        //init shader stuff
        foreach(var clover in cloverChildren)
        {
            clover.material.SetFloat("_GlowWarp", Random.Range(-0.3f, 0.3f));
        }

        StartCoroutine(GrowClovers());
    }
    void CalculatePositions()
    {
        foreach(var clover in cloverChildren)
        {
            RaycastHit hit;
            if (Physics.Raycast(clover.transform.position, -transform.up, out hit, groundLayer))
            {
                clover.transform.position = hit.point;
            }
        }
    }

    IEnumerator GrowClovers()
    {
        float t = 0;

        while(t < 5)
        {
            t += Time.deltaTime;
            foreach (var clover in cloverChildren)
            {
                clover.material.SetFloat("_Height", Mathf.SmoothStep(0, 5, t));
            }
            yield return null;
        }
        
    }
}
