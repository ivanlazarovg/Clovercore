using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandlesSnuffOut : MonoBehaviour
{
    private Candle[] allCandles;
    private AudioSource source;

    public TextStanza stanza;
    void Start()
    {
        source = GetComponent<AudioSource>();
        allCandles = FindObjectsByType(typeof(Candle), FindObjectsSortMode.None) as Candle[];
    }

    void Update()
    {
        if(stanza.textIndex == 3)
        {
            SnuffOut();
            Destroy(this);
        }
    }

    public void SnuffOut()
    {
        foreach(Candle candle in allCandles)
        {
            candle.SnuffOutFlame();
        }
        source.Play();
    }
}
