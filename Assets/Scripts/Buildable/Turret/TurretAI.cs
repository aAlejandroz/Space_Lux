﻿using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class TurretAI : Buildable {

    public Gun TurretGun;
    public List<GameObject> enemies; 
    private Vector2 targetVec;
    private SpriteRenderer spriteRenderer;
    private GameObject buildBar;    
    private float targetAngle;    
    private bool isSearchingAndDestroying;

    public TurretAI() {
        buildCost = 10;
        enemies = new List<GameObject>();      
		isSearchingAndDestroying = false;
        status = Status.DESTROYED;
        isbuilding = false;        
        buildTime = 5;
        timeLeftBuilding = buildTime;
    }

    public void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildingBar = GameObject.FindGameObjectWithTag("Slider");       
    }

    // TODO: Make enemies damage turret
    private void Update() {     

        canRepair = CurHP < MaxHP ? true : false;

        if (isSearchingAndDestroying && status == Status.ACTIVE) {
            StartCoroutine(SearchAndDestroy());
        }                

        if (isbuilding) {                                 
            float alpha = buildBar.GetComponentInChildren<Slider>().value;   // alpha = progress of slider            

            spriteRenderer.color = new Color(1, 1, 1, alpha);

            timeLeftBuilding -= Time.deltaTime;    // Start decreasing build time. Ex) 10 seconds for turrets
            if (timeLeftBuilding <= 0f) {
                spriteRenderer.color = new Color(1, 1, 1, alpha);
                isbuilding = false;
                status = Status.ACTIVE;
                Debug.Log(status);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Enemy") {
			trackEnemy(collision.gameObject);       
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            trackEnemy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Enemy") {            
            untrackEnemy(collision.gameObject);           
        }
    }

    private void trackEnemy(GameObject enemy) {
        if (!enemies.Contains(enemy)) {     //Only adds enemy if the enemy is not already in the List
            enemies.Add(enemy);
            if (!isSearchingAndDestroying) {
                isSearchingAndDestroying = true;
            }
        }		
	}

	private void untrackEnemy(GameObject enemy) {
        enemies.Remove(enemy);       
        isSearchingAndDestroying = false;
    }

    private IEnumerator SearchAndDestroy() {
        var targetEnemy = enemies.FirstOrDefault();
        if (targetEnemy != null) { // Aim, shoot, wait for gun cooldown, and try again
            targetVec = targetEnemy.transform.position - TurretGun.transform.position;
            targetAngle = Mathf.Atan2(targetVec.y, targetVec.x) * Mathf.Rad2Deg;
            TurretGun.transform.rotation = Quaternion.Euler(0.0f, 0.0f, targetAngle);
            TurretGun.Use();
            yield return new WaitForSeconds(TurretGun.FireRate);
            StartCoroutine(SearchAndDestroy());
        } else {
            untrackEnemy(targetEnemy);
        }

        isSearchingAndDestroying = false;
        yield break;
    }

    // Build function for turret
    public override GameObject Build(Transform spawnPoint, Grid grid) {                
        Vector3Int cellPosition = grid.WorldToCell(spawnPoint.position);            
        Vector3 turretSpawnPoint = new Vector3();
        turretSpawnPoint = grid.GetCellCenterLocal(cellPosition) + new Vector3(0f, 1f, 0f); // Add offset
        Debug.Log(turretSpawnPoint);

        Debug.Log("Spawning turret...");
        var turret = Instantiate(gameObject, turretSpawnPoint, Quaternion.identity);            
            
        buildBar = (GameObject)Instantiate(buildingBar, turretSpawnPoint - new Vector3(3.5f, 0.5f, 0f), Quaternion.Euler(Vector3.zero));
        buildBar.GetComponentInChildren<BuildTimer>().SetBuildTime(this.buildTime);

        turret.GetComponent<TurretAI>().isbuilding = true;
        turret.GetComponent<TurretAI>().buildBar = this.buildBar;

        return turret;
    }       

    protected override void OnDamaged(float damage) {
        CurHP -= damage;
    }

    protected override void OnDestroyed() {
        Destroy(gameObject);
    }
}
