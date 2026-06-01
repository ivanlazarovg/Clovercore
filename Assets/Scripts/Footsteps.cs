using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [Space(10)]
    [Header("AudioClips")]
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private AudioSource footstepAudio;

    public void PlayFootstep()
    {
        footstepAudio.clip = footstepClips[Random.Range(0, footstepClips.Length - 1)];
        footstepAudio.Play();
    }
}
