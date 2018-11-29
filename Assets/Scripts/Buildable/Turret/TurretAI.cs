using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class TurretAI : Buildable {

    public float offset;
    private float targetAngle;
    private bool isSearchingAndDestroying;
    private Animator anim;
    private Gun TurretGun;
    [SerializeField]
    private List<GameObject> enemies; 
    private Vector2 targetVec;
    private SpriteRenderer spriteRenderer;
    private BuildingHP CurrentHpDisplay;
    private GameObject buildUIInfo;    
    private AudioSource destroyedSound;
    private AudioManager audioManager;

    // Start function
    public void Start() {
        status = Status.DESTROYED;
        timeLeftBuilding = buildTime;
        enemies = new List<GameObject>();
        TurretGun = GetComponentInChildren<Gun>();
        anim = GetComponent<Animator>();
        destroyedSound = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioManager = AudioManager.instance;
        if(audioManager == null)
        {
            
        }
    }

    // Update function
    private void Update() {
        status = (CurHP > 0f  && !isbuilding) ? Status.ACTIVE : Status.DESTROYED;

        canRepair = CurHP < MaxHP ? true : false;                               // If damaged, then turret is repairable

        isSearchingAndDestroying = enemies.Count > 0 ? true : false;    // If there's any enemies in the list, isSearchingAndDestroying is true

        if (isSearchingAndDestroying && status == Status.ACTIVE) {
            StartCoroutine(SearchAndDestroy());
        }                

        if (isbuilding) {
            CurrentHpDisplay.gameObject.SetActive(false);
            float alpha = buildUIInfo.GetComponentInChildren<Slider>().value;   // alpha = progress of slider             

            spriteRenderer.color = new Color(0.7607844f, 0.7607844f, 0.7607844f, alpha);
            timeLeftBuilding -= Time.deltaTime;                                 // Start decreasing build time. Ex) 5 seconds for turrets

            if (timeLeftBuilding <= 0f) {
                spriteRenderer.color = new Color(0.7607844f, 0.7607844f, 0.7607844f, alpha);
                isbuilding = false;
                status = Status.ACTIVE;
                CurrentHpDisplay.gameObject.SetActive(true);
            }
        }
        
        if (status == Status.ACTIVE) {
            CurrentHpDisplay.UpdateHP(CurHP);
        }       
    }

    private void FixedUpdate() {
        SetAnimations();
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

            buildUIInfo = Instantiate(buildableUI, turretSpawnPoint - new Vector3(3.5f, 0.5f, 0f), Quaternion.Euler(Vector3.zero));
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
    
    // Function to set animation inputs
    private void setAnimInput(float x, float y) {
        anim.SetFloat("xInput", x);
        anim.SetFloat("yInput", y);
    }
   
    // Function to determine current animation
    protected void SetAnimations() {
        if (targetAngle <= 33.0f && targetAngle >= -33.0f) { // Right
            setAnimInput(1, 0);
        }
        else if (targetAngle <= 66.0f && targetAngle > 33.0f) { // Up right
            setAnimInput(1, 1);
        }
        else if (targetAngle <= 123.0f && targetAngle > 66.0f) { // Up
            setAnimInput(0, 1);
        }
        else if (targetAngle <= 156.0f && targetAngle > 123.0f) { // Up left
            setAnimInput(-1, 1);
        }
        else if (targetAngle >= Mathf.PI || targetAngle < -156.0f) { // Left
            setAnimInput(-1, 0);
        }
        else if (targetAngle >= -156.0f && targetAngle < -123.0f) { // Down left
            setAnimInput(-1, -1);
        }
        else if (targetAngle >= -123.0f && targetAngle < -66.0f) { // Down
            setAnimInput(0, -1);
        }
        else if (targetAngle >= -66.0f && targetAngle < -33.0f) { // Down right
            setAnimInput(1, -1);
        }
        
    }

    // Function called when hp <= 0
    protected override void OnDestroyed() {
        audioManager.PlaySound("Boom");
        //destroyedSound.Play();
        CurrentHpDisplay.UpdateHP(CurHP);
        Destroy(buildUIInfo);
        Destroy(gameObject);
    }
}
