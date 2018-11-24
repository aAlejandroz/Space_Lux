using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour {
   
    private int index;
    public float buildRate = 0.1f;
    private bool requestToBuild = false;
    [SerializeField] private bool canRequest = true;
    public bool canRemove = false;
    public List<Buildable> buildList;  // array of buildables. What the player can currently build  
    private Buildable currentBuilding;
    private PlayerPickup playerResource;   
    [SerializeField] private Buildable blockingObject;
    public Transform spawnPoint;            // Transform of gameobject in front of player
    public Grid grid;
    public GunUI gunDisplay;

    // Start function
    private void Awake() {
        playerResource = GetComponent<PlayerPickup>();
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        index = 0;        
    }

    // Update function
    private void Update() {
        // place turret sprite in build spawn sprite renderer
        
        canRemove = spawnPoint.GetComponent<DetectingBuildable>().canRemove;
        currentBuilding = buildList[index];

        if (Input.GetButton("Fire1")) {
            gameObject.GetComponent<BuildManager>().enabled = false;
            gameObject.GetComponent<PlayerController>().mode = PlayerController.Mode.SHOOTING_MODE;
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0f) {                             // Player choose what they build with the scroll wheel 
            index++;     
            
            if (index >= buildList.Count) {
                index = 0;
            }
        }

        if (Input.GetKey(KeyCode.C) && canRemove) {
            RepairBuildable();
        } else if (Input.GetKey(KeyCode.C) && canRequest) {                                          // Player builds with 'C' key
            requestToBuild = true;
        } else {

        }        
        
        if (Input.GetKey(KeyCode.V) && canRemove) {                                 
            blockingObject = spawnPoint.GetComponent<DetectingBuildable>().blockingObject.GetComponent<Buildable>();
            blockingObject.Remove();
            playerResource.IncrementResource(blockingObject.buildCost / 2);        // Destroying a building only returns half the cost                          
        }        

        gunDisplay.UpdateGunDisplay(buildList[index].GetComponent<SpriteRenderer>().sprite);
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

    public void RepairBuildable() {
        
        // cost to repairing turrets is decided by how damaged turret is
        // curHP >= 75% { 25% cost }
        // curHP < 75% && curHP >= 50% { 50% cost }
        // curHP <= 50%  { 75% cost }

        blockingObject = spawnPoint.GetComponent<DetectingBuildable>().blockingObject.GetComponent<Buildable>();

        if (blockingObject.canRepair) {
            int repairCost;
            float curHpPercent = blockingObject.CurHP / blockingObject.MaxHP;

            if (curHpPercent >= 0.75) {
                repairCost = (int)(blockingObject.buildCost * 0.25);
            } else if (curHpPercent < 0.75 && curHpPercent >= 0.50) {
                repairCost = (int)(blockingObject.buildCost * 0.50);
            } else {
                repairCost = (int)(blockingObject.buildCost * 0.75);
            }

            if (playerResource.GetResourceCount() >= repairCost) {
                playerResource.DecrementResource(repairCost);
                blockingObject.Repair();
            }
            else {
                Debug.Log("You do not have enough resource to repair!");
            }
        }
        
    }

    public IEnumerator Wait() {
        Debug.Log("Waiting");      
        yield return new WaitForSeconds(buildRate);
        canRequest = true;
    }
}
