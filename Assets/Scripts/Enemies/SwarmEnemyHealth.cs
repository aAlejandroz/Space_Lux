using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmEnemyHealth : Damageable {

    static System.Random rnd = new System.Random();
    public ParticleSystem OnDeathParticle;
    //public TMPro.TMP_TextEventHandler
    public List<GameObject> dropList;   // List of possible drop items
    private GameObject dropItem;
    private float dropRate = 0.60f;    
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.instance;
    }

    public void Awake() {
        int dropIndex = rnd.Next(dropList.Count);   // Picks a random drop from the drop list and declares it as the item the enemy will drop
        dropItem = dropList[dropIndex];
    }

    protected override void OnDamaged(float damage) {
        GetComponent<SpriteRenderer>().color = new Color(1, 0.3f, 0.3f, 1);
        StartCoroutine(WaitAndChangeColor());
        CurHP -= damage;
	}

	protected override void OnDestroyed() {
        if (Random.Range(0f,1f) <= dropRate) {
            Instantiate(dropItem, transform.position, dropItem.transform.rotation);
        }

        var deathParticle = Instantiate(OnDeathParticle, transform.position, Quaternion.Euler(0, 90, 0));
        Destroy(deathParticle.gameObject, 0.25f);
        audioManager.PlaySound("EnemyDeath");
        Destroy(gameObject);
	}

    public override IEnumerator WaitAndChangeColor() {
        yield return new WaitForSeconds(flickerDuration);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
