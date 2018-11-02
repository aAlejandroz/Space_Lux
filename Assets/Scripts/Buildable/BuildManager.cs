using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour {

    public List<Buildable> buildingPrefab;  // array of buildables for different buildables
    [SerializeField]
    private PlayerController player;
    public Transform spawnPoint;
    public Grid grid;
    public GunUI gunDisplay;
    private int index;

    private void Start() {
        player = GetComponent<PlayerController>();
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

        // Player builds with 'C' key
        if (Input.GetKeyDown(KeyCode.C)) {

            if (buildingPrefab[index].isBuildable(spawnPoint)) {
                if (player.ResourceCount >= buildingPrefab[index].buildCost) {
                    buildingPrefab[index].Build(spawnPoint, grid);
                    player.ResourceCount -= buildingPrefab[index].buildCost;
                }
                else {
                    Debug.Log("Not enough resource!");
                }
            } else {
                Debug.Log("Cannot build right now");
            }            
        }

        gunDisplay.UpdateGunDisplay(buildingPrefab[index]);
    }
    
}
