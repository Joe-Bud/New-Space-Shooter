using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClips : MonoBehaviour
{

    [SerializeField]
    private AudioSource[] audioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayLaserAudio()
    {
        audioSource[0].Play();
    }

    public void PlayExplosionAudio()
    {
        audioSource[1].Play();
    }

    public void PlayPowerupAudio()
    {
        audioSource[2].Play();
    }
}
