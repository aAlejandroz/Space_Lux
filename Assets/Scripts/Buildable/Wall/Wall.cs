using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wall : Buildable {
   
    private Vector3 mouseVec;
    private float mouseAngle;
    private float buildAngle;
    private float spawnAngle;    
    private GameObject buildUIInfo;
    private BuildingHP CurrentHpDisplay;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D objectCollider;

    // Constructor
    public Wall() {
        buildCost = 5;
        status = Status.DESTROYED;
        isbuilding = false;
        buildTime = 1;
        timeLeftBuilding = buildTime;
    }

    // Start function
    public void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildableUI = GameObject.FindGameObjectWithTag("Slider");      
        objectCollider = GetComponent<BoxCollider2D>();
        objectCollider.isTrigger = true;
    }

    // Update function
    public void Update() {        
        canRepair = CurHP < MaxHP ? true : false;                               // If damaged, then repairable

        if (isbuilding) {
            float alpha = buildUIInfo.GetComponentInChildren<Slider>().value;   // alpha = progress of slider            

            spriteRenderer.color = new Color(1, 1, 1, alpha);

            timeLeftBuilding -= Time.deltaTime;                                 // Start decreasing build time. Ex) 10 seconds for turrets
            if (timeLeftBuilding <= 0f) {
                spriteRenderer.color = new Color(1, 1, 1, alpha);
                isbuilding = false;
                status = Status.ACTIVE;
                objectCollider.isTrigger = false;                
            }
        }

        if (status == Status.ACTIVE) {
            CurrentHpDisplay.UpdateHP(CurHP);
        }
    }

    public override GameObject Build(Transform spawnPoint, Grid grid) {
        if (canBuild) {             
            Vector3Int cellPosition = grid.WorldToCell(spawnPoint.position);    // Converts World position to cell, and cell back to world      
            Vector3 wallSpawnPoint = new Vector3();           
            wallSpawnPoint = grid.GetCellCenterLocal(cellPosition);            
                        
            spawnAngle = spawnPoint.GetComponentInParent<RotateFromMouse>().transform.localEulerAngles.z;   // Spawn angle's rotation
            
            float offset = (grid.cellSize.x / 2);   // Since size of grid is 1, we want the wall to be 1/2 from the middle

            if ( (spawnAngle <= 45f && spawnAngle >= 0f) || (spawnAngle <= 359.9f && spawnAngle >= 315.0f) ) { // Right 
                buildAngle = 90;                
                wallSpawnPoint -= new Vector3(offset, 0f, 0f);             
            }            
            else if (spawnAngle <= 135.0f && spawnAngle > 45.0f) {  // Up
                buildAngle = 180;
                wallSpawnPoint -= new Vector3(0f, offset, 0f);
            }
            else if (spawnAngle >= 135.0f && spawnAngle < 225.0f) { // Left
                buildAngle = 270;
                wallSpawnPoint += new Vector3(offset, 0f, 0f);
            }            
            else if (spawnAngle >= 225.0f && spawnAngle < 315.0f) { // Down
                buildAngle = 360;
                wallSpawnPoint += new Vector3(0f, offset, 0f);
            }
            
            var wall = Instantiate(gameObject, wallSpawnPoint, Quaternion.Euler(0, 0, buildAngle));     // Spawning wall                

            // Setting up info for wall clone
            buildUIInfo = (GameObject)Instantiate(buildableUI, wallSpawnPoint - new Vector3(3.5f, 0.5f, 0f), Quaternion.Euler(Vector3.zero));
            buildUIInfo.GetComponentInChildren<BuildTimer>().SetBuildTime(this.buildTime);

            wall.GetComponent<Wall>().isbuilding = true;
            wall.GetComponent<Wall>().buildUIInfo = this.buildUIInfo;

            CurrentHpDisplay = buildUIInfo.GetComponentInChildren<BuildingHP>();
            wall.GetComponent<Wall>().CurrentHpDisplay = this.CurrentHpDisplay;

            return wall;
        }
        else {
            return null;
        }
    }

    // Function to determine what to do when destroyed
    // Overrides abstract function in Damageable
    protected override void OnDestroyed() {
        CurrentHpDisplay.UpdateHP(CurHP);
        Destroy(gameObject);
    }
}
