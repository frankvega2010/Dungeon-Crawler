using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    public delegate void OnPlacementAction();
    public OnPlacementAction OnPlacementFinalPieceAdded;

    [System.Serializable]
    public struct prefabInfo
    {
        public int id;
        public GameObject prefab;
        public Transform objectLocation;
    }

    public bool AreMultipleObjects;
    [Tooltip("Tick this if this craft will make the game end.")]
    public bool finalCraft;
    [Header("Multiple Object Settings")]
    public prefabInfo[] prefabs;
    public GameObject combinedObjectToShow;
    public Transform combinedObjectLocation;

    [Header("Single Object Settings")]
    public int idOfObject;
    public Transform objectLocation;
    public GameObject prefabToPlace;

    private BoxCollider placeCollider;
    private int amountPlaced;
    private GameObject[] spawnedPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        placeCollider = GetComponent<BoxCollider>();
        if (AreMultipleObjects)
        {
            spawnedPrefabs = new GameObject[prefabs.Length];
        }
            
    }

    public bool PlaceObject(int id)
    {
        if(AreMultipleObjects)
        {
            for (int i = 0; i < prefabs.Length; i++)
            {
                if (id == prefabs[i].id)
                {
                    GameObject newObject = Instantiate(prefabs[i].prefab);
                    spawnedPrefabs[i] = newObject;

                    newObject.transform.position = prefabs[i].objectLocation.position;
                    newObject.transform.rotation = prefabs[i].objectLocation.rotation;
                    amountPlaced++;

                    if(amountPlaced >= prefabs.Length)
                    {
                        for (int j = 0; j < spawnedPrefabs.Length; j++)
                        {
                            Destroy(spawnedPrefabs[j]);
                        }

                        GameObject combinedObject = Instantiate(combinedObjectToShow);

                        combinedObject.transform.position = combinedObjectLocation.position;
                        combinedObject.transform.rotation = combinedObjectLocation.rotation;

                        placeCollider.enabled = false;

                        if(finalCraft)
                        {
                            if(OnPlacementFinalPieceAdded != null)
                            {
                                OnPlacementFinalPieceAdded();
                            }
                        }

                        amountPlaced = 0;
                    }

                    return true;
                }
            }
            
        }
        else
        {
            if (id == idOfObject)
            {
                GameObject newObject = Instantiate(prefabToPlace);

                newObject.transform.position = objectLocation.position;
                newObject.transform.rotation = objectLocation.rotation;

                return true;
            }
        }
        

        return false;
    }
}
