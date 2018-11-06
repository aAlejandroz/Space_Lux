using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectingBuildable : MonoBehaviour {
    
    // Change to private with getters
    public bool canBuild = true;
    public bool isBlocked = false;   
    public bool canRemove = false;
    public Collider2D blockingObject;

    // Update Function
    public void Update() {
        if (!isBlocked) {
            canBuild = true;
            canRemove = false;
        }        
    }

    // Function to detect when object is not buildable
    private void OnTriggerStay2D(Collider2D collision) {

        if (collision.tag.Equals("Buildable"))
            blockingObject = collision;                         // Reference to blocking object. Turret / wall, etc.         

        if (collision.GetType() == typeof(BoxCollider2D)) {     // Checks if collider is a box collider. Need b/c turret has circle collider
            canBuild = false;
            isBlocked = true;
        }

        if (isBlocked && collision.tag.Equals("Buildable")) {   // Checks if object in front of player is removable
            if (collision.GetComponent<Buildable>().status == Damageable.Status.ACTIVE) {                
                canRemove = true;
            }
        }
     
    }
        
    // Function to detect when object is buildable
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.GetType() == typeof(BoxCollider2D)) {
            canBuild = true;
            isBlocked = false;
        } 
    } 
    
}
