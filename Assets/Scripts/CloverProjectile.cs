using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloverProjectile : MonoBehaviour
{
    [SerializeField] private GameObject cloverPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.layer == 9 || collision.collider.gameObject.GetComponent<TextReveal>())
        {
            StartCoroutine(InstantiateClover());
        }
        else if(collision.collider.gameObject.layer == 3)
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

        Instantiate(cloverPrefab, transform.position + Vector3.up * 0.3f, Quaternion.FromToRotation(Vector3.up, contact.normal));
        Destroy(gameObject);

        StopAllCoroutines();
    }
    IEnumerator InstantiateClover()
    {
        yield return new WaitForSeconds(0.02f);

        Instantiate(cloverPrefab, transform.position + -Camera.main.transform.forward * 0.8f , Quaternion.FromToRotation(Vector3.up, -Camera.main.transform.forward));
        Destroy(gameObject);

        StopAllCoroutines();
    }

}
