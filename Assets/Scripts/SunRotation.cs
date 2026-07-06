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

    public float distanceProgress;
    float fullDistance;
    bool hasReached = false;
    void Start()
    {
        fullDistance = Vector3.Distance(startPos.position, endPos.position);
        playerTransform = Camera.main.transform;
    }

    void Update()
    {

        if (hasReached)
        {
            return;
        }

        var currentDistance = Vector3.Distance(playerTransform.position, endPos.position);
        
        distanceProgress = Mathf.Lerp(0, fullDistance, currentDistance / fullDistance) / fullDistance;

        transform.rotation = Quaternion.Lerp(Quaternion.Euler(startEulerAngles), Quaternion.Euler(endEulerAngles), 1-distanceProgress);

        if (distanceProgress < 0.13f)
        {
            hasReached = true;
        }
    }

    public IEnumerator FinalizeSunSet()
    {
        while(distanceProgress > 0)
        {
            distanceProgress -= Time.deltaTime * 0.01f;
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(startEulerAngles), Quaternion.Euler(endEulerAngles), 1 - distanceProgress);
            yield return null;
        }
    }
}

