using UnityEngine;

public class Bullet : Projectile {

    public float Damage;    
    public float DisappearAfter;
    public float distance;
    public LayerMask whatIsSolid;

    private void Start() {

        // Layers:
        // 0 - Default
        // 10 - Buildables
        // 11 - Player Bullets, put enemy bullets on enemy layer
        Physics2D.IgnoreLayerCollision(0, 11); // Ignore layer collision so friendly bullets won't hit player        
        Physics2D.IgnoreLayerCollision(11, 10); // Ignore layer collision so bullets won't hit tower  
        Physics2D.IgnoreLayerCollision(11, 11);

        Destroy(gameObject, DisappearAfter);
    }

    private void Update() {
        bool willDestroyBullet = true;
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, distance, whatIsSolid);   // Raycast Info, only hits things in layermask "WhatIsSolid." (Enemies & Environment)
        Debug.DrawRay(transform.position, transform.right, Color.green, 1f);

        if (hitInfo.collider != null && hitInfo.collider.isTrigger == false) { // Collided with something                        

            if (hitInfo.collider.tag == "Enemy") {
                Debug.Log("Enemy hit!");                                    // She's in love with who I am
                hitInfo.collider.GetComponent<Damageable>().Damage(Damage); // Back in highschool, I used to bus it to the dance
            }
            else if (hitInfo.collider.tag == "Buildable" || hitInfo.collider.tag == "Player") {
                Debug.Log("Collision ignored");
                willDestroyBullet = false;
            }
            else {
                Debug.Log("Environment hit!");
            }

            if (willDestroyBullet)
                Destroy(gameObject);
        }        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag.Equals("Enemy")) {
            collision.gameObject.GetComponent<Damageable>().Damage(Damage);
        }

        if (collision.gameObject.tag != "Bullet") {
            Destroy(gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.tag != "Bullet") {            
            Destroy(gameObject);
        }
    }

}
    /*
    // If friendly bullet, ignore collision. Else, turret will take dmg from enemy fire
    private void OnCollisionEnter2D(Collision2D collision) {
        /*
        if (isFriendlyBullet && collision.gameObject.tag == "Buildable" && collision.GetType().Equals(typeof(BoxCollider2D))) {
            Debug.Log("Friendly turret, no damage taken");
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider2D>(), GetComponent<Collider2D>());
        }
        else {
            Debug.Log("Enemy bullet, damage taken");
            collision.gameObject.GetComponent<Damageable>().Damage(this.Damage);
        }
        
        if (isFriendlyBullet && collision.gameObject.tag == "Buildable") {
            Debug.Log("Turret hit");
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider2D>(), GetComponent<Collider2D>());
            //Debug.Log("Collision ignored");
        }
    }
    

    private void OnCollisionStay2D(Collision2D collision) {
        if (isFriendlyBullet && collision.gameObject.tag == "Buildable") {            
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider2D>(), GetComponent<Collider2D>());
            Debug.Log("Collision ignored");
        }
    }
    */
        // TODO: Bullet is hitting boxcollider of turret
        /*
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

        private void OnCollisionStay2D(Collision2D coll) {
            if (coll.gameObject.tag != "Bullet") {
                Destroy(gameObject);
            }        
        }
        */
