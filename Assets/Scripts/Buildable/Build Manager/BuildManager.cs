using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour {
   
    private int index;
    public float buildRate = 0.1f;
    private bool requestToBuild = false;
    private bool canRequest = true;
    public bool canRemove = false;
    public List<Buildable> buildList;  // array of buildables. What the player can currently build  
    private Buildable currentBuilding;
    private PlayerPickup playerResource;   
    private Buildable blockingObject;
    public Transform spawnPoint;            // Transform of gameobject in front of player
    public Grid grid;
    public GunUI gunDisplay;

    // Start function
    private void Start() {
        playerResource = GetComponent<PlayerPickup>();
        index = 0;
    }

    // Update function
    private void Update() {
        canRemove = spawnPoint.GetComponent<DetectingBuildable>().canRemove;
        currentBuilding = buildList[index];

        if (Input.GetAxis("Mouse ScrollWheel") != 0f) {                             // Player choose what they build with the scroll wheel 
            index++;     
            
            if (index >= buildList.Count) {
                index = 0;
            }
        }

        if (Input.GetKey(KeyCode.C) && canRequest) {                                          // Player builds with 'C' key
            requestToBuild = true;
        }

        if (Input.GetKey(KeyCode.V) && canRemove) {                                 
            blockingObject = spawnPoint.GetComponent<DetectingBuildable>().blockingObject.GetComponent<Buildable>();
            blockingObject.Remove();
            playerResource.IncrementResource(blockingObject.buildCost / 2);        // Destroying a building only returns half the cost                          
        }

        gunDisplay.UpdateGunDisplay(buildList[index]);
    }

    // TODO: Display prompt saying player can't build
    // FixedUpdate function
    private void FixedUpdate() {
        if (requestToBuild)                                                         // Player requested to build
        {                                                           
            if (currentBuilding.isBuildable(spawnPoint))                            // Determine if player is able to build   
            {                                          
                if (playerResource.GetResourceCount() >= currentBuilding.buildCost) // Determine if player has enough resource
                {
                    canRequest = false;
                    currentBuilding.Build(spawnPoint, grid);                    
                    playerResource.DecrementResource(currentBuilding.buildCost);
                    StartCoroutine(Wait());
                }
                else 
                {
                    Debug.Log("Not enough resource!");
                }
            }
            else 
            {
                Debug.Log("Cannot build right now");
            }

            requestToBuild = false;
        }        
    }

    public IEnumerator Wait() {
        Debug.Log("Waiting");      
        yield return new WaitForSeconds(buildRate);
        canRequest = true;
    }
}
