using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buildable : Damageable {

    public int buildCost;
    public float buildTime;
    protected float timeLeftBuilding;    
    public bool canBuild, canRepair, isbuilding;
    public GameObject buildingBar;

    public abstract GameObject Build(Transform spawnPoint, Grid grid);

    public bool isBuildable(Transform spawnPoint) {         
        canBuild = spawnPoint.GetComponent<DetectingBuildable>().canBuild;
        return canBuild;
    }   
}
