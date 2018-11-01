using UnityEngine;

public class Bullet : Projectile {

	public float Damage;
    public float DisappearAfter;

    private void Awake() {
        Destroy(gameObject, DisappearAfter);
    }

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
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag != "Bullet") {
            Destroy(gameObject);
        }
    }
}
