using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public delegate void OnPlayerAction();
    public OnPlayerAction OnPlayerPickUpItem;
    public OnPlayerAction OnPlayerPlaceItem;
    public OnPlayerAction OnPlayerPickUpLastItem;
    public OnPlayerAction OnPlayerFoundKeyItem;

    [System.Serializable]
    public struct objectProperties
    {
        public int id;
        public GameObject icon;
    }

    [System.Serializable]
    public struct checklistItem
    {
        public int id;
        public GameObject item;
    }

    [Header("General Settings")]
    public int maxHealth;

    [Header("Inventory Settings")]
    public int idOfWin;
    public int maxInventorySlots;
    public int idOfKeyItem;

    [Header("Assign Inputs")]
    public KeyCode checklistKey;

    [Header("Objects")]
    public AudioSource[] pickupSounds;
    public List<objectProperties> objectsGrabbed;

    [Header("Checklist Settings")]
    public AudioSource openChecklistSound;
    public AudioSource scracthChecklistSound;
    public GameObject checklistGameObject;
    public checklistItem[] checklistItems;

    [Header("Assign Components")]
    public MageStaff staffOfLighting;
    public GameObject playerCamera;

    [Header("Raycast Settings")]
    public LayerMask rayCastLayer;
    public float rayDistance;

    [Header("Check Variables")]
    public float health;

    private List<objectProperties> objectsToRemove = new List<objectProperties>();

    // Start is called before the first frame update
    void Start()
    {
        checklistGameObject.SetActive(false);
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(checklistKey))
        {
            checklistGameObject.SetActive(!checklistGameObject.activeSelf);

            if(checklistGameObject.activeSelf)
            {
                // Play sound
            }
            else
            {
                // Play another sound?
            }

        }

        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, rayDistance, rayCastLayer))
        {
            Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * hit.distance, Color.yellow);

            string layerHitted = LayerMask.LayerToName(hit.transform.gameObject.layer);

            switch (layerHitted)
            {
                case "pickup":
                    //Debug.Log("do et");
                    Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * hit.distance, Color.red);
                    if (Input.GetMouseButtonDown(0))
                    {
                        PickUpItem(hit.transform.gameObject);
                    }
                    break;
                case "place":
                    if (Input.GetMouseButtonDown(0))
                    {
                        PlaceItem(hit.transform.gameObject);
                    }
                    break;
                case "door":
                    Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * hit.distance, Color.blue);
                    if (Input.GetMouseButtonDown(0))
                    {
                        Interact(hit.transform.gameObject);
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * rayDistance, Color.white);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "laser")
        {

        }
    }

    public void ReceiveDamage(float amount)
    {
        health -= amount;
        if(health <= 0)
        {
            health = 0;
            //Game Over.
        }
    }

    private void PickUpItem(GameObject item)
    {
        if (objectsGrabbed.Count < maxInventorySlots)
        {
            if(item.GetComponent<ObjectCore>().canBePickedUp)
            {
                Debug.Log("nani1");
                int randomNumber = Random.Range(0, pickupSounds.Length);

                if (pickupSounds.Length > 0)
                {
                    pickupSounds[randomNumber].Play();
                }


                ObjectCore newObject = item.transform.gameObject.GetComponent<ObjectCore>();
                objectProperties newProperties = new objectProperties();
                newProperties.id = newObject.id;
                GameObject newIcon = Instantiate(newObject.icon);
                newProperties.icon = newIcon;

                objectsGrabbed.Add(newProperties);
                Destroy(item);

                if (OnPlayerPickUpItem != null)
                {
                    OnPlayerPickUpItem();
                }

                if (idOfKeyItem == newProperties.id)
                {
                    if (OnPlayerFoundKeyItem != null)
                    {
                        OnPlayerFoundKeyItem();
                    }
                }

                if (idOfWin == newProperties.id)
                {
                    /*if (OnPlayerGetRobot != null)
                    {
                        OnPlayerGetRobot();
                    }*/
                }

                bool isItemOnChecklist = false;

                for (int i = 0; i < checklistItems.Length; i++)
                {
                    if (checklistItems[i].id == newProperties.id)
                    {
                        GameObject checklistItem = checklistItems[i].item.transform.GetChild(0).gameObject;
                        checklistItem.SetActive(true);
                        Debug.Log("This item has been scratched");
                        isItemOnChecklist = true;
                    }
                }

                if(newObject.isLastItem)
                {
                    // Player can return to home.
                    Debug.Log("Player Can Go Home");

                    if(OnPlayerPickUpLastItem != null)
                    {
                        OnPlayerPickUpLastItem();
                    }
                }

                newObject.PickupObject();

                if (isItemOnChecklist)
                {
                    scracthChecklistSound.Play();
                }
            }
        }
    }

    private void PlaceItem(GameObject place)
    {
        ObjectPlacement newPlace = place.transform.gameObject.GetComponent<ObjectPlacement>();
        bool itemHasBeenPlaced = false;

        foreach (objectProperties item in objectsGrabbed)
        {
            if (newPlace.PlaceObject(item.id))
            {
                Destroy(item.icon);
                objectsToRemove.Add(item);
                if (OnPlayerPlaceItem != null)
                {
                    OnPlayerPlaceItem();
                }

                itemHasBeenPlaced = true;
            }
        }

        if (itemHasBeenPlaced)
        {
            foreach (objectProperties item in objectsToRemove)
            {
                objectsGrabbed.Remove(item);
            }

            objectsToRemove.Clear();
        }

    }

    private void Interact(GameObject item)
    {
        bool itemHasBeenUsed = false;
        bool couldOpenDoor = false;

        Door newDoor = item.GetComponentInParent<Door>();

        if (newDoor.lockedDoor)
        {
            foreach (objectProperties currentItem in objectsGrabbed)
            {
                if (newDoor.Interact(currentItem.id))
                {
                    Destroy(currentItem.icon);
                    objectsToRemove.Add(currentItem);
                    if (OnPlayerPlaceItem != null)
                    {
                        OnPlayerPlaceItem();
                    }

                    itemHasBeenUsed = true;
                    couldOpenDoor = true;
                }
                else
                {
                    // Couldnt open door.
                }
            }

            if (!couldOpenDoor)
            {
                Debug.Log("DIDNT FOUND KEY");
                newDoor.dialogueEvent.CheckMessage();
                if (newDoor.doorSounds.Length > 0)
                {
                    newDoor.doorSounds[(int)Door.doorStates.locked].Play();
                }
                
            }



            if (itemHasBeenUsed)
            {
                foreach (objectProperties currentItem in objectsToRemove)
                {
                    objectsGrabbed.Remove(currentItem);
                }

                objectsToRemove.Clear();
            }
        }
        else
        {
            newDoor.Interact(-1);
        }
    }
}
