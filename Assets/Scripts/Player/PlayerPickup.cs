using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour {

    private int ResourceCount = 0;
    public ResourceUI resourceUI;

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

    // Function that triggers whenever player collides a collision trigger
    public void OnTriggerEnter2D(Collider2D collision) {        
        if (collision.tag.Equals("Pick Up")) {      // If we enter the circle collider of crystal
            collision.gameObject.GetComponent<ResourceController>().MoveToPlayer(this.transform);
        }
    }
}
