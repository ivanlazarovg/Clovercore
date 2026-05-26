using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloverProjectile : MonoBehaviour
{
    [SerializeField] private GameObject cloverPrefab;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.layer == 3)
        {
            InstantiateClover();
        }

        Destroy(gameObject);
    }

    void InstantiateClover()
    {
        Instantiate(cloverPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
    }
}
