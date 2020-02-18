using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;

    public void SetMusicVol(float slider)
    {
        mixer.SextFloat("MusicVol",Mathf.Log10(slider)*20);
    }

}
