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

	private void OnTriggerEnter2D(Collider2D coll) {  
        if (coll.gameObject == target) {
            Debug.Log("Collided with Target");
			isTargetInRange = true;
			StartCoroutine(attackUntilOutOfRange());
		}
	}   

	private void OnTriggerExit2D(Collider2D coll) {
		if (coll.gameObject == target) {
			isTargetInRange = false;
		}
	}

	private IEnumerator attackUntilOutOfRange() {
		if (!isTargetInRange) {
			yield break;
		} else {
			var damageable = target.GetComponent<Damageable>();
			if (damageable != null) {
				damageable.Damage(AttackDamage);
			}
			yield return new WaitForSeconds(AttackSpeed);
			StartCoroutine(attackUntilOutOfRange());
		}
	}
}
