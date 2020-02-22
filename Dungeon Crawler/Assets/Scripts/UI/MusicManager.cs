using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviourSingleton<MusicManager>
{
    public GameObject menu;
    public GameObject gameplay;
    public GameObject outro;

    public AudioSource menuSong;
    public AudioSource gameplaySong;
    public AudioSource outroSong;
    // Start is called before the first frame update
    void Start()
    {
        menuSong = menu.GetComponent<AudioSource>();
        gameplaySong = gameplay.GetComponent<AudioSource>();
        outroSong = outro.GetComponent<AudioSource>();
        menuSong.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopAllSongs()
    {
        menuSong.Stop();
        gameplaySong.Stop();
        outroSong.Stop();
    }
}
