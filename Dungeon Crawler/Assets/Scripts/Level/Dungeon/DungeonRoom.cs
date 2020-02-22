using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    [System.Serializable]
    public class EnemyGroup
    {
        public GameObject[] enemies;
    }

    [Header("General Settings")]
    public Transform nextRoomLocation;
    public GameObject[] doors;
    public bool eventHasStarted;

    [Header("Item Settings")]
    public GameObject itemToSpawn;
    public Transform itemLocation;

    [Header("Spawn Settings")]
    public int maxEnemies;
    public GameObject SpawnPointGroup;
    public EnemyGroup[] enemyGroups;
    public List<GameObject> enemiesSpawned;
    private Transform[] allSpawnPoints;
    private EnemyDamageBox spawnTrigger;
    public GameObject spawnedItem;
    // Start is called before the first frame update
    void Start()
    {
        spawnTrigger = GetComponentInChildren<EnemyDamageBox>();
        spawnTrigger.OnBoxEnterCollider += StartSpawnEvent;
        //spawnTrigger.OnBoxExitCollider += FinishSpawnEvent;

        /*itemToSpawn = GameManager.Get().item;
        GameObject newItem = Instantiate(itemToSpawn);
        newItem.SetActive(true);
        newItem.transform.position = itemLocation.transform.position;
        newItem.transform.rotation = itemLocation.transform.rotation;
        newItem.GetComponent<ObjectCore>().canBePickedUp = false;
        spawnedItem = newItem;*/

    }

    // Update is called once per frame
    void Update()
    {
        if(eventHasStarted)
        {
            if(enemiesSpawned.Count <= 0)
            {
                FinishSpawnEvent();
                eventHasStarted = false;
            }
        }
    }

    public void StartSpawnEvent(string tag)
    {
        if(tag == "player")
        {
            spawnTrigger.gameObject.SetActive(false);
            CloseAllDoors();

            allSpawnPoints = new Transform[SpawnPointGroup.transform.childCount];
            for (int i = 0; i < allSpawnPoints.Length; i++)
            {
                allSpawnPoints[i] = SpawnPointGroup.transform.GetChild(i);
            }

            maxEnemies = allSpawnPoints.Length;

            int randomGroupNumber = Random.Range(0, enemyGroups.Length);

            for (int i = 0; i < enemyGroups[randomGroupNumber].enemies.Length; i++)
            {
                if(i < maxEnemies)
                {
                    GameObject newEnemy = Instantiate(enemyGroups[randomGroupNumber].enemies[i]);
                    newEnemy.transform.position = allSpawnPoints[i].transform.position;
                    newEnemy.transform.rotation = allSpawnPoints[i].transform.rotation;
                    enemiesSpawned.Add(newEnemy);
                    newEnemy.GetComponent<EnemyController>().OnEnemyDeath += DeleteFromList;
                }
            }

            eventHasStarted = true;
        }
    }

    public void FinishSpawnEvent()
    {
        // Open Gate
        doors[doors.Length-1].SetActive(false);
        spawnedItem.GetComponent<ObjectCore>().canBePickedUp = true;
    }

    public void OpenAllDoors()
    {
        // Open all doors
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].SetActive(false);
        }
    }

    public void CloseAllDoors()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].SetActive(true);
        }
    }

    public void DeleteFromList(GameObject currentObject)
    {
        enemiesSpawned.Remove(currentObject);
        currentObject.GetComponent<EnemyController>().OnEnemyDeath -= DeleteFromList;
    }

    private void OnDestroy()
    {
        spawnTrigger.OnBoxEnterCollider -= StartSpawnEvent;
        //spawnTrigger.OnBoxExitCollider -= FinishSpawnEvent;
    }
}
