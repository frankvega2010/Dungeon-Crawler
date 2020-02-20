using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DungeonGenerator : MonoBehaviour
{
    public delegate void OnGeneratorAction();
    public OnGeneratorAction OnGeneratorFinish;

    [System.Serializable]
    public class EnemyRoomSpawner
    {
        public DungeonRoom.EnemyGroup[] groups;
    }

    [Header("General Settings")]
    public Transform initialLocation;
    public GameObject[] presets;
    public GameObject finalRoomPreset;

    [Header("Spawn Settings")]
    public EnemyRoomSpawner[] roomEnemySpawner;
    //public DungeonRoom.EnemyGroup[] groups;

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
            DungeonRoom newRoomProperties0 = firstRoom.GetComponent<DungeonRoom>();
            newRoomProperties0.enemyGroups = roomEnemySpawner[0].groups;
            firstRoom.transform.position = initialLocation.position;
            firstRoom.transform.rotation = initialLocation.rotation;
            roomsSelected[0] = firstRoom;
            roomsSelected[0].SetActive(true);

            GameObject itemToSpawn = GameManager.Get().itemsNeeded[0];
            itemToSpawn.SetActive(true);
            itemToSpawn.transform.position = newRoomProperties0.itemLocation.transform.position;
            itemToSpawn.transform.rotation = newRoomProperties0.itemLocation.transform.rotation;
            itemToSpawn.GetComponent<ObjectCore>().canBePickedUp = false;
            itemToSpawn.GetComponent<ObjectCore>().OnObjectPickedUp += newRoomProperties0.OpenAllDoors;
            newRoomProperties0.spawnedItem = itemToSpawn;

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
                DungeonRoom newRoomProperties = newRoom.GetComponent<DungeonRoom>();
                newRoomProperties.enemyGroups = roomEnemySpawner[i].groups;
                newRoom.transform.position = lastRoom.nextRoomLocation.position;
                newRoom.transform.rotation = lastRoom.nextRoomLocation.rotation;
                roomsSelected[i] = newRoom;
                roomsSelected[i].SetActive(true);

                GameObject itemToSpawnIterator = GameManager.Get().itemsNeeded[i];
                itemToSpawnIterator.SetActive(true);
                itemToSpawnIterator.transform.position = newRoomProperties.itemLocation.transform.position;
                itemToSpawnIterator.transform.rotation = newRoomProperties.itemLocation.transform.rotation;
                itemToSpawnIterator.GetComponent<ObjectCore>().canBePickedUp = false;
                itemToSpawnIterator.GetComponent<ObjectCore>().OnObjectPickedUp += newRoomProperties.OpenAllDoors;
                newRoomProperties.spawnedItem = itemToSpawnIterator;

                lastRandomNumber = randomRoomNumber2;
            }


            DungeonRoom lastRoom2 = roomsSelected[maxRooms - 1].GetComponent<DungeonRoom>();
            GameObject finalRoom = Instantiate(finalRoomPreset);
            DungeonRoom newRoomProperties2 = finalRoom.GetComponent<DungeonRoom>();
            newRoomProperties2.enemyGroups = roomEnemySpawner[maxRooms].groups;
            finalRoom.transform.position = lastRoom2.nextRoomLocation.position;
            finalRoom.transform.rotation = lastRoom2.nextRoomLocation.rotation;
            roomsSelected[maxRooms] = finalRoom;
            roomsSelected[maxRooms].SetActive(true);

            GameObject itemToSpawnLast = GameManager.Get().itemsNeeded[maxRooms];
            itemToSpawnLast.SetActive(true);
            itemToSpawnLast.transform.position = newRoomProperties2.itemLocation.transform.position;
            itemToSpawnLast.transform.rotation = newRoomProperties2.itemLocation.transform.rotation;
            itemToSpawnLast.GetComponent<ObjectCore>().canBePickedUp = false;
            itemToSpawnLast.GetComponent<ObjectCore>().isLastItem = true;
            itemToSpawnLast.GetComponent<ObjectCore>().OnObjectPickedUp += newRoomProperties2.OpenAllDoors;
            newRoomProperties2.spawnedItem = itemToSpawnLast;
        }
        
        if(OnGeneratorFinish != null)
        {
            OnGeneratorFinish();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
