using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClips : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] Audio;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetLaserAudioClip()
    {
        Audio[0].Play();
    }

    public void GetExplosionAudio()
    {
        Audio[1].Play();
    }

    public void GetPowerUpAudio()
    {
        Audio[2].Play();
    }

    public void GetNoAmmoAudio()
    {
        Audio[3].Play();
    }
}
