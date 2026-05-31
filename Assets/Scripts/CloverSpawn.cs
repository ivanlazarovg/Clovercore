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
        foreach (var clover in cloverChildren)
        {
            RaycastHit hit;
            bool raycast = Physics.Raycast(clover.transform.position, -transform.up, out hit, 4, groundLayer, QueryTriggerInteraction.Collide) 
                || Physics.Raycast(clover.transform.position, -transform.right, out hit, 4, groundLayer, QueryTriggerInteraction.Collide) 
                || Physics.Raycast(clover.transform.position, transform.right, out hit, 4, groundLayer, QueryTriggerInteraction.Collide)
                || Physics.Raycast(clover.transform.position, -transform.forward, out hit, 4, groundLayer, QueryTriggerInteraction.Collide)
                || Physics.Raycast(clover.transform.position, transform.forward, out hit, 4, groundLayer, QueryTriggerInteraction.Collide);

            if (raycast)
            {
                if(hit.collider.gameObject.layer == 6 && hit.collider.gameObject != this.gameObject)
                {
                    clover.gameObject.SetActive(false);
                }
                else
                {
                    clover.transform.position = hit.point;
                    clover.transform.rotation = Quaternion.LookRotation(hit.normal);
                }
                
            }
            else
            {
                clover.gameObject.SetActive(false);
            }

        }
    }

    IEnumerator GrowClovers()
    {
        float t = 1.55f;

        while(t < 6)
        {
            t += Time.deltaTime * (growSpeed + ((6-t) * 0.5f));
            foreach (var clover in cloverChildren)
            {
                clover.material.SetFloat("_Height", t);
            }
            yield return null;
        }
        
    }
}
