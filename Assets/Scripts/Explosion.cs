using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    AudioClips AC;

    // Start is called before the first frame update
    void Start()
    {
        AC = GameObject.Find("AudioManager").GetComponent<AudioClips>();

        if (AC == null)
            Debug.LogError("Explosion:AudioClips == NULL");

        AC.PlayExplosionAudio();

        Destroy(this.gameObject, 2.8f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
