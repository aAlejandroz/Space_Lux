using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Buildable {
    
    private Vector3 mouseVec;
    private float mouseAngle;
    private float buildAngle;
    private float spawnAngle;   

    public void Update() {
        // TODO: Make enemies damage walls
        canRepair = CurHP < MaxHP ? true : false;
    }

    public override GameObject Build(Transform spawnPoint, Grid grid) {
        // TODO: Detect if there is any colliders in front of wall
        // canBuild currently harcoded to TRUE
        if (canBuild) { 
            // Converts World position to cell, and cell back to world
            Vector3Int cellPosition = grid.WorldToCell(spawnPoint.position);
            Vector3 wallSpawnPoint = new Vector3();           
            wallSpawnPoint = grid.GetCellCenterLocal(cellPosition);            
                        
            spawnAngle = spawnPoint.GetComponentInParent<RotateFromMouse>().transform.localEulerAngles.z;   // Spawn angle's rotation

            // Essentially 1 / 2  
            float offset = (grid.cellSize.x / 2);  // Since size of grid is 1, we want the wall to be 1/2 from the middle

            if ( (spawnAngle <= 45f && spawnAngle >= 0f) || (spawnAngle <= 359.9f && spawnAngle >= 315.0f) ) { // Right 
                buildAngle = 90;                
                wallSpawnPoint -= new Vector3(offset, 0f, 0f);             
            }            
            else if (spawnAngle <= 135.0f && spawnAngle > 45.0f) { // Up
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
            
            var turret = Instantiate(gameObject, wallSpawnPoint, Quaternion.Euler(0, 0, buildAngle));

            return turret;
        }
        else {
            return null;
        }
    }

    protected override void OnDamaged(float damage) {
        CurHP -= damage;
    }

    protected override void OnDestroyed() {
        Destroy(gameObject);
    }
}
