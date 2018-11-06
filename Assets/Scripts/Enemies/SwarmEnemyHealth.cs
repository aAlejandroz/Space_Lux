using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmEnemyHealth : Damageable {

    static System.Random rnd = new System.Random();
    public List<GameObject> dropList;   // List of possible drop items
    private GameObject dropItem;

    public void Awake() {
        int dropIndex = rnd.Next(dropList.Count);   // Picks a random drop from the drop list and declares it as the item the enemy will drop
        dropItem = dropList[dropIndex];
    }

    protected override void OnDamaged(float damage) {
		CurHP -= damage;
	}

	protected override void OnDestroyed() {
        Instantiate(dropItem, transform.position, dropItem.transform.rotation);
        Destroy(gameObject);
	}
}
