using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectingBuildable : MonoBehaviour {

    // Change to private with getters
    public bool canBuild = true;
    public bool isBlocked = false;   
    public bool canRemove = false;
    public bool inBuildingMode = false;    
    public GameObject hologramPrefab;
    public GameObject hologram;
    public GameObject player;
    public Transform spawnPoint;
    public Grid grid;
    public Collider2D blockingObject;
    public Vector3 currentSpawnPoint = Vector3.zero;
    public Vector3 hologramSpawnPoint = Vector3.zero;

    public void Awake() {                
        player = GameObject.FindGameObjectWithTag("Player");
        spawnPoint = GetComponent<Transform>();
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
    }

    // Update Function
    public void Update() {

        updateSpawnPoint();
        
        // Checks mode of player
        if (player.GetComponent<PlayerController>().mode == PlayerController.Mode.BUILDING_MODE) {
            inBuildingMode = true;
        } else {
            inBuildingMode = false;
        }

        // If we can build, display hologram
        if (canBuild && inBuildingMode) {
            if (currentSpawnPoint != hologramSpawnPoint) {
                Destroy(hologram);
                ShowHologram();                
            }
        } else {
            Destroy(hologram);
        }

        if (!isBlocked) {
            canBuild = true;
            canRemove = false;
        }        

    }

    // Updates spawn point of "Hologram"
    private void updateSpawnPoint() {
        Vector3Int cellPosition = grid.WorldToCell(spawnPoint.position);        
        hologramSpawnPoint = grid.GetCellCenterLocal(cellPosition);
    }

    // Show hologram
    private void ShowHologram() {
        updateSpawnPoint();
        currentSpawnPoint = hologramSpawnPoint;
        hologram = Instantiate(hologramPrefab, hologramSpawnPoint, Quaternion.identity);
    }

    // Function to detect when object is not buildable
    private void OnTriggerStay2D(Collider2D collision) {        
        if (collision.GetType() == typeof(BoxCollider2D) || collision.tag == "Environment") {     // Checks if collider is a box collider. Need b/c turret has circle collider
            canBuild = false;
            isBlocked = true;
        }

        if (collision.tag.Equals("Buildable") && collision.GetType() != typeof(CircleCollider2D)) {                  
            if (isBlocked && collision.GetComponent<Buildable>().status == Damageable.Status.ACTIVE) {  // Checks if object in front of player is removable
                canRemove = true;                
            }

            blockingObject = collision;      // Reference to blocking object. Turret / wall, etc.             
        }
    }

    // Function to detect when object is buildable
    private void OnTriggerExit2D(Collider2D collision) {        
        if (collision.GetType() == typeof(BoxCollider2D) || collision.GetType() == typeof(EdgeCollider2D)) {            
            canBuild = true;
            isBlocked = false;
            blockingObject = null;
        } 
                
    }
}
