using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [Header("General Settings")]
    public GameObject checklistItemUIPrefab;
    public GameObject playerPrefab;
    public GameObject player;
    public GameObject playerCheckList;
    public GameObject UI;
    public Door houseDoor;
    public StaffBar staffBar;
    public HealthBar hpBar;
    public Animator canvasAnimator;
    public GameObject initialRoom;
    public Door initialDoor;
    public DungeonGenerator generator;
    public ObjectPlacement uniquePlacement;

    [Header("Loss Win Settings")]
    public float waitTime;
    public GameObject winPanel;
    public GameObject lossPanel;
    public LevelLoader levelLoader;

    [Header("Spawn Settings")]
    public bool spawnInForest;
    public Transform houseSpawn;
    public Transform forestSpawn;

    [Header("Item Settings")]
    public int maxItemsNeeded;
    public GameObject[] allItems;
    public GameObject[] itemsNeeded;

    public List<int> itemNumber;
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        if(!player)
        {
            player = Instantiate(playerPrefab);
        }


        DontDestroyOnLoad(player);
        DontDestroyOnLoad(forestSpawn.gameObject);
        DontDestroyOnLoad(houseSpawn.gameObject);
        uniquePlacement = GetComponent<ObjectPlacement>();
        playerController = player.GetComponent<PlayerController>();
        playerController.OnPlayerPickUpLastItem += UnlockEntranceDoor;
        playerController.checklistGameObject = playerCheckList;

        for (int i = 0; i < maxItemsNeeded; i++)
        {
            int random = Random.Range(0, allItems.Length);

            while (itemNumber.Contains(random))
            {
                random = Random.Range(0, allItems.Length);
            }
            itemNumber.Add(random);
        }

        itemsNeeded = new GameObject[maxItemsNeeded];
        playerController.checklistItems = new PlayerController.checklistItem[maxItemsNeeded];
        uniquePlacement.prefabs = new ObjectPlacement.prefabInfo[itemsNeeded.Length];

        for (int i = 0; i < itemsNeeded.Length; i++)
        {
            GameObject newItem = Instantiate(allItems[itemNumber[i]]);
            GameObject newIngredientText = Instantiate(checklistItemUIPrefab);
            ObjectCore newItemProperties = newItem.GetComponent<ObjectCore>();

            itemsNeeded[i] = newItem;
            newIngredientText.transform.SetParent(playerController.checklistGameObject.transform,false);
            playerController.checklistItems[i].item = newIngredientText;
            playerController.checklistItems[i].id = newItemProperties.id;
            newIngredientText.name = "Ingredient #" + i + " Text";
            newIngredientText.GetComponent<Text>().text = "• " + newItemProperties.itemName;
            newItem.SetActive(true);
            newItem.SetActive(false);

            uniquePlacement.prefabs[i].id = newItemProperties.id;
        }

        RelocatePlayer();
        playerController.staffOfLighting.staffBar = staffBar;
        playerController.OnPlayerDeath += Loss;
        playerController.OnPlayerWin += Win;
        levelLoader = GetComponent<LevelLoader>();
        //generator.OnGeneratorFinish += SpawnPlayer;

    }

    /*// Update is called once per frame
    void Update()
    {
        
    }*/

    public void SpawnPlayer()
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = initialRoom.transform.position;
        player.transform.rotation = initialRoom.transform.rotation;
        player.transform.position += new Vector3(0, 1, 0);
        player.GetComponent<CharacterController>().enabled = true;
        //initialDoor = initialRoom.GetComponentInChildren<Door>();
    }

    public void RelocatePlayer()
    {
        if(spawnInForest)
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = forestSpawn.position;
            player.transform.rotation = forestSpawn.rotation;
            player.transform.position += new Vector3(0, 5, 0);
            player.GetComponent<CharacterController>().enabled = true;
            Invoke("DisableHouseDoor", 0.5f);

            spawnInForest = false;
        }
        else
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = houseSpawn.position;
            player.transform.rotation = houseSpawn.rotation;
            player.transform.position += new Vector3(0, 5, 0);
            player.GetComponent<CharacterController>().enabled = true;
            

            //spawnInForest = true;
        }
    }

    private void DisableHouseDoor()
    {
        Debug.Log("Disabling Door");
        if(houseDoor)
        {
            houseDoor.enabled = false;
            //Destroy(houseDoor);
            //houseDoor.gameObject.GetComponent<BoxCollider>().enabled = false;
            houseDoor.lockedDoor = true;
        }
        
    }

    private void UnlockEntranceDoor()
    {
        initialDoor.lockedDoor = false;
    }

    public void GetGenerator(DungeonGenerator newGenerator)
    {
        generator = newGenerator;
        generator.OnGeneratorFinish += SpawnPlayer;
    }

    public void Win()
    {
        winPanel.SetActive(true);
        lossPanel.SetActive(false);
        Invoke("ChangeScene", waitTime);
    }

    public void Loss()
    {
        MusicManager.Get().StopAllSongs();
        MusicManager.Get().lossSong.Play();
        winPanel.SetActive(false);
        lossPanel.SetActive(true);
        Invoke("ChangeScene", waitTime);
    }

    public void ChangeScene()
    {
        levelLoader.LoadMenu();
    }

    public void CleanEverything()
    {
        if (forestSpawn)
        {
            Destroy(forestSpawn.gameObject);
            Destroy(houseSpawn.gameObject);
        }

        if (UI)
        {
            Destroy(UI);
        }
        
        if(itemsNeeded.Length > 0)
        {
            for (int i = 0; i < itemsNeeded.Length; i++)
            {
                Destroy(itemsNeeded[i]);
            }
        }
        
        Destroy(player);
        Destroy(playerController);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if(houseDoor)
        {
            houseDoor.enabled = false;
        }
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        if(generator)
        {
            generator.OnGeneratorFinish -= SpawnPlayer;
        }

        if(playerController)
        {
            playerController.OnPlayerDeath -= Loss;
            playerController.OnPlayerWin -= Win;
        }
        
    }
}
