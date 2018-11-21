using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile {

    // Maps a string to an integer
    public Dictionary<string, int> layers = new Dictionary<string, int>();   

    public float Damage;    
    public float DisappearAfter;
    public float distance;
    public LayerMask whatIsSolidForPlayer;  // Same as turrets
    public LayerMask whatIsSolidForTurrets; 
    public LayerMask whatIsSolidForEnemy;
    public LayerMask whatIsSolid;

    private void Start() {

        // Layers:           
        layers["Default"] = 0;           // 0 - Default
        layers["Enemy"] = 8;             // 8 - Enemy
        layers["Buildables"] = 10;       // 10 - Buildables
        layers["FriendlyBullet"] = 11;   // 11 - Player bullet
        layers["EnemyBullet"] = 12;      // 12 = Enemy bullet
                           
        Physics2D.IgnoreLayerCollision(layers["Default"], layers["FriendlyBullet"]);        // Ignore layer collision so friendly bullets won't hit player        
        Physics2D.IgnoreLayerCollision(layers["FriendlyBullet"], layers["Buildables"]);     // Ignore layer collision so bullets won't hit tower  
        Physics2D.IgnoreLayerCollision(layers["FriendlyBullet"], layers["FriendlyBullet"]); // Ingore layer collision so friendly bullets won't collide
        Physics2D.IgnoreLayerCollision(layers["EnemyBullet"], layers["EnemyBullet"]);       // Ingore layer collision so enemy bullets won't collide
        Physics2D.IgnoreLayerCollision(layers["Enemy"], layers["EnemyBullet"]);             // Ingore layer collision so enemy bullet wont hit enemies
        Physics2D.IgnoreLayerCollision(layers["FriendlyBullet"], layers["EnemyBullet"]);    // Ingore layer collision so bullets won't collide

        if (isPlayerBullet) {
            whatIsSolid = whatIsSolidForPlayer;
        } else if (isTurretBullet) {
            whatIsSolid = whatIsSolidForTurrets;
        } else {
            whatIsSolid = whatIsSolidForEnemy;
        }

        Destroy(gameObject, DisappearAfter);
    }

    private void Update() {
        bool willDestroyBullet = true;

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, distance, whatIsSolid);   // Raycast Info, only hits things in layermask "WhatIsSolid." (Enemies & Environment)        
        Debug.DrawRay(transform.position, transform.right, Color.green, 1f);

        // TURRET BULLET
        if (isTurretBullet) {   
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
        // PLAYER BULLET
        else if (isPlayerBullet) {  
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
        // ENEMY BULLET
        else {    
            if (hitInfo.collider != null && hitInfo.collider.isTrigger == false) { // Collided with something                        

                if (hitInfo.collider.tag == "Player") {
                    Debug.Log("Player hit!");                                    // She's in love with who I am
                    hitInfo.collider.GetComponent<Damageable>().Damage(Damage); // Back in highschool, I used to bus it to the dance
                }
                else if (hitInfo.collider.tag == "Buildable") {
                    Debug.Log("Turret hit!");
                }
                else {
                    Debug.Log("Environment hit!");
                }

                if (willDestroyBullet)
                    Destroy(gameObject);
            }
        }
    }

    // Sometimes raycasts don't work so, we can use collision as well
    private void OnCollisionEnter2D(Collision2D collision) {        

        // Friendly Bullet controls
        if (isPlayerBullet || isTurretBullet) {
            if (collision.gameObject.tag.Equals("Enemy")) {
                Debug.Log("Enemy hit");
                collision.gameObject.GetComponent<Damageable>().Damage(Damage);
            }
        }

        // Enemy Bullet controls
        if (!isPlayerBullet && !isTurretBullet) {                   
            if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Buildable") {
                Debug.Log("Player/Turret hit");
                collision.gameObject.GetComponent<Damageable>().Damage(Damage);
            }

            if (collision.gameObject.tag == "Base") {
                collision.gameObject.GetComponent<BaseHealth>().Damage(Damage);
            }            
        }

        // Destroy the bullet if it collides with something other than a bullet
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
