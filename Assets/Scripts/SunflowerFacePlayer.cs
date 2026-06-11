using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunflowerFacePlayer : MonoBehaviour
{
    Transform target;

    private void Start()
    {
        target = Camera.main.transform;
    }
    void Update()
    {
        transform.rotation = Quaternion.LookRotation((transform.position - target.position).normalized);
    }
}
