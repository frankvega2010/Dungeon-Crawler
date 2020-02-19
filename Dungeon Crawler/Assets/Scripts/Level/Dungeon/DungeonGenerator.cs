using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("General Settings")]
    public Transform initialLocation;
    //public Transform finalLocation;
    public GameObject[] presets;
    public GameObject finalRoomPreset;

    [Header("Presets Settings")]
    public int maxRooms;

    [Header("Check Variables")]
    public GameObject[] roomsSelected;

    // Start is called before the first frame update
    void Start()
    {
        if(presets.Length >= 1)
        {
            roomsSelected = new GameObject[maxRooms + 1];
            int randomRoomNumber = Random.Range(0, presets.Length);

            GameObject firstRoom = Instantiate(presets[randomRoomNumber]);
            firstRoom.transform.position = initialLocation.position;
            firstRoom.transform.rotation = initialLocation.rotation;
            roomsSelected[0] = firstRoom;
            roomsSelected[0].SetActive(true);

            int repeatedRoom;
            int lastRandomNumber;

            if(randomRoomNumber != 0)
            {
                repeatedRoom = 1;
                lastRandomNumber = randomRoomNumber;
            }
            else
            {
                repeatedRoom = 0;
                lastRandomNumber = -1;
            }

            for (int i = 1; i < maxRooms; i++)
            {
                int randomRoomNumber2 = Random.Range(0, presets.Length);

                if(randomRoomNumber2 == lastRandomNumber && randomRoomNumber2 != 0)
                {
                    repeatedRoom++;
                    if(repeatedRoom >= 2)
                    {
                        randomRoomNumber2 = 0;
                        repeatedRoom = 1;
                    }
                }
                else
                {
                    repeatedRoom = 1;
                }

                DungeonRoom lastRoom = roomsSelected[i - 1].GetComponent<DungeonRoom>();
                GameObject newRoom = Instantiate(presets[randomRoomNumber2]);
                newRoom.transform.position = lastRoom.nextRoomLocation.position;
                newRoom.transform.rotation = lastRoom.nextRoomLocation.rotation;
                roomsSelected[i] = newRoom;
                roomsSelected[i].SetActive(true);
                lastRandomNumber = randomRoomNumber2;
            }


            DungeonRoom lastRoom2 = roomsSelected[maxRooms - 1].GetComponent<DungeonRoom>();
            GameObject finalRoom = Instantiate(finalRoomPreset);
            finalRoom.transform.position = lastRoom2.nextRoomLocation.position;
            finalRoom.transform.rotation = lastRoom2.nextRoomLocation.rotation;
            roomsSelected[maxRooms] = finalRoom;
            roomsSelected[maxRooms].SetActive(true);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
