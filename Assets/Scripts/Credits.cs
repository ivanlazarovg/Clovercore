using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    Transform playerCamera;
    Movement movement;
    SunRotation sunRotation;

    public UnityEngine.UI.Image image;

    public GameObject[] textStuff;

    bool cameraSwell = false;
    void Start()
    {
        movement = FindAnyObjectByType<Movement>();
        sunRotation = FindAnyObjectByType<SunRotation>();
        playerCamera = Camera.main.transform;

        DisableAllTexts();
    }

    private void Update()
    {
        if (cameraSwell)
        {
            playerCamera.transform.position = Vector3.Slerp(playerCamera.transform.position, playerCamera.transform.position + Vector3.up * Time.deltaTime * 2, 5 * Time.deltaTime);
        }
    }

    public IEnumerator Trigger()
    {
        movement.enabled = false;

        yield return new WaitForSeconds(5);
        cameraSwell = true;
        StartCoroutine(sunRotation.FinalizeSunSet());

        yield return new WaitForSeconds(6);
        StartCoroutine(AppearEndScreen());

        yield return new WaitForSeconds(7);
        DisableAllTexts();
        textStuff[0].SetActive(true);

        yield return new WaitForSeconds(5);
        DisableAllTexts();
        textStuff[1].SetActive(true);

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        DisableAllTexts();
        textStuff[2].SetActive(true);
        yield return new WaitForSeconds(1);

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        DisableAllTexts();
        textStuff[3].SetActive(true);
        yield return new WaitForSeconds(1);

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        DisableAllTexts();

    }

    public void DisableAllTexts()
    {
        foreach (var text in textStuff)
        {
            text.SetActive(false);
        }
    }

    IEnumerator AppearEndScreen()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 0.2f;
            image.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), t);
            yield return null;
        }
    }

}
