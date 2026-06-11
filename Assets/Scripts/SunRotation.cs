using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotation : MonoBehaviour
{
    public Transform endPos;
    public Transform startPos;

    Transform playerTransform;

    public Vector3 startEulerAngles;
    public Vector3 endEulerAngles;

    float distanceProgress;
    float fullDistance;
    void Start()
    {
        fullDistance = Vector3.Distance(startPos.position, endPos.position);
        playerTransform = Camera.main.transform;
    }

    void Update()
    {
        var currentDistance = Vector3.Distance(playerTransform.position, endPos.position);
        
        distanceProgress = Mathf.Lerp(0, fullDistance, currentDistance / fullDistance) / fullDistance;

        transform.rotation = Quaternion.Lerp(Quaternion.Euler(startEulerAngles), Quaternion.Euler(endEulerAngles), 1-distanceProgress);
    }
}

