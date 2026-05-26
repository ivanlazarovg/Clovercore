using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloverSpawn : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask cloverLayer;
    [SerializeField] private float growSpeed;

    private List<Renderer> cloverChildren;
    
    public void OnEnable()
    {
        cloverChildren = new List<Renderer>(GetComponentsInChildren<Renderer>());
        //init shader stuff
        foreach(var clover in cloverChildren)
        {
            clover.material.SetFloat("_GlowWarp", Random.Range(-0.3f, 0.3f));
            clover.material.SetVector("_CenterPosition", transform.position);
        }

        StartCoroutine(GrowClovers());
        CalculatePositions();
    }
    void CalculatePositions()
    {
        foreach(var clover in cloverChildren)
        {
            RaycastHit hit;
            if (Physics.Raycast(clover.transform.position, -transform.up, out hit, groundLayer))
            {
                clover.transform.position = hit.point;
                clover.transform.rotation = Quaternion.LookRotation(hit.normal);
            }

        }
    }

    IEnumerator GrowClovers()
    {
        float t = 1.55f;

        while(t < 5)
        {
            t += Time.deltaTime * (growSpeed + ((5-t) * 0.5f));
            foreach (var clover in cloverChildren)
            {
                clover.material.SetFloat("_Height", t);
            }
            yield return null;
        }
        
    }
}
