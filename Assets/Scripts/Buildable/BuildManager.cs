using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour {

    public List<Buildable> buildingPrefab;  // array of buildables for different buildables
    public Transform spawnPoint;
    public Grid grid;
    public GunUI gunDisplay;
    private int index;

    private void Start() {        
        index = 0;
    }

    void Update() {
                
        // Player is able to choose what they build with the scroll wheel
        if (Input.GetAxis("Mouse ScrollWheel") != 0f) {
            index++;
            if (index >= buildingPrefab.Count) {
                index = 0;
            }            
        }       
        
        if (Input.GetKeyDown(KeyCode.Alpha1) && buildingPrefab.Count > 0) {
            index = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && buildingPrefab.Count > 1) {
            index = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && buildingPrefab.Count > 2) {
            index = 2;
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            buildingPrefab[index].Build(spawnPoint, grid);
        }

        gunDisplay.UpdateGunDisplay(buildingPrefab[index]);
    }
    
}
