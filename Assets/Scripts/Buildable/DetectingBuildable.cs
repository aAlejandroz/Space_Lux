using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectingBuildable : MonoBehaviour {

    [SerializeField]
    public bool canBuild = true;

    private void OnTriggerStay2D(Collider2D collision) {        
        canBuild = false;        
    }

    private void OnTriggerExit2D(Collider2D collision) {
        canBuild = true;      
    }

}
