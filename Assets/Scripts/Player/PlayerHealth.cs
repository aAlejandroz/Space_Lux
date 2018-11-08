using System.Collections;
using UnityEngine;

public class PlayerHealth : Damageable {

    public AudioClip HurtSound;
	public HeartUI HeartUI;
    private AudioSource audioSource;

	public PlayerHealth() {
		isInvincible = false;
	}

    private void Awake() {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    protected override void OnDamaged(float damage) {
        if (!isInvincible) {
            audioSource.PlayOneShot(HurtSound);
            CurHP -= damage;                      
			HeartUI.UpdateHearts();
            GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0.5f, 1);
            StartCoroutine(WaitAndChangeColor());
            StartCoroutine(setInvincibleAndWait());
		}
	}

	protected override void OnDestroyed() {
		Destroy(gameObject);
	}

    public override IEnumerator WaitAndChangeColor() {
        yield return new WaitForSeconds(flickerDuration);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}

