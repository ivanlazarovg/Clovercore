using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloverProjectile : MonoBehaviour
{
    [SerializeField] private GameObject cloverPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.layer == 3 || collision.collider.gameObject.GetComponent<TextReveal>())
        {
            StartCoroutine(InstantiateClover(collision.GetContact(0)));
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    IEnumerator InstantiateClover(ContactPoint contact)
    {
        yield return new WaitForSeconds(0.02f);

        Instantiate(cloverPrefab, transform.position + Vector3.up * 0.5f, Quaternion.FromToRotation(Vector3.up, contact.normal));
        Destroy(gameObject);

        StopAllCoroutines();
    }
}
