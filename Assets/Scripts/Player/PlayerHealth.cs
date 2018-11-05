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
			StartCoroutine(setInvincibleAndWait());
		}
	}

	protected override void OnDestroyed() {
		Destroy(gameObject);
	}

}

