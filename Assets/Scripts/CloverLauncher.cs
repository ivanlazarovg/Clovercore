using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloverLauncher : MonoBehaviour
{
    [SerializeField] private GameObject cloverProjectilePrefab;
    [SerializeField] private Transform cloverLauncherTransform;
    [SerializeField] private float launchStrength;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LaunchProjectile();
        }

        transform.rotation = cloverLauncherTransform.rotation;
        transform.position = Vector3.Slerp(transform.position, cloverLauncherTransform.position, 40 * Time.deltaTime);

    }

    void LaunchProjectile()
    {
        var projectile = Instantiate(cloverProjectilePrefab, transform.position, Quaternion.identity);
        var currentCloverRb = projectile.GetComponent<Rigidbody>();

        currentCloverRb.AddForce(transform.forward * launchStrength, ForceMode.Impulse);
    }
}
