using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour {

    public int ResourceCount = 0;
    public ResourceUI resourceUI;
    public GameObject costUI;

    // Update function
    public void Update() {
        resourceUI.UpdateResource(ResourceCount);
    }

    // Function to increment resource count
    public void IncrementResource(int addedResource) {
        ResourceCount += addedResource;
    }

    // Function to decrement resource count
    public void DecrementResource(int addedResource) {
        ResourceCount -= addedResource;
    }

    // Function to return resource count
    public int GetResourceCount() {
        return ResourceCount;
    }

    public void DisplayNumber(int cost, Color color) {
        var clone = (GameObject)Instantiate(costUI, transform.position + new Vector3(0,1), Quaternion.Euler(Vector3.zero));
        clone.GetComponent<FloatingNumber>().color = color;
        clone.GetComponent<FloatingNumber>().cost = cost;
    }

    // Function that triggers whenever player collides a collision trigger
    public void OnTriggerEnter2D(Collider2D collision) {        
        if (collision.tag.Equals("Pick Up")) {      // If we enter the circle collider of crystal
            collision.gameObject.GetComponent<ResourceController>().MoveToPlayer(this.transform);
        }

        /*
        if (collision.tag == "Weapon") {
            string weaponName = collision.gameObject.name;
            string pathName = "Guns/" + weaponName; // ex: Guns/Flamethrower            

            GameObject weapon = Resources.Load<GameObject>(pathName);   // loads weapon prefab from pathname
            weapon.GetComponent<Gun>().isPickedUp = true;           

            gameObject.GetComponent<PlayerController>().WeaponInventory.Add(weapon);
            Debug.Log("New Weapon added to inventory");
            Destroy(collision.gameObject);            
        }
        */
    }
}
