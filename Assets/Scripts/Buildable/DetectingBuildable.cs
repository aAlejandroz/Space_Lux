using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectingBuildable : MonoBehaviour {

    [SerializeField]
    public bool canBuild = true;
    public bool isBlocked = false;

    public void Update() {
        if (!isBlocked) {
            canBuild = true;
        } 
    }

    // TODO: Player can build on top of building
    private void OnTriggerStay2D(Collider2D collision) {        
        
        if (!collision.tag.Equals("Buildable")) {            
            canBuild = false;
            isBlocked = true;
        } 
        
        /*
        // If the collision is not a circle collider
        if (!(collision.gameObject.GetComponent<Collider2D>().GetType().Equals(typeof(CircleCollider2D)))) {
            canBuild = false;
            isBlocked = true;
        }
        */
    }
        
    private void OnTriggerExit2D(Collider2D collision) {
        
        if (!collision.tag.Equals("Buildable")) {
            canBuild = true;
            isBlocked = false;
        }
        
        /*
        if (collision.GetComponent<Collider2D>().GetType().Equals(typeof(BoxCollider2D))) {
            canBuild = true;
            isBlocked = false;
        }
        */
    } 
    
}
