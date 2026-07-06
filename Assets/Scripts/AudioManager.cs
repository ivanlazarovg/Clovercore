using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioSource> allSources;
    public AudioSource musicSource;

    private AudioSource voiceOverSource;
    private Queue<AudioClip> voiceOverQueue = new Queue<AudioClip>();

    private bool audioDucked;

    private static AudioManager instance;

    public static AudioManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindAnyObjectByType<AudioManager>();
            }
            return instance;
        }
    }

    void Start()
    {
        var texts = FindObjectsByType<TextReveal>(FindObjectsSortMode.None);

        foreach (var text in texts)
        {
            allSources.Add(text.GetComponent<AudioSource>());
        }
        allSources.Add(musicSource);

        voiceOverSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!voiceOverSource.isPlaying && voiceOverQueue.Count > 0)
        {
            voiceOverSource.clip = voiceOverQueue.Dequeue();
            voiceOverSource.Play();
        }


        if(voiceOverSource.isPlaying && audioDucked == false)
        {
            StartCoroutine(DuckAudio());
        }
        else if(!voiceOverSource.isPlaying && audioDucked == true)
        {
            StartCoroutine(UnduckAudio());
        }
    }


    public IEnumerator DuckAudio()
    {
        audioDucked = true;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 2f;
            foreach (var source in allSources)
            {
                source.volume = Mathf.Lerp(1, 0.3f, t);
            }
            yield return null;
        }
        
    }

    public IEnumerator UnduckAudio()
    {

        audioDucked = false;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 2f;
            foreach (var source in allSources)
            {
                source.volume = Mathf.Lerp(0.3f, 1f, t);
            }
            yield return null;
        }


    }

    public void TriggerVoiceOver(AudioClip voiceclip)
    {
        if (!voiceOverSource.isPlaying)
        {
            voiceOverSource.clip = voiceclip;
            voiceOverSource.Play();
        }
        else
        {
            voiceOverQueue.Enqueue(voiceclip);
        }

    }
}
