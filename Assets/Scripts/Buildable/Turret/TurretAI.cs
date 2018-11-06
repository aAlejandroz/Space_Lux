using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class TurretAI : Buildable {

    private float targetAngle;
    private bool isSearchingAndDestroying;
    private Gun TurretGun;
    private List<GameObject> enemies; 
    private Vector2 targetVec;
    private SpriteRenderer spriteRenderer;
    private BuildingHP CurrentHpDisplay;
    private GameObject buildUIInfo;    
    private AudioSource destroyedSound;   
    
    // Constructor
    public TurretAI() {
        buildCost = 10;
        enemies = new List<GameObject>();      
		isSearchingAndDestroying = false;
        status = Status.DESTROYED;
        isbuilding = false;        
        buildTime = 5;
        timeLeftBuilding = buildTime;
    }

    // Start function
    public void Start() {
        TurretGun = GetComponentInChildren<Gun>();
        destroyedSound = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildableUI = GameObject.FindGameObjectWithTag("Slider");
    }

    // Update function
    private void Update() {        
        canRepair = CurHP < MaxHP ? true : false;                               // If damaged, then turret is repairable

        isSearchingAndDestroying = enemies.Any<GameObject>() ? true : false;    // If there's any enemies in the list, isSearchingAndDestroying is true

        if (isSearchingAndDestroying && status == Status.ACTIVE) {
            StartCoroutine(SearchAndDestroy());
        }                

        if (isbuilding) {                                 
            float alpha = buildUIInfo.GetComponentInChildren<Slider>().value;   // alpha = progress of slider            

            spriteRenderer.color = new Color(1, 1, 1, alpha);

            timeLeftBuilding -= Time.deltaTime;                                 // Start decreasing build time. Ex) 5 seconds for turrets
            if (timeLeftBuilding <= 0f) {
                spriteRenderer.color = new Color(1, 1, 1, alpha);
                isbuilding = false;
                status = Status.ACTIVE;
            }
        }

        if (status == Status.ACTIVE) {
            CurrentHpDisplay.UpdateHP(CurHP);
        }
    }

    // Function to track enemies when they enter the collider
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Enemy") {
			trackEnemy(collision.gameObject);       
        }
    }

    // Function to track enemies when they spawn in collider
    private void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            trackEnemy(collision.gameObject);
        }
    }

    // Function to untrack enemy when they leave collider
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Enemy") {            
            untrackEnemy(collision.gameObject);           
        }
    }

    // Function to track enemies
    private void trackEnemy(GameObject enemy) {
        if (!enemies.Contains(enemy)) {             // If enemy not in List, add them
            enemies.Add(enemy);
            if (!isSearchingAndDestroying) {        // If we weren't searching & destroying, you bet your butts we are now
                isSearchingAndDestroying = true;
            }
        }		
	}

    // Function to untrack enemies
	private void untrackEnemy(GameObject enemy) {
        enemies.Remove(enemy);       
        isSearchingAndDestroying = false;
    }

    // Function to Search and Destroy 
    private IEnumerator SearchAndDestroy() {
        var targetEnemy = enemies.FirstOrDefault(); // Takes aim at first enemy in the List
        if (targetEnemy != null) {                  // Aim, shoot, wait for gun cooldown, and try again
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
        if (canBuild) { 
            Vector3Int cellPosition = grid.WorldToCell(spawnPoint.position);
            Vector3 turretSpawnPoint = new Vector3();
            turretSpawnPoint = grid.GetCellCenterLocal(cellPosition) + new Vector3(0f, 1f, 0f); // Add offset
            
            var turret = Instantiate(gameObject, turretSpawnPoint, Quaternion.identity);

            buildUIInfo = (GameObject)Instantiate(buildableUI, turretSpawnPoint - new Vector3(3.5f, 0.5f, 0f), Quaternion.Euler(Vector3.zero));
            buildUIInfo.GetComponentInChildren<BuildTimer>().SetBuildTime(this.buildTime);

            turret.GetComponent<TurretAI>().isbuilding = true;
            turret.GetComponent<TurretAI>().buildUIInfo = this.buildUIInfo;
           
            CurrentHpDisplay = buildUIInfo.GetComponentInChildren<BuildingHP>();
            turret.GetComponent<TurretAI>().CurrentHpDisplay = this.CurrentHpDisplay;

            return turret;
        } else {
            return null;
        }
    }           

    protected override void OnDestroyed() {
        destroyedSound.Play();
        CurrentHpDisplay.UpdateHP(CurHP);
        Destroy(gameObject);
    }
}
