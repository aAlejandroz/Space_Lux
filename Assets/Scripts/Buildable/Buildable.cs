using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buildable : Damageable {

    public float flickerDuration = 0.5f;
    public int buildCost;
    public float buildTime;
    protected float timeLeftBuilding;    
    public bool canBuild, canRepair, isbuilding;    
    public GameObject buildableUI;

    public abstract GameObject Build(Transform spawnPoint, Grid grid);

    public bool isBuildable(Transform spawnPoint) {         
        canBuild = spawnPoint.GetComponent<DetectingBuildable>().canBuild;
        return canBuild;
    }

    protected override void OnDamaged(float damage) {
        if (!isInvincible) {
            CurHP -= damage;            
            GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine(WaitAndChangeColor());            
            StartCoroutine(setInvincibleAndWait());
        }       
    }

    public void SwapSpriteRenderer() {
        // If enabled, disabled. If Disabled, enabled       
        GetComponent<SpriteRenderer>().enabled = GetComponent<SpriteRenderer>().enabled ? false : true;        
    }

    public IEnumerator WaitAndChangeColor() {
        yield return new WaitForSeconds(flickerDuration);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

}
