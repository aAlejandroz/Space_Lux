using UnityEngine;

public class Bullet : Projectile {

	public float Damage;
    public float DisappearAfter;

    private void Awake() {
        Destroy(gameObject, DisappearAfter);
    }

    // TODO: Bullet is hitting boxcollider of turret
    // Fix
    private void OnCollisionEnter2D(Collision2D coll) {
		Damageable damageable;
		if (coll.gameObject.tag == "Enemy"
			&& (damageable = coll.gameObject.GetComponent<Damageable>()) != null)
		{
			damageable.Damage(Damage);
		}        
        if (coll.gameObject.tag != "Bullet") {
            Destroy(gameObject);
        }
        
        /*
        if (coll.gameObject.tag.Equals("Buildable")) {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            Debug.Log("isTRIGGER: " + gameObject.GetComponent<BoxCollider2D>().isTrigger);
            Debug.Log("Bullet hits buildable...");
        }
        */
    }

    private void OnCollisionStay2D(Collision2D coll) {
        if (coll.gameObject.tag != "Bullet") {
            Destroy(gameObject);
        }        
    }

    /*
    private void OnTriggerStay2D(Collider2D coll) {
        if (coll.gameObject.tag.Equals("Buildable")) {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            Debug.Log("isTRIGGER: " + gameObject.GetComponent<BoxCollider2D>().isTrigger);
            Debug.Log("Bullet inside buildable box collider...");
        }
    }
    */

    /*
    // Idea: This is called after bullet passes through wall
    private void OnTriggerExit2D(Collider2D coll) {
        if (coll.gameObject.tag.Equals("Buildable")) {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
            Debug.Log("isTRIGGER: " + gameObject.GetComponent<BoxCollider2D>().isTrigger);
            Debug.Log("Bullet exits buildable box collider...");
        }
    }
    */
}
