using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{

    public GameObject light;
    public GameObject flame;
    public void EnableFlame()
    {
        light.SetActive(true);
        flame.SetActive(true);
    }

    public void SnuffOutFlame()
    {
        light.SetActive(false);
        flame.SetActive(false);
    }
}
