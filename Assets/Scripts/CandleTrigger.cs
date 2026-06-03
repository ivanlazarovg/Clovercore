using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleTrigger : MonoBehaviour
{
    private Candle[] Candles;

    private void Start()
    {
        Candles = GetComponentsInChildren<Candle>();
    }
    private void OnTriggerEnter(Collider collider)
    {
        //check if projectile layer
        if(collider.gameObject.layer == 7)
        {
            foreach(var candle in Candles)
            {
                candle.EnableFlame();
            }
            Destroy(this);
        }
    }

}
