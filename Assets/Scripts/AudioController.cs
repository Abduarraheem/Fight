using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    AudioSource audioSource;
    float baseVolume;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        baseVolume = audioSource.volume;
    }

    // Update is called once per frame
    void Update()
    {
        SwapScene g = FindObjectOfType<SwapScene>();
        if (g) audioSource.volume = baseVolume * g.volume;
        Debug.Log(audioSource.volume);
    }
}
