using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buildable : Damageable {
   
    public float buildTime;
    protected float timeLeftBuilding;
    protected bool canBuild, canRepair, isbuilding;

    public abstract GameObject Build(Transform spawnPoint, Grid grid); 
}
