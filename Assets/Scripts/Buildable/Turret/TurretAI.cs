using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[RequireComponent (typeof(Collider2D))]
public class TurretAI : Buildable {
        
    public Gun TurretGun;
    public List<GameObject> enemies;    
    private Vector2 targetVec;
    private float targetAngle;
	private bool isSearchingAndDestroying;

    public TurretAI() {
        enemies = new List<GameObject>();
		isSearchingAndDestroying = false;        
    }

    private void Update() {    
        // TODO: Make enemies damage turret
        canRepair = CurHP < MaxHP ? true : false;
        if (isSearchingAndDestroying) {
            StartCoroutine(SearchAndDestroy());
        }                
    }

    private void OnTriggerEnter2D(Collider2D collision) {
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
		enemies.Add(enemy);
		if (!isSearchingAndDestroying) {
			isSearchingAndDestroying = true;
            StartCoroutine(SearchAndDestroy());
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
        if (canBuild) { // Can only build if nothing is blocking. Just gonna set this to true for now
            Vector3Int cellPosition = grid.WorldToCell(spawnPoint.position);            
            Vector3 turretSpawnPoint = new Vector3();
            turretSpawnPoint = grid.GetCellCenterLocal(cellPosition) + new Vector3(0f, 1f, 0f); // Add offset

            var turret = Instantiate(gameObject, turretSpawnPoint, Quaternion.identity);

            return turret;
        }
        else {
            return null;
        }
    }   

    protected override void OnDamaged(float damage) {
        CurHP -= damage;
    }

    protected override void OnDestroyed() {
        Destroy(gameObject);
    }

    /*
private IEnumerator searchAndDestroy(GameObject enemy) {
    if (enemy != null) { // Aim, shoot, wait for gun cooldown, and try again
        targetVec = enemy.transform.position - TurretGun.transform.position;
        targetAngle = Mathf.Atan2(targetVec.y, targetVec.x) * Mathf.Rad2Deg;
        TurretGun.transform.rotation = Quaternion.Euler(0.0f, 0.0f, targetAngle);
        TurretGun.Use();
        yield return new WaitForSeconds(TurretGun.FireRate);
        StartCoroutine(searchAndDestroy(enemy));
    } else { 
        enemy = enemies.Take(1).FirstOrDefault();

        if (enemy == null) {
            isSearchingAndDestroying = false;
            yield break;
        } else {
            StartCoroutine(searchAndDestroy(enemy));
        }
    }
}
*/
}
