using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] music;
    [SerializeField] private AudioSource audioSource;
    private void Awake()
    {
        audioSource.loop = false;
    }

    private void Update()
    {
        if(!audioSource.isPlaying)
        {
            audioSource.clip = GetRandomClip();
            audioSource.Play();
        }
    }
    private AudioClip GetRandomClip()
    {
        return music[Random.Range(0, music.Length)];
    }
}
