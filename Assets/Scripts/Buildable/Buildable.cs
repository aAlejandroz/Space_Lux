using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buildable : Damageable {

    public bool canBuild, canRepair;

    public abstract GameObject Build(Transform spawnPoint, Grid grid); 
}
