using System.Collections;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(Collider2D))]
public class SwarmEnemyAI : MonoBehaviour {

	public float MovementSpeed;
	public float AttackSpeed;
	public float AttackDamage;
	public GameObject target;
	private Vector2 targetVec;
	private bool isTargetInRange;
	private Rigidbody2D rb2d;
    public GameObject drop;

	private void Awake() {
		target = GameObject.Find("Player");
		rb2d = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate() {
		targetVec = target.transform.position - transform.position;
		targetVec.x = Mathf.Clamp(targetVec.x, -1.0f, 1.0f);
		targetVec.y = Mathf.Clamp(targetVec.y, -1.0f, 1.0f);
		rb2d.velocity = targetVec * MovementSpeed;
	}       

    private void OnCollisionStay2D(Collision2D coll) {
        if (coll.gameObject.GetComponent<Damageable>() && !(coll.gameObject.tag.Equals("Enemy"))) {   // Checks if collider is damagable & not an enemy          
            isTargetInRange = true;
            Damageable damageable = coll.gameObject.GetComponent<Damageable>();
            StartCoroutine(attackUntilOutOfRange(damageable));
        }
    }

    private void OnCollisionExit2D(Collision2D coll) {
        if (coll.gameObject.GetComponent<Damageable>()) {
            isTargetInRange = false;
        }
    }
  
	private IEnumerator attackUntilOutOfRange(Damageable damageable) {
		if (!isTargetInRange) {
			yield break;
		} else {
			//var damageable = target.GetComponent<Damageable>();
			if (damageable != null) {
                Debug.Log("Attacked...");
				damageable.Damage(AttackDamage);
			}
			yield return new WaitForSeconds(AttackSpeed);
			StartCoroutine(attackUntilOutOfRange(damageable));
		}
	}

    public void OnDestroy() {
        Instantiate(drop, transform.position, drop.transform.rotation);
    }

}
