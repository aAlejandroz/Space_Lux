using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealth : Damageable {

    public GameObject buildableUIPrefab;
    public GameObject buildUIInfo;

    public void Start() {        
        Vector3 position = transform.position - new Vector3(3.5f, 0.5f, 0f);
        position += new Vector3(0, 6f, 0);        
        buildUIInfo = Instantiate(buildableUIPrefab, position, Quaternion.Euler(Vector3.zero));
        buildUIInfo.GetComponentInChildren<BuildTimer>().gameObject.SetActive(false);
    }

    public void Update() {
        buildUIInfo.GetComponentInChildren<BuildingHP>().UpdateHP(CurHP);        
    }

    protected override void OnDamaged(float damage) {
        if (!isInvincible) {            
            CurHP -= damage;                       
            StartCoroutine(setInvincibleAndWait());
        }
    }

    protected override void OnDestroyed() {
        Destroy(gameObject);
    }
    
    public override IEnumerator WaitAndChangeColor() {
        yield break;    
    }
}