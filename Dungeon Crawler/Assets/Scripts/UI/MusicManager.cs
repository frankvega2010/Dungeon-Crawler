using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviourSingleton<MusicManager>
{
    public GameObject menu;
    public GameObject dungeon;
    public GameObject loss;
    public GameObject house;

    public AudioSource menuSong;
    public AudioSource dungeonSong;
    public AudioSource lossSong;
    public AudioSource houseSong;
    // Start is called before the first frame update
    void Start()
    {
        menuSong = menu.GetComponent<AudioSource>();
        dungeonSong = dungeon.GetComponent<AudioSource>();
        lossSong = loss.GetComponent<AudioSource>();
        houseSong = house.GetComponent<AudioSource>();
        menuSong.Play();
    }

    /*// Update is called once per frame
    void Update()
    {
        
    }*/

    public void StopAllSongs()
    {
        menuSong.Stop();
        dungeonSong.Stop();
        lossSong.Stop();
        houseSong.Stop();
    }
}
