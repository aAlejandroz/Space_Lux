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
    public KillCounterUI killCounterUI;

    public void Awake() {
        int dropIndex = rnd.Next(dropList.Count);   // Picks a random drop from the drop list and declares it as the item the enemy will drop
        dropItem = dropList[dropIndex];
        killCounterUI = GameObject.Find("KillCounter").GetComponent<KillCounterUI>();
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
        Destroy(Instantiate(OnDeathParticle, transform.position, Quaternion.Euler(0, 90, 0)), 0.25f);
        killCounterUI.IncKillCount();
        Destroy(gameObject);
	}

    public override IEnumerator WaitAndChangeColor() {
        yield return new WaitForSeconds(flickerDuration);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
