using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buildable : Damageable {

    // Change to private with getters
    public int buildCost;       
    public float buildTime;
    protected float timeLeftBuilding;    
    public bool canBuild, canRepair, isbuilding;    
    public GameObject buildableUI;

    public abstract GameObject Build(Transform spawnPoint, Grid grid);

    // Function to remove buildable object
    public void Remove() {  
        CurHP = 0f;
        OnDestroyed();
    }

    public void Repair() {
        Debug.Log("Repairing");
        CurHP = MaxHP;
    }

    // Function to determine if player can build on grid
    public bool isBuildable(Transform spawnPoint) {
        canBuild = spawnPoint.GetComponent<DetectingBuildable>().canBuild;        
        return canBuild;
    }

    // Function to determine what to do when damage is taken
    // Overrides abstract function in Damageable
    protected override void OnDamaged(float damage) {
        if (!isInvincible) {
            CurHP -= damage;            
            GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine(WaitAndChangeColor());            
            StartCoroutine(setInvincibleAndWait());
        }       
    }    

    // Function representing dmg feedback. Sprite blinks red
    public override IEnumerator WaitAndChangeColor() {
        yield return new WaitForSeconds(flickerDuration);
        GetComponent<SpriteRenderer>().color = new Color(0.7607844f, 0.7607844f, 0.7607844f, 1);      
    }

}
