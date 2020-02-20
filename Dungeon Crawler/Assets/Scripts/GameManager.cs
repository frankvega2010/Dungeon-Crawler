using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [Header("General Settings")]
    public GameObject player;
    public GameObject item;
    public DungeonGenerator generator;

    [Header("Item Settings")]
    public int maxItemsNeeded;
    public GameObject[] allItems;
    public GameObject[] itemsNeeded;

    public List<int> itemNumber;
    // Start is called before the first frame update
    void Start()
    {
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

        for (int i = 0; i < itemsNeeded.Length; i++)
        {
            GameObject newItem = Instantiate(allItems[itemNumber[i]]);
            itemsNeeded[i] = newItem;
        }

        generator.OnGeneratorFinish += SpawnPlayer;
        
    }

    /*// Update is called once per frame
    void Update()
    {
        
    }*/

    public void SpawnPlayer()
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = generator.roomsSelected[0].transform.localPosition;
        player.transform.rotation = generator.roomsSelected[0].transform.rotation;
        player.transform.position += new Vector3(0, 5, 0);
        player.GetComponent<CharacterController>().enabled = true;
        Debug.Log("Local : " + generator.roomsSelected[0].transform.localPosition);
        Debug.Log("World : " + generator.roomsSelected[0].transform.position);
        Debug.Log("Player : " + player.transform.position);
    }

    private void OnDestroy()
    {
        generator.OnGeneratorFinish -= SpawnPlayer;
    }
}
