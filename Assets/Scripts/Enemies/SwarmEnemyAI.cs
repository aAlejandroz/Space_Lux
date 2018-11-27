using System.Collections;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(Collider2D))]
public class SwarmEnemyAI : MonoBehaviour {

	public float MovementSpeed;
	public float AttackSpeed;
	public float AttackDamage;
    public float vertical, horizontal;
    private bool isTargetInRange;
    [SerializeField]
    private bool facingLeft;
    private Animator anim;
    public GameObject target;
    [SerializeField]
    private Vector2 targetVec;	
	private Rigidbody2D rb2d;

	private void Awake() {
        facingLeft = true;
        anim = GetComponent<Animator>();
		target = GameObject.Find("Base");
		rb2d = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate() {
        
        if (target == null) {
            Debug.Log("Gameover");
            GetComponent<SwarmEnemyAI>().enabled = false;
        }

        if (target != null) {
            targetVec = target.transform.position - transform.position;
            targetVec.x = Mathf.Clamp(targetVec.x, -1.0f, 1.0f);
            targetVec.y = Mathf.Clamp(targetVec.y, -1.0f, 1.0f);
            rb2d.velocity = targetVec * MovementSpeed;

            horizontal = targetVec.x;
            vertical = targetVec.y;

            Flip(horizontal);

            SetAnimations(horizontal, vertical);
        }
    }       

    private void OnCollisionStay2D(Collision2D coll) {
        if (coll.gameObject.GetComponent<Damageable>() && coll.gameObject.tag.Equals("Player")) {   // Checks if collider is damagable & not an enemy                      
            Damageable player = coll.gameObject.GetComponent<Damageable>();
            if (player != null) {
                player.Damage(AttackDamage);
            }          
        } else if (coll.gameObject.GetComponent<Damageable>() && coll.gameObject.tag.Equals("Base")) {
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
			if (damageable != null) {
				damageable.Damage(AttackDamage);
			}
			yield return new WaitForSeconds(AttackSpeed);
			StartCoroutine(attackUntilOutOfRange(damageable));
		}
	}

    private void Flip(float horizontal) {
        if (horizontal < 0 && !facingLeft || horizontal > 0 && facingLeft) {
            facingLeft = !facingLeft;

            Vector3 theScale = transform.localScale;

            theScale.x *= -1;

            transform.localScale = theScale;
        }
    }

    private void setAnimInput(float x, float y) {
        anim.SetFloat("xInput", x);
        anim.SetFloat("yInput", y);
    }

    protected void SetAnimations(float horizontal, float vertical) {
        if (vertical >= 0.5f) {             // Up
            setAnimInput(0, 1);
        } else if (horizontal >= 0.5f) {    // Right
            setAnimInput(1, 0);
        } else if (vertical <= -0.5f) {      // Down
            setAnimInput(0, -1);
        } else if (horizontal <= -0.5f) {    // Left
            setAnimInput(-1, 0);
        }
    }
}
