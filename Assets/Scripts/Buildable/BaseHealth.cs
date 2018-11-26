using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseHealth : Damageable {

    //public GameObject buildableUIPrefab;
    //public GameObject buildUIInfo;

    public Slider baseHealth;

    public void Start() {        
        baseHealth.value = MaxHP;
    }

    public void Update() {

    }

    public void Repair() {
        Debug.Log("Repairing");
        CurHP += 40;
    }

    protected override void OnDamaged(float damage) {
        if (!isInvincible) {            
            CurHP -= damage;
            baseHealth.value = (CurHP/ MaxHP);
            StartCoroutine(setInvincibleAndWait());
        }
    }

    protected override void OnDestroyed() {       
        Debug.Log("GAME OVER");        
        Destroy(gameObject);
        
    }
    
    public override IEnumerator WaitAndChangeColor() {
        yield break;

    }
    
}